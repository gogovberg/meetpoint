using MeetpointPrinterNew.Pages;
using System.Windows;

namespace MeetpointPrinterNew
{

    public partial class App : Application
    {
        public string CurrentUser { set; get; }
        public string CurrentEvent { set; get; }
        public string CurrentEventLocation { set; get; }

        public bool IsPrinter { set; get; }
        public bool IsAccount { set; get; }
        public bool IsTemplate { set; get; }

        public string PrintAcountBrush { set; get; }
        public string AccountTemplateBrush { set; get; }


        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
            Application.Current.MainWindow.Content = new LoginPage(); 
            
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
            GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;

            if(IsComplete(GlobalSettings.CurrentPageID))
            {
                switch (GlobalSettings.CurrentPageID)
                {
                    case 2:
                        IsPrinter = true;
                        IsAccount = false;
                        IsTemplate = false;
                        Application.Current.MainWindow.Content = new SetupPage(GlobalSettings.ApplicationSettings, 1);
                        break;
                    case 3:
                        IsPrinter = true;
                        IsAccount = true;
                        IsTemplate = false;
                        Application.Current.MainWindow.Content = new SetupPagePrintTemplate(GlobalSettings.ApplicationSettings);
                        break;
                    case 4:
                        IsPrinter = true;
                        IsAccount = true;
                        IsTemplate = true;
                        Application.Current.MainWindow.Content = new SettingsPage(GlobalSettings.ApplicationSettings);
                        break;
                }
            }
        
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GlobalSettings.ApplicationSettings = Helpers.ReadUserSettings(CurrentUser, GlobalSettings.ApplicationSettings.Event.EventID.ToString());

            if(GlobalSettings.ApplicationSettings !=null)
            {
                CurrentUser = GlobalSettings.ApplicationSettings.Username;
                CurrentEvent = GlobalSettings.ApplicationSettings.Event.EventName;
                CurrentEventLocation = GlobalSettings.ApplicationSettings.Event.EventLocation;

                GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;

                switch (GlobalSettings.CurrentPageID)
                {
                    case 2:
                        Application.Current.MainWindow.Content = new EventPage(GlobalSettings.ApplicationSettings.Username, GlobalSettings.ApplicationSettings.AuthToken, GlobalSettings.ApplicationSettings.Event.EventID.ToString());
                        break;
                    case 3:
                        Application.Current.MainWindow.Content = new SetupPage(GlobalSettings.ApplicationSettings, 0);
                        break;
                    case 4:
                        Application.Current.MainWindow.Content = new SetupPage(GlobalSettings.ApplicationSettings, 1);
                        break;
                }
            }
        }

        private void btnEvents_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
            GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;
            Application.Current.MainWindow.Content = new EventPage(GlobalSettings.ApplicationSettings.Username,GlobalSettings.ApplicationSettings.AuthToken, GlobalSettings.ApplicationSettings.Event.EventID.ToString());
        }

        private bool IsComplete(int currentPageID)
        {
           
            switch(currentPageID)
            {
                case 2:
                    if(!string.IsNullOrEmpty(GlobalSettings.ApplicationSettings.Printer))
                    {
                       return true;
                    }
                    break;
                case 3:
                    if(GlobalSettings.ApplicationSettings.Accounts.Account.Count>0)
                    {
                        return true;
                    }
                    break;
                case 4:
                    if (GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption.Count > 0 &&
                        !string.IsNullOrEmpty(GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate) &&
                        !string.IsNullOrEmpty(GlobalSettings.ApplicationSettings.PrinterSetup.LayoutSizeID)
                        )
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
