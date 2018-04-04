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

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {

           Application.Current.MainWindow.Content = new LoginPage(); 

        }
      
    }
}
