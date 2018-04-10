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
    /// Interaction logic for PrintSetupPage.xaml
    /// </summary>
    public partial class SetupPage : Page
    {

        private int pageType = -1;
        public int SetupPageType { get { return this.pageType; } }
        public SetupPage(int setupPageType)
        {
         
            InitializeComponent();

            this.pageType = setupPageType;

            switch (pageType)
            {
                case 0:

                    Style cbPrinterStyle = (Style)FindResource("ChecBoxPrinterStyle");
                    lblPrintingDevice.Content = "SELECT PRINTING DEVICE";
                    CheckBox cbp1 = new CheckBox();
                    cbp1.Style = cbPrinterStyle;
                    cbp1.Content = "Printer name 1";
                    CheckBox cbp2 = new CheckBox();
                    cbp2.Style = cbPrinterStyle;
                    cbp2.Content = "Printer name 2";
                    CheckBox cbp3 = new CheckBox();
                    cbp3.Style = cbPrinterStyle;
                    cbp3.Content = "Printer name 3";
                    CheckBox cbp4 = new CheckBox();
                    cbp4.Style = cbPrinterStyle;
                    cbp4.Content = "Printer name 4";
                    CheckBox cbp5 = new CheckBox();
                    cbp5.Style = cbPrinterStyle;
                    cbp5.Content = "Printer name 5";

                    icPrinterItems.Items.Add(cbp1);
                    icPrinterItems.Items.Add(cbp2);
                    icPrinterItems.Items.Add(cbp3);
                    icPrinterItems.Items.Add(cbp4);
                    icPrinterItems.Items.Add(cbp5);
                    break;
                case 1:
                    Style cbAccountStyle = (Style)FindResource("ChecBoxAccountStyle");
                    lblPrintingDevice.Content = "SELECT ACCOUNTS";

                    CheckBox cba1 = new CheckBox();
                    cba1.Style = cbAccountStyle;
                    cba1.Content = "Account name 1";
                    CheckBox cba2 = new CheckBox();
                    cba2.Style = cbAccountStyle;
                    cba2.Content = "Account name 2";
                    CheckBox cba3 = new CheckBox();
                    cba3.Style = cbAccountStyle;
                    cba3.Content = "Account name 3";
                    CheckBox cba4 = new CheckBox();
                    cba4.Style = cbAccountStyle;
                    cba4.Content = "Account name 4";
                    CheckBox cba5 = new CheckBox();
                    cba5.Style = cbAccountStyle;
                    cba5.Content = "Account name 5";
                    
                    icPrinterItems.Items.Add(cba1);
                    icPrinterItems.Items.Add(cba2);
                    icPrinterItems.Items.Add(cba3);
                    icPrinterItems.Items.Add(cba4);
                    icPrinterItems.Items.Add(cba5);
                    break;
                case 2:
                    lblPrintingDevice.Content = "SELECT LABEL TEMPLATE";

                    break;
            }
        }
    }
}
