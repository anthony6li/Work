namespace AudioClient
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.ddlAudioOut = new System.Windows.Forms.ComboBox();
            this.recordButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.leftTrackBar = new System.Windows.Forms.TrackBar();
            this.muteButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(67, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(277, 21);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "http://10.10.1.77:8092/?micid=1";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(348, 10);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(52, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "连接";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // panel
            // 
            this.panel.Location = new System.Drawing.Point(67, 79);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(277, 50);
            this.panel.TabIndex = 6;
            // 
            // ddlAudioOut
            // 
            this.ddlAudioOut.FormattingEnabled = true;
            this.ddlAudioOut.Location = new System.Drawing.Point(67, 43);
            this.ddlAudioOut.Name = "ddlAudioOut";
            this.ddlAudioOut.Size = new System.Drawing.Size(277, 20);
            this.ddlAudioOut.TabIndex = 7;
            // 
            // recordButton
            // 
            this.recordButton.Location = new System.Drawing.Point(464, 10);
            this.recordButton.Name = "recordButton";
            this.recordButton.Size = new System.Drawing.Size(51, 23);
            this.recordButton.TabIndex = 8;
            this.recordButton.Text = "录音";
            this.recordButton.UseVisualStyleBackColor = true;
            this.recordButton.Click += new System.EventHandler(this.recordButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "扬声器";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::AudioClient.Properties.Resources.speak;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel1.Location = new System.Drawing.Point(14, 79);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(46, 50);
            this.panel1.TabIndex = 9;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(350, 58);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(167, 20);
            this.progressBar1.TabIndex = 11;
            this.progressBar1.Value = 100;
            // 
            // leftTrackBar
            // 
            this.leftTrackBar.Location = new System.Drawing.Point(350, 84);
            this.leftTrackBar.Maximum = 100;
            this.leftTrackBar.Name = "leftTrackBar";
            this.leftTrackBar.Size = new System.Drawing.Size(167, 45);
            this.leftTrackBar.TabIndex = 12;
            this.leftTrackBar.Value = 100;
            this.leftTrackBar.Scroll += new System.EventHandler(this.leftTrackBar_Scroll);
            // 
            // muteButton
            // 
            this.muteButton.Location = new System.Drawing.Point(406, 10);
            this.muteButton.Name = "muteButton";
            this.muteButton.Size = new System.Drawing.Size(52, 23);
            this.muteButton.TabIndex = 14;
            this.muteButton.Text = "静音";
            this.muteButton.UseVisualStyleBackColor = true;
            this.muteButton.Click += new System.EventHandler(this.muteButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "源";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(350, 36);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 16);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "是否开机启动";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "哈哈哈哈，你看啥？";
            this.notifyIcon1.BalloonTipTitle = "噫？";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1 Test";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 133);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.muteButton);
            this.Controls.Add(this.leftTrackBar);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.recordButton);
            this.Controls.Add(this.ddlAudioOut);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "AudioClient";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.ComboBox ddlAudioOut;
        private System.Windows.Forms.Button recordButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.TrackBar leftTrackBar;
        private System.Windows.Forms.Button muteButton;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

