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
       
        public string CurrentPage { set; get; }
        public string CurrentUser { set; get; }

        public UserSettings ApplicationSettings { set; get; }

        
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {

           Application.Current.MainWindow.Content = new LoginPage(); 

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            PrintSetupPage sp = (PrintSetupPage)Application.Current.MainWindow.Content;
            
            switch(sp.SetupPageType)
            {
                case 0:
                    Application.Current.MainWindow.Content = new PrintSetupPage(1);
                    break;
                case 1:
                    Application.Current.MainWindow.Content = new PrintSetupPage(2);
                    break;
                case 2:
                    Application.Current.MainWindow.Content = new SettingsPage();
                    break;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            PrintSetupPage sp = (PrintSetupPage)Application.Current.MainWindow.Content;

            switch (sp.SetupPageType)
            {
                case 0:
                    Application.Current.MainWindow.Content = new EventPage(this.ApplicationSettings.Username, this.ApplicationSettings.AuthToken);
                    break;
                case 1:
                    Application.Current.MainWindow.Content = new PrintSetupPage(0);
                    break;
                case 2:
                    Application.Current.MainWindow.Content = new PrintSetupPage(1);
                    break;
            }
        }
    }
}
