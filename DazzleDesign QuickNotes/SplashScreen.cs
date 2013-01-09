using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace DazzleDesign_QuickNotes
{
    public partial class SplashScreen : Form
    {
        public static MainForm MainForm = new MainForm();
        public SplashScreen()
        {
            InitializeComponent();

            this.labelCopyright.Text = AssemblyCopyright;

        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            this.Hide();

            if (Properties.Settings.Default.startload == true)
            {
                if (MainForm.LoadAllFiles() == true)
                {
                    MainForm.Show();
                }
            }
            else { MainForm.Show(); }
           
          

        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            System.IO.Directory.CreateDirectory(PathGenerator.PathGen.GetPath(PathGenerator.PathGen.PathType.DocQnDir));

        }


    }
}
