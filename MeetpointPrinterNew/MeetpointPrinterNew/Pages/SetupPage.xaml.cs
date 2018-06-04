using MeetpointPrinterNew.CustomControls;
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

      
        public SetupPage(UserSettings settings, int setupPageType)
        {
         
            InitializeComponent();

            try
            {
                
                _settings = settings;
                _accounts = _settings.Accounts.Account;
                GlobalSettings.ApplicationSettings = settings;
                this.pageType = setupPageType;
                BitmapImage imgsrc;
                Style tbicStyle = (Style)FindResource("TextBlockImageStyle"); ;
                switch (pageType)
                {
                    case 0:
                        GlobalSettings.CurrentPageID = 2;
                        
                        lblPrintingDevice.Content = "SELECT PRINTING DEVICE";
                        imgsrc = new BitmapImage(new Uri("/Images/icon_printer_primary.png", UriKind.Relative));

                        var server = new LocalPrintServer();
                        PrintQueueCollection myPrintQueues = server.GetPrintQueues();

                        foreach (System.Printing.PrintQueue pq in myPrintQueues)
                        {
                            pq.Refresh();

                            TextBlockImageControl tbic = new TextBlockImageControl();
                            tbic.ContentText = pq.Name;
                            tbic.ContentID = pq.Name;
                            tbic.ContentImageSource = imgsrc;
                            tbic.IsSelected = pq.Name.Equals(_settings.Printer);
                            tbic.Control_Click += new EventHandler(tbicPrinter_Checked);
                            tbic.Control_UnClick += new EventHandler(tbicPrinter_Unchecked);
                            tbic.Style = tbicStyle;
                            icPrinterItems.Items.Add(tbic);
                        }
                        break;
                    case 1:
                        GlobalSettings.CurrentPageID = 3;
                        List<User> users = Helpers.GetCustomerUsers(_settings.AuthToken);

                        lblPrintingDevice.Content = "SELECT ACCOUNTS";
                        imgsrc = new BitmapImage(new Uri("/Images/icon_user_primary.png", UriKind.Relative));

                        foreach (User item in users)
                        {
                            TextBlockImageControl tbic = new TextBlockImageControl();

                            tbic.ContentText = item.value;
                            tbic.ContentID = item.key.ToString();
                            tbic.ContentImageSource = imgsrc;
                            tbic.IsSelected = _accounts.Where(q => q.AccountID == item.key.ToString() && q.AccountName == item.value).Count() == 1;
                            tbic.Control_Click += new EventHandler(cbAccount_Checked);
                            tbic.Control_UnClick += new EventHandler(cbAccount_Unchecked);
                            tbic.Style = tbicStyle;
                            icPrinterItems.Items.Add(tbic);
                        }
                        break;
                    case 2:
                        lblPrintingDevice.Content = "SELECT LABEL TEMPLATE";

                        break;
                }
            }
            catch(Exception ex)
            {
                //DebugLog("")
            }

        }

        protected void tbicPrinter_Checked(object sender, EventArgs e)
        {
            TextBlockImageControl cb = (TextBlockImageControl)sender;
            _settings.Printer = cb.ContentText;
            foreach (TextBlockImageControl cbItem in icPrinterItems.Items)
            {
                if(!cb.ContentText.Equals(cbItem.ContentText))
                {
                    cbItem.IsSelected = false;
                }
            }
        }
        protected void tbicPrinter_Unchecked(object sender, EventArgs e)
        {
            _settings.Printer = "";
        }

        protected void cbAccount_Checked(object sender, EventArgs e)
        {
            TextBlockImageControl cb = (TextBlockImageControl)sender;
            Account ac = new Account();
            ac.AccountID = cb.ContentID.ToString();
            ac.AccountName = cb.ContentText.ToString();
            _accounts.Add(ac);
        }
        protected void cbAccount_Unchecked(object sender, EventArgs e)
        {
            TextBlockImageControl cb = (TextBlockImageControl)sender;
            Helpers.RemoveAccountItem(_accounts, cb.ContentID.ToString());
        }

       
    }
}
