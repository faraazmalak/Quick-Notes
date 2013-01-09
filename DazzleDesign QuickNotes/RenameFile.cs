using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DazzleDesign_QuickNotes
{
    public partial class RenameFile : Form
    {
        public RenameFile()
        {
            InitializeComponent();
        }
        private bool isMouseDown = false;
        private Point StartPoint;

        private void OK_Button_Click(object sender, EventArgs e)
        {
            string OldFilePath = string.Concat(PathGenerator.PathGen.GetPath(PathGenerator.PathGen.PathType.DocQnDir), @"\", SplashScreen.MainForm.tabControl1.SelectedTab.Name, ".qnote");

                        string NewFilePath=string.Concat(PathGenerator.PathGen.GetPath(PathGenerator.PathGen.PathType.DocQnDir),@"\",textBox1.Text.Trim(),".qnote");

                        bool result = PathGenerator.PathGen.RenameFile(OldFilePath, NewFilePath, textBox1.Text.Trim());


                        if (result == true)
                        {
                            SplashScreen.MainForm.tabControl1.SelectedTab.Name = textBox1.Text.Trim();

                            SplashScreen.MainForm.tabControl1.SelectedTab.Text = textBox1.Text.Trim();
                        }
          
               this.Dispose();
        
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void RenameFile_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.displaytop == true)
            {
                this.TopMost = true;

            }
        }

        #region Form Move
        private void RenameFile_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            StartPoint = new Point(e.X, e.Y);
        }

        private void RenameFile_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void RenameFile_MouseMove(object sender, MouseEventArgs e)
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
