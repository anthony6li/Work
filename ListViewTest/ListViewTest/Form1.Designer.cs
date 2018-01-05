namespace ListViewTest
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonLargeIcon = new System.Windows.Forms.RadioButton();
            this.radioButtonSmallIcon = new System.Windows.Forms.RadioButton();
            this.radioButtonList = new System.Windows.Forms.RadioButton();
            this.radioButtonDetails = new System.Windows.Forms.RadioButton();
            this.radioButtonTile = new System.Windows.Forms.RadioButton();
            this.labelCurrentPath = new System.Windows.Forms.Label();
            this.listViewFilesAndFolders = new System.Windows.Forms.ListView();
            this.buttonBack = new System.Windows.Forms.Button();
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonTile);
            this.groupBox1.Controls.Add(this.radioButtonDetails);
            this.groupBox1.Controls.Add(this.radioButtonList);
            this.groupBox1.Controls.Add(this.radioButtonSmallIcon);
            this.groupBox1.Controls.Add(this.radioButtonLargeIcon);
            this.groupBox1.Location = new System.Drawing.Point(561, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 205);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View Mode";
            // 
            // radioButtonLargeIcon
            // 
            this.radioButtonLargeIcon.Location = new System.Drawing.Point(7, 21);
            this.radioButtonLargeIcon.Name = "radioButtonLargeIcon";
            this.radioButtonLargeIcon.Size = new System.Drawing.Size(175, 24);
            this.radioButtonLargeIcon.TabIndex = 0;
            this.radioButtonLargeIcon.Text = "Large Icon";
            this.radioButtonLargeIcon.UseVisualStyleBackColor = true;
            this.radioButtonLargeIcon.CheckedChanged += new System.EventHandler(this.radioButtonLargeIcon_CheckedChanged);
            // 
            // radioButtonSmallIcon
            // 
            this.radioButtonSmallIcon.Location = new System.Drawing.Point(7, 58);
            this.radioButtonSmallIcon.Name = "radioButtonSmallIcon";
            this.radioButtonSmallIcon.Size = new System.Drawing.Size(174, 24);
            this.radioButtonSmallIcon.TabIndex = 1;
            this.radioButtonSmallIcon.Text = "Small Icon";
            this.radioButtonSmallIcon.UseVisualStyleBackColor = true;
            this.radioButtonSmallIcon.CheckedChanged += new System.EventHandler(this.radioButtonSmallIcon_CheckedChanged);
            // 
            // radioButtonList
            // 
            this.radioButtonList.Location = new System.Drawing.Point(6, 93);
            this.radioButtonList.Name = "radioButtonList";
            this.radioButtonList.Size = new System.Drawing.Size(176, 24);
            this.radioButtonList.TabIndex = 2;
            this.radioButtonList.Text = "List";
            this.radioButtonList.UseVisualStyleBackColor = true;
            this.radioButtonList.CheckedChanged += new System.EventHandler(this.radioButtonList_CheckedChanged);
            // 
            // radioButtonDetails
            // 
            this.radioButtonDetails.Checked = true;
            this.radioButtonDetails.Location = new System.Drawing.Point(6, 133);
            this.radioButtonDetails.Name = "radioButtonDetails";
            this.radioButtonDetails.Size = new System.Drawing.Size(176, 24);
            this.radioButtonDetails.TabIndex = 3;
            this.radioButtonDetails.TabStop = true;
            this.radioButtonDetails.Text = "Deteils";
            this.radioButtonDetails.UseVisualStyleBackColor = true;
            this.radioButtonDetails.CheckedChanged += new System.EventHandler(this.radioButtonDetails_CheckedChanged);
            // 
            // radioButtonTile
            // 
            this.radioButtonTile.Location = new System.Drawing.Point(6, 174);
            this.radioButtonTile.Name = "radioButtonTile";
            this.radioButtonTile.Size = new System.Drawing.Size(176, 24);
            this.radioButtonTile.TabIndex = 4;
            this.radioButtonTile.Text = "Tile";
            this.radioButtonTile.UseVisualStyleBackColor = true;
            this.radioButtonTile.CheckedChanged += new System.EventHandler(this.radioButtonTile_CheckedChanged);
            // 
            // labelCurrentPath
            // 
            this.labelCurrentPath.Location = new System.Drawing.Point(13, 13);
            this.labelCurrentPath.Name = "labelCurrentPath";
            this.labelCurrentPath.Size = new System.Drawing.Size(447, 23);
            this.labelCurrentPath.TabIndex = 1;
            // 
            // listViewFilesAndFolders
            // 
            this.listViewFilesAndFolders.LargeImageList = this.imageListLarge;
            this.listViewFilesAndFolders.Location = new System.Drawing.Point(12, 29);
            this.listViewFilesAndFolders.Name = "listViewFilesAndFolders";
            this.listViewFilesAndFolders.Size = new System.Drawing.Size(540, 220);
            this.listViewFilesAndFolders.SmallImageList = this.imageListSmall;
            this.listViewFilesAndFolders.TabIndex = 2;
            this.listViewFilesAndFolders.UseCompatibleStateImageBehavior = false;
            this.listViewFilesAndFolders.View = System.Windows.Forms.View.Details;
            this.listViewFilesAndFolders.ItemActivate += new System.EventHandler(this.listViewFilesAndFolders_ItemActivate);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(466, 8);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(86, 23);
            this.buttonBack.TabIndex = 3;
            this.buttonBack.Text = "返回";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // imageListSmall
            // 
            this.imageListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmall.ImageStream")));
            this.imageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSmall.Images.SetKeyName(0, "folder16x16.ico");
            this.imageListSmall.Images.SetKeyName(1, "Text 16x16.ico");
            // 
            // imageListLarge
            // 
            this.imageListLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLarge.ImageStream")));
            this.imageListLarge.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListLarge.Images.SetKeyName(0, "folder32x32.ico");
            this.imageListLarge.Images.SetKeyName(1, "New file.ico");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 261);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.listViewFilesAndFolders);
            this.Controls.Add(this.labelCurrentPath);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "ListView";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonTile;
        private System.Windows.Forms.RadioButton radioButtonDetails;
        private System.Windows.Forms.RadioButton radioButtonList;
        private System.Windows.Forms.RadioButton radioButtonSmallIcon;
        private System.Windows.Forms.RadioButton radioButtonLargeIcon;
        private System.Windows.Forms.Label labelCurrentPath;
        private System.Windows.Forms.ListView listViewFilesAndFolders;
        private System.Windows.Forms.ImageList imageListLarge;
        private System.Windows.Forms.ImageList imageListSmall;
        private System.Windows.Forms.Button buttonBack;
    }
}

