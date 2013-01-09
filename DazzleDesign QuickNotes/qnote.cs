using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DazzleDesign_QuickNotes
{
[Serializable]
    class qnote
    {
        private object DataProperty;
        public object Data
        {
            get
            {
                return DataProperty;
            }
            set
            {
                DataProperty = value;
            }
        }


        private string AppVersionProperty;

        public string AppVersion
        {
            get
            {
                return AppVersionProperty;
            }
            set
            {
                AppVersionProperty = value;
            }
        }
    }
}
