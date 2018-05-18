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
        private App _currentApp = (Application.Current as App);
        private UserSettings _settings;
        private int _pageType = -1;
        private List<string> _dataOptions;

        BitmapImage imgBigSrc = new BitmapImage(new Uri("/Images/icon_qr_code_big.png", UriKind.Relative));
        BitmapImage imgSmallSrc = new BitmapImage(new Uri("/Images/icon_qr_code.png", UriKind.Relative));

        private double _borderWidth = 0;
        private double _borderHeight = 0;

        public int SetupPageType { get { return this._pageType; } }

        private bool _isOnLoadChecked;
        
        public SetupPagePrintTemplate(UserSettings settings)
        {
            _settings = settings;
            _isOnLoadChecked = false;
            _pageType = 2;
            InitializeComponent();
            lblPrintingDevice.Content = "SET LABEL TEMPLATE";

            _dataOptions = new List<string>();

            CanvasControlClearPosition(imgQrPreview);
            CanvasControlClearPosition(spDataOptions);
            _borderWidth = 279;
            _borderHeight = 108;
         
            switch(_currentApp.ApplicationSettings.PrinterSetup.LayoutSizeID)
            {
                case "cbSizeOne":
                    cbSizeTwo.IsChecked = true;
                    break;
                case "cbSizeTwo":
                    cbSizeTwo.IsChecked = true;
                    break;
                case "cbSizeThree":
                    cbSizeThree.IsChecked = true;
                    break;
                default:
                    cbSizeOne.IsChecked = false;
                    cbSizeTwo.IsChecked = false;
                    cbSizeThree.IsChecked = false;
                    break;
            }


            switch(_currentApp.ApplicationSettings.PrinterSetup.LayoutTemplate)
            {
                case "cbLayoutQRT":
                    cbLayoutQRT.IsChecked = true;
                    break;
                case "cbLayoutQRB":
                    cbLayoutQRB.IsChecked = true;
                    break;
                case "cbLayoutHR":
                    cbLayoutHR.IsChecked = true;
                    break;
                case "cbLayoutQLT":
                    cbLayoutQLT.IsChecked = true;
                    break;
                case "cbLayoutQLB":
                    cbLayoutQLB.IsChecked = true;
                    break;
                case "cbLayoutHL":
                    cbLayoutHL.IsChecked = true;
                    break;
                default:
                    cbLayoutQRT.IsChecked = false;
                    cbLayoutQRB.IsChecked = false;
                    cbLayoutHR.IsChecked = false;
                    cbLayoutQLT.IsChecked = false;
                    cbLayoutQLB.IsChecked = false;
                    cbLayoutHL.IsChecked = false;
                    break;
            }
            _dataOptions = _currentApp.ApplicationSettings.PrinterSetup.DataOptions.DataOption;
            _isOnLoadChecked = true;
            foreach (string cb in _dataOptions)
            {
                ((CheckBox)this.FindName(cb)).IsChecked = true;
            }
            SwitchDataOptions();
            _isOnLoadChecked = false;



        }

        private void cbSizeOne_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cbSizeTwo.IsChecked = false;
            cbSizeThree.IsChecked = false;

            bdrPreview.Width = _borderWidth * 0.6;
            bdrPreview.Height = _borderHeight * 0.6;

            _currentApp.ApplicationSettings.PrinterSetup.LayoutSizeID = cb.Name;
            _currentApp.ApplicationSettings.PrinterSetup.LayoutWidth = bdrPreview.Width;
            _currentApp.ApplicationSettings.PrinterSetup.LayoutHeight = bdrPreview.Height;
        }

        private void cbSizeTwo_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cbSizeOne.IsChecked = false;
            cbSizeThree.IsChecked = false;
            bdrPreview.Width = _borderWidth * 0.8;
            bdrPreview.Height = _borderHeight * 0.8;

            _currentApp.ApplicationSettings.PrinterSetup.LayoutSizeID = cb.Name;
            _currentApp.ApplicationSettings.PrinterSetup.LayoutWidth = bdrPreview.Width;
            _currentApp.ApplicationSettings.PrinterSetup.LayoutHeight = bdrPreview.Height;
        }

        private void cbSizeThree_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cbSizeOne.IsChecked = false;
            cbSizeTwo.IsChecked = false;
            bdrPreview.Width = _borderWidth * 1.0;
            bdrPreview.Height = _borderHeight * 1.0;

            _currentApp.ApplicationSettings.PrinterSetup.LayoutSizeID = cb.Name;
            _currentApp.ApplicationSettings.PrinterSetup.LayoutWidth = bdrPreview.Width;
            _currentApp.ApplicationSettings.PrinterSetup.LayoutHeight = bdrPreview.Height;
        }

        private void cbLayoutQRT_Checked(object sender, RoutedEventArgs e)
        {
         
            cbLayoutHL.IsChecked = false;
            cbLayoutHR.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;

            imgQrPreview.Source = imgSmallSrc;
            SetControlCanvasPosition(imgQrPreview, double.NaN, 10, 10, double.NaN);
            SetControlCanvasPosition(spDataOptions, 10, double.NaN, double.NaN, double.NaN);
            CheckBox cb = (CheckBox)sender;
            _currentApp.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;
        }

        private void cbLayoutQRB_Checked(object sender, RoutedEventArgs e)
        {
            cbLayoutHL.IsChecked = false;
            cbLayoutHR.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRT.IsChecked = false;

            imgQrPreview.Source = imgSmallSrc;
            SetControlCanvasPosition(imgQrPreview, double.NaN, double.NaN, 10, 10);
            SetControlCanvasPosition(spDataOptions, 10, double.NaN, double.NaN, double.NaN);

            CheckBox cb = (CheckBox)sender;
            _currentApp.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;

        }

        private void cbLayoutHR_Checked(object sender, RoutedEventArgs e)
        {
            cbLayoutHL.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;

            imgQrPreview.Source = imgBigSrc;
            SetControlCanvasPosition(imgQrPreview, double.NaN, double.NaN, 10, double.NaN);
            SetControlCanvasPosition(spDataOptions, 10, double.NaN, double.NaN, double.NaN);

            CheckBox cb = (CheckBox)sender;
            _currentApp.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;
        }

        private void cbLayoutQLT_Checked(object sender, RoutedEventArgs e)
        {
            cbLayoutHL.IsChecked = false;
            cbLayoutHR.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;

            imgQrPreview.Source = imgSmallSrc;
            SetControlCanvasPosition(imgQrPreview, 10, 10, double.NaN, double.NaN);
            SetControlCanvasPosition(spDataOptions, double.NaN, double.NaN, 10, double.NaN);

            CheckBox cb = (CheckBox)sender;
            _currentApp.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;
        }

        private void cbLayoutQLB_Checked(object sender, RoutedEventArgs e)
        {
            cbLayoutHL.IsChecked = false;
            cbLayoutHR.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;

            imgQrPreview.Source = imgSmallSrc;
            SetControlCanvasPosition(imgQrPreview, 10, double.NaN, double.NaN, 10);
            SetControlCanvasPosition(spDataOptions, double.NaN, double.NaN, 10, double.NaN);

            CheckBox cb = (CheckBox)sender;
            _currentApp.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;

        }

        private void cbLayoutHL_Checked(object sender, RoutedEventArgs e)
        {

            cbLayoutHR.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;

            imgQrPreview.Source = imgBigSrc;
            SetControlCanvasPosition(imgQrPreview, 10, double.NaN, double.NaN, double.NaN);
            SetControlCanvasPosition(spDataOptions, double.NaN, double.NaN, 10, double.NaN);

            CheckBox cb = (CheckBox)sender;
            _currentApp.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;
        }

        private void cbOption_Checked(object sender, RoutedEventArgs e)
        {
            if(!_isOnLoadChecked)
            {
                DataOptionsLogic(sender);
                _currentApp.ApplicationSettings.PrinterSetup.DataOptions.DataOption = _dataOptions;
            }
        }

        private void cbOption_Unchecked(object sender, RoutedEventArgs e)
        {
            if(!_isOnLoadChecked)
            {
                CheckBox cb = (CheckBox)sender;
                int removeIndex = -1;

                for (int i = 0; i < _dataOptions.Count; i++)
                {
                    if (_dataOptions[i].Equals(cb.Name))
                    {
                        removeIndex = i;
                    }
                }
                if (removeIndex >= 0)
                {
                    _dataOptions.RemoveAt(removeIndex);
                }
                SwitchDataOptions();

                _currentApp.ApplicationSettings.PrinterSetup.DataOptions.DataOption = _dataOptions;
            }
          
        }

        private void DataOptionsLogic(Object sender)
        {
            CheckBox cb = (CheckBox)sender;
            if (!OptionExists(cb.Name))
            {
                if (_dataOptions.Count < 3)
                {
                    _dataOptions.Insert(0, cb.Name);
                    SwitchDataOptions();
                }
                else
                {
                    CheckBox tempCb = (CheckBox)this.FindName(_dataOptions[2]);
                    if(tempCb!=null)
                    {
                        _dataOptions.RemoveAt(2);
                        _dataOptions.Insert(0, cb.Name);
                        tempCb.IsChecked = false;
                    }
                 
                }
            }
        }
        private bool OptionExists(string cbName)
        {
           foreach(string cb in _dataOptions)
            {
                if(cb.Equals(cbName))
                {
                    return true;
                }
            }

            return false;
        }

        private void SwitchDataOptions()
        {
            switch (_dataOptions.Count)
            {
                case 1:
                    tbOptOne.Text = ((CheckBox)this.FindName(_dataOptions[0])).Content.ToString();
                    tbOptTwo.Text = "Option two";
                    tbOptThree.Text = "Option three";
                    break;
                case 2:
                    tbOptOne.Text = ((CheckBox)this.FindName(_dataOptions[1])).Content.ToString();
                    tbOptTwo.Text = ((CheckBox)this.FindName(_dataOptions[0])).Content.ToString();
                    tbOptThree.Text = "Option three";
                    break;
                case 3:
                    tbOptOne.Text = ((CheckBox)this.FindName(_dataOptions[2])).Content.ToString();
                    tbOptTwo.Text = ((CheckBox)this.FindName(_dataOptions[1])).Content.ToString();
                    tbOptThree.Text = ((CheckBox)this.FindName(_dataOptions[0])).Content.ToString();
                    break;
                default:
                    tbOptOne.Text = "Option one";
                    tbOptTwo.Text = "Option two";
                    tbOptThree.Text = "Option three";
                    break;
            }
        }

        private void CanvasControlClearPosition(UIElement control)
        {

            SetControlCanvasPosition(control, double.NaN, double.NaN, double.NaN, double.NaN);
            control.Visibility = Visibility.Hidden;
        }

        private void SetControlCanvasPosition(UIElement control, double left, double top, double right, double bottom)
        {
            Canvas.SetLeft(control, left);
            Canvas.SetTop(control, top);
            Canvas.SetRight(control, right);
            Canvas.SetBottom(control, bottom);
            control.Visibility = Visibility.Visible;
        }

     
    }
}
