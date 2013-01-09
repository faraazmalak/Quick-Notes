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
    public partial class NewFile : Form
    {
        public NewFile()
        {
            InitializeComponent();
           

        }
        private bool isMouseDown = false;
        private Point StartPoint;

        private void button1_Click(object sender, EventArgs e)
        {
            string NewFilePath=string.Concat(PathGenerator.PathGen.GetPath(PathGenerator.PathGen.PathType.DocQnDir),@"\",textBox1.Text.Trim(),".qnote");

         bool result=PathGenerator.PathGen.CheckFile(NewFilePath);

         if (result == true)
         {
             
             this.DialogResult = DialogResult.OK;
             this.Close();

         }
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.displaytop == true)
            {
                this.TopMost = true;

            }
        }
        #region Form Move
         private void NewFile_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            StartPoint = new Point(e.X, e.Y);
        }
  private void NewFile_MouseUp(object sender, MouseEventArgs e)
         {
             isMouseDown = false;
         }

         private void NewFile_MouseMove(object sender, MouseEventArgs e)
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
