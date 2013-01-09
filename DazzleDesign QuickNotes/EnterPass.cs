using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace DazzleDesign_QuickNotes
{
    public partial class EnterPass : Form
    {
        public static string PassFilePath = string.Concat(PathGenerator.PathGen.GetPath(PathGenerator.PathGen.PathType.AppQnDir), @"\readpass.qpass");
 public  string Pass = "";
        private bool FirstPass = false;

        private bool isMouseDown = false;
        private Point StartPoint;
        public EnterPass()
        {
            InitializeComponent();
            if (Properties.Settings.Default.displaytop == true)
            {
                this.TopMost = true;

            }
        }
        public  void Deserialize()
        { 
         BinaryFormatter Formatter = new BinaryFormatter();
                FileStream Stream = new FileStream(PassFilePath, FileMode.Open);
                Pass = (string)Formatter.Deserialize(Stream);
                MessageBox.Show(Pass);    
            Stream.Close();
           
        }

        public  void Serailize()
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream Stream = new FileStream(PassFilePath, FileMode.Create);
            Formatter.Serialize(Stream, Pass);
            Stream.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            //Checks to see if Password file exists
            if (System.IO.File.Exists(PassFilePath) == true)
            {
                //Reads Password from the file
                Deserialize();

                //Link to change password is enabled as User has already created a Password
                changepasslinklabel.Enabled = true;

              
            }

            else if (System.IO.File.Exists(PassFilePath) == false)
            {
                //Updates the Label to guide the user
                label1.Text = "You must first set a password. Type a password below:";
            
                

                //True value indicates that Password is being created for first time
                FirstPass = true;

                //Link to Change Password is disabled as Password is being created for first time
                changepasslinklabel.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
                this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Checks to see if User is Creating Password for the First Time
          // if Password already created matches the password entered
            if  (FirstPass==true) 
            {
                //Sets the Value of teh variable, which is used by 
                //Serialize method
               Pass=textBox1.Text;

//Writes the Password to the file
                Serailize();

                //Closes the Dialog
                this.DialogResult = DialogResult.OK;
                this.Dispose();
                
            }

            // if Password already created matches the password entered
            else if (string.Compare(textBox1.Text, Pass, false) == 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Dispose();
            }

                //Checks to see if Password entered by the user is a valid entry
            else if (((FirstPass == false) && (string.Compare(textBox1.Text, Pass, false) != 0)) || (string.IsNullOrEmpty(textBox1.Text) == true))
            {
                MessageBox.Show("Invalid Password.", "DazzleDesign QuickNotes", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                textBox1.ResetText();

            }
        
        }

        private void changepasslinklabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangePass ChangePassDialog = new ChangePass();

            if (ChangePassDialog.ShowDialog() == DialogResult.OK)
            {
                Serailize();
                textBox1.ResetText();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)

            { e.SuppressKeyPress = true; }
        }


        #region MyRegion
         private void EnterPass_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            StartPoint = new Point(e.X, e.Y);
        }

         private void EnterPass_MouseUp(object sender, MouseEventArgs e)
         {
             isMouseDown = false;
         }

         private void EnterPass_MouseMove(object sender, MouseEventArgs e)
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
