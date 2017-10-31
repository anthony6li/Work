using System;
using System.Net;
using System.Windows.Forms;
using Util;
using System.Text;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using System.Xml;
using System.Collections;

namespace JsonTestServer
{
    public partial class FrmTestSystem : Form
    {
        const string EXPAND = "Expand Tree";
        const string COLLAPSE = "Collapse Tree";
        HttpUtil htmlUtil = new HttpUtil();
        XmlDocument doc = new XmlDocument();
        StringBuilder sb = new StringBuilder();
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
        public ackStyle ackSty = ackStyle.actual;
        //XML每行的内容
        private string xmlLine = "";


        public FrmTestSystem()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            this.lb_Data.Text = string.Format("数据（{0}）：", rbt_Deteil.Text);
            this.tv_Method.ExpandAll();
            doc.Load("Resource\\TreeXml.xml");
            //doc.Load(Properties.Resources.TreeXml); 
            RecursionTreeControl(doc.DocumentElement, tv_Method.Nodes);//将加载完成的XML文件显示在TreeView控件
            tv_Method.ExpandAll();
            if (tv_Method.Nodes[0] != null)
            {
                tv_Method.TopNode = tv_Method.Nodes[0];
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


        private void btb_SaveNodesToXml_Click(object sender, EventArgs e)
        {
            try
            {
                string localFilePath = "";
                string fileNameExt = "";
                string newFileName = "";
                string FilePath = "";
                saveFileDialog1.Filter = "Xml files(*.xml)|*.xml|txt files(*.txt)|*.txt|All files(*.*)|*.*";
                saveFileDialog1.FileName = "TreeXml";
                saveFileDialog1.DefaultExt = "xml";
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.RestoreDirectory = true;
                DialogResult result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    localFilePath = saveFileDialog1.FileName.ToString();
                    fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);
                    FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));
                    StringBuilder sb = new StringBuilder();
                    //写文件头部内容
                    //下面是生成RSS的OPML文件
                    sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    sb.Append("<Tree>");
                    //遍历根节点
                    foreach (TreeNode node in tv_Method.Nodes)
                    {
                        xmlLine = GetRSSText(node);
                        sb.Append(xmlLine);
                        //递归遍历节点
                        parseNode(node, sb);
                        sb.Append("</Node>");
                    }
                    sb.Append("</Tree>");

                    Stream fs = saveFileDialog1.OpenFile();
                    StreamWriter sr = new StreamWriter(fs, System.Text.Encoding.UTF8);
                    sr.Write(sb.ToString());
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                this.rtb_ACK.Text = "Save Tree Failed.";
                this.rtb_ACK.Text += ex.ToString();
            }
        }

        /// <summary>
        /// 递归遍历节点内容,最关键的函数
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="sb"></param>
        private void parseNode(TreeNode tn, StringBuilder sb)
        {
            IEnumerator ie = tn.Nodes.GetEnumerator();

            while (ie.MoveNext())
            {
                TreeNode ctn = (TreeNode)ie.Current;
                xmlLine = GetRSSText(ctn);
                sb.Append(xmlLine);
                //如果还有子节点则继续遍历
                if (ctn.Nodes.Count > 0)
                {
                    parseNode(ctn, sb);
                }
                sb.Append("</Node>");
            }
        }

        /// <summary>
        /// 成生XML文本行
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetRSSText(TreeNode node)
        {
            //根据Node属性生成XML文本
            string tempText = "<Node Name=\"" + node.Name + "\" Text=\"" + node.Text + "\" >";

            return tempText;
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

        /// <summary>
        /// 选中TreeView节点,返回相应的请求数据并更新至RichTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Expand_Click(object sender, EventArgs e)
        {
            object locker = new object();
            lock (locker)
            {
                if (string.Equals(this.btn_Expand.Text, COLLAPSE))
                {
                    tv_Method.CollapseAll();
                    btn_Expand.Text = EXPAND;
                }
                else if (string.Equals(this.btn_Expand.Text, EXPAND))
                {
                    tv_Method.ExpandAll();
                    if (tv_Method.Nodes[0] != null)
                    {
                        tv_Method.TopNode = tv_Method.Nodes[0];
                        btn_Expand.Text = COLLAPSE;
                    }
                }
            }
        }

        /// <summary>
        /// 修改Json请求格式为Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbt_JsonDeteil_CheckedChanged(object sender, EventArgs e)
        {
            reqStyle = requestStyle.detail;
            if (this.tv_Method.SelectedNode != null)
            {
                updateDateToRTB(this.tv_Method.SelectedNode.Name);
            }
            lb_Data.Text = string.Format("数据（{0}）：", rbt_Deteil.Text);
        }

        /// <summary>
        /// 修改Json请求格式为Sample
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbt_JsonSample_CheckedChanged(object sender, EventArgs e)
        {
            reqStyle = requestStyle.sample;
            if (this.tv_Method.SelectedNode != null)
            {
                updateDateToRTB(this.tv_Method.SelectedNode.Name);
            }
            lb_Data.Text = string.Format("数据（{0}）：", rbt_JsonSample.Text);
        }

        string temp = string.Empty;
        private void rbtn_ActualACK_CheckedChanged(object sender, EventArgs e)
        {
            ackSty = ackStyle.actual;
            if (!string.IsNullOrEmpty(temp))
            {
                rtb_ACK.Text = temp;
            }
            else
            {
                rtb_ACK.Text = "请发起请求（POST/POST-UTF8/GET）";
            }
        }

        private void rbtn_ExpectedACK_CheckedChanged(object sender, EventArgs e)
        {
            ackSty = ackStyle.expected;
            if (!string.IsNullOrEmpty(this.rtb_ACK.Text))
            {
                temp = this.rtb_ACK.Text;
            }
            //提供期待结果
            updateACKToRTB();
        }

        /// <summary>
        /// 显示数据RichTextBox的文本
        /// </summary>
        /// <param name="type">JsonMethodType</param>
        private void updateDateToRTB(string type)
        {
            if (!string.IsNullOrEmpty(type))
            {
                //如果传递的是JsonMethodType，传递相应的JsonString至数据RichTextBox
                //如果不不是，则输出文本信息到应答RichTextBox
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

        /// <summary>
        /// 显示应答RichTextBox的文本
        /// </summary>
        /// <param name="type"></param>
        private void updateACKToRTB()
        {
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

        private void btb_SaveNodesToXml_MouseEnter(object sender, EventArgs e)
        {
            ShowToolTipMouseEnter(sender, "保存左侧目录树所有内容为Xml文件至自定义路径。");
        }

        private void btn_Expand_MouseEnter(object sender, EventArgs e)
        {
            ShowToolTipMouseEnter(sender, "收缩或展开左侧目录树。");
        }

        private void Tb_IP_MouseEnter(object sender, EventArgs e)
        {
            ShowToolTipMouseEnter(sender, "测试服务器的IP地址");
        }

        private void gb_RequestDetailOrSample_MouseHover(object sender, EventArgs e)
        {
            ShowToolTipMouseEnter(sender, "切换请求数据窗口内的的文本格式\r\n（请求的详细说明或者请求的实例样例）");
        }

        private void gb_ACK_Expected_MouseHover(object sender, EventArgs e)
        {
            ShowToolTipMouseEnter(sender, "切换返回的数据窗口内的文本格式\r\n（期待结果还是实际结果）");
        }
    }
}
