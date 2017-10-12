using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using NAudio.Wave.SampleProviders;
using System.Net.Sockets;
using g711audio;
using System.Threading;
using System.Net;
using Timer = System.Timers.Timer;
using System.Timers;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace AudioClient
{
    public partial class MainForm : Form
    {
        #region Private
        private string NoDevices = "没有可输入的设备";
        private readonly ManualResetEvent _stopEvent = new ManualResetEvent(false);
        private int _bitsPerSample = 16;
        private int _channels = 1;
        private long _lastRun = DateTime.Now.Ticks;
        private int _milliCount;
        private bool _processing;
        private int _sampleRate = 15000;
        private double _value;
        private WaveFormat _recordingFormat;
        private WaveIn _waveIn;

        private WaveInProvider _waveProvider;
        private MeteringSampleProvider _meteringProvider;
        private SampleChannel _sampleChannel;

        public BufferedWaveProvider WaveOutProvider { get; set; }

        #endregion

        public objectsMicrophone OutMic;
        public int nPort = 8092;
        public string AudioOutDevice;
        public int selind = -1;
        public VolumeLevel OutVolumeLevel;
        public delegate void SetVolumnCallBack(int value);
        public delegate void SetVolumnMuteCallBack();
        private const int BufferSize = 8192;//接收或返送数据的缓冲区大小
        static object locker = new object();
        private int valueOfProsessBar = 0;

        private static ImageCodecInfo _encoder;
        public static EncoderParameters EncoderParams;

        private Timer _updateTime;

        public MainForm()
        {
            InitializeComponent();
            //Form运行在屏幕右下角逻辑
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
            Point p = new Point(x, y);
            this.PointToScreen(p);
            this.Location = p;

            ShowCurrentVolume();

        }
        private objectsMicrophone AddMicrophone()
        {
            objectsMicrophone Mic = new objectsMicrophone
            {
                alerts = new objectsMicrophoneAlerts(),
                detector = new objectsMicrophoneDetector(),
                notifications = new objectsMicrophoneNotifications(),
                recorder = new objectsMicrophoneRecorder(),
                schedule = new objectsMicrophoneSchedule
                {
                    entries
                                                    =
                                                    new objectsMicrophoneScheduleEntry
                                                    [
                                                    0
                                                    ]
                }
            };
            Mic.settings = new objectsMicrophoneSettings();

            Mic.id = 1;
            //om.directory = RandomString(5);
            Mic.x = 0;
            Mic.y = 0;
            Mic.width = 160;
            Mic.height = 40;
            Mic.name = "MIC";
            Mic.description = "";
            Mic.newrecordingcount = 0;

            int port = 257;
            //foreach (objectsMicrophone om2 in Microphones)
            //{
            //    if (om2.port > port)
            //        port = om2.port + 1;
            //}
            Mic.port = port;

            Mic.settings.typeindex = 0;
            // if (audioSourceIndex == 2)
            //   om.settings.typeindex = 1;
            Mic.settings.deletewav = true;
            Mic.settings.buffer = 4;
            Mic.settings.samples = 8000;
            Mic.settings.bits = 16;
            Mic.settings.channels = 1;
            Mic.settings.decompress = true;
            Mic.settings.active = false;
            Mic.settings.notifyondisconnect = false;
            //Mic.settings.messaging = false;

            Mic.detector.sensitivity = 60;
            Mic.detector.nosoundinterval = 30;
            Mic.detector.soundinterval = 0;
            Mic.detector.recordondetect = true;

            Mic.alerts.mode = "sound";
            Mic.alerts.minimuminterval = 60;
            Mic.alerts.executefile = "";
            Mic.alerts.active = false;
            Mic.alerts.alertoptions = "false,false";

            Mic.recorder.inactiverecord = 5;
            Mic.recorder.maxrecordtime = 900;

            Mic.notifications.sendemail = false;
            Mic.notifications.sendsms = false;

            Mic.schedule.active = false;
            return Mic;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            //Mic = AddMicrophone();

            EncoderParams = new EncoderParameters(1)
            {
                Param =
                                {
                                    [0] =
                                        new EncoderParameter(
                                        System.Drawing.Imaging.Encoder.Quality,
                                        75)
                                }
            };
            try
            {
                //输出设备
                int i = 0, j = 0;
                var d = DirectSoundOut.Devices;
                if (d != null)
                {
                    foreach (var dev in d)
                    {
                        ddlAudioOut.Items.Add(new ListItem(dev.Description, new string[] { dev.Guid.ToString() }));
                        if (dev.Guid.ToString() == AudioOutDevice)
                            i = j;
                        j++;
                    }
                    if (ddlAudioOut.Items.Count > 0)
                        ddlAudioOut.SelectedIndex = i;
                    else
                    {
                        ddlAudioOut.Enabled = false;
                    }
                }
            }
            catch (ApplicationException ex)
            {
            }

            _updateTime = new Timer(500);
            _updateTime.Elapsed += _updateTime_Elapsed;
            _updateTime.AutoReset = true;
            _updateTime.Start();
        }

        private void ShowCurrentVolume()
        {
            uint volume;
            NativeCalls.waveOutGetVolume(IntPtr.Zero, out volume);
            int left = (int)(volume & 0xFFFF);
            int right = (int)((volume >> 16) & 0xFFFF);
            //leftTrackBar.Value = (left+right)/2;
            leftTrackBar.Value = (left | right) * 100 / 0xFFFF;
            progressBar1.Value = (left | right) * 100 / 0xFFFF;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("程序关闭后无法接受音频，是否关闭？\r\n Yes：关闭。No：最小化到托盘", "取消", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //this.Dispose();
            }
            else if (result == DialogResult.No)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                e.Cancel = true;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            if (!string.IsNullOrEmpty(str))
            {
                OutMic = AddMicrophone();
                OutMic.settings.sourcename = str;
                OutMic.settings.deviceout = ((ListItem)ddlAudioOut.SelectedItem).Value[0];
                if (OutVolumeLevel == null)
                {
                    OutVolumeLevel = new VolumeLevel(OutMic);
                }
                this.panel.Controls.Add(OutVolumeLevel);
                OutVolumeLevel.Dock = DockStyle.Fill;
                OutVolumeLevel.ParentForm = this;

                OutVolumeLevel.Listening = true;
                OutVolumeLevel.Refresh();
                OutVolumeLevel.Enable();
                //OutVolumeLevel.Invalidate();
            }
        }

        public static ImageCodecInfo Encoder
        {
            get
            {
                if (_encoder != null)
                    return _encoder;
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.FormatID == ImageFormat.Jpeg.Guid)
                    {
                        _encoder = codec;
                        return codec;
                    }
                }
                return null;
            }
        }

        private struct ListItem
        {
            internal string Name;
            internal string[] Value;
            public override string ToString()
            {
                return Name;
            }
            public ListItem(string Name, string[] Value) { this.Name = Name; this.Value = Value; }
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            if (OutVolumeLevel != null)
            {
                if (OutVolumeLevel.IsEnabled)
                {
                    string str = OutVolumeLevel.RecordSwitch(true);
                    MessageBox.Show(str);
                }
            }
        }

        private void setVolumeFromTrackBar()
        {
            uint valueTemp = (uint)(((double)0xffff * (double)leftTrackBar.Value) / (double)(this.leftTrackBar.Maximum - this.leftTrackBar.Minimum));
            uint volume = (uint)(valueTemp | (valueTemp << 16));
            NativeCalls.waveOutSetVolume(IntPtr.Zero, volume);
        }

        private void leftTrackBar_Scroll(object sender, EventArgs e)
        {
            this.progressBar1.Value = this.leftTrackBar.Value;
            setVolumeFromTrackBar();
        }

        private void muteButton_Click(object sender, EventArgs e)
        {
            setVolumeMute();
        }

        private void setVolumeMute()
        {
            if (this.InvokeRequired)
            {
                SetVolumnMuteCallBack svmCallBack = new SetVolumnMuteCallBack(setVolumeMute);
                this.Invoke(svmCallBack);
            }
            else
            {
                NativeCalls.SendMessage(this.Handle, NativeCalls.WM_APPCOMMAND, IntPtr.Zero, (IntPtr)NativeCalls.APPCOMMAND_VOLUME_MUTE);
            }

        }

        private void _updateTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (OutVolumeLevel != null)
            {
                OutVolumeLevel.Tick();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string path = this.GetType().Assembly.Location;
            //string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //string proName = System.AppDomain.CurrentDomain.SetupInformation.ApplicationName;
            if (checkBox1.Checked)
            {
                SetAutoRun(path, true);  //设置自动启动当前程序
            }
            else
            {
                SetAutoRun(path, false);  //取消自动启动
            }

        }

        public void SetAutoRun(string fileName, bool isAutoRun)
        {
            RegistryKey reg = null;
            try
            {
                if (!System.IO.File.Exists(fileName))
                    throw new Exception("该文件不存在!");
                String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoRun)
                    reg.SetValue(name, fileName);
                else
                    reg.DeleteValue(name, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw new Exception(ex.ToString());
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }
    }
}

