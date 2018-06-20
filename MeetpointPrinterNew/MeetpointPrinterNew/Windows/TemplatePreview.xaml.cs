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
using System.Windows.Shapes;

namespace MeetpointPrinterNew.Windows
{
    /// <summary>
    /// Interaction logic for TemplatePriview.xaml
    /// </summary>
    public partial class TemplatePreview : Window
    {
        public TemplatePreview()
        {
            InitializeComponent();

        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

         private void PrivewTemplateLogic()
        {
            //BitmapImage imgBigSrc = new BitmapImage(new Uri("/Images/icon_qr_code_big.png", UriKind.Relative));
            //BitmapImage imgSmallSrc = new BitmapImage(new Uri("/Images/icon_qr_code.png", UriKind.Relative));

            //tbOptOne.Text = GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[0];
            //tbOptTwo.Text = GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[1];
            //tbOptThree.Text = GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[2];


            //bdrPreview.Width = GlobalSettings.ApplicationSettings.PrinterSetup.LayoutWidth;
            //bdrPreview.Height = GlobalSettings.ApplicationSettings.PrinterSetup.LayoutHeight;

            //switch (GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate)
            //{
            //    case "cbLayoutQRT":
            //        imgQrPreview.Source = imgSmallSrc;
            //        SetControlCanvasPosition(imgQrPreview, double.NaN, 0, 0, double.NaN);
            //        SetControlCanvasPosition(spDataOptions, 10, double.NaN, double.NaN, double.NaN);
            //        break;
            //    case "cbLayoutQRB":
            //        imgQrPreview.Source = imgSmallSrc;
            //        SetControlCanvasPosition(imgQrPreview, double.NaN, double.NaN, 0, 0);
            //        SetControlCanvasPosition(spDataOptions, 0, double.NaN, double.NaN, double.NaN);
            //        break;
            //    case "cbLayoutHR":
            //        imgQrPreview.Source = imgBigSrc;
            //        SetControlCanvasPosition(imgQrPreview, double.NaN, double.NaN, 0, double.NaN);
            //        SetControlCanvasPosition(spDataOptions, 0, double.NaN, double.NaN, double.NaN);
            //        break;
            //    case "cbLayoutQLT":
            //        imgQrPreview.Source = imgSmallSrc;
            //        SetControlCanvasPosition(imgQrPreview, 0, 0, double.NaN, double.NaN);
            //        SetControlCanvasPosition(spDataOptions, double.NaN, double.NaN, 0, double.NaN);
            //        break;
            //    case "cbLayoutQLB":
            //        imgQrPreview.Source = imgSmallSrc;
            //        SetControlCanvasPosition(imgQrPreview, 0, 0, double.NaN, double.NaN);
            //        SetControlCanvasPosition(spDataOptions, double.NaN, double.NaN, 10, double.NaN);
            //        break;
            //    case "cbLayoutHL":
            //        imgQrPreview.Source = imgBigSrc;
            //        SetControlCanvasPosition(imgQrPreview, 0, double.NaN, double.NaN, double.NaN);
            //        SetControlCanvasPosition(spDataOptions, double.NaN, double.NaN, 0, double.NaN);
            //        break;
            //    case "cbLayoutClean":
            //        CanvasControlClearPosition(imgQrPreview);
            //        SetControlCanvasPosition(spDataOptions, 0, double.NaN, double.NaN, double.NaN);
            //        break;
            //}
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
