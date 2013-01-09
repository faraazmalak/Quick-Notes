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
    public partial class Paragraph : Form
    {
        public Paragraph()
        {
            InitializeComponent();
        }
        private bool isMouseDown = false;
        private Point StartPoint;

        private void OK_Button_Click(object sender, EventArgs e)
        {
           
                this.DialogResult = DialogResult.OK;
                this.Close();

           

        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();

        }

        private void Paragraph_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.displaytop == true)
            {
                this.TopMost = true;

            }
        }

        #region Form Move
        private void Paragraph_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void Paragraph_MouseMove(object sender, MouseEventArgs e)
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
        private void Paragraph_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            StartPoint = new Point(e.X, e.Y);

        }

        #endregion


   
    }
}
