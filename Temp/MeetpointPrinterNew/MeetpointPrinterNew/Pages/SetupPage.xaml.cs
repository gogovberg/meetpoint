using hgi.Environment;
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


        App _currentApp = ((App)Application.Current);
        private List<Account> _accounts;
        private UserSettings _settings;

        private bool _isCompleted;
        private bool _isUnchecked;

        public SetupPage(UserSettings settings, int setupPageType)
        {

            InitializeComponent();

            try
            {
                _isCompleted = false;
                _isUnchecked = false;
                _settings = settings;
                _accounts = _settings.Accounts.Account;
                GlobalSettings.ApplicationSettings = settings;
                this.pageType = setupPageType;

                headerControl.CurrentUser = GlobalSettings.CurrentUser;
                subHeaderControl.EventName = GlobalSettings.CurrentEvent;
                subHeaderControl.EventDateLocation = GlobalSettings.CurrentEventLocation;
                subHeaderControl.btnBack.Visibility = Visibility.Collapsed;


                switch (pageType)
                {
                    case 0:

                        SetPrinterWizardStepsState();

                        GlobalSettings.CurrentPageID = 2;
                        Style cbPrinterStyle = (Style)FindResource("ChecBoxPrinterStyle");
                        lblPrintingDevice.Text = "SELECT PRINTING DEVICE";
                        lblHelper.Text = "Please select 1 device.";
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
                            
                            rpsGridPrinterAccount.Children.Add(cbp);
                            //icPrinterItems.Items.Add(cbp);
                        }
                        break;
                    case 1:

                        SetAccountWizardStepsState();

                        GlobalSettings.CurrentPageID = 3;
                        List<User> users = Helpers.GetCustomerUsers(_settings.AuthToken);

                        Style cbAccountStyle = (Style)FindResource("ChecBoxAccountStyle");
                        lblPrintingDevice.Text = "SELECT ACCOUNTS";
                        lblHelper.Text = "Selected printing device will print from scanners connected to the accounts selected below.";
                        foreach (User item in users)
                        {
                            CheckBox cbp = new CheckBox();
                            cbp.Style = cbAccountStyle;
                            cbp.Content = item.value;
                            cbp.Tag = item.key.ToString();
                            cbp.IsChecked = _accounts.Where(q => q.AccountID == item.key.ToString() && q.AccountName == item.value).Count() == 1;
                            cbp.Checked += cbAccount_Checked;
                            cbp.Unchecked += cbAccount_Unchecked;
                            rpsGridPrinterAccount.Children.Add(cbp);
                            //icPrinterItems.Items.Add(cbp);
                        }
                        break;
                    case 2:
                        lblPrintingDevice.Text = "SELECT LABEL TEMPLATE";

                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("MeetpointPrinter", ex.ToString());
            }

           

            ButtonNextLogic();
        }

        private void cbPrinter_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            _settings.Printer = cb.Content.ToString();
            _isUnchecked = true;
            foreach (CheckBox cbItem in rpsGridPrinterAccount.Children)
            {
                if (!cb.Content.Equals(cbItem.Content))
                {
                    cbItem.IsChecked = false;
                }

            }
            _isUnchecked = false;
            ButtonNextLogic();


        }

        private void cbPrinter_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!_isUnchecked)
            {
                _settings.Printer = "";
            }
            ButtonNextLogic();
        }

        private void cbAccount_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Account ac = new Account();
            ac.AccountID = cb.Tag.ToString();
            ac.AccountName = cb.Content.ToString();
            _accounts.Add(ac);
            ButtonNextLogic();
        }

        private void cbAccount_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Helpers.RemoveAccountItem(_accounts, cb.Tag.ToString());
            ButtonNextLogic();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if(_isCompleted)
            {
                Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
                GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;

                if (GlobalSettings.CurrentPageID == 2)
                {
                    Application.Current.MainWindow.Content = new SetupPage(GlobalSettings.ApplicationSettings, 1);
                }
                else if(GlobalSettings.CurrentPageID == 3)
                {
                    Application.Current.MainWindow.Content = new SetupPagePrintTemplate(GlobalSettings.ApplicationSettings);
                }
                
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            GlobalSettings.ApplicationSettings = Helpers.ReadUserSettings(GlobalSettings.CurrentUser, GlobalSettings.ApplicationSettings.Event.EventID.ToString());

            if (GlobalSettings.ApplicationSettings != null)
            {
                GlobalSettings.CurrentUser = GlobalSettings.ApplicationSettings.Username;
                GlobalSettings.CurrentEvent = GlobalSettings.ApplicationSettings.Event.EventName;
                GlobalSettings.CurrentEventLocation = GlobalSettings.ApplicationSettings.Event.EventLocation;

                GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;

                if (GlobalSettings.CurrentPageID == 2)
                {
                    Application.Current.MainWindow.Content = new EventPage(GlobalSettings.ApplicationSettings.Username, GlobalSettings.ApplicationSettings.AuthToken, GlobalSettings.ApplicationSettings.Event.EventID.ToString());
                }
                else if (GlobalSettings.CurrentPageID == 3)
                {
                    Application.Current.MainWindow.Content = new SetupPage(GlobalSettings.ApplicationSettings, 0);
                }
            }

        }

        private void ButtonNextLogic()
        {
            _isCompleted = Helpers.IsComplete(GlobalSettings.CurrentPageID);
            Style enable = (Style)FindResource("ButtonPrimary");
            Style disable = (Style)FindResource("ButtonPrimaryDisabled");

            btnNext.Style = _isCompleted ? enable : disable;

            switch (GlobalSettings.CurrentPageID)
            {
                case 2:
                    GlobalSettings.IsPrinterSet = _isCompleted;
                    if(_isCompleted && !GlobalSettings.ApplicationSettings.IsPrinterSet)
                    {
                        GlobalSettings.ApplicationSettings.IsPrinterSet = _isCompleted;
                    }
                    SetPrinterWizardStepsState();
                    break;
                case 3:
                    GlobalSettings.IsAccountSet = _isCompleted;
                    if (_isCompleted && !GlobalSettings.ApplicationSettings.IsAccountsSet)
                    {
                        GlobalSettings.ApplicationSettings.IsAccountsSet = _isCompleted;
                    }
                    SetAccountWizardStepsState();
                    break;
            }

            if(GlobalSettings.IsAccountSet && GlobalSettings.IsPrinterSet && GlobalSettings.IsTemplateSet)
            {
                subHeaderControl.BackButtonVisibility = Visibility.Visible;
            }
            else
            {
                subHeaderControl.BackButtonVisibility = Visibility.Collapsed;
            }
        }
        private void SetPrinterWizardStepsState()
        {

            if (GlobalSettings.ApplicationSettings.IsPrinterSet)
            {
                wizardSteps.SetWizardStepState(WizardStep.Printer, WizardStepState.CurrentFilled,WizardStepLine.Empty);
            }
            else
            {
                wizardSteps.SetWizardStepState(WizardStep.Printer, WizardStepState.CurrentEmpty, WizardStepLine.Empty);
            }

            if (GlobalSettings.ApplicationSettings.IsAccountsSet)
            {
                wizardSteps.SetWizardStepState(WizardStep.Account, WizardStepState.Filled, WizardStepLine.PrinterAccount);
            }
            else
            {
                wizardSteps.SetWizardStepState(WizardStep.Account, WizardStepState.Empty, WizardStepLine.Empty);
            }

            if (GlobalSettings.ApplicationSettings.IsTemplateSet)
            {
                wizardSteps.SetWizardStepState(WizardStep.Template, WizardStepState.Filled, WizardStepLine.AccountTemplate);
            }
            else
            {
                WizardStepLine sl = WizardStepLine.PrinterAccount;
                if(!GlobalSettings.ApplicationSettings.IsAccountsSet)
                {
                    sl = WizardStepLine.Empty;
                }
                wizardSteps.SetWizardStepState(WizardStep.Template, WizardStepState.Empty, sl);
            }
        }
        private void SetAccountWizardStepsState()
        {
            if (GlobalSettings.ApplicationSettings.IsPrinterSet)
            {
                wizardSteps.SetWizardStepState(WizardStep.Printer, WizardStepState.Filled, WizardStepLine.Empty);
            }
            else
            {
                wizardSteps.SetWizardStepState(WizardStep.Printer, WizardStepState.Empty, WizardStepLine.Empty);
            }
            if (GlobalSettings.ApplicationSettings.IsAccountsSet)
            {
                wizardSteps.SetWizardStepState(WizardStep.Account, WizardStepState.CurrentFilled, WizardStepLine.PrinterAccount);
            }
            else
            {
                wizardSteps.SetWizardStepState(WizardStep.Account, WizardStepState.CurrentEmpty, WizardStepLine.Empty);
            }
            if (GlobalSettings.ApplicationSettings.IsTemplateSet)
            {
                wizardSteps.SetWizardStepState(WizardStep.Template, WizardStepState.Filled, WizardStepLine.AccountTemplate);
            }
            else
            {
                wizardSteps.SetWizardStepState(WizardStep.Template, WizardStepState.Empty, WizardStepLine.PrinterAccount);
            }
        }
        private void SetCurrentWizardStepsState()
        {
            switch(GlobalSettings.CurrentPageID)
            {
                case 2:
                    if(GlobalSettings.ApplicationSettings.IsPrinterSet)
                    {
                        wizardSteps.SetWizardStepState(WizardStep.Printer, WizardStepState.CurrentFilled, WizardStepLine.Empty);
                    }
                    else
                    {
                        wizardSteps.SetWizardStepState(WizardStep.Printer, WizardStepState.CurrentEmpty, WizardStepLine.Empty);
                    }
                    break;
                case 3:
                    if (GlobalSettings.ApplicationSettings.IsAccountsSet)
                    {
                        wizardSteps.SetWizardStepState(WizardStep.Account, WizardStepState.CurrentFilled, WizardStepLine.Empty);
                    }
                    else
                    {
                        wizardSteps.SetWizardStepState(WizardStep.Account, WizardStepState.CurrentEmpty, WizardStepLine.Empty);
                    }
                    break;
            }
        }
    }
}
