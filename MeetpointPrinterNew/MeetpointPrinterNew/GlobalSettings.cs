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

        public static string CurrentUser { set; get; }
        public static string CurrentEvent { set; get; }
        public static string CurrentEventLocation { set; get; }
        public static int PreviousPageID { set; get; }
        public static int CurrentPageID { set; get; }

        public static bool IsPrinterSet { set; get; }
        public static bool IsAccountSet { set; get; }
        public static bool IsTemplateSet { set; get; }
    }
}
