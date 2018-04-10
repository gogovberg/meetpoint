using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeetpointPrinterNew.Pages
{
    /// <summary>
    /// Interaction logic for SetupPagePrintTemplate.xaml
    /// </summary>
    public partial class SetupPagePrintTemplate : Page
    {
        private int pageType = -1;
        public int SetupPageType { get { return this.pageType; } }
        public SetupPagePrintTemplate()
        {
            pageType = 2;
            InitializeComponent();

            lblPrintingDevice.Content = "SET LABEL TEMPLATE";
        }
    }
}
