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
            this.menu_SaveTreeNodes = new System.Windows.Forms.ToolStripMenuItem();
            this.模块批量测试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.menuStrip2.Size = new System.Drawing.Size(828, 25);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // menu_Operation
            // 
            this.menu_Operation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_SaveTreeNodes,
            this.模块批量测试ToolStripMenuItem,
            this.menu_CloseMainFrom});
            this.menu_Operation.Name = "menu_Operation";
            this.menu_Operation.Size = new System.Drawing.Size(44, 21);
            this.menu_Operation.Text = "操作";
            // 
            // menu_SaveTreeNodes
            // 
            this.menu_SaveTreeNodes.Name = "menu_SaveTreeNodes";
            this.menu_SaveTreeNodes.Size = new System.Drawing.Size(192, 22);
            this.menu_SaveTreeNodes.Text = "单模块访问测试";
            this.menu_SaveTreeNodes.Click += new System.EventHandler(this.menu_SaveTreeNodes_Click);
            // 
            // 模块批量测试ToolStripMenuItem
            // 
            this.模块批量测试ToolStripMenuItem.Name = "模块批量测试ToolStripMenuItem";
            this.模块批量测试ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.模块批量测试ToolStripMenuItem.Text = "模块批量测试(待实现)";
            // 
            // menu_CloseMainFrom
            // 
            this.menu_CloseMainFrom.Name = "menu_CloseMainFrom";
            this.menu_CloseMainFrom.Size = new System.Drawing.Size(192, 22);
            this.menu_CloseMainFrom.Text = "关闭";
            this.menu_CloseMainFrom.Click += new System.EventHandler(this.menu_CloseMainFrom_Click);
            // 
            // menu_help
            // 
            this.menu_help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_About});
            this.menu_help.Name = "menu_help";
            this.menu_help.Size = new System.Drawing.Size(44, 21);
            this.menu_help.Text = "帮助";
            // 
            // menu_About
            // 
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
            this.pl_MainForm.Size = new System.Drawing.Size(828, 617);
            this.pl_MainForm.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 642);
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
        private System.Windows.Forms.ToolStripMenuItem menu_SaveTreeNodes;
        private System.Windows.Forms.ToolStripMenuItem menu_help;
        private System.Windows.Forms.ToolStripMenuItem menu_About;
        private System.Windows.Forms.ToolStripMenuItem 模块批量测试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menu_CloseMainFrom;
        private System.Windows.Forms.Panel pl_MainForm;
    }
}