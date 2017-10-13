using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ChatRoom
{
    public partial class frmClient : Form
    {
        private TcpClient Client;
        private NetworkStream net;

        public frmClient()
        {
            InitializeComponent();
            timer1.Tick += new EventHandler(this.timer1_Tick);
            timer1.Enabled = true;
            timer1.Interval = 200;
            timer1.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Client = new TcpClient();
                Client.Connect(Dns.GetHostAddresses(textBox2.Text)[0], 8000);
                this.Text = "服务器连接成功";
                Thread th = new Thread(new ThreadStart(JieShou));
                th.Start();
                timer1.Enabled = true;
            }
            catch (SocketException)
            {
                Client.Close();
                Client = null;
            }

        }
        private void JieShou()
        {
            try
            {
                while (Client != null)
                {
                    net = Client.GetStream();
                    if (net.CanWrite == false) { Client = null; return; }
                    if (net.DataAvailable == true)
                    {
                        byte[] tmp = new byte[1024];
                        MemoryStream memory = new MemoryStream();
                        int len = 1;
                        while (len != 0)
                        {
                            if (net.DataAvailable == false) { break; }
                            len = net.Read(tmp, 0, tmp.Length);
                            memory.Write(tmp, 0, len);
                        }
                        if (memory.ToArray().Length != 4)
                        {
                            NetMsg msg = (NetMsg)XuLie.ByteToObj(memory.ToArray());
                            textBox1.Text += msg.Fip.ToString() + "说： " + msg.msg + "\r\n";
                        }
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                lock (textBox1)
                {
                    //textBox1.Text = err.Message;
                }
            }
        }
        private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (net.CanWrite == true)
            {
                NetMsg msg = new NetMsg();
                msg.msg = "QUIT";
                byte[] tmp = XuLie.ObjToByte(msg);
                try
                {
                    net.Write(tmp, 0, tmp.Length);
                }
                catch (IOException)
                {
                    textBox1.Text += "已经从服务器断开连接\r\n";
                    Client.Close();
                    Client = null;
                    return;
                }
            }
            Client = null;
            GC.Collect();
            Application.ExitThread();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Client != null)
                {
                    if (net != null)
                    {
                        NetMsg msg = new NetMsg();
                        msg.Fip = Dns.GetHostAddresses(Dns.GetHostName())[0];
                        msg.JieIP = Dns.GetHostAddresses(textBox3.Text)[0];
                        msg.msg = textBox4.Text;
                        byte[] tmp = XuLie.ObjToByte(msg);
                        net.Write(tmp, 0, tmp.Length);
                    }
                }
                else
                {
                    textBox1.Text += "未与服务器建立连接\r\n";
                }
            }
            catch (Exception)
            {
                textBox1.Text += "未与服务器建立连接\r\n";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Client != null)
                {
                    if (net.CanWrite == true)
                    {
                        NetMsg msg = new NetMsg();
                        msg.msg = "0000";
                        byte[] tmp = XuLie.ObjToByte(msg);
                        try
                        {
                            net.Write(tmp, 0, tmp.Length);
                        }
                        catch (IOException)
                        {
                            textBox1.Text += "已经从服务器断开连接\r\n";
                            Client.Close();
                            Client = null;
                            return;
                        }

                    }
                }
                else
                {
                    textBox1.Text += "未与服务器建立连接\r\n";
                }
            }
            catch (Exception err)
            {
                textBox1.Text += err.Message + "r\n";
            }
        }
    }
}
