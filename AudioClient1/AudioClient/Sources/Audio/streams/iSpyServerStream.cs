using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using g711audio;
using System.Text;
using System.Windows.Forms;
using AudioClient;

namespace AudioClient.Sources.Audio.streams
{
    class iSpyServerStream : IAudioSource, IDisposable
    {
        private string _source;
        private float _gain;
        private bool _listening;
        private ManualResetEvent _stopEvent;
        private delegate void SetVolumnCallBack(int value);
        public delegate void SetVolumnMuteCallBack();
        private MainForm parentForm;

        private Thread _thread;

        private BufferedWaveProvider _waveProvider;
        private SampleChannel _sampleChannel;

        public BufferedWaveProvider WaveOutProvider { get; set; }

        /// <summary>
        /// New frame event.
        /// </summary>
        /// 
        /// <remarks><para>Notifies clients about new available frame from audio source.</para>
        /// 
        /// <para><note>Since audio source may have multiple clients, each client is responsible for
        /// making a copy (cloning) of the passed audio frame, because the audio source disposes its
        /// own original copy after notifying of clients.</note></para>
        /// </remarks>
        /// 
        public event DataAvailableEventHandler DataAvailable;

        /// <summary>
        /// New frame event.
        /// </summary>
        /// 
        /// <remarks><para>Notifies clients about new available frame from audio source.</para>
        /// 
        /// <para><note>Since audio source may have multiple clients, each client is responsible for
        /// making a copy (cloning) of the passed audio frame, because the audio source disposes its
        /// own original copy after notifying of clients.</note></para>
        /// </remarks>
        /// 
        public event LevelChangedEventHandler LevelChanged;

        /// <summary>
        /// audio source error event.
        /// </summary>
        /// 
        /// <remarks>This event is used to notify clients about any type of errors occurred in
        /// audio source object, for example internal exceptions.</remarks>
        /// 
        //public event AudioSourceErrorEventHandler AudioSourceError;

        /// <summary>
        /// audio playing finished event.
        /// </summary>
        /// 
        /// <remarks><para>This event is used to notify clients that the audio playing has finished.</para>
        /// </remarks>
        /// 
        public event AudioFinishedEventHandler AudioFinished;

        /// <summary>
        /// audio source.
        /// </summary>
        /// 
        /// <remarks>URL, which provides JPEG files.</remarks>
        /// 
        public virtual string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public float Gain
        {
            get { return _gain; }
            set
            {
                _gain = value;
                if (_sampleChannel != null)
                {
                    _sampleChannel.Volume = value;
                }
            }
        }

        public bool Listening
        {
            get
            {
                if (IsRunning && _listening)
                    return true;
                return false;

            }
            set
            {
                if (RecordingFormat == null)
                {
                    _listening = false;
                    return;
                }

                if (WaveOutProvider != null)
                {
                    if (WaveOutProvider.BufferedBytes > 0) WaveOutProvider.ClearBuffer();
                    WaveOutProvider = null;
                }
                if (value)
                {
                    WaveOutProvider = new BufferedWaveProvider(RecordingFormat) { DiscardOnBufferOverflow = true, BufferDuration = TimeSpan.FromMilliseconds(500) };
                }

                _listening = value;
            }
        }


        /// <summary>
        /// State of the audio source.
        /// </summary>
        /// 
        /// <remarks>Current state of audio source object - running or not.</remarks>
        /// 
        public bool IsRunning => _thread != null && !_thread.Join(TimeSpan.Zero);


        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDeviceStream"/> class.
        /// </summary>
        /// 
        public iSpyServerStream() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDeviceStream"/> class.
        /// </summary>
        /// 
        /// <param name="source">source, which provides audio data.</param>
        /// 
        public iSpyServerStream(string source, MainForm parent)
        {
            _source = source;
            parentForm = parent;
        }

        private readonly object _lock = new object();

        /// <summary>
        /// Start audio source.
        /// </summary>
        /// 
        /// <remarks>Starts audio source and return execution to caller. audio source
        /// object creates background thread and notifies about new frames with the
        /// help of <see cref="DataAvailable"/> event.</remarks>
        /// 
        /// <exception cref="ArgumentException">audio source is not specified.</exception>
        /// 
        public void Start()
        {
            if (IsRunning) return;
            // check source
            if (string.IsNullOrEmpty(_source))
                throw new ArgumentException("Audio source is not specified.");

            lock (_lock)
            {
                _waveProvider = new BufferedWaveProvider(RecordingFormat);
                _sampleChannel = new SampleChannel(_waveProvider);
                _sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;

                _stopEvent = new ManualResetEvent(false);
                _thread = new Thread(SpyServerListener)
                {
                    Name = "iSpyServer Audio Receiver (" + _source + ")"
                };
                _thread.IsBackground = true;
                _thread.Start();
            }
        }

        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }


        private void SpyServerListener()
        {
            var data = new byte[3200];
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(_source);
                request.Timeout = 10000;
                request.ReadWriteTimeout = 5000;
                var response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream == null)
                        throw new Exception("Stream is null");
                    stream.ReadTimeout = 5000;
                    while (!_stopEvent.WaitOne(0, false))
                    {
                        int recbytesize = stream.Read(data, 0, 3200);
                        if (recbytesize == 0)
                        {
                            throw new Exception("lost stream");
                        }
                        var recive = Encoding.UTF8.GetString(data, 0, recbytesize);
                        if (recive.Equals("setCurrentVolumeMute"))
                        {
                            setVolumeMute();
                        }
                        else if (recive.StartsWith("setCurrentVolume"))
                        {
                            if (recive.IndexOf("#") > -1)
                            {
                                var cmd = recive.Split(new[] { '#' });
                                Thread setValueThread = new Thread(new ParameterizedThreadStart(this.setVolumeFromServer));
                                setValueThread.IsBackground = true;
                                setValueThread.Start(cmd[1]);


                                Thread setValueThread1 = new Thread(new ParameterizedThreadStart(this.setVolumeWINMM));
                                setValueThread1.IsBackground = true;
                                setValueThread1.Start(cmd[1]);

                            }
                        }
                        else
                        {
                            byte[] dec;
                            ALawDecoder.ALawDecode(data, recbytesize, out dec);
                            var da = DataAvailable;
                            if (da != null)
                            {
                                if (_sampleChannel != null)
                                {
                                    _waveProvider.AddSamples(dec, 0, dec.Length);

                                    var sampleBuffer = new float[dec.Length];
                                    int read = _sampleChannel.Read(sampleBuffer, 0, dec.Length);

                                    da(this, new DataAvailableEventArgs((byte[])dec.Clone(), read));


                                    if (Listening)
                                    {
                                        WaveOutProvider?.AddSamples(dec, 0, read);
                                    }

                                }
                            }
                            else
                            {
                                break;
                            }
                            // need to stop ?
                            if (_stopEvent.WaitOne(0, false))
                                break;
                        }
                    }

                }

                AudioFinished?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.StoppedByUser));
            }
            catch (Exception e)
            {
                var af = AudioFinished;
                af?.Invoke(this, new PlayingFinishedEventArgs(ReasonToFinishPlaying.DeviceLost));

                //Logger.LogExceptionToFile(e,"ispyServer");
            }

            if (_sampleChannel != null)
            {
                _sampleChannel.PreVolumeMeter -= SampleChannelPreVolumeMeter;
                _sampleChannel = null;
            }

            if (_waveProvider?.BufferedBytes > 0)
                _waveProvider.ClearBuffer();

            if (WaveOutProvider?.BufferedBytes > 0) WaveOutProvider?.ClearBuffer();
        }


        private void setVolumeMute()
        {
            if (parentForm.InvokeRequired)
            {
                SetVolumnMuteCallBack svmCallBack = new SetVolumnMuteCallBack(setVolumeMute);
                parentForm.Invoke(svmCallBack);
            }
            else
            {
                NativeCalls.SendMessage(parentForm.Handle, NativeCalls.WM_APPCOMMAND, IntPtr.Zero, (IntPtr)NativeCalls.APPCOMMAND_VOLUME_MUTE);
            }
        }

        private void setVolumeWINMM(object value)
        {
            uint intValue = Convert.ToUInt32(value);
            if (intValue < 0)
            {
                intValue = 0;
            }
            if (intValue > 0xffff)
            {
                intValue = 0xffff;
            }
            uint valueTemp = (uint)((0xffff * (double)intValue) / (double)100);
            uint volume = valueTemp << 16 | valueTemp;
            NativeCalls.waveOutSetVolume(IntPtr.Zero, volume);
        }


        public void setVolumeFromServer(object value)
        {
            //需要确保object里面存储的为正确的int值，否则会抛错
            int intValue = Convert.ToInt32(value);

            //parentForm.ValueOfProsessBar = intValue;
            setVolumeFromTrackBar(intValue);
        }

        private void setVolumeFromTrackBar(int value)
        {
            if (parentForm.InvokeRequired)
            {
                SetVolumnCallBack svCallBack = delegate (int v)
                {
                    parentForm.progressBar1.Value = v;
                };
                //SetVolumnCallBack svCallBack = setVolumeFromTrackBar;
                //parentForm.Invoke(svCallBack, new object[] { value });
                parentForm.Invoke(svCallBack,value );
            }
            else
            {
                if ((value <= 100) && (value >= 0))
                {
                    parentForm.progressBar1.Value = value;
                    Application.DoEvents();
                    //parentForm.leftTrackBar.Value = value;
                }
            }
        }


        public void WaitForStop()
        {
            if (!IsRunning) return;
            // wait for thread stop
            _stopEvent.Set();

            try
            {
                while (_thread != null && !_thread.Join(0))
                    Application.DoEvents();
            }
            catch
            {
                // ignored
            }

            _stopEvent?.Close();
            _stopEvent = null;
        }

        /// <summary>
        /// Stop audio source.
        /// </summary>
        /// 
        /// <remarks><para>Stops audio source.</para>
        /// </remarks>
        /// 
        public void Stop()
        {
            WaitForStop();
        }


        public WaveFormat RecordingFormat { get; set; }

        private bool _disposed;
        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _stopEvent?.Close();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}
