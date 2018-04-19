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

        BitmapImage imgBigSrc = new BitmapImage(new Uri("/Images/icon_qr_code_big.png", UriKind.Relative));
        BitmapImage imgSmallSrc = new BitmapImage(new Uri("/Images/icon_qr_code.png", UriKind.Relative));

        private double _borderWidth = 0;
        private double _borderHeight = 0;

        public int SetupPageType { get { return this._pageType; } }
        
        public SetupPagePrintTemplate()
        {
            _pageType = 2;
            InitializeComponent();
            lblPrintingDevice.Content = "SET LABEL TEMPLATE";

            _dataOptions = new List<CheckBox>();

            CanvasControlClearPosition(imgQrPreview);
            CanvasControlClearPosition(spDataOptions);

            _borderWidth = 279;
            _borderHeight = 108;
        }

        private void cbSizeOne_Checked(object sender, RoutedEventArgs e)
        {
            cbSizeTwo.IsChecked = false;
            cbSizeThree.IsChecked = false;

            bdrPreview.Width = _borderWidth * 0.6;
            bdrPreview.Height = _borderHeight * 0.6;
        }

        private void cbSizeTwo_Checked(object sender, RoutedEventArgs e)
        {
            cbSizeOne.IsChecked = false;
            cbSizeThree.IsChecked = false;
            bdrPreview.Width = _borderWidth * 0.8;
            bdrPreview.Height = _borderHeight * 0.8;
        }

        private void cbSizeThree_Checked(object sender, RoutedEventArgs e)
        {
            cbSizeOne.IsChecked = false;
            cbSizeTwo.IsChecked = false;
            bdrPreview.Width = _borderWidth * 1.0;
            bdrPreview.Height = _borderHeight * 1.0;
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
        }

        private void cbOption_Checked(object sender, RoutedEventArgs e)
        {
            DataOptionsLogic(sender);
        }

        private void cbOption_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            int removeIndex = -1;
           
            for(int i=0; i<_dataOptions.Count; i++)
            {
                if(_dataOptions[i].Name.Equals(cb.Name))
                {
                    removeIndex = i;
                }
            }
            if(removeIndex>=0)
            {
                _dataOptions.RemoveAt(removeIndex);
            }
            SwitchDataOptions();
        }

        private void DataOptionsLogic(Object sender)
        {
            CheckBox cb = (CheckBox)sender;
            if (!OptionExists(cb.Name))
            {
                if (_dataOptions.Count < 3)
                {
                    _dataOptions.Insert(0, cb);
                    SwitchDataOptions();
                }
                else
                {
                    CheckBox tempCb = _dataOptions[2];
                    _dataOptions.RemoveAt(2);
                    _dataOptions.Insert(0, cb);
                    tempCb.IsChecked = false;
                }
            }
        }

        private bool OptionExists(string cbName)
        {
           foreach(CheckBox cb in _dataOptions)
            {
                if(cb.Name.Equals(cbName))
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
                    tbOptOne.Text = _dataOptions[0].Content.ToString();
                    tbOptTwo.Text = "Option two";
                    tbOptThree.Text = "Option three";
                    break;
                case 2:
                    tbOptOne.Text = _dataOptions[1].Content.ToString();
                    tbOptTwo.Text = _dataOptions[0].Content.ToString();
                    tbOptThree.Text = "Option three";
                    break;
                case 3:
                    tbOptOne.Text = _dataOptions[2].Content.ToString();
                    tbOptTwo.Text = _dataOptions[1].Content.ToString();
                    tbOptThree.Text = _dataOptions[0].Content.ToString();
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
