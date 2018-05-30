using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetpointPrinterNew
{
    public class GlobalSettings
    {
        public static UserSettings ApplicationSettings { set; get; }

        public static int PreviousPageID { set; get; }
        public static int CurrentPageID { set; get; }
    }
}
