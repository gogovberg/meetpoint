using MeetpointPrinter.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MeetpointPrinter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
   
    public partial class App : Application
    {
        private LoginPage _objLogin;
        public string CurrentPage { set; get; }
        public string CurrentUser { set; get; }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    _objLogin = new LoginPage();
                    _objLogin.Show();
                    _objLogin.Activate();
                    CloseAllWindows();
                    break;
            }
          

        }
        private void CloseAllWindows()
        {

            foreach(Window w in App.Current.Windows)
            {
                if(!w.Name.Equals("Login"))
                {
                    w.Close();
                }
                
            }
                
        }
    }
}
