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
        private string NoDevices = "没有可输入的设备";
        public objectsMicrophone Mic;
        public int nPort = 8092;
        public string AudioOutDevice;
        public int selind = -1;
        public SendVolumeLevel MicVolumeLevel;
        internal static iSpyLANServer MWS;
        private string addressIPv4;
        public static List<int> clientId = new List<int>();

        #region  F2热键处理
        public bool ifF2Press = false;
        public DateTime ifF2PressTime = new DateTime();
        public static bool ifF2PressProsessing = false;
        private Timer _updateTime;
        private Timer checkF2HotKey;
        private delegate void handleF2PressDelegate();
        private delegate void handleF2PressProcessingDelegate();
        private delegate void handleRichTextDelegate(string message);
        #endregion

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

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MWS.StopServer();
            HotKey.UnregisterHotKey(Handle, 100);
        }

        private void _updateTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            handleF2PressProsesingSetFalse();
            if (MicVolumeLevel != null)
            {
                MicVolumeLevel.Tick();
            }

            string strClient = string.Empty;
            strClient += "Check 当前的ClientId有：";
            foreach (var temp in clientId)
            {
                strClient += temp+",";
            }
            strClient = strClient.TrimEnd(',');
            //sendMessageToRichTextBox(strClient);
        }

        private void sendMessageToRichTextBox(string message)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    handleRichTextDelegate rt = delegate (string str)
                    {
                        this.richTextBox1.Text += message;
                    };
                    this.Invoke(rt,message);
                }
                else
                {
                    this.richTextBox1.Text += message;
                }
            }
            catch (System.Exception ex)
            {
            	
            }
        }

        /// <summary>
        /// Timer Elapsed事件定时处理赋值，认定F2键已经弹起
        /// </summary>
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
            }
        }

        /// <summary>
        /// 连接音频流，并开启监听逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 滑块控制音量，发送给客端。音量范围（0-100）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            MicVolumeLevel.SendVolumn(string.Format(@"setCurrentVolume#{0}#", Convert.ToInt32(this.trackBar1.Value)));
        }

        private void CheckF2HotKey_Elapsed(object sender, ElapsedEventArgs e)
        {
            handleF2PressProsessingSetFalse();
        }

        /// <summary>
        /// 判定在经过一个Form Timer周期，F2键依然判定为弹起，结束F2长按处理Timer
        /// </summary>
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
        
        /// <summary>
        /// 给客端发送Mute信息
        /// </summary>
        private void muteButton_Click(object sender, EventArgs e)
        {
            //需要解决Client端程序未运行时，无法连接的异常。
            MicVolumeLevel.SendVolumn(@"setCurrentVolumeMute");
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

        /// <summary>
        /// 截获全局热键F2，开启Timer纪录F2长按事件
        /// </summary>
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

        /// <summary>
        /// 控制RichTextBox中的行数，默认为100行。避免长时间使用，占用更多内存(一条Log占一行)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 增加一个ClientId，默认从不存在的最小数开始添加。比如现在1278会添加3
        /// 同时更新ClientId给 功能iSpyServer使用。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            for (int i = 1; i > 0; i++)
            {
                if (i == 30)
                {
                    break;
                }
                if (!this.checkedListBox1.Items.Contains(i.ToString()))
                {
                    //for (int j = 0;j >=0;j++)
                    //{
                    //}
                    this.checkedListBox1.Items.Insert(i-1,i);
                    //this.checkedListBox1.Items.Add(i);
                    break;
                }
            }
            clientId.Clear();
            foreach (var temp in this.checkedListBox1.Items)
            {
                clientId.Add(Convert.ToInt32(temp));
            }
        }

        private void checkedListBoxSort()
        {        


            List<int> data = new List<int>();
            data.Insert(0,1);
            for (int i = data.Count - 1; i > 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    if (data[j] > data[j + 1])
                    { }
                    //Swap(data, j, j + 1);
                }
            }
        }

        /// <summary>
        /// 删除CheckedListBox中所勾选的ClientId
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
            {
                if (this.checkedListBox1.GetItemChecked(i))
                {
                    this.checkedListBox1.Items.RemoveAt(i);
                    if (clientId.Contains(i+1))
                    {
                        clientId.Remove(i+1);
                    }
                }
            }
        }

        /// <summary>
        /// 遍历实现全选和全反选功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string strClients = string.Empty;
            clientId.Clear();
            if (this.checkBox1.Checked)
            {
                for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
                {
                    if (!this.checkedListBox1.GetItemChecked(i))
                    {
                        this.checkedListBox1.SetItemChecked(i, true);
                    }
                }
            }
            else if (!this.checkBox1.Checked)
            {
                for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
                {
                    if (this.checkedListBox1.GetItemChecked(i))
                    {
                        this.checkedListBox1.SetItemChecked(i, false);
                    }
                }
            }
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (this.checkedListBox1.GetItemChecked(i))
                {
                    clientId.Add(i + 1);
                    strClients += i + 1 + "号,";
                }
            }
            if (this.checkBox1.Checked)
            {
                this.richTextBox1.Text += strClients.TrimEnd(',') + " 可以收听 \r\n";
            }
            else
            {
                this.richTextBox1.Text += "暂无Client可以收听语音。 \r\n";
            }

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.checkedListBox1.BeginUpdate();
            string strClients = string.Empty;
            clientId.Clear();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (this.checkedListBox1.GetItemChecked(i))
                {
                    clientId.Add(i + 1);
                    strClients += i + 1;
                    strClients +=  "号,";
                }
            }
            this.richTextBox1.Text += strClients.TrimEnd(',') + " 可以收听 \r\n";
            Application.DoEvents();
            //Thread.Sleep(1000);
            //MWS.StopServer();
            //MWS.StartServer();
            this.checkedListBox1.EndUpdate();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}

