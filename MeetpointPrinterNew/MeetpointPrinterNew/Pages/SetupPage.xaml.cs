using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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

        private App _currentApp = ((App)Application.Current);

        private List<string> _printers;
        private List<string> _accounts;
        private UserSettings _settings;

        private char[] _charsToTrim = { ' ', '\t' };
      
        public SetupPage(UserSettings settings, int setupPageType)
        {
         
            InitializeComponent();
            _settings = settings;
            _printers = _settings.Printers.Printer;
            _accounts = _settings.Accounts.Account;

            _currentApp.ApplicationSettings = settings;

            this.pageType = setupPageType;

            switch (pageType)
            {
                case 0:

                    Style cbPrinterStyle = (Style)FindResource("ChecBoxPrinterStyle");
                    lblPrintingDevice.Content = "SELECT PRINTING DEVICE";

                    var server = new LocalPrintServer();
                    PrintQueueCollection myPrintQueues = server.GetPrintQueues();

                    foreach (System.Printing.PrintQueue pq in myPrintQueues)
                    {
                        pq.Refresh();
                        CheckBox cbp = new CheckBox();
                        cbp.Style = cbPrinterStyle;
                        cbp.Content = pq.Name;
                        cbp.Tag = pq;
                        cbp.IsChecked = _printers.Contains(pq.Name);
                        cbp.Checked += cbPrinter_Checked;
                        cbp.Unchecked += cbPrinter_Unchecked;
                        icPrinterItems.Items.Add(cbp);
                    }
                    break;
                case 1:

                    List<User> users = Helpers.GetCustomerUsers(_settings.AuthToken);

                    Style cbAccountStyle = (Style)FindResource("ChecBoxAccountStyle");
                    lblPrintingDevice.Content = "SELECT ACCOUNTS";

                   foreach(User item in users)
                    {
                        CheckBox cbp = new CheckBox();
                        cbp.Style = cbAccountStyle;
                        cbp.Content = item.value;
                        cbp.Tag = item.key.ToString();
                        cbp.IsChecked = _accounts.Contains(cbp.Tag.ToString());
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
            _printers.Add(cb.Content.ToString());
        }

        private void cbPrinter_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Helpers.RemoveListItem(_printers, cb.Content.ToString());
        }
        private void cbAccount_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            _accounts.Add(cb.Tag.ToString());
        }
        private void cbAccount_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Helpers.RemoveListItem(_accounts, cb.Tag.ToString());
        }

       
    }
}
