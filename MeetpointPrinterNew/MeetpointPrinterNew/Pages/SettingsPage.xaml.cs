using Com.SharpZebra;
using Com.SharpZebra.Commands;
using Com.SharpZebra.Printing;
using hgi.Environment;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using MeetpointPrinterNew.Windows;
using MeetpointPrinterNew.CustomControls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Graphics;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;

namespace MeetpointPrinterNew.Pages
{
    /// <summary>
    /// Interaction logic for PrintSettingsPage.xaml
    /// </summary>
    /// 

    public partial class SettingsPage : Page
    {
        private App _currentApp = ((App)Application.Current);
        private UserSettings _settings;

        private Timer _timerPrint;
        private object _printObject = new object();
        private double _printTimer;

        private Timer _timerPrinterStatus;
        private object _printerStatusObject = new object();
        private double _printerStatusTimer;

        private System.Printing.PrintQueue _printQueue;
        private List<string> _dataOptions;

        private object _printerPrintingObject = new object();

        private ZebraPrinter _printer;

        public SettingsPage(UserSettings settings)
        {


            InitializeComponent();
            GlobalSettings.CurrentPageID = 5;
            
            if(GlobalSettings.PrintQueueItemLog==null)
            {
                GlobalSettings.PrintQueueItemLog = new List<PrintQueueItem>();
            }

            _settings = settings;
            _printTimer = 10000;
            _printerStatusTimer = 5000;

            headerControl.CurrentUser = GlobalSettings.CurrentUser;
            subHeaderControl.EventName = GlobalSettings.CurrentEvent;
            subHeaderControl.EventDateLocation = GlobalSettings.CurrentEventLocation;
            subHeaderControl.btnBack.Visibility = Visibility.Collapsed;

            string _tempPrintName = _settings.Printer;

            BitmapImage imgAccountSrc = new BitmapImage(new Uri("/Images/icon_printer.png", UriKind.Relative));
            BitmapImage imgPrinterSrc = new BitmapImage(new Uri("/Images/icon_users.png", UriKind.Relative));



            foreach (var item in _settings.Accounts.Account)
            {
                TextBlockImageControl tbic = new TextBlockImageControl();
                tbic.ContentText = item.AccountName;
                tbic.ContentID = item.AccountID;
                tbic.ContentImageSource = imgAccountSrc;
                icAccountItem.Items.Add(tbic);
            }

            _timerPrint = new Timer();
            _timerPrint.Elapsed += new ElapsedEventHandler(PrintQueueLabels);
            _timerPrint.Interval = _printTimer; // 1000 ms => 1 second
            _timerPrint.Enabled = false;


            _timerPrinterStatus = new Timer();
            _timerPrinterStatus.Elapsed += new ElapsedEventHandler(PrinterStatusCheck);
            _timerPrinterStatus.Interval = _printerStatusTimer; // 1000 ms => 1 second
            _timerPrinterStatus.Enabled = true;

            lock (_printObject)
            {
                var server = new LocalPrintServer();
                PrintQueueCollection myPrintQueues = server.GetPrintQueues();
                foreach (System.Printing.PrintQueue queue in myPrintQueues)
                {
                    if (queue.Name.Equals(_tempPrintName))
                    {
                        _printQueue = queue;
                        break;
                    }
                }
            }

            try
            {
                UsbPrinterConnector ps = new UsbPrinterConnector(_settings.Printer);
                if (ps.BeginSend())
                {
                    GlobalSettings.IsPrinterOnline = true;
                    bdrOffline.Visibility = Visibility.Collapsed;
                    bdrOnline.Visibility = Visibility.Visible;
                }
                else
                {
                    GlobalSettings.IsPrinterOnline = false;
                    bdrOnline.Visibility = Visibility.Collapsed;
                    bdrOffline.Visibility = Visibility.Visible;
                }
            }
            catch(Exception ex)
            {
                Debug.Log("MeetpointPrinter", ex.ToString());
                bdrOnline.Visibility = Visibility.Collapsed;
                bdrOffline.Visibility = Visibility.Visible;
                GlobalSettings.IsPrinterOnline = false;
            }
           
           


            tbicPrinter.ContentImageSource = imgPrinterSrc;
            tbicPrinter.ContentID = _tempPrintName;
            tbicPrinter.ContentText = _tempPrintName;

            cbInactive.IsChecked = true;


            _dataOptions = GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption;

            PrivewTemplateLogic();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new LoginPage();
        }

        private void btneditPrinter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SetupPage(_settings, 0);
        }

        private void btnEditAccount_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SetupPage(_settings, 1);
        }

        private void btnEditTemplate_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SetupPagePrintTemplate(_settings);
        }

        private void btnPrintEmpty_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int left = int.Parse(tbLeft.Text);
                int top = int.Parse(tbTop.Text);

                ComboBoxItem jus = (ComboBoxItem)cmbJustification.SelectedItem;
                ComboBoxItem mag = (ComboBoxItem)cmbMagnitude.SelectedItem;

                int justification = int.Parse(jus.Content.ToString());
                int magnitude = int.Parse(mag.Content.ToString());


                char fontType = tbFontType.Text[0];
                int fontSize = int.Parse(tbFontSize.Text);
                int fontLeft = int.Parse(tbFontLeft.Text);
                int fontTop = int.Parse(tbFontTop.Text);
                int fontLineWidth = int.Parse(tbFontLineWidth.Text);
                int fontLines = int.Parse(tbFontLines.Text);
              
                foreach (DiscoveredUsbPrinter usbPrinter in UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter()))
                {

                    Connection connection = usbPrinter.GetConnection();
                    connection.Open();
                    ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);
                   
                    string qrCommand = QRWrite(570, 250, 0, 5, QRErrorCorrection.ULTRA_HIGH, "e47624f6-6ed8-4e8b-991d-f982526ee7f9");
                    string commandOne =   TextWrite(fontType, fontSize, fontLeft, fontTop, fontLineWidth, fontLines, tbContent.Text);
                    //string commandTwo =   TextWrite('0', 55, 160, 100, 600, 3, "ABC  fghijklmnopqrstu");
                    //string commandThree = TextWrite('0', 50, 160, 160, 600, 3, "ABC  fghijklmnopqrstu");
                    //string commandFour=   TextWrite('0', 30, 160, 250, 600, 3, "ABC  fghijklmnopqrstu");
                    //string commandFive =  TextWrite('0', 30, 160, 300, 600, 3, "ABC  fghijklmnopqrstu");

                    printer.SendCommand(WholeCommand(qrCommand, commandOne, "", "", "", ""));
                    connection.Close();
                    break;
                }

            }
            catch(Exception ex)
            {
                Windows.MessageBox mb = new Windows.MessageBox(ex.Message.ToString());
                mb.Owner = _currentApp.MainWindow;
                mb.ShowDialog();
            }

          
        }

        private void btnPrintTest_Click(object sender, RoutedEventArgs e)
        {
            foreach (DiscoveredUsbPrinter usbPrinter in UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter()))
            {

                Connection connection = usbPrinter.GetConnection();
                connection.Open();
                ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);
                printer.Calibrate();
                //string qrCommand = QRWrite(50, 20, 0, 4, QRErrorCorrection.ULTRA_HIGH, "e47624f6-6ed8-4e8b-991d-f982526ee7f9");
                ////string commandOne = TextWrite(260, 25, ElementDrawRotation.NO_ROTATION,ZebraFont.STANDARD_NORMAL,40,20, 370, 2,"Mahatma Ghandhi dasdsadasd");
                ////string commandTwo = TextWrite(260, 105, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_SMALL, 30, 15, 370, 2,"Indian activist asdasdas ");
                ////string commandThree = TextWrite(260, 185, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_SMALL, 30, 15, 370, 4,"Born and raised and killed and fucked and raped and jailed");
                ////printer.SendCommand(WholeCommand(qrCommand,commandOne,commandTwo,commandThree));
                //printer.SendCommand(WholeCommand(qrCommand, "", "", ""));
                connection.Close();

            }
            Windows.MessageBox mb = new Windows.MessageBox("Printer is out of paper.");
            mb.Owner = _currentApp.MainWindow;
            mb.ShowDialog();
        }

        private void btnViewLog_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new LogPage(_settings);
        }

        private void cbInactive_Checked(object sender, RoutedEventArgs e)
        {
            cbActive.IsChecked = false;
            _timerPrint.Enabled = false;
        }

        private void cbActive_Checked(object sender, RoutedEventArgs e)
        {
            cbInactive.IsChecked = false;
            _timerPrint.Enabled = true;
        }

        private void PrintQueueLabels(object source, ElapsedEventArgs e)
        {

            lock (_printObject)
            {
                try
                {
                    if (GlobalSettings.IsPrinterOnline)
                    {
                        foreach (DiscoveredUsbPrinter usbPrinter in UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter()))
                        {
                            Connection connection = usbPrinter.GetConnection();
                            connection.Open();
                            _printer = ZebraPrinterFactory.GetInstance(connection);

                            string users = "";
                            foreach (Account user in _settings.Accounts.Account)
                            {
                                users = users + user.AccountID + ",";
                            }
                            List<PrintQueueItem> items = Helpers.GetPrintQueue(_settings.AuthToken, users);

                            foreach (PrintQueueItem item in items)
                            {
                                lock (_printerPrintingObject)
                                {
                                    PrintLabelStickers(item);
                                }
                            }

                            connection.Close();
                        }
                    }
                }
                catch (Exception ex)
                {

                    Debug.Log("MeetpointPrinter", ex.ToString());
                }
            }

        }

        private void PrinterStatusCheck(object source, ElapsedEventArgs e)
        {

            lock (_printerStatusObject)
            {
                try
                {
                    
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => bdrOnline.Visibility = Visibility.Collapsed));
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => bdrOffline.Visibility = Visibility.Visible));
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => GlobalSettings.IsPrinterOnline = false));
                    UsbPrinterConnector ps = new UsbPrinterConnector(_settings.Printer);
                  
                    if(ps.BeginSend())
                    {
                       
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => GlobalSettings.IsPrinterOnline = true));
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => bdrOnline.Visibility = Visibility.Visible));
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => bdrOffline.Visibility = Visibility.Collapsed));
                    }
                    
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => GlobalSettings.IsPrinterOnline = false));
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => bdrOnline.Visibility = Visibility.Collapsed));
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => bdrOffline.Visibility = Visibility.Visible));
                    Debug.Log("MeetpointPrinter", ex.ToString());
                }
            }

        }

        private void PrintLabelStickers(PrintQueueItem item)
        {
            try
            {
              

                string qrCommand = QRWrite(570, 250, 0, 5, QRErrorCorrection.ULTRA_HIGH, "e47624f6-6ed8-4e8b-991d-f982526ee7f9");

                item.FirstName = string.IsNullOrEmpty(item.FirstName) ? "" : item.FirstName;
                item.LastName = string.IsNullOrEmpty(item.LastName) ? "" : item.FirstName;
                item.Country = string.IsNullOrEmpty(item.Country) ? "" : item.Country;
                item.JobPosition = string.IsNullOrEmpty(item.JobPosition) ? "" : item.JobPosition;
                item.Company = string.IsNullOrEmpty(item.Company) ? "" : item.Company;

                TextFontSize tfs = Helpers.SetTextFontSize(item.FirstName.Length, TextField.FieldOne);
                string commandOne = TextWrite('0', (int)tfs, 160, 40, 600, 0, item.FirstName);

                tfs = Helpers.SetTextFontSize(item.LastName.Length, TextField.FieldTwo);
                string commandTwo = TextWrite('0', (int)tfs, 160, 100, 600, 0, item.LastName);

                tfs = Helpers.SetTextFontSize(item.Country.Length, TextField.FieldThree);
                string commandThree = TextWrite('0', (int)tfs, 160, 160, 600, 0, item.Country);

                tfs = Helpers.SetTextFontSize(item.JobPosition.Length, TextField.FieldFour);
                string commandFour = TextWrite('0', (int)tfs, 160, 250, 400, 3, item.JobPosition);

                tfs = Helpers.SetTextFontSize(item.Country.Length, TextField.FieldFive);
                string commandFive = TextWrite('0', (int)tfs, 160, 350, 400, 3, item.Company);

                _printer.SendCommand(WholeCommand(qrCommand, commandOne, commandTwo, commandThree, commandFour, commandFive));
                item.Status = 1;
            }
            catch(Exception ex)
            {
                item.Status = 2;
                Debug.Log("MeetpointPrinter", ex.ToString());
            }


            int index = GetPrintQueueExistingItem(item.PrintUserID);

            if(index<0)
            {
                GlobalSettings.PrintQueueItemLog.Add(item);
            }
            else
            {
                GlobalSettings.PrintQueueItemLog[index].Status = item.Status;
            }

        }
        private int GetPrintQueueExistingItem(int PrintUserID)
        {
            for(int i=0; i<GlobalSettings.PrintQueueItemLog.Count; i++)
            {
                if(PrintUserID == GlobalSettings.PrintQueueItemLog[i].PrintUserID)
                {
                    return i;
                }
            }
            return -1;
        }

        private void btnEvents_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
            GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;
            Application.Current.MainWindow.Content = new EventPage(GlobalSettings.ApplicationSettings.Username, GlobalSettings.ApplicationSettings.AuthToken, GlobalSettings.ApplicationSettings.Event.EventID.ToString());
        }

        private void PrivewTemplateLogic()
        {

            printTemplate.tbOptOne.Text = _dataOptions[0];
            printTemplate.tbOptTwo.Text = _dataOptions[1];
            printTemplate.tbOptThree.Text = _dataOptions[2];

            Helpers.SetPrintTemplateSize(printTemplate, PrintTemplateSize.SettingsBig);
            if(GlobalSettings.ApplicationSettings.PrinterSetup.LayoutSizeID.Equals("cbSizeTwo"))
            {
                Helpers.SetPrintTemplateSize(printTemplate, PrintTemplateSize.SettingsSmall);
            }

        
            switch (GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate)
            {
                case "cbLayoutHR":
                    printTemplate.LayoutHalfRight();
                    break;
                case "cbLayoutHL":
                    printTemplate.LayoutHalfLeft();
                    break;
                case "cbLayoutClean":
                    printTemplate.LayoutClean();
                    break;
            }
        }

        private void PrintLabelHalfLeft()
        {

        }
        private void PrintLabelHalfRight()
        {
            string command = QRWrite(20, 20, 0, 10, QRErrorCorrection.ULTRA_HIGH, "ef65da48-ca8b-455f-a92e-10d86b56cea6");
        }
        private void PrintLabelClear(PrintQueueItem item)
        {
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = _settings.Printer;
            ps.Width = (int)(203 * 3);
            ps.Length = (int)(203 * 1);
            ps.Darkness = 30;

            List<byte> page = new List<byte>();

            page.AddRange(ZPLCommands.ClearPrinter(ps));
            page.AddRange(ZPLCommands.TextWrite(15, 75, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_LARGEST, 45, 30, Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[0], item)));
            page.AddRange(ZPLCommands.TextWrite(15, 150, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_NORMAL, 30, 20, Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[1], item)));
            page.AddRange(ZPLCommands.TextWrite(15, 225, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_NORMAL, 20, 15, Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[2], item)));
            page.AddRange(ZPLCommands.PrintBuffer(1));

            new SpoolPrinter(ps).Print(page.ToArray());
            item.Status = 1;
        }

        private string QRWrite(int left, int top, int justification, int magnitude, QRErrorCorrection errorCorrection, string barcode)
        {
            string command = string.Format(
                                 "^FO{0},{1},{2}," +
                                 "^BQ,2,{3}" +
                                 "^FD72M,A{5}" +
                                 "^FS", (object)left, (object)top, (object)justification, (object)magnitude, (object)(char)errorCorrection, (object)barcode);

            return command;
        }
        private string TextWrite(char fontType, int fontSize, int left, int top,  int blokSize, int blockLines, string text)
        {
            string command = string.Format(
                                 "^CI28" +
                                 "^CF{0},{1}" +
                                 "^FO{2},{3}" +
                                 "^FB{4},{5}," +
                                 "^FH" +
                                 "^FD{6}^FS", (object)(char)fontType, (object)fontSize, (object)left, (object)top, (object)blokSize, (object)blockLines, (object)text);
            return command;
        }
        private string WholeCommand(string commandQr, string commandTextOne, string commandTextTwo, string commandTextThree, string commandTextFour, string commandTextFive)
        {
            return string.Format("^XA{0}{1}{2}{3}{4}{5}^XZ",commandQr, commandTextOne, commandTextTwo, commandTextThree, commandTextFour, commandTextFive);
        }
    }
}
