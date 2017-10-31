using System;
using System.Windows.Forms;

namespace JsonTestServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void menu_TestSystem_Click(object sender, EventArgs e)
        {
            pl_MainForm.Controls.Clear();
            FrmTestSystem fts = new FrmTestSystem();
            fts.FormBorderStyle = FormBorderStyle.None;
            fts.Dock = System.Windows.Forms.DockStyle.Fill;
            fts.TopLevel = false;
            pl_MainForm.Controls.Add(fts);
            fts.Show();
        }

        private void menu_PerformantTest_Click(object sender, EventArgs e)
        {
            pl_MainForm.Controls.Clear();
            FrmPerformentTest fpts = new FrmPerformentTest();
            fpts.FormBorderStyle = FormBorderStyle.None;
            fpts.Dock = System.Windows.Forms.DockStyle.Fill;
            fpts.TopLevel = false;
            pl_MainForm.Controls.Add(fpts);
            fpts.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.pl_MainForm.Controls.Clear();
                FrmTestSystem fts = new FrmTestSystem();
                fts.FormBorderStyle = FormBorderStyle.None;
                fts.Dock = System.Windows.Forms.DockStyle.Fill;
                fts.TopLevel = false;
                this.pl_MainForm.Controls.Add(fts);
                fts.Show();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Can't Open Form Page.");
            }
        }

        private void menu_CloseMainFrom_Click(object sender, EventArgs e)
        {
            //彻底退出进程
            System.Environment.Exit(0);
        }

        private void menu_About_Click(object sender, EventArgs e)
        {
            try
            {
                this.pl_MainForm.Controls.Clear();
                InfoForm infoForm = new InfoForm();
                infoForm.FormBorderStyle = FormBorderStyle.None;
                infoForm.Dock = System.Windows.Forms.DockStyle.Fill;
                infoForm.TopLevel = false;
                this.pl_MainForm.Controls.Add(infoForm);
                infoForm.Show();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Can't Open About Infomation Page.");
            }
        }

    }
}
