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
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            this.rtb_DeviceInfo.Location = new System.Drawing.Point(0,0);
            this.rtb_DeviceInfo.Anchor = AnchorStyles.Top|AnchorStyles.Bottom|AnchorStyles.Left|AnchorStyles.Right;
            string temp = string.Empty;
            if (this.rtb_DeviceInfo.Lines.Length>0)
            {
                temp = this.rtb_DeviceInfo.Lines[0];
                rtb_DeviceInfo.SelectionStart = 0;
                rtb_DeviceInfo.SelectionLength = temp.Length;
                //rtb_DeviceInfo.SelectionColor = Color.Purple;
                rtb_DeviceInfo.SelectionFont = new System.Drawing.Font("微软雅黑", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.rtb_DeviceInfo.Lines[0] = temp;
            }
            if (this.rtb_DeviceInfo.Lines.Length > 6)
            {
                temp = this.rtb_DeviceInfo.Lines[6];
                rtb_DeviceInfo.SelectionStart = rtb_DeviceInfo.Text.IndexOf(temp);
                rtb_DeviceInfo.SelectionLength = temp.Length;
                //rtb_DeviceInfo.SelectionColor = Color.Purple;
                rtb_DeviceInfo.SelectionFont = new System.Drawing.Font("微软雅黑", 20, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.rtb_DeviceInfo.Lines[6] = temp;
            }
        }

        private void btn_CloseInfoFrm_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
