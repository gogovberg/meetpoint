using MeetpointPrinterNew.Pages;
using System;
using System.Windows;
using System.Windows.Threading;

namespace MeetpointPrinterNew
{

    public partial class App : Application
    {
        public bool IsPrinter { set; get; }
        public bool IsAccount { set; get; }
        public bool IsTemplate { set; get; }

        public string PrintAcountBrush { set; get; }
        public string AccountTemplateBrush { set; get; }

        public Style ButtonNextStyle { set; get; }

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
            GlobalSettings.ApplicationSettings = Helpers.ReadUserSettings(GlobalSettings.CurrentUser, GlobalSettings.ApplicationSettings.Event.EventID.ToString());

            if(GlobalSettings.ApplicationSettings !=null)
            {
                GlobalSettings.CurrentUser = GlobalSettings.ApplicationSettings.Username;
                GlobalSettings.CurrentEvent = GlobalSettings.ApplicationSettings.Event.EventName;
                GlobalSettings.CurrentEventLocation = GlobalSettings.ApplicationSettings.Event.EventLocation;

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

        public bool IsComplete(int currentPageID)
        {

            Style enable = (Style)FindResource("ButtonPrimary");
            Style disable = (Style)FindResource("ButtonPrimaryDisabled");
            bool isComplete = false;
            
            switch (currentPageID)
            {
                case 2:
                    if (!string.IsNullOrEmpty(GlobalSettings.ApplicationSettings.Printer))
                    {
                        isComplete = true;
                    }
                    break;
                case 3:
                    if (GlobalSettings.ApplicationSettings.Accounts.Account.Count > 0)
                    {
                        isComplete = true;
                    }
                    break;
                case 4:
                    if (GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption.Count > 0 &&
                        !string.IsNullOrEmpty(GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate) &&
                        !string.IsNullOrEmpty(GlobalSettings.ApplicationSettings.PrinterSetup.LayoutSizeID)
                        )
                    {
                        isComplete = true;
                    }
                    break;
            }

          
            ButtonNextStyle = isComplete ? enable : disable;
         
            return isComplete;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
