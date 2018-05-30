using MeetpointPrinterNew.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MeetpointPrinterNew
{

    public partial class App : Application
    {
        public string CurrentUser { set; get; }
        public string CurrentEvent { set; get; }
        public string CurrentEventLocation { set; get; }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
            Application.Current.MainWindow.Content = new LoginPage(); 
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
            GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;

            switch (GlobalSettings.CurrentPageID)
            {
                case 2:
                    Application.Current.MainWindow.Content = new SetupPage(GlobalSettings.ApplicationSettings,1);
                    break;
                case 3:
                    Application.Current.MainWindow.Content = new SetupPagePrintTemplate(GlobalSettings.ApplicationSettings);
                    break;
                case 4:
                    Application.Current.MainWindow.Content = new SettingsPage(GlobalSettings.ApplicationSettings);
                    break;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GlobalSettings.ApplicationSettings = Helpers.ReadUserSettings(CurrentUser);

            if(GlobalSettings.ApplicationSettings !=null)
            {
                CurrentUser = GlobalSettings.ApplicationSettings.Username;
                CurrentEvent = GlobalSettings.ApplicationSettings.Event.EventName;
                CurrentEventLocation = GlobalSettings.ApplicationSettings.Event.EventLocation;

                GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;

                switch (GlobalSettings.CurrentPageID)
                {
                    case 2:
                        Application.Current.MainWindow.Content = new EventPage(GlobalSettings.ApplicationSettings.Username, GlobalSettings.ApplicationSettings.AuthToken);
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
            Application.Current.MainWindow.Content = new EventPage(GlobalSettings.ApplicationSettings.Username,GlobalSettings.ApplicationSettings.AuthToken);
        }
    }
}
