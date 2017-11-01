using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;
using System.IO;
using Newtonsoft.Json;

namespace NetClient
{

    public partial class MainForm : Form
    {
        HttpUtil htmlutil = new HttpUtil();

        public MainForm()
        {
            InitializeComponent();
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.comboBox_url.Items.Add("http://127.0.0.1:9905/api/hedacmdreq");
            this.comboBox_url.Items.Add("http://127.0.0.1:9906/api/hedacmdreq");
            this.comboBox_url.Text = "http://127.0.0.1:9905/api/hedacmdreq";
        }

        private void button_post_Click(object sender, EventArgs e)
        {
            string strUrl = this.comboBox_url.Text.Trim();
            string postdata = this.richTextBox_data.Text.Trim();

            this.richTextBox_ack.Text = htmlutil.HttpPost(strUrl, postdata);
        }

        private void button_postutf8_Click(object sender, EventArgs e)
        {
            string strUrl = this.comboBox_url.Text.Trim();
            string postdata = this.richTextBox_data.Text.Trim();

            this.richTextBox_ack.Text = htmlutil.HttpPostUTF8(strUrl, postdata);
        }

        private void button_get_Click(object sender, EventArgs e)
        {
            //string getdata = string.Format("wkds_id={0}&file_size={1}&thumb_id={2}", "1", "1024", "123456789");
            string strUrl = this.comboBox_url.Text.Trim();
            string getdata = this.richTextBox_data.Text.Trim();

            this.richTextBox_ack.Text = htmlutil.HttpGet(strUrl, getdata);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strUrl = "http://114.215.236.123:17922/router/log/download?callback=jQuery11020283594865984004_1508287346059&logs%5B0%5D%5Buserid%5D=22718969&logs%5B0%5D%5Bsoftid%5D=6749647&_=1508287346063";

            string temp = htmlutil.HttpGet(strUrl);
            this.richTextBox_ack.Text = string.IsNullOrEmpty(temp) ? "啥也没反馈啊！" : temp;

        }
    }

    
}
