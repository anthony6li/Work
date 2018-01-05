using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ListViewTest
{
    public partial class Form1 : Form
    {
        private StringCollection folderCol;

        public Form1()
        {
            InitializeComponent();

            folderCol = new StringCollection();
            CreateHeaderAndFileListView();
            PaintListView(@"D:\");
            folderCol.Add(@"D:\");
        }

        private void CreateHeaderAndFileListView()
        {
            ColumnHeader colHead;

            colHead = new ColumnHeader();
            colHead.Text = "Filename";
            listViewFilesAndFolders.Columns.Add(colHead);

            colHead = new ColumnHeader();
            colHead.Text = "Size";
            listViewFilesAndFolders.Columns.Add(colHead);

            colHead = new ColumnHeader();
            colHead.Text = "Last accessed";
            listViewFilesAndFolders.Columns.Add(colHead);
        }

        private void PaintListView(string root)
        {
            try
            {
                ListViewItem lvi;
                ListViewItem.ListViewSubItem lvsi;

                if (string.IsNullOrEmpty(root))
                {
                    return;
                }

                DirectoryInfo dir = new DirectoryInfo(root);
                DirectoryInfo[] dirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();
                listViewFilesAndFolders.Items.Clear();
                labelCurrentPath.Text = root;
                listViewFilesAndFolders.BeginUpdate();

                foreach (DirectoryInfo di in dirs)
                {
                    lvi = new ListViewItem();
                    lvi.Text = di.Name;
                    lvi.ImageIndex = 0;
                    lvi.Tag = di.FullName;

                    lvsi = new ListViewItem.ListViewSubItem();
                    lvsi.Text = "";
                    lvi.SubItems.Add(lvsi);

                    lvsi = new ListViewItem.ListViewSubItem();
                    lvsi.Text = di.LastAccessTime.ToString();
                    lvi.SubItems.Add(lvsi);
                    listViewFilesAndFolders.Items.Add(lvi);
                }
                foreach (FileInfo fi in files)
                {
                    lvi = new ListViewItem();
                    lvi.Text = fi.Name;
                    lvi.ImageIndex = 1;
                    lvi.Tag = fi.FullName;

                    lvsi = new ListViewItem.ListViewSubItem();
                    lvsi.Text = fi.Length.ToString();
                    lvi.SubItems.Add(lvsi);

                    lvsi = new ListViewItem.ListViewSubItem();
                    lvsi.Text = fi.LastAccessTime.ToString();
                    lvi.SubItems.Add(lvsi);
                    listViewFilesAndFolders.Items.Add(lvi);
                }

                listViewFilesAndFolders.EndUpdate();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void listViewFilesAndFolders_ItemActivate(object sender, EventArgs e)
        {
            ListView lw = (ListView)sender;
            string fileName = lw.SelectedItems[0].Tag.ToString();
            if (lw.SelectedItems[0].ImageIndex != 0)
            {
                try
                {
                    Process.Start(fileName);
                }
                catch (System.Exception ex)
                {
                    return;
                }
            }
            else
            {
                PaintListView(fileName);
                folderCol.Add(fileName);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (folderCol.Count > 1)
            {
                PaintListView(folderCol[folderCol.Count-2].ToString());
                folderCol.RemoveAt(folderCol.Count-1);
            }
            else
            {
                PaintListView(folderCol[0].ToString());
            }
        }

        private void radioButtonLargeIcon_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = (RadioButton)sender;
            if (rdb.Checked)
            {
                this.listViewFilesAndFolders.View = View.LargeIcon;
            }
        }

        private void radioButtonSmallIcon_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = (RadioButton)sender;
            if (rdb.Checked)
            {
                this.listViewFilesAndFolders.View = View.SmallIcon;
            }
        }

        private void radioButtonList_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = (RadioButton)sender;
            if (rdb.Checked)
            {
                this.listViewFilesAndFolders.View = View.List;
            }
        }

        private void radioButtonDetails_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = (RadioButton)sender;
            if (rdb.Checked)
            {
                this.listViewFilesAndFolders.View = View.Details;
            }
        }

        private void radioButtonTile_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = (RadioButton)sender;
            if (rdb.Checked)
            {
                this.listViewFilesAndFolders.View = View.Tile;
            }
        }
    }
}
