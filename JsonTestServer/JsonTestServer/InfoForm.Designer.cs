namespace JsonTestServer
{
    partial class InfoForm
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
            this.btn_CloseInfoFrm = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btn_CloseInfoFrm
            // 
            this.btn_CloseInfoFrm.Location = new System.Drawing.Point(197, 12);
            this.btn_CloseInfoFrm.Name = "btn_CloseInfoFrm";
            this.btn_CloseInfoFrm.Size = new System.Drawing.Size(75, 23);
            this.btn_CloseInfoFrm.TabIndex = 0;
            this.btn_CloseInfoFrm.Text = "button1";
            this.btn_CloseInfoFrm.UseVisualStyleBackColor = true;
            this.btn_CloseInfoFrm.Click += new System.EventHandler(this.btn_CloseInfoFrm_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(23, 74);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(585, 432);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "1.3.1.1摄像头/麦克风\n1-带云台的摄像头\n2-不带云台的摄像头\n3-麦克风\n4-云台\n5-其他摄像头或麦克风\n1.3.1.2对接设备\n50-报警主机\n51" +
    "-行为分析服务器(智能服务器)\n52-中央存储器(CVR)\n53-解码器\n54-门禁主机\n55-网络键盘\n56-电源时序器\n57-同录设备";
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 568);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btn_CloseInfoFrm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InfoForm";
            this.Text = "InfoForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_CloseInfoFrm;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}