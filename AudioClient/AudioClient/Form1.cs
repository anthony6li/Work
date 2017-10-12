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

namespace AudioClient
{
    public partial class Form1 : Form
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

        public bool IsEdit;
        public bool NoSource;
        public Queue<short> PlayBuffer;
        public bool ResizeParent;
        public objectsMicrophone Mic;
        public objectsMicrophone OutMic;
        private bool _shuttingDown;
        public int nPort = 8092;
        public string AudioOutDevice;
        public int selind = -1;
        public SendVolumeLevel MicVolumeLevel;
        public VolumeLevel OutVolumeLevel;
        public static float Volume;
        internal static iSpyLANServer MWS;
        private Action<float> setVolumeDelegate;
        private Action<string> setVolumeTextDelegate;


        private static ImageCodecInfo _encoder;
        public static EncoderParameters EncoderParams;
        private string addressIPv4;

        private Timer _updateTime;

        public Form1()
        {
            InitializeComponent();


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
            Mic = AddMicrophone();

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
                MWS = new iSpyLANServer(this);
                MWS.StartServer();

                //获取输入设备
                int _i = 0;
                for (int n = 0; n < WaveIn.DeviceCount; n++)
                {
                    ddlDevice.Items.Add(WaveIn.GetCapabilities(n).ProductName);
                    if (WaveIn.GetCapabilities(n).ProductName == Mic.settings.sourcename)
                        selind = _i;
                    _i++;

                }
                ddlDevice.Enabled = true;
                if (selind > -1)
                    ddlDevice.SelectedIndex = selind;
                else
                {
                    if (ddlDevice.Items.Count == 0)
                    {
                        ddlDevice.Items.Add(NoDevices);
                        ddlDevice.Enabled = false;
                    }
                    else
                        ddlDevice.SelectedIndex = 0;
                }
                //取得输出设备
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
                ddlDevice.Items.Add(NoDevices);
                ddlDevice.Enabled = false;
                ddlAudioOut.Items.Add(NoDevices);
                ddlAudioOut.Enabled = false;
                
            }
            _updateTime = new Timer(500);
            _updateTime.Elapsed += _updateTime_Elapsed;
            _updateTime.AutoReset = true;
            _updateTime.Start();
        }

        private void _updateTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            if (MicVolumeLevel != null)
            {
                MicVolumeLevel.Tick();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //
            Mic.settings.sourcename = ddlDevice.SelectedItem.ToString();
            AudioOutDevice = ddlDevice.SelectedItem.ToString();
            MicVolumeLevel = new SendVolumeLevel(Mic, 0, this);
            MicVolumeLevel.AudioMode = 0;
            this.panel1.Controls.Add(MicVolumeLevel);
            MicVolumeLevel.Dock = DockStyle.Fill;
            MicVolumeLevel.Enable();


            this.setVolumeDelegate = (vol) => this.MicVolumeLevel.Value = vol;
            this.setVolumeTextDelegate = (voll) => this.label1.Text = voll;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MWS.StopServer();
            _shuttingDown = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string str = textBox1.Text;
            if (!string.IsNullOrEmpty(str))
            {
                OutMic = AddMicrophone();
                OutMic.settings.sourcename = str;
                OutMic.settings.active = true;
                OutMic.settings.deviceout = ((ListItem)ddlAudioOut.SelectedItem).Value[0];
                OutVolumeLevel = new VolumeLevel(OutMic);

                this.panel2.Controls.Add(OutVolumeLevel);
                OutVolumeLevel.Dock = DockStyle.Fill;

                OutVolumeLevel.Listening = true;

               // OutVolumeLevel.Enable();
                OutVolumeLevel.Apply();
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

        public string AddressIPv4
        {
            get
            {
                string name = Dns.GetHostName();
                IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
                List<IPAddress> ip4list = new List<IPAddress>();
                foreach (IPAddress ipa in ipadrlist)
                {
                    if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ip4list.Add(ipa);
                    }
                }
                if (ip4list.Count() > 0)
                {
                    addressIPv4 = ip4list.First().ToString();
                }
                return addressIPv4;
            }

            set
            {
                addressIPv4 = value;
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

        private void button3_Click(object sender, EventArgs e)
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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //float value = this.trackBar1.Value / 100f;
            //Mic.settings.volume = this.trackBar1.Value;
            //this.label1.Text = "音量：" + this.trackBar1.Value + "%";
            if (setVolumeDelegate != null)
            {
                setVolumeDelegate(trackBar1.Value);
            }
            if (setVolumeTextDelegate != null)
            {
                setVolumeTextDelegate("音量：" + this.trackBar1.Value + "%");
            }
            this.playButton.Enabled = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("当前音量为 "+ this.trackBar1.Value +"%");
        }

        private void muteButton_Click(object sender, EventArgs e)
        {
            this.playButton.Enabled = true;
            //if (MicVolumeLevel != null)
            //{
            //    MicVolumeLevel.Disable();
            //}
            //MicVolumeLevel = new SendVolumeLevel(Mic, 0, this);
            //MicVolumeLevel.AudioMode = 0;
            //this.panel1.Controls.Add(MicVolumeLevel);
            //MicVolumeLevel.Dock = DockStyle.Fill;
            //MicVolumeLevel.Disable();
            ////this.MicVolumeLevel.WaveOut.Pause();
            //Mic.settings.volume = 0;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            //MicVolumeLevel = new SendVolumeLevel(Mic, 0, this);
            //MicVolumeLevel.AudioMode = 0;
            //this.panel1.Controls.Add(MicVolumeLevel);
            //MicVolumeLevel.Dock = DockStyle.Fill;
            //MicVolumeLevel.Enable();
        }
    }
}

