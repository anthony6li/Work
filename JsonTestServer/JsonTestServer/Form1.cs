using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonTestServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void menu_SaveTreeNodes_Click(object sender, EventArgs e)
        {
            pl_MainForm.Controls.Clear();
            FrmTestSystem fts = new FrmTestSystem();
            fts.FormBorderStyle = FormBorderStyle.None;
            fts.Dock = System.Windows.Forms.DockStyle.Fill;
            fts.TopLevel = false;
            pl_MainForm.Controls.Add(fts);
            fts.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pl_MainForm.Controls.Clear();
            FrmTestSystem fts = new FrmTestSystem();
            fts.FormBorderStyle = FormBorderStyle.None;
            fts.Dock = System.Windows.Forms.DockStyle.Fill;
            fts.TopLevel = false;
            pl_MainForm.Controls.Add(fts);
            fts.Show();
        }

        private void menu_CloseMainFrom_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void menu_About_Click(object sender, EventArgs e)
        {
            pl_MainForm.Controls.Clear();
            InfoForm infoForm = new InfoForm();
            infoForm.FormBorderStyle = FormBorderStyle.None;
            infoForm.Dock = System.Windows.Forms.DockStyle.Fill;
            infoForm.TopLevel = false;
            pl_MainForm.Controls.Add(infoForm);
            infoForm.Show();
        }
    }
}
