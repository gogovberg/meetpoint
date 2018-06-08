using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MeetpointPrinterNew.Pages
{
    /// <summary>
    /// Interaction logic for SetupPagePrintTemplate.xaml
    /// </summary>
    public partial class SetupPagePrintTemplate : Page
    {
       
        private UserSettings _settings;
        private int _pageType = -1;
        private List<string> _dataOptions;

        BitmapImage imgBigSrc = new BitmapImage(new Uri("/Images/big_qr.png", UriKind.Relative));
        BitmapImage imgSmallSrc = new BitmapImage(new Uri("/Images/big_qr.png", UriKind.Relative));

        private double _borderWidth = 376;
        private double _borderHeight = 156;
        private double _xPositionOffset = 0;
        private double _yPositionOffset = 0;

        public int SetupPageType { get { return this._pageType; } }

        private bool _isOnLoadChecked;
        
        public SetupPagePrintTemplate(UserSettings settings)
        {
      
            InitializeComponent();
            GlobalSettings.CurrentPageID = 4;
            lblPrintingDevice.Content = "SET LABEL TEMPLATE";
            _settings = settings;
            _isOnLoadChecked = false;
            _pageType = 2;
            _dataOptions = new List<string>();

           

            CanvasControlClearPosition(imgQrPreview);
            CanvasControlClearPosition(spDataOptions);

            SetPriviewBorderSize(1);



            switch (GlobalSettings.ApplicationSettings.PrinterSetup.LayoutSizeID)
            {
                case "cbSizeOne":
                    cbSizeOne.IsChecked = true;
                    break;
                case "cbSizeTwo":
                    cbSizeTwo.IsChecked = true;
                    break;

                default:
                    cbSizeOne.IsChecked = false;
                    cbSizeTwo.IsChecked = false;
               
                    break;
            }


            switch(GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate)
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
                case "cbLayoutClean":
                    cbLayoutClean.IsChecked = true;
                    break;
                default:
                    cbLayoutQRT.IsChecked = false;
                    cbLayoutQRB.IsChecked = false;
                    cbLayoutHR.IsChecked = false;
                    cbLayoutQLT.IsChecked = false;
                    cbLayoutQLB.IsChecked = false;
                    cbLayoutHL.IsChecked = false;
                    cbLayoutClean.IsChecked = false;
                    break;
            }
            _dataOptions = GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption;

            if(_dataOptions.Count<3)
            {
                _dataOptions.Add("");
                _dataOptions.Add("");
                _dataOptions.Add("");
            }

            _isOnLoadChecked = true;
            foreach (string cb in _dataOptions)
            {
                if(!string.IsNullOrEmpty(cb))
                {
                    ((CheckBox)this.FindName(cb)).IsChecked = true;
                }
              
            }
            SwitchDataOptions();
            _isOnLoadChecked = false;



        }

        private void cbSizeOne_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cbSizeTwo.IsChecked = false;


            SetPriviewBorderSize(0.6);
            SetPriviewControlsSize();

            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutSizeID = cb.Name;
            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutWidth = bdrPreview.Width;
            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutHeight = bdrPreview.Height;
        }

        private void cbSizeTwo_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cbSizeOne.IsChecked = false;

            SetPriviewBorderSize(0.8);
            SetPriviewControlsSize();

            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutSizeID = cb.Name;
            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutWidth = bdrPreview.Width;
            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutHeight = bdrPreview.Height;
        }

     
        private void cbLayoutQRT_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;

            cbLayoutHL.IsChecked = false;
            cbLayoutHR.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutClean.IsChecked = false;

            SetPriviewControlsSize();
            imgQrPreview.Source = imgSmallSrc;
            SetControlCanvasPosition(imgQrPreview, double.NaN, _xPositionOffset, _xPositionOffset, double.NaN);
            SetControlCanvasPosition(spDataOptions, 10, double.NaN, double.NaN, double.NaN);
           
        }

        private void cbLayoutQRB_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;

            cbLayoutHL.IsChecked = false;
            cbLayoutHR.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRT.IsChecked = false;
            cbLayoutClean.IsChecked = false;

            SetPriviewControlsSize();
            imgQrPreview.Source = imgSmallSrc;
            SetControlCanvasPosition(imgQrPreview, double.NaN, double.NaN, _xPositionOffset, _xPositionOffset);
            SetControlCanvasPosition(spDataOptions, 10, double.NaN, double.NaN, double.NaN);

         

        }

        private void cbLayoutHR_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;

            cbLayoutHL.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;
            cbLayoutClean.IsChecked = false;

            SetPriviewControlsSize();
            imgQrPreview.Source = imgBigSrc;
            SetControlCanvasPosition(imgQrPreview, double.NaN, double.NaN, _xPositionOffset, double.NaN);
            SetControlCanvasPosition(spDataOptions, 10, double.NaN, double.NaN, double.NaN);

        
        }

        private void cbLayoutQLT_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;

            cbLayoutHL.IsChecked = false;
            cbLayoutHR.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;
            cbLayoutClean.IsChecked = false;

            SetPriviewControlsSize();
            imgQrPreview.Source = imgSmallSrc;
            SetControlCanvasPosition(imgQrPreview, _xPositionOffset, _xPositionOffset, double.NaN, double.NaN);
            SetControlCanvasPosition(spDataOptions, double.NaN, double.NaN, 10, double.NaN);

           
        }

        private void cbLayoutQLB_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;

            cbLayoutHL.IsChecked = false;
            cbLayoutHR.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;
            cbLayoutClean.IsChecked = false;

            SetPriviewControlsSize();
            imgQrPreview.Source = imgSmallSrc;
            SetControlCanvasPosition(imgQrPreview, _xPositionOffset, double.NaN, double.NaN, _xPositionOffset);
            SetControlCanvasPosition(spDataOptions, double.NaN, double.NaN, 10, double.NaN);

           

        }

        private void cbLayoutHL_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;

            cbLayoutHR.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;
            cbLayoutClean.IsChecked = false;

            SetPriviewControlsSize();
            imgQrPreview.Source = imgBigSrc;
            SetControlCanvasPosition(imgQrPreview, _xPositionOffset, double.NaN, double.NaN, double.NaN);
            SetControlCanvasPosition(spDataOptions, double.NaN, double.NaN, 10, double.NaN);

       
        }

        private void cbLayoutClean_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate = cb.Name;

            cbLayoutHR.IsChecked = false;
            cbLayoutHL.IsChecked = false;
            cbLayoutQLB.IsChecked = false;
            cbLayoutQLT.IsChecked = false;
            cbLayoutQRB.IsChecked = false;
            cbLayoutQRT.IsChecked = false;

            SetPriviewControlsSize();
            CanvasControlClearPosition(imgQrPreview);
            SetControlCanvasPosition(spDataOptions, 10, double.NaN, double.NaN, double.NaN);
          
        }

        private void cbOption_Checked(object sender, RoutedEventArgs e)
        {
            if(!_isOnLoadChecked)
            {
                DataOptionsLogic(sender);
                GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption = _dataOptions;
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
                    _dataOptions[removeIndex] = "";
                }
                SwitchDataOptions();

                GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption = _dataOptions;
            }
          
        }

        private void DataOptionsLogic(Object sender)
        {
            CheckBox cb = (CheckBox)sender;
            if (!OptionExists(cb.Name))
            {
                for (int i = 0; i < _dataOptions.Count; i++)
                {
                    if (string.IsNullOrEmpty(_dataOptions[i]))
                    {
                        _dataOptions[i] = cb.Name;
                        break;
                    }
                }
                SwitchDataOptions();

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
            tbOptOne.Text = string.IsNullOrEmpty(_dataOptions[0]) ? "" : ((CheckBox)this.FindName(_dataOptions[0])).Content.ToString(); 
            tbOptTwo.Text = string.IsNullOrEmpty(_dataOptions[1]) ? "" : ((CheckBox)this.FindName(_dataOptions[1])).Content.ToString(); 
            tbOptThree.Text = string.IsNullOrEmpty(_dataOptions[2]) ? "" : ((CheckBox)this.FindName(_dataOptions[2])).Content.ToString(); 
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

        private void SetPriviewBorderSize(double borderScale)
        {
            bdrPreview.Width = _borderWidth * borderScale;
            bdrPreview.Height = _borderHeight * borderScale;
            _xPositionOffset = bdrPreview.Width * 0.1;
            _yPositionOffset = bdrPreview.Height * 0.1;
        }

        private void SetPriviewControlsSize()
        {

            double imgWidth = 0;
            double imgHeight = 0;

            switch (GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate)
            {
                case "cbLayoutQRT":
                case "cbLayoutQRB":
                case "cbLayoutQLT":
                case "cbLayoutQLB":
                    imgWidth  = (bdrPreview.Width / 4);
                    imgHeight = (bdrPreview.Height / 4);
                    break;
                case "cbLayoutHR":
                case "cbLayoutHL":
                    imgWidth  = (bdrPreview.Width / 2);
                    imgHeight = (bdrPreview.Height / 2);
                    break;

                default:
                    imgWidth = _borderWidth;
                    imgHeight = _borderHeight;
                    break;
            }
            spDataOptions.Width = (bdrPreview.Width / 2);
            spDataOptions.Height = (bdrPreview.Height / 2);
            imgQrPreview.Width = imgWidth;
            imgQrPreview.Height = imgHeight;
        }
    }
}
