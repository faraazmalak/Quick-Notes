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
    public partial class ChangePass : Form
    {
        public ChangePass()
        {
            InitializeComponent();
        }
        private bool isMouseDown = false;
        private Point StartPoint;

        private void button1_Click(object sender, EventArgs e)
        {
            //Checks if password entered by the user matches the saved password
            if ((string.Compare(textBox1.Text , SplashScreen.MainForm.EnterPassDialog. Pass,false)==0) && (string.IsNullOrEmpty(textBox2.Text)==false) )
            {
                //Checks to see if Old and new passwords are the same
                if ((string.Compare(textBox1.Text,textBox2.Text,false)==0) && (string.Compare(SplashScreen.MainForm.EnterPassDialog. Pass,textBox2.Text,false)==0))
                {

                    //Tells the user old and new password are the same
                MessageBox.Show("Old and new passwords cannot be the the same.","DazzleDesign QuickNotes",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                textBox1.ResetText();
                textBox2.ResetText();
                
                }
                else{
                    //If old and new passwords are not the same, assigns the Value to
                    //Pass variable, which is used by Serialize() method of EnterPass form
                    //to write the Password to the file
                    SplashScreen.MainForm.EnterPassDialog.Pass = textBox2.Text;

                    //Closes the dialog
                this.DialogResult = DialogResult.OK;
                this.Dispose();  
                }

              
            }

            else if (string.IsNullOrEmpty(textBox1.Text) == true)
            {

                MessageBox.Show("Enter the old password.", "DazzleDesign QuickNotes", MessageBoxButtons.OK, MessageBoxIcon.Stop);


            }

            else if (string.IsNullOrEmpty(textBox2.Text) == true)
            {
                MessageBox.Show("Enter the new password.", "DazzleDesign QuickNotes", MessageBoxButtons.OK, MessageBoxIcon.Stop);


            }

            else if ((string.Compare(textBox1.Text, SplashScreen.MainForm.EnterPassDialog.Pass, false) != 0) )
            {
                
                MessageBox.Show("Invalid entrie(s). Make sure you have entered the old password correctly and that old and new passwords are not the same.", "DazzleDesign QuickNotes", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                textBox1.ResetText();
                textBox2.ResetText();
            }

          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.displaytop == true)
            {
                this.TopMost = true;

            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)

            { e.SuppressKeyPress = true; }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)

            { e.SuppressKeyPress = true; }
        }


        #region Form Move
         private void ChangePass_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            StartPoint = new Point(e.X, e.Y);
        } 
         private void ChangePass_MouseMove(object sender, MouseEventArgs e)
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
  private void ChangePass_MouseUp(object sender, MouseEventArgs e)
         {
             isMouseDown = false;
         }
        #endregion

       

      
    }
}
