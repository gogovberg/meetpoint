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

        private List<string> _printers;
        private List<string> _accounts;

        private App _currentApp = (Application.Current as App);

        public SetupPage(int setupPageType)
        {
         
            InitializeComponent();

            _printers = _currentApp.ApplicationSettings.Printers.Printer;
            _accounts = _currentApp.ApplicationSettings.Accounts.Account;

            this.pageType = setupPageType;

            switch (pageType)
            {
                case 0:

                    Style cbPrinterStyle = (Style)FindResource("ChecBoxPrinterStyle");
                    lblPrintingDevice.Content = "SELECT PRINTING DEVICE";

                    for(int i=1; i<=5; i++)
                    {
                        CheckBox cbp = new CheckBox();
                        cbp.Style = cbPrinterStyle;
                        cbp.Content = "Printer name "+i;
                        cbp.Name = "cbp" + i;
                        cbp.IsChecked = _printers.Contains(cbp.Name);
                        cbp.Checked += cbPrinter_Checked;
                        cbp.Unchecked += cbPrinter_Unchecked;
                        icPrinterItems.Items.Add(cbp);
                    }

                    break;
                case 1:

                    Style cbAccountStyle = (Style)FindResource("ChecBoxAccountStyle");
                    lblPrintingDevice.Content = "SELECT ACCOUNTS";

                    for (int i = 1; i <= 5; i++)
                    {
                        CheckBox cbp = new CheckBox();
                        cbp.Style = cbAccountStyle;
                        cbp.Content = "Account name " + i;
                        cbp.Name = "cba" + i;
                        cbp.IsChecked = _accounts.Contains(cbp.Name);
                        cbp.Checked += cbAccount_Checked;
                        cbp.Unchecked += cbAccount_Unchecked;
                        icPrinterItems.Items.Add(cbp);
                    }
                    break;
                case 2:
                    lblPrintingDevice.Content = "SELECT LABEL TEMPLATE";

                    break;
            }
        }
        private void cbPrinter_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            _printers.Add(cb.Name);
        }

        private void cbPrinter_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Helpers.RemoveListItem(_printers, cb.Name);
        }
        private void cbAccount_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            _accounts.Add(cb.Name);
        }

        private void cbAccount_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Helpers.RemoveListItem(_accounts, cb.Name);
        }

       
    }
}
