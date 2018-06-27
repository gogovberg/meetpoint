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

              
                foreach (DiscoveredUsbPrinter usbPrinter in UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter()))
                {

                    Connection connection = usbPrinter.GetConnection();
                    connection.Open();
                    ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);
                    string qrCommand = QRWrite(50,20,0, magnitude, QRErrorCorrection.ULTRA_HIGH, "e47624f6-6ed8-4e8b-991d-f982526ee7f9");
                    string commandOne = TextWrite(260, 25, ElementDrawRotation.NO_ROTATION,ZebraFont.STANDARD_LARGEST,40,20, 370, 2,"Mahatma Ghandhi dasdsadasd");
                    string commandTwo = TextWrite(260, 105, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_SMALL, 30, 15, 370, 2,"Indian activist asdasdas ");
                    string commandThree = TextWrite(260, 185, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_SMALL, 30, 15, 370, 4,"Born and raised and killed and fucked and raped and jailed");
                    printer.SendCommand(WholeCommand(qrCommand,commandOne,commandTwo,commandThree));
                    connection.Close();

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
                    string users = "";
                    foreach (Account user in _settings.Accounts.Account)
                    {
                        users = users + user.AccountID + ",";
                    }
                    List<PrintQueueItem> items = Helpers.GetPrintQueue(_settings.AuthToken, users);
             
                    foreach (PrintQueueItem item in items)
                    {
                        PrintLabelStickers(item);
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
                if (GlobalSettings.IsPrinterOnline)
                {
                    PrinterSettings ps = new PrinterSettings();
                    ps.PrinterName = _settings.Printer;
                    ps.Width = (int)(203 * 3);
                    ps.Length = (int)(203 * 1);
                    ps.Darkness = 30;

                    List<byte> page = new List<byte>();

                    //page.AddRange(ZPLCommands.ClearPrinter(ps));
                    page.AddRange(ZPLCommands.TextWrite(15, 75, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_LARGEST, 45, 30, Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[0], item)));
                    page.AddRange(ZPLCommands.TextWrite(15, 150, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_NORMAL, 30, 20, Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[1], item)));
                    page.AddRange(ZPLCommands.TextWrite(15, 225, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_NORMAL, 20, 15, Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[2], item)));
                    page.AddRange(ZPLCommands.PrintBuffer(1));

                    new SpoolPrinter(ps).Print(page.ToArray());
                    item.Status = 1;
                }
               
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
            return string.Format("^FO{0},{1},{2},^BQ,2,{3}^FD72M,A{5}^FS", (object)left, (object)top, (object)justification, (object)magnitude, (object)(char)errorCorrection, (object)barcode);
        }
        public string TextWrite(int left, int top, ElementDrawRotation rotation, ZebraFont font, int height, int width, int blokSize, int blockLines, string text)
        {
            return string.Format("^FO{0},{1}^A{2}{3},{4},{5}^FB{6},{7},,^FD{8}^FS", left, top, (char)font, (char)rotation, height, width, blokSize, blockLines,text);
        }

        private string WholeCommand(string commandQr, string commandTextOne, string commandTextTwo, string commandTextThree)
        {
            return string.Format("^XA{0}{1}{2}{3}^XZ",commandQr, commandTextOne, commandTextTwo, commandTextThree);
        }
    }
}
