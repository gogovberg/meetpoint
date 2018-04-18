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
        private int _pageType = -1;
        private List<CheckBox> _dataOptions;

        public int SetupPageType { get { return this._pageType; } }
        
        public SetupPagePrintTemplate()
        {
            _pageType = 2;
            InitializeComponent();
            lblPrintingDevice.Content = "SET LABEL TEMPLATE";

            _dataOptions = new List<CheckBox>();
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


        private void cbName_Checked(object sender, RoutedEventArgs e)
        {
            DataOptionsLogic(sender);
        }
        private void cbSurName_Checked(object sender, RoutedEventArgs e)
        {
            DataOptionsLogic(sender);
        }

        private void cbCompanyName_Checked(object sender, RoutedEventArgs e)
        {
            DataOptionsLogic(sender);
        }

        private void cbFunctionName_Checked(object sender, RoutedEventArgs e)
        {
            DataOptionsLogic(sender);
        }

        private void cbCountryName_Checked(object sender, RoutedEventArgs e)
        {
            DataOptionsLogic(sender);
        }

        private void cbGroupName_Checked(object sender, RoutedEventArgs e)
        {
            DataOptionsLogic(sender);

        }

        private void DataOptionsLogic(Object sender)
        {
            CheckBox cb = (CheckBox)sender;
            if (_dataOptions.Count < 3)
            {
                _dataOptions.Insert(0, cb);
            }
            else
            {
                _dataOptions[2].IsChecked = false;
                _dataOptions.RemoveAt(2);
                _dataOptions.Insert(0, cb);
            }
        }
    }
}
