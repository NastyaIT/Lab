using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Z1
{
    public partial class Form1 : Form
    {
        public string fullPath;
        public Form1()
        {
            InitializeComponent();
            DriveTreeInit();
            
            
        }
        public void DriveTreeInit()
        {
            string[] drivesArray = Directory.GetLogicalDrives();
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            foreach (string s in drivesArray)
            {
                TreeNode drive=new TreeNode(s,0,0);
                treeView1.Nodes.Add(drive);
                GetDirts(drive);
            }
            treeView1.EndUpdate();
        }

        public void GetDirts(TreeNode node)
        {
            DirectoryInfo[] diArray;
            node.Nodes.Clear();
            string fullPath = node.FullPath;
            DirectoryInfo di = new DirectoryInfo(fullPath);
            try
            {
                diArray = di.GetDirectories();
            }
            catch
            {
                return;
            }
            foreach (DirectoryInfo dirinfo in diArray)
            {
                TreeNode dir = new TreeNode(dirinfo.Name, 0, 0);
                node.Nodes.Add(dir);
            }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            treeView1.BeginUpdate();
            foreach (TreeNode node in e.Node.Nodes)
            {
                GetDirts(node);
                treeView1.EndUpdate();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            fullPath = selectedNode.FullPath;

            DirectoryInfo di = new DirectoryInfo(fullPath);
            FileInfo[] fiArray;
            DirectoryInfo[] diArray;

            try
            {
                fiArray = di.GetFiles();
                diArray = di.GetDirectories();
            }
            catch
            {
                return;
            }

            listView1.Items.Clear();

            foreach (DirectoryInfo dirInfo in diArray)
            {
                ListViewItem lvi = new ListViewItem(dirInfo.Name);
                lvi.SubItems.Add("0");
                lvi.SubItems.Add(dirInfo.LastWriteTime.ToString());
                lvi.ImageIndex = 0;

                listView1.Items.Add(lvi);
            }


            foreach (FileInfo fileInfo in fiArray)
            {
                ListViewItem lvi = new ListViewItem(fileInfo.Name);
                lvi.SubItems.Add(fileInfo.Length.ToString());
                lvi.SubItems.Add(fileInfo.LastWriteTime.ToString());

                string filenameExtension =
                  Path.GetExtension(fileInfo.Name).ToLower();

                switch (filenameExtension)
                {
                    case ".com":
                        {
                            lvi.ImageIndex = 2;
                            break;
                        }
                    case ".exe":
                        {
                            lvi.ImageIndex = 2;
                            break;
                        }
                    case ".hlp":
                        {
                            lvi.ImageIndex = 3;
                            break;
                        }
                    case ".txt":
                        {
                            lvi.ImageIndex = 4;
                            break;
                        }
                    case ".doc":
                        {
                            lvi.ImageIndex = 5;
                            break;
                        }
                    default:
                        {
                            lvi.ImageIndex = 1;
                            break;
                        }
                }

                listView1.Items.Add(lvi);
            }

        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {

                string ext = Path.GetExtension(lvi.Text).ToLower();
                if (ext == ".txt" || ext == ".htm" || ext == ".html")
                {
                    try
                    {
                        richTextBox1.LoadFile(Path.Combine(fullPath, lvi.Text), RichTextBoxStreamType.PlainText);
                        statusStrip1.Text = lvi.Text;
                    }
                    catch
                    {
                        return;
                    }
                }
                else if (ext == ".rtf")
                {
                    try
                    {
                        richTextBox1.LoadFile(Path.Combine(fullPath, lvi.Text), RichTextBoxStreamType.RichText);
                        statusStrip1.Text = lvi.Text;
                    }
                    catch 
                    {
                        return;
                    }
                }
            }
        }
        
    }
}
