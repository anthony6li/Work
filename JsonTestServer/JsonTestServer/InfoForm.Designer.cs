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
            this.rtb_DeviceInfo = new System.Windows.Forms.RichTextBox();
            this.tc_Info = new System.Windows.Forms.TabControl();
            this.tab_DeviceType = new System.Windows.Forms.TabPage();
            this.tab_Supplier = new System.Windows.Forms.TabPage();
            this.tab_AreaType = new System.Windows.Forms.TabPage();
            this.tab_PTZ = new System.Windows.Forms.TabPage();
            this.tab_Sequencer = new System.Windows.Forms.TabPage();
            this.rtb_Supplier = new System.Windows.Forms.RichTextBox();
            this.rtb_AreaType = new System.Windows.Forms.RichTextBox();
            this.rtb_PTZ = new System.Windows.Forms.RichTextBox();
            this.rtb_Sequencer = new System.Windows.Forms.RichTextBox();
            this.tc_Info.SuspendLayout();
            this.tab_DeviceType.SuspendLayout();
            this.tab_Supplier.SuspendLayout();
            this.tab_AreaType.SuspendLayout();
            this.tab_PTZ.SuspendLayout();
            this.tab_Sequencer.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_CloseInfoFrm
            // 
            this.btn_CloseInfoFrm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_CloseInfoFrm.Location = new System.Drawing.Point(251, 580);
            this.btn_CloseInfoFrm.Name = "btn_CloseInfoFrm";
            this.btn_CloseInfoFrm.Size = new System.Drawing.Size(137, 23);
            this.btn_CloseInfoFrm.TabIndex = 0;
            this.btn_CloseInfoFrm.Text = "返回";
            this.btn_CloseInfoFrm.UseVisualStyleBackColor = true;
            this.btn_CloseInfoFrm.Click += new System.EventHandler(this.btn_CloseInfoFrm_Click);
            // 
            // rtb_DeviceInfo
            // 
            this.rtb_DeviceInfo.BackColor = System.Drawing.SystemColors.Control;
            this.rtb_DeviceInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_DeviceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_DeviceInfo.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtb_DeviceInfo.Location = new System.Drawing.Point(3, 3);
            this.rtb_DeviceInfo.Name = "rtb_DeviceInfo";
            this.rtb_DeviceInfo.Size = new System.Drawing.Size(639, 540);
            this.rtb_DeviceInfo.TabIndex = 1;
            this.rtb_DeviceInfo.Text = "1.3.1.1摄像头/麦克风\n1-带云台的摄像头\n2-不带云台的摄像头\n3-麦克风\n4-云台\n5-其他摄像头或麦克风\n1.3.1.2对接设备\n50-报警主机\n51" +
    "-行为分析服务器(智能服务器)\n52-中央存储器(CVR)\n53-解码器\n54-门禁主机\n55-网络键盘\n56-电源时序器\n57-同录设备";
            // 
            // tc_Info
            // 
            this.tc_Info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tc_Info.Controls.Add(this.tab_DeviceType);
            this.tc_Info.Controls.Add(this.tab_AreaType);
            this.tc_Info.Controls.Add(this.tab_Supplier);
            this.tc_Info.Controls.Add(this.tab_PTZ);
            this.tc_Info.Controls.Add(this.tab_Sequencer);
            this.tc_Info.Location = new System.Drawing.Point(13, 2);
            this.tc_Info.Name = "tc_Info";
            this.tc_Info.SelectedIndex = 0;
            this.tc_Info.Size = new System.Drawing.Size(653, 572);
            this.tc_Info.TabIndex = 2;
            // 
            // tab_DeviceType
            // 
            this.tab_DeviceType.Controls.Add(this.rtb_DeviceInfo);
            this.tab_DeviceType.Location = new System.Drawing.Point(4, 22);
            this.tab_DeviceType.Name = "tab_DeviceType";
            this.tab_DeviceType.Padding = new System.Windows.Forms.Padding(3);
            this.tab_DeviceType.Size = new System.Drawing.Size(645, 546);
            this.tab_DeviceType.TabIndex = 0;
            this.tab_DeviceType.Text = "摄像头麦克风";
            this.tab_DeviceType.UseVisualStyleBackColor = true;
            // 
            // tab_Supplier
            // 
            this.tab_Supplier.Controls.Add(this.rtb_Supplier);
            this.tab_Supplier.Location = new System.Drawing.Point(4, 22);
            this.tab_Supplier.Name = "tab_Supplier";
            this.tab_Supplier.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Supplier.Size = new System.Drawing.Size(645, 546);
            this.tab_Supplier.TabIndex = 1;
            this.tab_Supplier.Text = "供应商";
            this.tab_Supplier.UseVisualStyleBackColor = true;
            // 
            // tab_AreaType
            // 
            this.tab_AreaType.Controls.Add(this.rtb_AreaType);
            this.tab_AreaType.Location = new System.Drawing.Point(4, 22);
            this.tab_AreaType.Name = "tab_AreaType";
            this.tab_AreaType.Size = new System.Drawing.Size(645, 546);
            this.tab_AreaType.TabIndex = 2;
            this.tab_AreaType.Text = "房间类型";
            this.tab_AreaType.UseVisualStyleBackColor = true;
            // 
            // tab_PTZ
            // 
            this.tab_PTZ.Controls.Add(this.rtb_PTZ);
            this.tab_PTZ.Location = new System.Drawing.Point(4, 22);
            this.tab_PTZ.Name = "tab_PTZ";
            this.tab_PTZ.Size = new System.Drawing.Size(645, 546);
            this.tab_PTZ.TabIndex = 3;
            this.tab_PTZ.Text = "云台控制命令";
            this.tab_PTZ.UseVisualStyleBackColor = true;
            // 
            // tab_Sequencer
            // 
            this.tab_Sequencer.Controls.Add(this.rtb_Sequencer);
            this.tab_Sequencer.Location = new System.Drawing.Point(4, 22);
            this.tab_Sequencer.Name = "tab_Sequencer";
            this.tab_Sequencer.Size = new System.Drawing.Size(645, 546);
            this.tab_Sequencer.TabIndex = 4;
            this.tab_Sequencer.Text = "时序器业务模块类型";
            this.tab_Sequencer.UseVisualStyleBackColor = true;
            // 
            // rtb_Supplier
            // 
            this.rtb_Supplier.BackColor = System.Drawing.SystemColors.Control;
            this.rtb_Supplier.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_Supplier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Supplier.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtb_Supplier.Location = new System.Drawing.Point(3, 3);
            this.rtb_Supplier.Name = "rtb_Supplier";
            this.rtb_Supplier.Size = new System.Drawing.Size(639, 540);
            this.rtb_Supplier.TabIndex = 2;
            this.rtb_Supplier.Text = "0-无厂家\n1-海康威视\n2-大华\n20-傲视恒安";
            // 
            // rtb_AreaType
            // 
            this.rtb_AreaType.BackColor = System.Drawing.SystemColors.Control;
            this.rtb_AreaType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_AreaType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_AreaType.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtb_AreaType.Location = new System.Drawing.Point(0, 0);
            this.rtb_AreaType.Name = "rtb_AreaType";
            this.rtb_AreaType.Size = new System.Drawing.Size(645, 546);
            this.rtb_AreaType.TabIndex = 2;
            this.rtb_AreaType.Text = "1-机房\n2-双规室\n3-谈话室\n4-走廊\n5-指挥室";
            // 
            // rtb_PTZ
            // 
            this.rtb_PTZ.BackColor = System.Drawing.SystemColors.Control;
            this.rtb_PTZ.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_PTZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_PTZ.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtb_PTZ.Location = new System.Drawing.Point(0, 0);
            this.rtb_PTZ.Name = "rtb_PTZ";
            this.rtb_PTZ.Size = new System.Drawing.Size(645, 546);
            this.rtb_PTZ.TabIndex = 2;
            this.rtb_PTZ.Text = "11-焦距变大（倍率放大）\n12-焦距变小（倍率放小）\n13-焦点前调\n14-焦点后调\n15-光圈放大\n16-光圈缩小\n\n21-上\n22-下\n23-左\n24-右\n" +
    "25-左上\n26-右上\n27-左下\n28-右下";
            // 
            // rtb_Sequencer
            // 
            this.rtb_Sequencer.BackColor = System.Drawing.SystemColors.Control;
            this.rtb_Sequencer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_Sequencer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Sequencer.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtb_Sequencer.Location = new System.Drawing.Point(0, 0);
            this.rtb_Sequencer.Name = "rtb_Sequencer";
            this.rtb_Sequencer.Size = new System.Drawing.Size(645, 546);
            this.rtb_Sequencer.TabIndex = 2;
            this.rtb_Sequencer.Text = "1-推门录";
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 615);
            this.Controls.Add(this.tc_Info);
            this.Controls.Add(this.btn_CloseInfoFrm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InfoForm";
            this.Text = "InfoForm";
            this.Load += new System.EventHandler(this.InfoForm_Load);
            this.tc_Info.ResumeLayout(false);
            this.tab_DeviceType.ResumeLayout(false);
            this.tab_Supplier.ResumeLayout(false);
            this.tab_AreaType.ResumeLayout(false);
            this.tab_PTZ.ResumeLayout(false);
            this.tab_Sequencer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_CloseInfoFrm;
        private System.Windows.Forms.RichTextBox rtb_DeviceInfo;
        private System.Windows.Forms.TabControl tc_Info;
        private System.Windows.Forms.TabPage tab_DeviceType;
        private System.Windows.Forms.TabPage tab_Supplier;
        private System.Windows.Forms.TabPage tab_AreaType;
        private System.Windows.Forms.TabPage tab_PTZ;
        private System.Windows.Forms.TabPage tab_Sequencer;
        private System.Windows.Forms.RichTextBox rtb_Supplier;
        private System.Windows.Forms.RichTextBox rtb_AreaType;
        private System.Windows.Forms.RichTextBox rtb_PTZ;
        private System.Windows.Forms.RichTextBox rtb_Sequencer;
    }
}