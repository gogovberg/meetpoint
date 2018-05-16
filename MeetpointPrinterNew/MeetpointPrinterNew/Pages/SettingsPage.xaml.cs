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
    /// Interaction logic for PrintSettingsPage.xaml
    /// </summary>
    /// 

    public partial class SettingsPage : Page
    {
        private App _currentApp = ((App)Application.Current);
        private UserSettings _settings;
        public SettingsPage(UserSettings settings)
        {
            _settings = settings;
            InitializeComponent();

            _currentApp.ApplicationSettings.Event = settings.Event;
            _currentApp.CurrentEvent = settings.Event.EventName;
            _currentApp.CurrentEventLocation =  settings.Event.EventStartDate.ToShortDateString()+" "+
                                                settings.Event.EventEndDate.ToShortDateString()+" "+
                                                settings.Event.EventLocation;
            foreach (var item in _settings.Accounts.Account)
            {
                icAccountItem.Items.Add(item);
            }
            
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new LoginPage();
        }

        private void btneditPrinter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditAccount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditTemplate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
