using MeetpointPrinterNew.CustomControls;
using MeetpointPrinterNew.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;

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

            //for (int i=1; i<10; i++)
            //{
            //    PrintQueueItem pqi = new PrintQueueItem();
            //    pqi.FirstName = "Test name " + i;
            //    pqi.LastName = "Test last name " + i;
            //    pqi.JobPosition = "Job position " + i;
            //    pqi.Status = i % 3;
            //    pqi.Country = "Country " + i;
            //    pqi.PrintUserID = i;
            //    pqi.EventPosition = "Event position " + i;
              
            //}

            foreach(PrintQueueItem pqi in GlobalSettings.PrintQueueItemLog)
            {
                LogControl lc = new LogControl();
                lc.LogUsername = pqi.FirstName + " " + pqi.LastName;
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
            tp.PrivewTemplateLogic(lc.PrintQueueItem);
            tp.ShowDialog();
        }
        private void btnPrintAgain_Click(object sender, EventArgs e)
        {
            LogControl lc = (LogControl)sender;
            //TODO:print again logic bro

            if (GlobalSettings.IsPrinterOnline)
            {
                foreach (DiscoveredUsbPrinter usbPrinter in UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter()))
                {
                    Connection connection = usbPrinter.GetConnection();
                    connection.Open();
                    ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);

                    if(lc!=null)
                    {
                        PrintQueueItem item = lc.PrintQueueItem;
                        if(item != null)
                        {
                            switch (GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate)
                            {
                                case "cbLayoutHR":
                                    Helpers.PrintLabelQRight(item, printer);
                                    break;
                                case "cbLayoutClean":
                                    Helpers.PrintLabelClear(item, printer);
                                    break;
                                default:
                                    Helpers.PrintLabelQRight(item, printer);
                                    break;
                            }

                            item.Status = 1;
                        }
                    }

                    connection.Close();
                }
            }
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
