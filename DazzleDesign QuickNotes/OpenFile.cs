using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DazzleDesign_QuickNotes
{
    public partial class OpenFile : Form
    {
        public OpenFile()
        {
            InitializeComponent();
        }
        private bool isMouseDown = false;
        private Point StartPoint;

        private void LoadFiles(string SearchPattern)
        {
        listBox1.Items.Clear();
            listBox1.BeginUpdate();
            foreach (string FilePath in System.IO.Directory.GetFiles(PathGenerator.PathGen.GetPath(PathGenerator.PathGen.PathType.DocQnDir),SearchPattern,SearchOption.AllDirectories))
            {
       FileInfo FileInfo = new FileInfo(FilePath);

                listBox1.Items.Add(FileInfo.Name.Substring(0, FileInfo.Name.LastIndexOf(".")));
            }
            listBox1.EndUpdate();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.displaytop == true)
            {
                this.TopMost = true;

            }

            LoadFiles("*.qnote");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string NewFilePath = string.Concat(PathGenerator.PathGen.GetPath(PathGenerator.PathGen.PathType.DocQnDir), @"\", listBox1.SelectedItem.ToString(), ".qnote");

            bool result = PathGenerator.PathGen.CheckChars(textBox1.Text.Trim());
            if (result == true)
            {
               
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

switch (button1.Text)
	{
        case "Search":
            { 
              textBox1.Text=textBox1.Text.Trim();

          if (textBox1.Text.Length==1)
 
          {
            LoadFiles(string.Concat(textBox1.Text.Substring(0, 1), "*.qnote"));

     
          }
          else if (textBox1.Text.Length>1)
          {
                      LoadFiles(string.Concat(textBox1.Text.Substring(0, 2), "*.qnote"));

          }
         button1.Text = "Cancel";
          break;
            }	

    case "Cancel":
            {
                LoadFiles("*.qnote");
                button1.Text = "Search";
                textBox1.ResetText();
                break;
            }
}
          
           
        }
        #region MyRegion
         private void OpenFile_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            StartPoint = new Point(e.X, e.Y);
        }



         private void OpenFile_MouseUp(object sender, MouseEventArgs e)
         {
             isMouseDown = false;

         }

         private void OpenFile_MouseMove(object sender, MouseEventArgs e)
         {
             if (isMouseDown == true)
             {
                 Point p1 = new Point(e.X, e.Y);
                 Point p2 = this.PointToScreen(p1);
                 Point p3 = new Point(p2.X - this.StartPoint.X,
                                      p2.Y - this.StartPoint.Y);
                 this.Location = p3;
             }
         }
        #endregion

       
    }
    }

