namespace JsonTestServer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.menu_Operation = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_TestSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_PerformantTest = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_CloseMainFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_help = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_About = new System.Windows.Forms.ToolStripMenuItem();
            this.pl_MainForm = new System.Windows.Forms.Panel();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_Operation,
            this.menu_help});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(798, 25);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // menu_Operation
            // 
            this.menu_Operation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_TestSystem,
            this.menu_PerformantTest,
            this.menu_CloseMainFrom});
            this.menu_Operation.Image = global::JsonTestServer.Properties.Resources.control_16xMD;
            this.menu_Operation.Name = "menu_Operation";
            this.menu_Operation.Size = new System.Drawing.Size(60, 21);
            this.menu_Operation.Text = "操作";
            // 
            // menu_TestSystem
            // 
            this.menu_TestSystem.Image = global::JsonTestServer.Properties.Resources.test_32x_LG;
            this.menu_TestSystem.Name = "menu_TestSystem";
            this.menu_TestSystem.Size = new System.Drawing.Size(192, 22);
            this.menu_TestSystem.Text = "单模块访问测试";
            this.menu_TestSystem.Click += new System.EventHandler(this.menu_TestSystem_Click);
            // 
            // menu_PerformantTest
            // 
            this.menu_PerformantTest.Image = global::JsonTestServer.Properties.Resources.Accessibility_2336;
            this.menu_PerformantTest.Name = "menu_PerformantTest";
            this.menu_PerformantTest.Size = new System.Drawing.Size(192, 22);
            this.menu_PerformantTest.Text = "模块批量测试(待实现)";
            this.menu_PerformantTest.Click += new System.EventHandler(this.menu_PerformantTest_Click);
            // 
            // menu_CloseMainFrom
            // 
            this.menu_CloseMainFrom.Image = global::JsonTestServer.Properties.Resources.Close_16xMD;
            this.menu_CloseMainFrom.Name = "menu_CloseMainFrom";
            this.menu_CloseMainFrom.Size = new System.Drawing.Size(192, 22);
            this.menu_CloseMainFrom.Text = "关闭";
            this.menu_CloseMainFrom.Click += new System.EventHandler(this.menu_CloseMainFrom_Click);
            // 
            // menu_help
            // 
            this.menu_help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_About});
            this.menu_help.Image = global::JsonTestServer.Properties.Resources.Information_Help__7833;
            this.menu_help.Name = "menu_help";
            this.menu_help.Size = new System.Drawing.Size(60, 21);
            this.menu_help.Text = "帮助";
            // 
            // menu_About
            // 
            this.menu_About.Image = global::JsonTestServer.Properties.Resources.text_16xMD;
            this.menu_About.Name = "menu_About";
            this.menu_About.Size = new System.Drawing.Size(213, 22);
            this.menu_About.Text = "关于辽源JW项目测试系统";
            this.menu_About.Click += new System.EventHandler(this.menu_About_Click);
            // 
            // pl_MainForm
            // 
            this.pl_MainForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pl_MainForm.Location = new System.Drawing.Point(0, 25);
            this.pl_MainForm.Name = "pl_MainForm";
            this.pl_MainForm.Size = new System.Drawing.Size(798, 617);
            this.pl_MainForm.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 642);
            this.Controls.Add(this.pl_MainForm);
            this.Controls.Add(this.menuStrip2);
            this.Name = "Form1";
            this.Text = "辽源JW项目测试系统";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem menu_Operation;
        private System.Windows.Forms.ToolStripMenuItem menu_TestSystem;
        private System.Windows.Forms.ToolStripMenuItem menu_help;
        private System.Windows.Forms.ToolStripMenuItem menu_About;
        private System.Windows.Forms.ToolStripMenuItem menu_PerformantTest;
        private System.Windows.Forms.ToolStripMenuItem menu_CloseMainFrom;
        private System.Windows.Forms.Panel pl_MainForm;
    }
}