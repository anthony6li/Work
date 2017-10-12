using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceProcess;

namespace WindowsServiceTestARUI
{
    public partial class HandleService : Form
    {
        public HandleService()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string svcPath = System.Environment.CurrentDirectory + "\\Services\\WindowsServiceTest.exe";
            ServiceInstall.InstallService(svcPath, true);
            typeTextToRichTextBox("安装服务成功.\r\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string svcPath = System.Environment.CurrentDirectory + "\\Services\\WindowsServiceTest.exe";
            ServiceInstall.UninstallService(svcPath, true);
            typeTextToRichTextBox("卸载服务成功.\r\n");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ServiceController serviceController = new ServiceController("ServiceTestAR");
            serviceController.Start();
            typeTextToRichTextBox("服务启动成功.\r\n");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ServiceController serviceController = new ServiceController("ServiceTestAR");
            serviceController.Stop();
            typeTextToRichTextBox("服务停止成功.\r\n");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ServiceController serviceController = new ServiceController("ServiceTestAR");
            if (serviceController.CanPauseAndContinue)
            {
                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    serviceController.Pause();
                    typeTextToRichTextBox("服务暂停成功.\r\n");
                }
                else if (serviceController.Status == ServiceControllerStatus.Paused)
                {
                    serviceController.Continue();
                    typeTextToRichTextBox("服务继续成功.\r\n");
                }
            }
            else
            {
                typeTextToRichTextBox("服务不支持暂停继续操作.\r\n");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ServiceController serviceController = new ServiceController("ServiceTestAR");
            string status = string.Empty;
            try
            {
                status = serviceController.Status.ToString();
            }
            catch (System.Exception ex)
            {
                typeTextToRichTextBox(ex.Message);
            }
            typeTextToRichTextBox("服务的状态为:"+status+ "\r\n");
        }

        private void typeTextToRichTextBox(string str)
        {
            this.richTextBox1.Text += str;
        }
    }
}
