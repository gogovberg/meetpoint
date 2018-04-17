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
    /// Interaction logic for SetupPagePrintTemplate.xaml
    /// </summary>
    public partial class SetupPagePrintTemplate : Page
    {
        private int pageType = -1;
        public int SetupPageType { get { return this.pageType; } }
        public SetupPagePrintTemplate()
        {
            pageType = 2;
            InitializeComponent();

            lblPrintingDevice.Content = "SET LABEL TEMPLATE";
        }



        private void cbTwoHunderd_Checked(object sender, RoutedEventArgs e)
        {
            cbHunderd.IsChecked = false;
            cbOneHunderd.IsChecked = false;
        }

        private void cbOneHunderd_Checked(object sender, RoutedEventArgs e)
        {
            cbHunderd.IsChecked = false;
            cbTwoHunderd.IsChecked = false;
        }

        private void cbHunderd_Checked(object sender, RoutedEventArgs e)
        {
            cbOneHunderd.IsChecked = false;
            cbTwoHunderd.IsChecked = false;
        }

        private void cbLayoutHL_Checked(object sender, RoutedEventArgs e)
        {
          
            cbLayoutHR.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;
           
        }

        private void cbLayoutQRT_Checked(object sender, RoutedEventArgs e)
        {
            cbLayoutHL.IsChecked = false;
            cbLayoutHR.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            
        }

        private void cbLayoutQRB_Checked(object sender, RoutedEventArgs e)
        {
            cbLayoutHL.IsChecked = false;
            cbLayoutHR.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRT.IsChecked = false;
        }

        private void cbLayoutHR_Checked(object sender, RoutedEventArgs e)
        {
            cbLayoutHL.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;
        }

        private void cbLayoutQLT_Checked(object sender, RoutedEventArgs e)
        {
            cbLayoutHL.IsChecked = false;
            cbLayoutHR.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;
        }

        private void cbLayoutQLB_Checked(object sender, RoutedEventArgs e)
        {
            cbLayoutHL.IsChecked = false;
            cbLayoutHR.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;
        }
    }
}
