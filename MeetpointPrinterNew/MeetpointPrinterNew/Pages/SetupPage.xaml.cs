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
        private const int PageID = 2;
        private int pageType = -1;
        public int SetupPageType { get { return this.pageType; } }



        private List<Account> _accounts;
        private UserSettings _settings;


        private bool _isUnchecked;

        public SetupPage(UserSettings settings, int setupPageType)
        {

            InitializeComponent();

            try
            {
                _isUnchecked = false;
                _settings = settings;
                _accounts = _settings.Accounts.Account;
                GlobalSettings.ApplicationSettings = settings;
                this.pageType = setupPageType;

                switch (pageType)
                {
                    case 0:
                        GlobalSettings.CurrentPageID = 2;
                        Style cbPrinterStyle = (Style)FindResource("ChecBoxPrinterStyle");
                        lblPrintingDevice.Text = "SELECT PRINTING DEVICE";

                        var server = new LocalPrintServer();
                        PrintQueueCollection myPrintQueues = server.GetPrintQueues();

                        foreach (System.Printing.PrintQueue pq in myPrintQueues)
                        {
                            pq.Refresh();
                            CheckBox cbp = new CheckBox();
                            cbp.Style = cbPrinterStyle;
                            cbp.Content = pq.Name;
                            cbp.Tag = pq;
                            cbp.IsChecked = pq.Name.Equals(_settings.Printer);
                            cbp.Checked += cbPrinter_Checked;
                            cbp.Unchecked += cbPrinter_Unchecked;
                            icPrinterItems.Items.Add(cbp);
                        }
                        break;
                    case 1:
                        GlobalSettings.CurrentPageID = 3;
                        List<User> users = Helpers.GetCustomerUsers(_settings.AuthToken);

                        Style cbAccountStyle = (Style)FindResource("ChecBoxAccountStyle");
                        lblPrintingDevice.Text = "SELECT ACCOUNTS";

                        foreach (User item in users)
                        {
                            CheckBox cbp = new CheckBox();
                            cbp.Style = cbAccountStyle;
                            cbp.Content = item.value;
                            cbp.Tag = item.key.ToString();

                            cbp.IsChecked = _accounts.Where(q => q.AccountID == item.key.ToString() && q.AccountName == item.value).Count() == 1;
                            cbp.Checked += cbAccount_Checked;
                            cbp.Unchecked += cbAccount_Unchecked;
                            icPrinterItems.Items.Add(cbp);
                        }
                        break;
                    case 2:
                        lblPrintingDevice.Text = "SELECT LABEL TEMPLATE";

                        break;
                }
            }
            catch (Exception ex)
            {
                //DebugLog("")
            }

        }
        private void cbPrinter_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            _settings.Printer = cb.Content.ToString();
            _isUnchecked = true;
            foreach (CheckBox cbItem in icPrinterItems.Items)
            {
                if (!cb.Content.Equals(cbItem.Content))
                {
                    cbItem.IsChecked = false;
                }

            }
            _isUnchecked = false;
        }
        private void cbPrinter_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!_isUnchecked)
            {
                _settings.Printer = "";
            }

        }

        private void cbAccount_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Account ac = new Account();
            ac.AccountID = cb.Tag.ToString();
            ac.AccountName = cb.Content.ToString();
            _accounts.Add(ac);
        }
        private void cbAccount_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Helpers.RemoveAccountItem(_accounts, cb.Tag.ToString());
        }


    }
}
