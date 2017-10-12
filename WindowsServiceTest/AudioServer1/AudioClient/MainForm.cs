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
using AudioClient;

namespace AudioServer1
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

        public bool IsEdit;
        public bool NoSource;
        public Queue<short> PlayBuffer;
        public bool ResizeParent;
        public objectsMicrophone Mic;
        private bool _shuttingDown;
        public int nPort = 8092;
        public string AudioOutDevice;
        public int selind = -1;
        public SendVolumeLevel MicVolumeLevel;
        internal static iSpyLANServer MWS;
        public delegate void SetVolumnCallBack(int value);
        private string addressIPv4;

        public bool ifF2Press = false;
        public DateTime ifF2PressTime = new DateTime();
        public static bool ifF2PressProsessing = false;
        private Timer _updateTime;
        private Timer checkF2HotKey;
        private delegate void handleF2PressDelegate();
        private delegate void handleF2PressProcessingDelegate();


        public MainForm()
        {
            try
            {
                InitializeComponent();
                //Form运行在屏幕右下角逻辑
                int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
                int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
                Point p = new Point(x, y);
                this.PointToScreen(p);
                this.Location = p;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

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

        private void MainForm_Load(object sender, EventArgs e)
        {
            //设置F2热键的ID为100，注册热键
            HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.None, Keys.F2);

            Mic = AddMicrophone();

            try
            {
                MWS = new iSpyLANServer(this);
                MWS.StartServer();

                //输入设备
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

            }
            catch (ApplicationException ex)
            {
                ddlDevice.Items.Add(NoDevices);
                ddlDevice.Enabled = false;
                MessageBox.Show(ex.ToString());
            }
            _updateTime = new Timer(500);
            _updateTime.Elapsed += _updateTime_Elapsed;
            _updateTime.AutoReset = true;
            _updateTime.Start();

            //用来记录F2按下的时间
            checkF2HotKey = new Timer(500);
            checkF2HotKey.AutoReset = true;
            checkF2HotKey.Elapsed += CheckF2HotKey_Elapsed;
        }

        private void CheckF2HotKey_Elapsed(object sender, ElapsedEventArgs e)
        {
            handleF2PressProsessingSetFalse();
        }
        private void handleF2PressProsessingSetFalse()
        {
            if (this.InvokeRequired)
            {
                handleF2PressProcessingDelegate hf2 = new handleF2PressProcessingDelegate(handleF2PressProsessingSetFalse);
                this.Invoke(hf2);
            }
            else
            {
                if (!ifF2Press)
                {
                    TimeSpan ts = DateTime.Now - ifF2PressTime;
                    if (ts.TotalMilliseconds > 600)
                    {
                        MainForm.ifF2PressProsessing = false;
                        this.richTextBox1.Text += "F2 unpress.time is " + DateTime.Now.ToString() + "\r\n";
                        checkF2HotKey.Stop();
                    }
                }
            }
        }


        private void _updateTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            handleF2PressProsesingSetFalse();
            if (MicVolumeLevel != null)
            {
                MicVolumeLevel.Tick();
            }
        }
        private void handleF2PressProsesingSetFalse()
        {
            if (this.InvokeRequired)
            {
                handleF2PressDelegate hf2 = new handleF2PressDelegate(handleF2PressProsesingSetFalse);
                this.Invoke(hf2);
            }
            else
            {
                ifF2Press = false;
                //this.richTextBox1.Text += string.Format("Timmer1 Test! time is {0} \r\n", DateTime.Now);
            }
        }

        private void getButton_Click(object sender, EventArgs e)
        {
            try
            {
                Mic.settings.sourcename = ddlDevice.SelectedItem.ToString();
                AudioOutDevice = ddlDevice.SelectedItem.ToString();
                MicVolumeLevel = new SendVolumeLevel(Mic, 0, this);
                MicVolumeLevel.AudioMode = 0;
                this.panel1.Controls.Add(MicVolumeLevel);
                MicVolumeLevel.Dock = DockStyle.Fill;
                MicVolumeLevel.Enable();
                this.muteButton.Enabled = true;
                this.trackBar1.Enabled = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            setVolumeToClient(this.trackBar1.Value);
        }

        private void setVolumeToClient(object value)
        {
            MicVolumeLevel.SendVolumn(string.Format(@"setCurrentVolume#{0}#", Convert.ToInt32(value)));
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MWS.StopServer();
            HotKey.UnregisterHotKey(Handle, 100);
            _shuttingDown = true;
        }

        internal static int byte2int(byte[] res)
        {
            // 一个byte数据左移24位变成0x??000000，再右移8位变成0x00??0000   

            int targets = (res[0] & 0xff) | ((res[1] << 8) & 0xff00) // | 表示安位或   
            | ((res[2] << 24) >> 8) | (res[3] << 24);
            return targets;
        }

        internal static byte[] int2byte(int res)
        {
            byte[] targets = new byte[4];

            targets[0] = (byte)(res & 0xff);// 最低位   
            targets[1] = (byte)((res >> 8) & 0xff);// 次低位   
            targets[2] = (byte)((res >> 16) & 0xff);// 次高位   
            targets[3] = (byte)(res >> 24);// 最高位,无符号右移。   
            return targets;
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

        private void ddlDevice_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void setVolumeMuteToClient()
        {
            MicVolumeLevel.SendVolumn(@"setCurrentVolumeMute");
        }

        private void muteButton_Click(object sender, EventArgs e)
        {
            //需要解决Client端程序未运行时，无法连接的异常。
            setVolumeMuteToClient();
        }
        /// <summary>
        /// 执行本地Mute操作方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            NativeCalls.SendMessage(this.Handle, NativeCalls.WM_APPCOMMAND, IntPtr.Zero, (IntPtr)NativeCalls.APPCOMMAND_VOLUME_MUTE);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:
                            handleF2PressSetTrue();
                            break;
                        default:
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void handleF2PressSetTrue()
        {
            //if (!ifF2Press)
            {
                ifF2Press = true;
                MainForm.ifF2PressProsessing = true;
                ifF2PressTime = DateTime.Now;
            }
            if (!checkF2HotKey.Enabled)
            {
                checkF2HotKey.Start();
                this.richTextBox1.Text += string.Format("F2 Press! time is {0} \r\n", DateTime.Now);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            int maxLines = 100;
            if (this.richTextBox1.Lines.Length > maxLines)
            {
                int moreLines = this.richTextBox1.Lines.Length - maxLines;
                string[] lines = this.richTextBox1.Lines;
                Array.Copy(lines, moreLines, lines, 0, maxLines);
                Array.Resize(ref lines, maxLines);
                this.richTextBox1.Lines = lines;
            }
        }
    }
}

