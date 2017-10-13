using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Net.Sockets;
using System.Net;

namespace ChatRoom
{
    public partial class frmServer : Form
    {
        public frmServer()
        {
            InitializeComponent();
            Form client = new frmClient();
            client.Show();
        }
        private ArrayList arr;
        private TcpListener Server1;
        private TcpClient col;
        private ArrayList LianJIe;

        private void frmServer_Load(object sender, EventArgs e)
        {
            arr = new ArrayList();
            LianJIe = new ArrayList();
            Server1 = new TcpListener(IPAddress.Any, 8000);
            Server1.Start();
            timer1.Interval = 100;
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {

                if (Server1.Pending() == true)
                {
                    col = Server1.AcceptTcpClient();
                    arr.Add(col);
                    ServerJieShou server = new ServerJieShou(col, arr);
                    
                    LianJIe.Add(server);
                }

                if (arr.Count == 0) { return; }
                listBox1.Items.Clear();
                foreach (TcpClient Col in arr)
                {
                    IPEndPoint ip = (IPEndPoint)Col.Client.RemoteEndPoint;
                    listBox1.Items.Add(ip.ToString());
                }
                foreach (var temp in LianJIe)
                {
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                // Application.Exit();  
            }
        }

        private void frmServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                foreach (ServerJieShou Col in LianJIe)
                {
                    Col.th.Abort();
                    Col.th.Join();
                }
                foreach (TcpClient Col in arr)
                {

                    Col.Close();
                }
            }
            finally
            {
                Application.Exit();
            }
        }

    }
}

