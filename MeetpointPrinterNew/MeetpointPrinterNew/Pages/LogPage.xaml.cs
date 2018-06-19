using MeetpointPrinterNew.CustomControls;
using MeetpointPrinterNew.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MeetpointPrinterNew.Pages
{
    /// <summary>
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
       
        private UserSettings _settings;

        private App _currentApp = ((App)Application.Current);

        private BitmapImage _imgNameSrc = new BitmapImage(new Uri("/Images/icon_badge.png", UriKind.Relative));

        private BitmapImage _imgStatusPrinting = new BitmapImage(new Uri("/Images/icon_more.png", UriKind.Relative));
        private BitmapImage _imgStatusFailed = new BitmapImage(new Uri("/Images/icon_failed.png", UriKind.Relative));
        private BitmapImage _imgStatusSuccessful = new BitmapImage(new Uri("/Images/icon_tick.png", UriKind.Relative));

        public LogPage(UserSettings settings)
        {
            InitializeComponent();
            GlobalSettings.CurrentPageID = 6;

            _settings = settings;

            headerControl.CurrentUser = GlobalSettings.CurrentUser;
            subHeaderControl.EventName = GlobalSettings.CurrentEvent;
            subHeaderControl.EventDateLocation = GlobalSettings.CurrentEventLocation;

   

            foreach (PrintQueueItem pqi in GlobalSettings.PrintQueueItemLog)
            {
                LogControl lc = new LogControl();
                lc.LogUsername = pqi.FirstName+" "+pqi.LastName;
                lc.LogStatus = pqi.Status.ToString();
                lc.ButtonPrintAgainContent = "PRINT AGAIN";
                lc.ButtonPrivewContent = "PREVIEW LABEL";
                lc.Preview_Click += new EventHandler(btnPreview_Click);
                lc.PrintAgain_Click += new EventHandler(btnPrintAgain_Click);
                lc.PrintQueueItem = pqi;

                SetPrintingStatusSource(lc);
                icEventItems.Items.Add(lc);
            }

         
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SettingsPage(_settings);
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            LogControl lc = (LogControl)sender;

            TemplatePreview tp = new TemplatePreview();
            tp.Owner = _currentApp.MainWindow;

            PrivewTemplateLogic(lc.PrintQueueItem, tp);
            tp.ShowDialog();
        }
        private void btnPrintAgain_Click(object sender, EventArgs e)
        {
            //TODO:print again logic bro
        }
        private void PrivewTemplateLogic(PrintQueueItem item, TemplatePreview tp)
        {
            BitmapImage imgBigSrc = new BitmapImage(new Uri("/Images/icon_qr_code_big.png", UriKind.Relative));
            BitmapImage imgSmallSrc = new BitmapImage(new Uri("/Images/icon_qr_code.png", UriKind.Relative));



            tp.tbOptOne.Text = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[0], item);
            tp.tbOptTwo.Text = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[1], item);
            tp.tbOptThree.Text = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[2], item);


            tp.bdrPreview.Width = GlobalSettings.ApplicationSettings.PrinterSetup.LayoutWidth;
            tp.bdrPreview.Height = GlobalSettings.ApplicationSettings.PrinterSetup.LayoutHeight;

            switch (GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate)
            {
                case "cbLayoutQRT":
                    tp.imgQrPreview.Source = imgSmallSrc;
                    SetControlCanvasPosition(tp.imgQrPreview, double.NaN, 0, 0, double.NaN);
                    SetControlCanvasPosition(tp.spDataOptions, 10, double.NaN, double.NaN, double.NaN);
                    break;
                case "cbLayoutQRB":
                    tp.imgQrPreview.Source = imgSmallSrc;
                    SetControlCanvasPosition(tp.imgQrPreview, double.NaN, double.NaN, 0, 0);
                    SetControlCanvasPosition(tp.spDataOptions, 0, double.NaN, double.NaN, double.NaN);
                    break;
                case "cbLayoutHR":
                    tp.imgQrPreview.Source = imgBigSrc;
                    SetControlCanvasPosition(tp.imgQrPreview, double.NaN, double.NaN, 0, double.NaN);
                    SetControlCanvasPosition(tp.spDataOptions, 0, double.NaN, double.NaN, double.NaN);
                    break;
                case "cbLayoutQLT":
                    tp.imgQrPreview.Source = imgSmallSrc;
                    SetControlCanvasPosition(tp.imgQrPreview, 0, 0, double.NaN, double.NaN);
                    SetControlCanvasPosition(tp.spDataOptions, double.NaN, double.NaN, 0, double.NaN);
                    break;
                case "cbLayoutQLB":
                    tp.imgQrPreview.Source = imgSmallSrc;
                    SetControlCanvasPosition(tp.imgQrPreview, 0, 0, double.NaN, double.NaN);
                    SetControlCanvasPosition(tp.spDataOptions, double.NaN, double.NaN, 10, double.NaN);
                    break;
                case "cbLayoutHL":
                    tp.imgQrPreview.Source = imgBigSrc;
                    SetControlCanvasPosition(tp.imgQrPreview, 0, double.NaN, double.NaN, double.NaN);
                    SetControlCanvasPosition(tp.spDataOptions, double.NaN, double.NaN, 0, double.NaN);
                    break;
                case "cbLayoutClean":
                    CanvasControlClearPosition(tp.imgQrPreview);
                    SetControlCanvasPosition(tp.spDataOptions, 0, double.NaN, double.NaN, double.NaN);
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
        private void SetPrintingStatusSource(LogControl lc)
        {
            switch (lc.PrintQueueItem.Status)
            {
                case 0:
                    lc.StatusLogoSource = _imgStatusPrinting;
                    lc.LogStatus = "Printing";
                    break;
                case 1:
                    lc.StatusLogoSource = _imgStatusSuccessful;
                    lc.LogStatus = "Completed";
                    break;
                case 2:
                    lc.StatusLogoSource = _imgStatusFailed;
                    lc.LogStatus = "Failed";
                    break;
            }

        }
       
    }
}
