 //Namespace Declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace DazzleDesign_QuickNotes//Creates Form1 under this namespace
{
    public partial class MainForm : Form
    {
       private bool ChangesMade = false;
      
        public EnterPass EnterPassDialog =new EnterPass();
        private string OriginalData = new RichTextBox().Rtf;
        private TextSearch Search = new TextSearch();
        private bool FindText = false;
        private bool ReplaceText = false;
        private bool isMouseDown = false;
        private bool OnLeft = false;
        private bool OnRight = false;
        private bool OnTop = false;
        private bool OnBottom = false;
        private int TopDist = 0;
        private int LeftDist = 0;
        private Point StartPoint;
        public MainForm()
        {
            InitializeComponent();
            
            this.printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_BeginPrint);
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            this.btnPageSetup.Click += new System.EventHandler(this.btnPageSetup_Click);
          //---------------

            switch (Properties.Settings.Default.formstate)
            {
                case FormWindowState.Maximized:
                    this.WindowState = FormWindowState.Maximized;
                    maximizebttn.BackgroundImage = Properties.Resources.normicon;
                    break;
              
                case FormWindowState.Normal:
                      if (Properties.Settings.Default.formloc.IsEmpty == true)
            {
                this.StartPosition = FormStartPosition.CenterScreen;

                          this.Size = Properties.Settings.Default.formsize;

                          maximizebttn.BackgroundImage = Properties.Resources.maxicon;
            }
            else{
            this.StartPosition=FormStartPosition.Manual;
            this.Size = Properties.Settings.Default.formsize;
            this.Location = Properties.Settings.Default.formloc;
            }
                    break;
                
            }
          


            richTextBox1.ReadOnly = Properties.Settings.Default.readonlydoc;
            richTextBox1.ForeColor = Properties.Settings.Default.defforecolor;
                  richTextBox1.Font = Properties.Settings.Default.deffont;
                 
           deffonttxtbox.Text = Properties.Settings.Default.deffont.Name;
            deffontsizetxtbox.Text = string.Concat((string) Convert.ChangeType( Properties.Settings.Default.deffont.Size,typeof(string))," ",(string) Convert.ChangeType(Properties.Settings.Default.deffont.Unit,typeof(string)));
            defforecolortxtbox.Text = Properties.Settings.Default.defforecolor.Name;
            defhcolortxtbox.Text = Properties.Settings.Default.hcolor.Name;

            bulletindenttxtbox.Text = (string) Convert.ChangeType( Properties.Settings.Default.bulletindent,typeof(string));


            
        }

        public bool LoadAllFiles()
        {
            this.TopMost = Properties.Settings.Default.displaytop;

         
                foreach (string File in System.IO.Directory.GetFiles(PathGenerator.PathGen.GetPath(PathGenerator.PathGen.PathType.DocQnDir), "*.qnote", SearchOption.AllDirectories))
                {
                    FileInfo FileInfo = new FileInfo(File);
                    string FileName=FileInfo.Name.Substring(0, FileInfo.Name.LastIndexOf("."));
                    TabPage NewTab = new TabPage();

                    NewTab.Text = FileName;
                    NewTab.Name = FileName;

                    tabControl1.TabPages.Add(NewTab);
                }
                tabControl1.SelectedIndex = 0;
                return true;

        }

    
      private  void WriteFile(string FileName, bool ClearRTF)
        {
            string FilePath = string.Concat(PathGenerator.PathGen.GetPath(PathGenerator.PathGen.PathType.DocQnDir), @"\", FileName, ".qnote");

          FileInfo FileInfo=new FileInfo(FilePath);
            if (ClearRTF == true)
            {
                richTextBox1.Clear();
            }

            qnote Note = new qnote();


            Note.AppVersion = Application.ProductVersion;
            Note.Data = richTextBox1.Rtf;

            BinaryFormatter Formatter = new BinaryFormatter();

            FileStream Stream = new FileStream(FilePath, FileMode.Create);

            Formatter.Serialize(Stream, Note);
            Stream.Close();
        
          statuslabel.Text=string.Concat("Created On:  ",(string) Convert.ChangeType(FileInfo.CreationTime,typeof(string)),"   |   Modified On:  ",(string) Convert.ChangeType(FileInfo.LastWriteTime,typeof(string)));

        }
private void LoadFile(string FileName,bool CheckFile)
        {

            string FilePath = string.Concat(PathGenerator.PathGen.GetPath(PathGenerator.PathGen.PathType.DocQnDir), @"\", FileName, ".qnote");






            if ((tabControl1.TabPages.ContainsKey(FileName) == true) && CheckFile==true)
            {
                MessageBox.Show("This file is already open.", "DazzleDesign QuickNotes", MessageBoxButtons.OK, MessageBoxIcon.Stop);


            }
            else
            {
                FileInfo FileInfo = new FileInfo(FilePath);

                qnote Note = new qnote();

                BinaryFormatter Formatter = new BinaryFormatter();
                FileStream Stream = new FileStream(FilePath, FileMode.Open);
                Note = (qnote)Formatter.Deserialize(Stream);
                Stream.Close();

                richTextBox1.Clear();
                OriginalData = "";


                richTextBox1.Rtf = (string)Note.Data;
                OriginalData = (string)Note.Data;

                statuslabel.Text = string.Concat("Created On:  ", (string)Convert.ChangeType(FileInfo.CreationTime, typeof(string)), "   |   Modified On:  ", (string)Convert.ChangeType(FileInfo.LastWriteTime, typeof(string)));

            }
        }

        public void CreateTab(string FileName)
        {
           

                TabPage NewTab = new TabPage();
               
                NewTab.Text = FileName;
 NewTab.Name = FileName;
                tabControl1.TabPages.Add(NewTab);
                tabControl1.SelectedTab = NewTab;

              panel1.Parent = NewTab;

           panel1.Dock = DockStyle.Fill;
            panel1.Visible = true; 
            
        }

        private void SwitchTab()
        {
            panel1.Parent = tabControl1.SelectedTab;
            panel1.Dock = DockStyle.Fill;
            panel1.Visible = true;
        }

        #region Form
        private void MainForm_Load(object sender, EventArgs e)
        {


            panel1.Visible = false;
            toolStrip1.Visible = false;
        }

   
  
        #endregion




  #region RTF
 

  private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
  {
      process1.StartInfo.FileName = e.LinkText;
      process1.Start();
  }
     
          private void richTextBox1_SelectionChanged(object sender, EventArgs e)
          {
              if (richTextBox1.SelectionLength > 0 && highlighter.Checked == true)
              {
                  richTextBox1.SelectionBackColor = Properties.Settings.Default.hcolor;


              }

              else if (highlighter.Checked == false && richTextBox1.SelectionLength == 0)


              { richTextBox1.SelectionBackColor = Color.Transparent; }

              else if (richTextBox1.SelectionLength == 0 && highlighter.Checked == true)
              {
                  richTextBox1.SelectionBackColor = Color.Transparent;
              }
            
              
          }


  #endregion
      


        #region FileMenu
          private void closedoc_Click(object sender, EventArgs e)
          {

              tabControl1.TabPages.Remove(tabControl1.SelectedTab);
          }

          private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
          {
              if (MessageBox.Show("Are you sure you want to delete this file?", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
              {
                  string FilePath = string.Concat(PathGenerator.PathGen.GetPath(PathGenerator.PathGen.PathType.DocQnDir), @"\", tabControl1.SelectedTab.Name, ".qnote");

                  tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                  System.IO.File.Delete(FilePath);

              }
          }

          private void renamedoc_Click(object sender, EventArgs e)
          {
              RenameFile RenameFile = new RenameFile();
              RenameFile.Show();
          }
          private void newdocbttn_Click(object sender, EventArgs e)
          {

              NewFile NewFileDialog = new NewFile();
              NewFileDialog.TopMost = true;
              if (NewFileDialog.ShowDialog() == DialogResult.OK)
              {



                  

                  CreateTab( NewFileDialog.textBox1.Text);
                  NewFileDialog.Dispose();

              }
          }

     
   

        private void opendocbttn_Click_1(object sender, EventArgs e)
        {
            OpenFile OpenFileDialog = new OpenFile();
            OpenFileDialog.TopMost = true;
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {



                if (tabControl1.TabPages.ContainsKey((string)OpenFileDialog.listBox1.SelectedItem) == true)
                {
                    tabControl1.SelectTab((string)OpenFileDialog.listBox1.SelectedItem);
                }
                else
                {
                    CreateTab((string)OpenFileDialog.listBox1.SelectedItem);
                    OpenFileDialog.Dispose();
                }
            }
        }

        private void closeallbttn_Click(object sender, EventArgs e)
        {
            foreach (TabPage Tab in tabControl1.TabPages)
            {
               if (Tab.Text != StartPage.Text)
                {
                    tabControl1.TabPages.Remove(Tab);
                }
            }
        }

        private void savedocbttn_Click(object sender, EventArgs e)
        {

            WriteFile(tabControl1.SelectedTab.Text,false);          


        }


    private void exitappbttn_Click(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case FormWindowState.Maximized:

                    Properties.Settings.Default.formstate = FormWindowState.Maximized;
                    break;

                case FormWindowState.Normal:
                    Properties.Settings.Default.formstate = FormWindowState.Normal;
                    Properties.Settings.Default.formloc = this.Location;
                    Properties.Settings.Default.formsize = this.Size;


                    break;

            }
            Properties.Settings.Default.Save();


            if (string.Compare(OriginalData, richTextBox1.Rtf, false) != 0 )
            {
                ChangesMade = true;


            }

            if (ChangesMade == true)
            {
                switch (Properties.Settings.Default.autosave)
                {
                    case true:
                        {
                            if (tabControl1.SelectedTab.Text != "*StartPage")
                            { 
                             WriteFile(tabControl1.SelectedTab.Name, false);
                            Properties.Settings.Default.Save();
                            }
                           


                            ChangesMade = false;

                          

                            Application.Exit();
                            break;
                        }
                    case false:
                        {
                            DialogResult Result = MessageBox.Show("Changes made to this document have not been saved. Save Changes before proceeding?", "DazzleDesign QuickNotes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                            if (Result == DialogResult.Yes)
                            {
                                WriteFile(tabControl1.SelectedTab.Name, false);
                                ChangesMade = false;
                             
                                Application.Exit();
                            }
                         
                            else if (Result == DialogResult.No)
                            {
                                ChangesMade = false;
                               
                                Application.Exit();
                            }

                          
                            break;
                        }
                }
            }
            else if (ChangesMade == false)
            {
                Application.Exit();
            }
    
        }
        #endregion


        #region TabPage
    private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
    {

        if (e.TabPage.Text != "*Start Page")
        {
         SwitchTab();
              string NewFilePath=string.Concat(PathGenerator.PathGen.GetPath(PathGenerator.PathGen.PathType.DocQnDir),@"\",e.TabPage.Text.Trim(),".qnote");

              bool result = File.Exists(NewFilePath);
         if (result == true)
         { 
           LoadFile(e.TabPage.Text,false);
         }
         else if (result == false)
         {
             WriteFile(e.TabPage.Text.Trim(),true);
         }
          
          

        }
    }


    private void tabControl1_Deselecting(object sender, TabControlCancelEventArgs e)
    {

        if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Text != "*Start Page")
        {
            if (string.Compare(OriginalData, richTextBox1.Rtf, false) != 0 && e.TabPage.Text != "*StartPage")
            {
                ChangesMade = true;


            }


            if (ChangesMade == true)
            {
                switch (Properties.Settings.Default.autosave)
                {
                    case true:
                        {
                            WriteFile(e.TabPage.Text, false);



                            ChangesMade = false;
                            break;
                        }
                    case false:
                        {
                            DialogResult Result = MessageBox.Show("Changes made to this document have not been saved. Save Changes before proceeding?", "DazzleDesign QuickNotes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                            if (Result == DialogResult.Yes)
                            {
                                WriteFile(e.TabPage.Text, false);
                                ChangesMade = false;
                            }
                            else if (Result == DialogResult.Cancel)
                            {
                                e.Cancel = true;
                            }
                            else if (Result == DialogResult.No)
                            {
                                ChangesMade = false;
                            }
                            break;
                        }
                }
            }

        }
    }

        #endregion

   

 


        #region Edit Menu
    private void clearToolStripMenuItem_Click(object sender, EventArgs e)
    {
        richTextBox1.Clear();

    }

 private void undobttn_Click(object sender, EventArgs e)
    {
       
      richTextBox1.Undo();  
       
     
    }
 private void redobttn_Click(object sender, EventArgs e)
 {
  
       richTextBox1.Redo();

   
 }

 private void cutbttn_Click(object sender, EventArgs e)
 {
     
       richTextBox1.Cut();
    
   
 }
        
        private void copybttn_Click(object sender, EventArgs e)
 {
    
      richTextBox1.Copy();
    
    
 }

        private void pastebttn_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void selectallbttn_Click(object sender, EventArgs e)
        {
           
            richTextBox1.SelectAll();
        }

        private void clearclipbttn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will clear all the data on the clipboard. Do you want to proceed?", "DazzleDesign QuickNotes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
              Clipboard.Clear();
            }

          
        }

        private void clearbttn_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedRtf = null;
        }
        #endregion
        #region RTF ContextMenu
        private void cutbttn2_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void copybttn2_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pastebttn2_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void clearbttn2_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedRtf = null;
        }
        #endregion

        #region Format Menu
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paragraph ParaDialog = new Paragraph();
            ParaDialog.TextBox1.Text = (string)Convert.ChangeType(richTextBox1.SelectionIndent, typeof(string));
            ParaDialog.TextBox2.Text = (string)Convert.ChangeType(richTextBox1.SelectionRightIndent, typeof(string));
            ParaDialog.TextBox3.Text = (string)Convert.ChangeType(richTextBox1.SelectionHangingIndent, typeof(string));
            ParaDialog.ComboBox1.Text = (string)Convert.ChangeType(richTextBox1.SelectionAlignment, typeof(string));

            if (ParaDialog.ShowDialog() == DialogResult.OK)
            {

                richTextBox1.SelectionIndent = (int)Convert.ChangeType(ParaDialog.TextBox1.Text.Trim(), typeof(int));
                richTextBox1.SelectionRightIndent = (int)Convert.ChangeType(ParaDialog.TextBox2.Text.Trim(), typeof(int));
                richTextBox1.SelectionHangingIndent = (int)Convert.ChangeType(ParaDialog.TextBox3.Text.Trim(), typeof(int));
                switch (ParaDialog.ComboBox1.Text)
                {
                    case "Left":
                        {
                            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
                            break;
                        }
                    case "Right":
                        {
                            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
                            break;
                        }

                    case "Center":
                        {
                            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
                            break;
                        }

                }



            }

            ParaDialog.Dispose();
        }

        private void formatmenu_DropDownOpening(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionBullet == true)
            {
                bullets.Checked = true;
            }

            else if (richTextBox1.SelectionBullet == false)
            {
                bullets.Checked = false;
            }
        }

        private void bullets_Click(object sender, EventArgs e)
        {
            if (bullets.Checked == true)
            {
                bullets.Checked = false;
                richTextBox1.SelectionBullet = false;
            }
            else if (bullets.Checked == false)
            {
                bullets.Checked = true;
                richTextBox1.SelectionBullet = true;
            }

        }


        private void font_Click(object sender, EventArgs e)
        {
            if (fontDialog2.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionFont = fontDialog2.Font;
            }
        }

        #endregion


        #region ControlPanel
        private void displaytopchkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (displaytopchkbox.Checked == true)
            {
                this.TopMost = true;
               
            }

            else if (displaytopchkbox.Checked == false)
            { this.TopMost = false; }
        }
       private void offreadonlyradio_Click(object sender, EventArgs e)
        {
        if (offreadonlyradio.Checked == true)
            {
                EnterPass ReadOnlyBox = new EnterPass();
                DialogResult Result = ReadOnlyBox.ShowDialog();   
            if (Result == DialogResult.OK)
                {
                    richTextBox1.ReadOnly = false;

                }
            else if (Result == DialogResult.Cancel)
            {
                onreadonlyradio.Checked = true;
            }
            }
        }

        private void onreadonlyradio_Click(object sender, EventArgs e)
        {
      if (onreadonlyradio.Checked == true)
            {

                DialogResult Result = EnterPassDialog.ShowDialog();
          if ( Result== DialogResult.OK)
                {
                    richTextBox1.ReadOnly = true;
                }
          else if (Result == DialogResult.Cancel)
          {
              offreadonlyradio.Checked = true;
          }
            }
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Text)
            {
                    //---Processed when Start Page is selected
                case "*Start Page":
                    {
                        //---File Menu
                        if ( Properties.Settings.Default.readonlydoc==false)
                        {
                           newdocbttn.Enabled = true;
                        }
                        else if ( Properties.Settings.Default.readonlydoc==true)
                        {
                            newdocbttn.Enabled = false;
                        }   
                        
                      
  
                            opendocbttn.Enabled = true;

                  

                        btnPrint.Enabled = false;
                        btnPrintPreview.Enabled = false;
                        btnPageSetup.Enabled = false;
                        savedocbttn.Enabled = false;
                        closedoc.Enabled = false;
                        deletedoc.Enabled = false;
                        renamedoc.Enabled = false;

                        if (Properties.Settings.Default.readonlydoc == false && tabControl1.TabPages.Count > 1)
                        {
                            closeallbttn.Enabled = true;
                        }
                        else if (Properties.Settings.Default.readonlydoc == true || tabControl1.TabPages.Count == 1)
                        {
                            closeallbttn.Enabled = false;
                        }
                        //---------
                       

                        //-------DropDown Menus
                        editmenu.Enabled = false;
                            formatmenu.Enabled=false;
                            insertmenu.Enabled = false;
                            
                        //--------------
                    }
                    break;

                default:
                    //---Processed when StartPage is NOT selected
                   
                   //----------File Menu
                    btnPrint.Enabled = true;
                    btnPrintPreview.Enabled = true;
                    btnPageSetup.Enabled = true;
                    //----------


                    //----------Edit Menu
                    editmenu.Enabled = true;

                    if (richTextBox1.CanUndo == true)
                    {
                        undobttn.Enabled = true;
                       
                    }
                    else if (richTextBox1.CanUndo == false)
                    {
                        undobttn.Enabled = false;
                    }
                    //----------
                    if (richTextBox1.CanRedo == true)
                    {
                        redobttn.Enabled = true;

                    }
                    else if (richTextBox1.CanRedo == false)
                    {
                        redobttn.Enabled = false;
                    }
                    //------
                    if (richTextBox1.SelectionLength == 0)
                    {
                      
                        copybttn.Enabled = false;
              

                      
                        copybttn2.Enabled = false;
                        
                    }
                    else if (richTextBox1.SelectionLength > 0)
                    {
                       
                        copybttn.Enabled = true;
                        

                        copybttn2.Enabled = true;
                      
                    }
                    //-----------
                    if (richTextBox1.Text.Length == 0)
                    {
                        selectallbttn.Enabled = false;
                       
                    }
                    else if (richTextBox1.Text.Length>0)
                    {
                    selectallbttn.Enabled=true;
                    }
                    //-------End Edit Menu
                 

if(Properties.Settings.Default.readonlydoc == true) 

 {
 //----File Menu
  newdocbttn.Enabled = false;
                opendocbttn.Enabled = true;
               
                savedocbttn.Enabled = false;
                closedoc.Enabled = false;
                closeallbttn.Enabled = false;
                deletedoc.Enabled = false;
                renamedoc.Enabled = false;
     //------------------

     //----------------Edit Menu
                clearclipbttn.Enabled = false;
                clearbttn.Enabled = false;
                cutbttn.Enabled = false;
                pastebttn.Enabled = false;
     //-------------


    //--------Context Menu
                cutbttn2.Enabled = false;
                pastebttn2.Enabled = false;
                clearbttn2.Enabled = false;

    //--------------------

     //------Drop Down Menus
                formatmenu.Enabled = true;
                insertmenu.Enabled = false;

               
               
     //---------------------

 }


 else if (Properties.Settings.Default.readonlydoc == false) 
 {
     //-----------File Menu
     newdocbttn.Enabled = true;
     opendocbttn.Enabled = true;
     savedocbttn.Enabled = true;

     if (tabControl1.TabPages.Count > 0)
     { 
        closedoc.Enabled = true;
     closeallbttn.Enabled = true;
     deletedoc.Enabled = true;
     renamedoc.Enabled = true;
     }
     else if (tabControl1.TabPages.Count == 0)
     {
         closedoc.Enabled = false;
         closeallbttn.Enabled = false;
         deletedoc.Enabled = false;
         renamedoc.Enabled = false;
     }
     //-------------------

     //-------Edit Menu
     clearclipbttn.Enabled = true;

     if (richTextBox1.SelectionLength == 0)
     {
         clearbttn.Enabled = false;
         cutbttn.Enabled = false;
     }
     else if (richTextBox1.SelectionLength > 0)
     {
         clearbttn.Enabled = true;
         cutbttn.Enabled = true;


     }

     pastebttn.Enabled = true;
     //---------------------

     //--------Context Menu
     if (richTextBox1.SelectionLength == 0)
     {
         clearbttn2.Enabled = false;
         cutbttn2.Enabled = false;
     }
     else if (richTextBox1.SelectionLength > 0)
     {
         clearbttn2.Enabled = true;
         cutbttn2.Enabled = true;


     }

     pastebttn.Enabled = true;

     //--------------------


     //--------DropDown Menus
     formatmenu.Enabled = true;
     insertmenu.Enabled = true;
    
     //------------------------
     
 


 }
 break;
            }//End Switch

          
        }
        #region Control Panel
        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Properties.Settings.Default.hcolor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {

                defhcolortxtbox.Text = colorDialog1.Color.Name;

                Properties.Settings.Default.hcolor = colorDialog1.Color;


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = richTextBox1.Font;

            if (fontDialog1.ShowDialog ()== DialogResult.OK)
            {
                Properties.Settings.Default.deffont = fontDialog1.Font;

                richTextBox1.Font = Properties.Settings.Default.deffont;
          

                deffonttxtbox.Text = Properties.Settings.Default.deffont.Name;
                defforecolortxtbox.Text = Properties.Settings.Default.defforecolor.Name;


            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = richTextBox1.ForeColor;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.defforecolor = colorDialog1.Color;
                deffonttxtbox.Text = Properties.Settings.Default.deffont.Name;

                richTextBox1.ForeColor = Properties.Settings.Default.defforecolor;

                defforecolortxtbox.Text = Properties.Settings.Default.defforecolor.Name;



            }
        }

        private void bulletindenttxtbox_Validated(object sender, EventArgs e)
        {
            Properties.Settings.Default.bulletindent = (int)Convert.ChangeType(bulletindenttxtbox.Text, typeof(int));
            richTextBox1.BulletIndent = Properties.Settings.Default.bulletindent;
        }
        #endregion


       





        

       

       




        #region Insert Menu
        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                Object Data = new Object();

                Data = Clipboard.GetDataObject();
                Clipboard.Clear();


                string ImagePath = openFileDialog1.FileName;
                Image Img;
                Img = Image.FromFile(ImagePath);
                Clipboard.SetDataObject(Img);

                DataFormats.Format df;

                df = DataFormats.GetFormat(DataFormats.Bitmap);

                if ( this.richTextBox1. CanPaste(df))
                {

                  this.richTextBox1.Paste(df);

                }

                Clipboard.Clear();

                Clipboard.SetDataObject(Data);
            }
        }
        private void dateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
              Object Data = new Object();

                Data = Clipboard.GetDataObject();
                Clipboard.Clear();


                Clipboard.SetText((string)Convert.ChangeType(DateTime.Now, typeof(string)));


                DataFormats.Format df;

                df = DataFormats.GetFormat(DataFormats.Text);

                if ( this.richTextBox1. CanPaste(df))
                {

                  this.richTextBox1.Paste(df);

                }

                Clipboard.Clear();

                Clipboard.SetDataObject(Data);
            
        }

        #endregion

        #region Print
        private int checkPrint;
		private void btnPageSetup_Click(object sender, System.EventArgs e)
		{
         
			pageSetupDialog1.ShowDialog();
		}

		private void btnPrintPreview_Click(object sender, System.EventArgs e)
		{
			printPreviewDialog1.ShowDialog();
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			if (printDialog1.ShowDialog() == DialogResult.OK)
				printDocument1.Print();
		}

		private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			checkPrint = 0;
		}

		private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			// Print the content of RichTextBox. Store the last character printed.
			checkPrint = richTextBox1.Print(checkPrint, richTextBox1.TextLength, e);

			// Check for more pages
			if (checkPrint < richTextBox1.TextLength)
				e.HasMorePages = true;
			else
				e.HasMorePages = false;
		}
        #endregion

       



        private void tabPage5_Enter(object sender, EventArgs e)
        {
            this.TopMost = false;

            About About=new About();
            if (About.ShowDialog() == DialogResult.OK)
            { 
                        tabControl2.SelectedTab = this.tabPage1;

            }


                

            
        }

       
        #region Find
             private void findbttn_Click(object sender, EventArgs e)
        {
            if (FindText == true)
            {
                switch (searchcriteriacombo.Text)
                {
                    default:
                        Search.FindMyText(findtxtbox.Text, true, false, false, true);

                        break;

                    case "Match Case":
                        Search.FindMyText(findtxtbox.Text, false, true, false, true);
                        break;

                    case "Whole Word":
                        Search.FindMyText(findtxtbox.Text, false, false, true, true);
                        break;


                }
            }
            else if (FindText == false)
            {
                MessageBox.Show("No search keywords entered.", "DazzleDesign QuickNotes", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            
        }
           private void findreplacebttn_Click(object sender, EventArgs e)
        {
            if (findreplacebttn.Checked == true)
            {
                findreplacebttn.Checked = false;
                toolStrip1.Visible = false;
            }

            else if (findreplacebttn.Checked == false)
            {
                findreplacebttn.Checked = true;
             //   richTextBox1.Dock = DockStyle.None;
                toolStrip1.Visible = true;
                
            }
        }
        private void findtxtbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (FindText == false)
            {
                findtxtbox.Text = "";
                FindText = true;
            }

            findtxtbox.ForeColor = Color.Black;
        }


        private void findtxtbox_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(findtxtbox.Text) == true)
            {

                findtxtbox.ForeColor = Color.Gray;
                FindText = false;
                findtxtbox.Text = "Search";
            }
            else { findtxtbox.ForeColor = Color.Black; }
        }
        private void replacebttn_Click(object sender, EventArgs e)
        {
            if (FindText == true && ReplaceText == true)
            {
                if (richTextBox1.SelectionLength > 0)
                {
                    int CompareResult = string.Compare(findtxtbox.Text, richTextBox1.SelectedText, false);

                    if (CompareResult == 0)
                    {

                        richTextBox1.SelectedText = replacetxtbox.Text;


                    }

                    else if (CompareResult != 0)
                    {
                        bool FindResult = false;
                        switch (searchcriteriacombo.Text)
                        {
                            default:
                                FindResult = Search.FindMyText(findtxtbox.Text, true, false, false, true);

                                break;

                            case "Match Case":
                                FindResult = Search.FindMyText(findtxtbox.Text, false, true, false, true);
                                break;

                            case "Whole Word":
                                FindResult = Search.FindMyText(findtxtbox.Text, false, false, true, true);
                                break;

                        }

                        if (FindResult == true)
                        { richTextBox1.SelectedText = replacetxtbox.Text; }
                        else if (FindResult == false)
                        { MessageBox.Show("Specified text not found.", "Search Complete", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                    }
                }

                else if (richTextBox1.SelectionLength == 0)
                {
                    bool FindResult = false;
                    switch (searchcriteriacombo.Text)
                    {
                        default:
                            FindResult = Search.FindMyText(findtxtbox.Text, true, false, false, true);
                            MessageBox.Show((string)Convert.ChangeType(FindResult, typeof(string)));

                            break;

                        case "Match Case":
                            FindResult = Search.FindMyText(findtxtbox.Text, false, true, false, true);
                            break;

                        case "Whole Word":
                            FindResult = Search.FindMyText(findtxtbox.Text, false, false, true, true);
                            break;

                    }

                    if (FindResult == true)
                    { richTextBox1.SelectedText = replacetxtbox.Text; }
                }
            }

            else if (FindText == false)
            { MessageBox.Show("No search keywords entered.", "DazzleDesign QuickNotes", MessageBoxButtons.OK, MessageBoxIcon.Stop); }

            else if (ReplaceText == false)
            { MessageBox.Show("No data entered in 'Replace with' field.", "DazzleDesign QuickNotes", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
        }
        private void replaceallbttn_Click(object sender, EventArgs e)
        {
            if (FindText == true && ReplaceText == true)
            {
                Search.start = 0;
                int Replaced = 0;
                bool FindResult = false;
                do
                {

                    switch (searchcriteriacombo.Text)
                    {
                        default:
                            FindResult = Search.FindMyText(findtxtbox.Text, true, false, false, false);

                            break;

                        case "Match Case":
                            FindResult = Search.FindMyText(findtxtbox.Text, false, true, false, false);
                            break;

                        case "Whole Word":
                            FindResult = Search.FindMyText(findtxtbox.Text, false, false, true, false);
                            break;

                    }

                    if (FindResult == true)
                    {
                        richTextBox1.SelectedText = replacetxtbox.Text;

                        Replaced += 1;

                    }

                    else if (FindResult == false)
                    {
                        MessageBox.Show(string.Concat(Replaced, " occourance(s) have been successfully replaced."), "DazzleDesign QuickNotes", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                } while (FindResult == true);
            }

            else if(FindText==false)
            { MessageBox.Show("No search keywords entered.", "DazzleDesign QuickNotes", MessageBoxButtons.OK, MessageBoxIcon.Stop); }

            else if (ReplaceText==false)
            { MessageBox.Show("No data entered in 'Replace with' field.", "DazzleDesign QuickNotes", MessageBoxButtons.OK, MessageBoxIcon.Stop); }

        }
        private void replacetxtbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (ReplaceText == false)
            {
                replacetxtbox.Text = "";
                ReplaceText = true;
            }

           replacetxtbox.ForeColor = Color.Black;
        }
        private void replacetxtbox_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(replacetxtbox.Text) == true)
            {

                replacetxtbox.ForeColor = Color.Gray;
                ReplaceText = false;
                replacetxtbox.Text = "Replace with...";
            }
            else { replacetxtbox.ForeColor = Color.Black; }
        }

        #endregion


        #region ResizeForm
     private void MainForm_MouseUp(object sender, MouseEventArgs e)
 {
     isMouseDown = false;
     OnLeft = false;
     OnRight = false;
     OnTop = false;
     OnBottom = false;
 }

 private void MainForm_MouseMove(object sender, MouseEventArgs e)
 {
   
     
     //if ((e.X>=0 && e.X<5) && (e.Y>=0 &&e.Y<5))
     //{
     //    //Top Left Corner
     //    this.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
        
     //    if (isMouseDown == true)
     //    {



     //        if ((PointToScreen(e.Location).X < PointToScreen(MoveXY).X) && (this.Left > 0) && (this.Top > 0))
     //        {
     //            this.Left -= PointToScreen(MoveXY).X - PointToScreen(e.Location).X;
     //            this.Width += PointToScreen(MoveXY).X - PointToScreen(e.Location).X; ;

     //            this.Top -= PointToScreen(MoveXY).Y - PointToScreen(e.Location).Y;
     //            this.Height += PointToScreen(MoveXY).Y - PointToScreen(e.Location).Y;
     //        }
     //        else if ((PointToScreen(e.Location).X > PointToScreen(MoveXY).X) && (this.Left > 0) && (this.Top > 0))
     //        {
     //            this.Left += PointToScreen(e.Location).X - PointToScreen(MoveXY).X;
     //            this.Width -= PointToScreen(e.Location).X - PointToScreen(MoveXY).X;

     //            this.Top += PointToScreen(e.Location).Y - PointToScreen(MoveXY).Y;
     //            this.Height -= PointToScreen(e.Location).Y - PointToScreen(MoveXY).Y;
     //        }


             
     //    }
     
     //}

     //else if((e.X>=0 && e.X<5) && (e.Y>=this.Height-5 && e.Y<=this.Height))
     //{
     //    //Bottom Left Corner
     //    this.Cursor = System.Windows.Forms.Cursors.SizeNESW;

     //    if (isMouseDown == true)
     //    {
     //        if ((PointToScreen(e.Location).X < PointToScreen(MoveXY).X) && (this.Left > 0) && (this.Top > 0))
     //        {
     //            this.Left -= PointToScreen(MoveXY).X - PointToScreen(e.Location).X;
     //            this.Width += PointToScreen(MoveXY).X - PointToScreen(e.Location).X; ;

     //            this.Height += PointToScreen(MoveXY).Y - PointToScreen(e.Location).Y;
     //        }
     //        else if ((PointToScreen(e.Location).Y > PointToScreen(MoveXY).Y) && (this.Left > 0) && (this.Top > 0))
     //        {
     //           // this.Left += PointToScreen(e.Location).X - PointToScreen(MoveXY).X;
     //           // this.Width -= PointToScreen(e.Location).X - PointToScreen(MoveXY).X;

     //            this.Height += PointToScreen(MoveXY).Y - PointToScreen(e.Location).Y;
     //        }
     //    }

     //}

     //else if ((e.Y >= 0 && e.Y < 5) && (e.X >= this.Width - 5 && e.X <= this.Width))
     //{
     //    // Top Right Corner
     //    this.Cursor = System.Windows.Forms.Cursors.SizeNESW;
     //    if(isMouseDown==true)
     //    {
     //        if ((PointToScreen(e.Location).X < PointToScreen(MoveXY).X) && (this.Left > 0) && (this.Top > 0))
     //        {
     //            this.Width += PointToScreen(MoveXY).X - PointToScreen(e.Location).X; ;

     //            this.Top -= PointToScreen(MoveXY).Y - PointToScreen(e.Location).Y;
     //            this.Height += PointToScreen(MoveXY).Y - PointToScreen(e.Location).Y;
     //        }
     //        else if ((PointToScreen(e.Location).X > PointToScreen(MoveXY).X) && (this.Left > 0) && (this.Top > 0))
     //        {
     //            this.Width -= PointToScreen(e.Location).X - PointToScreen(MoveXY).X;

     //            this.Top += PointToScreen(e.Location).Y - PointToScreen(MoveXY).Y;
     //            this.Height -= PointToScreen(e.Location).Y - PointToScreen(MoveXY).Y;
     //        }
     //    }

     //}

     //else if ((e.X >= this.Width-5 && e.X <= this.Width) && (e.Y >= this.Height - 5 && e.Y <= this.Height))
     //{
     //    //Bottom Right Corner
     //    this.Cursor = System.Windows.Forms.Cursors.SizeNWSE;

        
     //}
     if (this.WindowState == FormWindowState.Normal)
     {
         switch (isMouseDown)
         {
             case false:
                 {

                     if ((e.X >= 0 && e.X <= 10) && (e.Y < this.Height - 10))
                     {
                         //Left Side
                         this.Cursor = System.Windows.Forms.Cursors.SizeWE;


                     }
                     else if ((e.X >= this.Width - 10 && e.X <= this.Width) && (e.Y > 10 && e.Y <= this.Height - 10))
                     {
                         //Right Side
                         this.Cursor = System.Windows.Forms.Cursors.SizeWE;


                     }
                    
                     else if ((e.X >= 10 && e.X <= this.Width - 10) && (e.Y >= this.Height - 10 && e.Y <= this.Height))
                     {
                         //Bottom Side
                         this.Cursor = System.Windows.Forms.Cursors.SizeNS;

                     }
                     else
                     {


                         this.Cursor = System.Windows.Forms.Cursors.Arrow;

                     }
                 }
                 break;


             case true:
                 {

                     Point pt = PointToScreen(e.Location);

                     if (OnLeft == true)
                     {
                         this.Left = pt.X;

                         if ((LeftDist > this.Left) && (this.Width < Screen.PrimaryScreen.WorkingArea.Width))
                         {

                             this.Width += LeftDist - this.Left;
                             this.Refresh();
                         }
                         else if ((LeftDist < this.Left) && (this.Width < Screen.PrimaryScreen.WorkingArea.Width))
                         {
                             this.Width -= this.Left - LeftDist;
                             this.Refresh();

                         }


                     }
                     else if (OnRight == true)
                     {
                         this.Width = PointToClient(pt).X;
                         this.Refresh();

                     }
                 
                     else if (OnBottom == true)
                     {
                         this.Height = PointToClient(pt).Y;
                         this.Refresh();
                     }
                 }

                 break;
         }
     }
 }


 private void MainForm_SizeChanged(object sender, EventArgs e)
 {
     switch (this.WindowState)
     {
         case FormWindowState.Maximized:
             maximizebttn.BackgroundImage = Properties.Resources.normicon;
             break;

         case FormWindowState.Normal:
             maximizebttn.BackgroundImage = Properties.Resources.maxicon;
             break;
     }

    
 }


        


 private void MainForm_MouseDown(object sender, MouseEventArgs e)
 {
     isMouseDown = true;

     if ((e.X >= 0 && e.X <= 10) && (e.Y < this.Height - 10))
     {
         //Left Side
     

         OnLeft = true;
     }
     else if ((e.X >= this.Width - 10 && e.X <= this.Width) && (e.Y > 10 && e.Y <= this.Height - 10))
     {
         //Right Side
       
         OnRight = true;

     }
    
     else if ((e.X >= 10 && e.X <= this.Width - 10) && (e.Y >= this.Height - 10 && e.Y <= this.Height))
     {
         //Bottom Side
        
         OnBottom = true;
     }
 }

 private void MainForm_Resize(object sender, EventArgs e)
 {
     LeftDist = this.Left;
     TopDist = this.Top;
 }

 private void MainForm_MouseLeave(object sender, EventArgs e)
 {
     this.Cursor = System.Windows.Forms.Cursors.Arrow;

 }
        #endregion

 #region Control Box
  private void maximizebttn_Click(object sender, EventArgs e)
 {
     switch (this.WindowState)
     {
         case FormWindowState.Maximized:
             this.WindowState = FormWindowState.Normal;
             this.Refresh();
             break;

         case FormWindowState.Normal:
             this.WindowState = FormWindowState.Maximized;
             this.Refresh();
             break;
     }
 }

 private void exitbttn_Click(object sender, EventArgs e)
 {
     switch (this.WindowState)
     {
         case FormWindowState.Maximized:

             Properties.Settings.Default.formstate = FormWindowState.Maximized;
             break;

         case FormWindowState.Normal:
             Properties.Settings.Default.formstate = FormWindowState.Normal;
             Properties.Settings.Default.formloc = this.Location;
             Properties.Settings.Default.formsize = this.Size;


             break;

     }
     Properties.Settings.Default.Save();


     if (string.Compare(OriginalData, richTextBox1.Rtf, false) != 0)
     {
         ChangesMade = true;


     }

     if (ChangesMade == true)
     {
         switch (Properties.Settings.Default.autosave)
         {
             case true:
                 {
                     if (tabControl1.SelectedTab.Text != "*StartPage")
                     {
                         WriteFile(tabControl1.SelectedTab.Name, false);
                         Properties.Settings.Default.Save();
                     }



                     ChangesMade = false;



                     Application.Exit();
                     break;
                 }
             case false:
                 {
                     DialogResult Result = MessageBox.Show("Changes made to this document have not been saved. Save Changes before proceeding?", "DazzleDesign QuickNotes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                     if (Result == DialogResult.Yes)
                     {
                         WriteFile(tabControl1.SelectedTab.Name, false);
                         ChangesMade = false;

                         Application.Exit();
                     }

                     else if (Result == DialogResult.No)
                     {
                         ChangesMade = false;

                         Application.Exit();
                     }


                     break;
                 }
         }
     }
     else if (ChangesMade == false)
     {
         Application.Exit();
     }
 }
 private void minimizebttn_Click(object sender, EventArgs e)
 {
     this.WindowState = FormWindowState.Minimized;
 }

 #endregion

 #region Form Move
 
 






        #region Title Bar
 private void panel3_MouseDown(object sender, MouseEventArgs e)
 {
     isMouseDown = true;

     if ((e.X >= 5 && e.X <= this.Width - 5) && (e.Y >= 0 && e.Y <= 5))
     {
         //Top Side

         OnTop = true;
     }
     else
     {
         StartPoint = new Point(e.X, e.Y);
     }
 }
 private void panel3_MouseMove(object sender, MouseEventArgs e)
 {
     switch (isMouseDown)
     {
         case true:

             if (this.WindowState == FormWindowState.Normal)
             {
                 if (OnTop == true)
                 {
                     Point pt = PointToScreen(e.Location);

                     this.Top = pt.Y;
                     if ((TopDist > this.Top) && (this.Height < Screen.PrimaryScreen.WorkingArea.Height))
                     {

                         this.Height += TopDist - this.Top;
                         this.Refresh();
                     }
                     else if ((TopDist < this.Top) && (this.Height < Screen.PrimaryScreen.WorkingArea.Height))
                     {
                         this.Height -= this.Top - TopDist;
                         this.Refresh();

                     }
                 }

                 else
                 {
                     Point p1 = new Point(e.X, e.Y);
                     Point p2 = this.PointToScreen(p1);
                     Point p3 = new Point(p2.X - this.StartPoint.X,
                                          p2.Y - this.StartPoint.Y);
                     this.Location = p3;
                 }
             }
             break;
         case false:
             if ((e.X >= 5 && e.X <= this.Width - 5) && (e.Y >= 0 && e.Y <= 5))
             {
                 //Top Side
                 this.Cursor = System.Windows.Forms.Cursors.SizeNS;

             }
             else
             {
                 this.Cursor = System.Windows.Forms.Cursors.Arrow;

             }
             break;
     }


 }

         private void panel3_MouseUp(object sender, MouseEventArgs e)
 {
     isMouseDown = false;
 }
 #endregion

 private void panel3_MouseLeave(object sender, EventArgs e)
 {
     this.Cursor = System.Windows.Forms.Cursors.Arrow;
 }
        #endregion















 

 



    

 

       
      

     
        

       
      

     



       
        
       

       

    

       









     
    
       

      

        
        
      
        

      

     
     


       
        
        

       

     


 
     
       
        
       
        
       







    }
}
