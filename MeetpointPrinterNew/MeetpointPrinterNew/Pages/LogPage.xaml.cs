using MeetpointPrinterNew.CustomControls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MeetpointPrinterNew.Pages
{
    /// <summary>
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
       
        private UserSettings _settings;
      

        public LogPage(UserSettings settings)
        {
            InitializeComponent();
            GlobalSettings.CurrentPageID = 6;

            _settings = settings;

            headerControl.CurrentUser = GlobalSettings.CurrentUser;
            subHeaderControl.EventName = GlobalSettings.CurrentEvent;
            subHeaderControl.EventDateLocation = GlobalSettings.CurrentEventLocation;

            BitmapImage imgNameSrc = new BitmapImage(new Uri("/Images/icon_badge.png", UriKind.Relative));
            BitmapImage imgStatusSrc = new BitmapImage(new Uri("/Images/icon_more.png", UriKind.Relative));

            for (int i=1; i<=10; i++)
            {
                LogControl lc = new LogControl();
                lc.LogUsername = "username "+i;
                lc.LogStatus = "status " + i;
                lc.UsernameLogoSource = imgNameSrc;
                lc.StatusLogoSource = imgStatusSrc;
                lc.ButtonPrintAgainContent = "PRINT AGAIN";
                lc.ButtonPrivewContent = "PREVIEW LABEL";
                icEventItems.Items.Add(lc);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SettingsPage(_settings);
        }
    }
}
