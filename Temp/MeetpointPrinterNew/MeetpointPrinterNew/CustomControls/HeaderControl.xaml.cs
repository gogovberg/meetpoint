using MeetpointPrinterNew.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace MeetpointPrinterNew.CustomControls
{
    /// <summary>
    /// Interaction logic for HeaderControl.xaml
    /// </summary>
    public partial class HeaderControl : UserControl
    {
        public string CurrentUser
        {
            get { return LblCurrentUser.Content.ToString(); }
            set { LblCurrentUser.Content = value; }
        }
        public ImageSource ApplicationLogo
        {
            get { return imgLogoMpWhite.Source; }
            set { imgLogoMpWhite.Source = value; }
        }

        public HeaderControl()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
            Application.Current.MainWindow.Content = new LoginPage();

        }

        private void btnEvents_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
            GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;
            Application.Current.MainWindow.Content = new EventPage(GlobalSettings.ApplicationSettings.Username, GlobalSettings.ApplicationSettings.AuthToken, GlobalSettings.ApplicationSettings.Event.EventID.ToString());
        }
    }
}
