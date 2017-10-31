using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Util;

namespace JsonTestServer
{
    public partial class FrmPerformentTest : Form
    {
        HttpUtil htmlUtil = new HttpUtil();
        XmlDocument doc = new XmlDocument();
        public enum requestStyle
        {
            detail,
            sample
        };
        public enum ackStyle
        {
            expected,
            actual,
        };

        public requestStyle reqStyle = requestStyle.detail;

        public FrmPerformentTest()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.tv_Method.ExpandAll();
                doc.Load("Resource\\PerformanceTreeXml.xml");
                //doc.Load(Properties.Resources.TreeXml); 
                RecursionTreeControl(doc.DocumentElement, tv_Method.Nodes);//将加载完成的XML文件显示在TreeView控件
                tv_Method.ExpandAll();
                if (tv_Method.Nodes[0] != null)
                {
                    tv_Method.TopNode = tv_Method.Nodes[0];
                }
            }
            catch (Exception ex)
            {
                this.rtb_Data.Text = ex.ToString();
            }
        }

        private void btn_POST_Click(object sender, EventArgs e)
        {
            string strUrl = GetUrlString();
            string postdata = this.rtb_Data.Text;
            try
            {
                this.rtb_ACK.Text = htmlUtil.HttpPost(strUrl, postdata);
            }
            catch (System.Exception ex)
            {
                updateDateToRTB(ex.Message);
            }
        }

        private void btn_POST8_Click(object sender, EventArgs e)
        {
            string strUrl = GetUrlString();
            string postdata = this.rtb_Data.Text;
            try
            {
                this.rtb_ACK.Text = htmlUtil.HttpPostUTF8(strUrl, postdata);
            }
            catch (System.Exception ex)
            {
                updateDateToRTB(ex.Message);
            }
        }

        private void btn_GET_Click(object sender, EventArgs e)
        {
            string strUrl = GetUrlString();
            string postdata = this.rtb_Data.Text;
            try
            {
                this.rtb_ACK.Text = htmlUtil.HttpGet(strUrl, postdata);
            }
            catch (System.Exception ex)
            {
                updateDateToRTB(ex.Message);
            }
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

        private void tv_Method_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                updateDateToRTB(e.Node.Name);
            }
            else
            {
                this.rtb_Data.Text = string.Format("请选择【{0}】的子节点获取请求Json模板。", e.Node.Text);
            }
        }

        private void Tb_IP_MouseEnter(object sender, EventArgs e)
        {
            ShowToolTipMouseEnter(sender, "测试服务器的IP地址");
        }

        /// <summary>
        /// 为控件显示ToolsTips
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="str">ToolTips显示的内容</param>
        private void ShowToolTipMouseEnter(object sender, string str)
        {
            ToolTip tt = new ToolTip();
            tt.ShowAlways = true;
            tt.SetToolTip((Control)sender, str);
        }

        private void updateDateToRTB(string type)
        {
            if (!string.IsNullOrEmpty(type))
            {
                if (Enum.IsDefined(typeof(JsonMethodType), type))
                {
                    string temp = string.Empty;
                    //暂时支持Request为详情和用例两种样式
                    switch (reqStyle)
                    {
                        case requestStyle.detail:
                            temp = htmlUtil.JsonDetailStringCreator(type);
                            break;
                        case requestStyle.sample:
                            temp = htmlUtil.JsonSampleStringCreator(type);
                            break;
                        default:
                            break;
                    }
                    if ((temp.StartsWith("http://") && temp.EndsWith(JsonRequestType.upload.ToString())))
                    {
                        this.rtb_Data.Text = "请访问：\r\n";
                        this.rtb_Data.Text += string.Format(temp, this.Tb_IP.Text, this.Tb_Port.Text);
                    }
                    else
                    {
                        this.rtb_Data.Text = temp;
                    }
                }
                else
                {
                    this.rtb_ACK.Text = type;
                }
            }
        }

        private void updateLogToRTB(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                string temp = message;
                this.rtb_ACK.Text = temp;
            }
        }

        /// <summary>
        /// RecursionTreeControl:表示将XML文件的内容显示在TreeView控件中
        /// </summary>
        /// <param name="xmlNode">将要加载的XML文件中的节点元素</param>
        /// <param name="nodes">将要加载的XML文件中的节点集合</param>
        private void RecursionTreeControl(XmlNode xmlNode, TreeNodeCollection nodes)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)//循环遍历当前元素的子元素集合
            {
                TreeNode new_child = new TreeNode();//定义一个TreeNode节点对象
                new_child.Name = node.Attributes["Name"].Value;
                new_child.Text = node.Attributes["Text"].Value;
                nodes.Add(new_child);//向当前TreeNodeCollection集合中添加当前节点
                RecursionTreeControl(node, new_child.Nodes);//调用本方法进行递归
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tv_Method_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void btn_LogPathChoose_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.SelectedPath = this.tb_LogPath.Text;
            path.ShowDialog();
            this.tb_LogPath.Text = path.SelectedPath;
        }
    }
}
