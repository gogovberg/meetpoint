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

        public UserSettings ApplicationSettings { set; get; }

      
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SaveUserSettings(ApplicationSettings);
            Application.Current.MainWindow.Content = new LoginPage(); 
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SaveUserSettings(ApplicationSettings);

            int SetupPageType = -1;
            if (Application.Current.MainWindow.Content.GetType().Name.Equals("SetupPage"))
            {
                SetupPage sp = (SetupPage)Application.Current.MainWindow.Content;
                SetupPageType = sp.SetupPageType;
            }
            else if(Application.Current.MainWindow.Content.GetType().Name.Equals("SetupPagePrintTemplate"))
            {
                SetupPagePrintTemplate sp = (SetupPagePrintTemplate)Application.Current.MainWindow.Content;
                SetupPageType = sp.SetupPageType;
            }
            
            switch (SetupPageType)
            {
                case 0:
                    Application.Current.MainWindow.Content = new SetupPage(ApplicationSettings,1);
                    break;
                case 1:
                    Application.Current.MainWindow.Content = new SetupPagePrintTemplate();
                    break;
                case 2:
                    Application.Current.MainWindow.Content = new SettingsPage(ApplicationSettings);
                    break;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ApplicationSettings = Helpers.ReadUserSettings(CurrentUser);
            CurrentUser = ApplicationSettings.Username;
            CurrentEvent = ApplicationSettings.Event.EventName;
            CurrentEventLocation = ApplicationSettings.Event.EventLocation;
           
            int SetupPageType = -1;
            if (Application.Current.MainWindow.Content.GetType().Name.Equals("SetupPage"))
            {
                SetupPage sp = (SetupPage)Application.Current.MainWindow.Content;
                SetupPageType = sp.SetupPageType;
            }
            else if (Application.Current.MainWindow.Content.GetType().Name.Equals("SetupPagePrintTemplate"))
            {
                SetupPagePrintTemplate sp = (SetupPagePrintTemplate)Application.Current.MainWindow.Content;
                SetupPageType = sp.SetupPageType;
            }
           
            switch (SetupPageType)
            {
                case 0:
                    Application.Current.MainWindow.Content = new EventPage(ApplicationSettings.Username, ApplicationSettings.AuthToken);
                    break;
                case 1:
                    Application.Current.MainWindow.Content = new SetupPage(ApplicationSettings,0);
                    break;
                case 2:
                    Application.Current.MainWindow.Content = new SetupPage(ApplicationSettings,1);
                    break;
               
                }
        }
    }
}
