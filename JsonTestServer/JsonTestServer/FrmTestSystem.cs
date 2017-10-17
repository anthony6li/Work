using System;
using System.Net;
using System.Windows.Forms;
using Util;
using System.Text;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;

namespace JsonTestServer
{
    public partial class FrmTestSystem : Form
    {
        HttpUtil htmlUtil = new HttpUtil();
        public FrmTestSystem()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
        }

        private void btn_POST_Click(object sender, EventArgs e)
        {
            string strUrl = GetUrlString();
            string postdata = this.rtb_Data.Text;
            this.rtb_ACK.Text = htmlUtil.HttpPost(strUrl, postdata);
        }

        private void btn_POST8_Click(object sender, EventArgs e)
        {
            string strUrl = GetUrlString();
            string postdata = this.rtb_Data.Text;
            this.rtb_ACK.Text = htmlUtil.HttpPostUTF8(strUrl, postdata);
        }

        private void btn_GET_Click(object sender, EventArgs e)
        {
            string strUrl = GetUrlString();
            string postdata = this.rtb_Data.Text;
            this.rtb_ACK.Text = htmlUtil.HttpGet(strUrl, postdata);
        }

        private string GetUrlString()
        {
            string strUrl = string.Empty;
            try
            {
                Uri uri = new Uri(new Uri("http://" + this.Tb_IP.Text + ":" + this.Tb_Port.Text + "/"), this.cb_Request.SelectedItem.ToString());
                strUrl = uri.OriginalString;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Url不合法，请检查输入项。\r\n{0}", ex.Message);
            }
            return strUrl;
        }

        private void tv_Method_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = 1;//指向展开的图标
        }

        private void tv_Method_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 0;
            e.Node.SelectedImageIndex = 0;//指向关闭的图标
        }

        private void tv_Method_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //e.Node.ImageIndex = 2;
            //e.Node.SelectedImageIndex = 2;//指向展开的图标
        }

        private void tv_Method_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                if (Enum.IsDefined(typeof(JsonMethodType), e.Node.Name))
                {
                    string temp = JsonObjectCreator(e.Node.Name);
                    this.rtb_Data.Text = temp;
                }
                else
                {
                    this.rtb_Data.Text = string.Empty;
                }
            }
            else
            {
                this.rtb_Data.Text = string.Empty;
            }
        }

        private string JsonObjectCreator(string type)
        {
            string temp = string.Empty;
            try
            {
                JsonMethodType a = (JsonMethodType)Enum.Parse(typeof(JsonMethodType), type);
                JsonRequest jr = new JsonRequest();
                jr.method = type;
                switch (a)
                {
                    case JsonMethodType.Version:
                    case JsonMethodType.RunStatus:
                        break;
                    case JsonMethodType.UserLogSearch:
                        jr.begtime = "开始时间，格式：YYYY-MM-DD";
                        jr.endtime = "结束时间，格式：YYYY-MM-DD";
                        jr.user = "用户名或用户ID";
                        break;
                    case JsonMethodType.AddDevice:
                        jr.devicetype = "设备类型";
                        jr.name = "设备名";
                        jr.ip = "设备IP";
                        jr.port = "设备端口";
                        jr.user = "设备登录用户";
                        jr.pwd = "设备登录密码";
                        jr.pin = "设备产品型号";
                        jr.supplier = "设备供应商";
                        jr.in_channel = "报警主机输入端口数量";
                        jr.out_channel = "报警主机输出端口数量";
                        jr.in_out = "门禁读卡器门内门外标记";
                        jr.chipin_count = "电源时序器插口数量";
                        jr.channel_count = "同录或CVR或智能分析仪通道数量";
                        break;
                    case JsonMethodType.AddCamera:
                        jr.devicetype = "设备类型";
                        jr.devicename = "设备名";
                        jr.bindid = "绑定设备ID";
                        jr.groupid = "上级组ID";
                        jr.loginid = "设备登录ID";
                        jr.loginpwd = "设备登录密码";
                        jr.ip = "设备IP";
                        jr.port = "设备端口";
                        jr.mainrtsp = "主流地址";
                        jr.auxrtsp = "辅流地址";
                        jr.flag = "使用主辅流标记";
                        jr.devicestate = "设备状态";
                        jr.note = "备注";
                        jr.mic = "拾音器";
                        jr.radio = "扬声器";
                        break;
                    default:
                        break;
                }
                temp = ConvertJsonString(JsonConvert.SerializeObject(jr, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
                return temp;
            }
            catch (System.Exception ex)
            {
                return string.Format("An error occurred ", ex.Message);
            }
        }

        private string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }
    }
}
