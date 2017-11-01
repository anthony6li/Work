namespace NetClient
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox_data = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_post = new System.Windows.Forms.Button();
            this.button_get = new System.Windows.Forms.Button();
            this.comboBox_url = new System.Windows.Forms.ComboBox();
            this.richTextBox_ack = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_postutf8 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL：";
            // 
            // richTextBox_data
            // 
            this.richTextBox_data.Location = new System.Drawing.Point(15, 70);
            this.richTextBox_data.Name = "richTextBox_data";
            this.richTextBox_data.Size = new System.Drawing.Size(630, 183);
            this.richTextBox_data.TabIndex = 2;
            this.richTextBox_data.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "数据：";
            // 
            // button_post
            // 
            this.button_post.Location = new System.Drawing.Point(390, 258);
            this.button_post.Name = "button_post";
            this.button_post.Size = new System.Drawing.Size(75, 23);
            this.button_post.TabIndex = 4;
            this.button_post.Text = "POST";
            this.button_post.UseVisualStyleBackColor = true;
            this.button_post.Click += new System.EventHandler(this.button_post_Click);
            // 
            // button_get
            // 
            this.button_get.Location = new System.Drawing.Point(570, 258);
            this.button_get.Name = "button_get";
            this.button_get.Size = new System.Drawing.Size(75, 23);
            this.button_get.TabIndex = 5;
            this.button_get.Text = "GET";
            this.button_get.UseVisualStyleBackColor = true;
            this.button_get.Click += new System.EventHandler(this.button_get_Click);
            // 
            // comboBox_url
            // 
            this.comboBox_url.FormattingEnabled = true;
            this.comboBox_url.Location = new System.Drawing.Point(54, 19);
            this.comboBox_url.Name = "comboBox_url";
            this.comboBox_url.Size = new System.Drawing.Size(591, 20);
            this.comboBox_url.TabIndex = 6;
            // 
            // richTextBox_ack
            // 
            this.richTextBox_ack.Location = new System.Drawing.Point(15, 285);
            this.richTextBox_ack.Name = "richTextBox_ack";
            this.richTextBox_ack.Size = new System.Drawing.Size(630, 183);
            this.richTextBox_ack.TabIndex = 7;
            this.richTextBox_ack.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 260);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "应答：";
            // 
            // button_postutf8
            // 
            this.button_postutf8.Location = new System.Drawing.Point(480, 258);
            this.button_postutf8.Name = "button_postutf8";
            this.button_postutf8.Size = new System.Drawing.Size(75, 23);
            this.button_postutf8.TabIndex = 10;
            this.button_postutf8.Text = "POST(UTF8)";
            this.button_postutf8.UseVisualStyleBackColor = true;
            this.button_postutf8.Click += new System.EventHandler(this.button_postutf8_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(680, 150);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 54);
            this.button1.TabIndex = 11;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 480);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_postutf8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.richTextBox_ack);
            this.Controls.Add(this.comboBox_url);
            this.Controls.Add(this.button_get);
            this.Controls.Add(this.button_post);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBox_data);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "网络请求测试";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox_data;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_post;
        private System.Windows.Forms.Button button_get;
        private System.Windows.Forms.ComboBox comboBox_url;
        private System.Windows.Forms.RichTextBox richTextBox_ack;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_postutf8;
        private System.Windows.Forms.Button button1;
    }
}

