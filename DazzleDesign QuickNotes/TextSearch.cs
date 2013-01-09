using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DazzleDesign_QuickNotes
{
    class TextSearch
    {
        public int start = 0;
        private bool TextFoud = false;
        public bool FindMyText(string text, bool None, bool Case, bool Wrord, bool DisplayMessages)
        {
            // Initialize the return value to false by default.
            bool returnValue = false;
            int Length = (int)Convert.ChangeType(SplashScreen.MainForm.richTextBox1.Text.Length, typeof(int));
            SplashScreen.MainForm.richTextBox1.DeselectAll();

            // Ensure a search string has been specified.

            if (text.Length > 0)
            {
                // Obtain the location of the search string in richTextBox1.

                if (Case == true)
                {
                    start = SplashScreen.MainForm.richTextBox1.Find(text, start, RichTextBoxFinds.MatchCase);

                }
                else if (Wrord == true)
                {
                    start = SplashScreen.MainForm.richTextBox1.Find(text, start, RichTextBoxFinds.WholeWord);

                }
                else if (None == true)
                {
                    start = SplashScreen.MainForm.richTextBox1.Find(text, start, RichTextBoxFinds.None);

                }


                // Determine if the text was found in richTextBox1.

                {
                    if (start >= 0)
                    {


                        if (start == Length - 1)
                        {
                            returnValue = true;
                            start = 0;

                            if (DisplayMessages == true)
                            { MessageBox.Show("The application has finished searching the document.", "Search Complete", MessageBoxButtons.OK, MessageBoxIcon.Information); }

                        }
                        else
                        {
                            TextFoud = true;
                            returnValue = true;
                            start += 1;
                        }


                    }
                    else if (start < 0)
                    {
                        switch (TextFoud)
                        {
                            case true:
                                TextFoud = false;
                                start = 0;

                                if (DisplayMessages == true)
                                { MessageBox.Show("The application has finished searching the document.", "Search Complete", MessageBoxButtons.OK, MessageBoxIcon.Information); }

                                break;

                            case false:
                                start = 0;

                                if (DisplayMessages == true)
                                { MessageBox.Show("Specified text not found.", "Search Complete", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                                break;
                        }

                    }






                }

               
            }
 return returnValue;
        }
    }
}
