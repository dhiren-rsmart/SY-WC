using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Common
{
    public class ApplicationVersion
    {
        public int Major { get{return 1;} }

        public int Minor { get {return 1;} }
        
        public int Revision { get {return 0;} }

        public int Build { get { return 0; } }

        public string GetVersion() {
            return string.Format("{0}.{1}.{2}.{3}", Major, Minor, Revision, Build);
        }
    }
}
