using APIMASH_OrdrInLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMASH_Ordr.In_StarterKit_Phone
{
    class Globals
    {
        static Globals _instance=null;
        
        public APIMASH_OrdrIn ordrin;

        public static Globals Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Globals();
                return _instance;
            }
        }

        public Globals()
        {
            ordrin = new APIMASH_OrdrIn();
        }
    }
}
