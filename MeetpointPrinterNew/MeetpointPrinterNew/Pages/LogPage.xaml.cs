using MeetpointPrinterNew.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
        private App _currentApp = ((App)Application.Current);
        private UserSettings _settings;
        public LogPage(UserSettings settings)
        {
    

            InitializeComponent();

            _settings = settings;
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
                lc.ButtonPrivewContent = "PREVIEW CONTENT";
                icEventItems.Items.Add(lc);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SettingsPage(_settings);
        }
    }
}
