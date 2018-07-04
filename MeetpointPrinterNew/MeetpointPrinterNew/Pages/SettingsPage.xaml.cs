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
                //int left = int.Parse(tbLeft.Text);
                //int top = int.Parse(tbTop.Text);

                //ComboBoxItem jus = (ComboBoxItem)cmbJustification.SelectedItem;
                //ComboBoxItem mag = (ComboBoxItem)cmbMagnitude.SelectedItem;

                //int justification = int.Parse(jus.Content.ToString());
                //int magnitude = int.Parse(mag.Content.ToString());


                //char fontType = tbFontType.Text[0];
                //int fontSize = int.Parse(tbFontSize.Text);
                //int fontLeft = int.Parse(tbFontLeft.Text);
                //int fontTop = int.Parse(tbFontTop.Text);
                //int fontLineWidth = int.Parse(tbFontLineWidth.Text);
                //int fontLines = int.Parse(tbFontLines.Text);

                //List<DiscoveredUsbPrinter> printers = UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter());
                //foreach (DiscoveredUsbPrinter usbPrinter in printers)
                //{
                //    Connection connection = usbPrinter.GetConnection();
                //    connection.Open();
                //    ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);

                //    string qrCommand = QRWrite(570, 250, 0, 5, QRErrorCorrection.ULTRA_HIGH, "e47624f6-6ed8-4e8b-991d-f982526ee7f9");
                //    string commandOne = TextWrite('0', 60, 125, 40 , 580, 0, "ABC  fghijklmnopqrstu");
                //    string commandTwo = TextWrite('0', 55, 125, 110 , 580, 0, "ABC  fghijklmnopqrstu");
                //    string commandThree = TextWrite('0', 50, 125, 180 , 580, 0, "ABC  fghijklmnopqrstu");
                //    string commandFour = TextWrite('0', 30, 125, 280 , 440, 2, "ABC  fghijklmnopqrstu ABC  fghijklmnopqrstu");
                //    string commandFive = TextWrite('0', 30, 125, 350 , 440, 2, "ABC  fghijklmnopqrstu ABC  fghijklmnopqrstu");

                //    printer.SendCommand(WholeCommand("", commandOne, commandTwo, commandThree, commandFour, ""));
                //    connection.Close();

                //    break;
                //}
                var encodings = System.Text.Encoding.GetEncodings();

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
            PrintQueueItem item = new PrintQueueItem();
            item.FirstName = "Mohandas Karamchand";
            item.LastName = "Gandhi ";
            item.Status = 1;
            item.JobPosition = "Lawyer Politician Activist Writer";
            item.Company = "Indian National Congress";
            item.Country = "India";
            item.ActionUID = "9ab1d1d8-2687-4a8e-824b-55459376b0a3";


            if (GlobalSettings.IsPrinterOnline)
            {
                foreach (DiscoveredUsbPrinter usbPrinter in UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter()))
                {
                    Connection connection = usbPrinter.GetConnection();
                    connection.Open();
                    _printer = ZebraPrinterFactory.GetInstance(connection);

                    PrintLabelStickers(item,true);

                    connection.Close();
                }
            }
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
                                    PrintLabelStickers(item,false);
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

        private void PrintLabelStickers(PrintQueueItem item,bool isTest)
        {
            try
            {

              switch(GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate)
              {
                    case "cbLayoutHR":
                        PrintLabelQRight(item);
                        break;
                    case "cbLayoutClean":
                        PrintLabelClear(item);
                        break;
                    default:
                        PrintLabelQRight(item);
                        break;
              }

                item.Status = 1;
            }
            catch(Exception ex)
            {
                item.Status = 2;
                Debug.Log("MeetpointPrinter", ex.ToString());
            }

            if(!isTest)
            {
                int index = GetPrintQueueExistingItem(item.PrintUserID);

                if (index < 0)
                {
                    GlobalSettings.PrintQueueItemLog.Add(item);
                }
                else
                {
                    GlobalSettings.PrintQueueItemLog[index].Status = item.Status;
                }
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
                case "cbLayoutClean":
                    printTemplate.LayoutClean();
                    break;
            }
        }

        private void PrintLabelQRight(PrintQueueItem item)
        {
            TextFontSize tfs;

            string commandOne = "";
            string commandTwo = "";
            string commandThree = "";
            string commandFour = "";
            string commandFive = "";
            string qrHash = "";
            string qrCommand = "";
            int dataOptionsLength = 0;

            foreach(string opt in GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption)
            {
                if(!string.IsNullOrEmpty(opt))
                {
                    dataOptionsLength++;
                }
            }

            string fieldOne = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[0], item);
            string fieldTwo = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[1], item);
            string fieldThree = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[2], item);
            string fieldFour = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[3], item);
            string fieldFive = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[4], item);

            fieldOne = string.IsNullOrEmpty(fieldOne) ? "" : fieldOne;
            fieldTwo = string.IsNullOrEmpty(fieldTwo) ? "" : fieldTwo;
            fieldThree = string.IsNullOrEmpty(fieldThree) ? "" : fieldThree;
            fieldFour = string.IsNullOrEmpty(fieldFour) ? "" : fieldFour;
            fieldFive = string.IsNullOrEmpty(fieldFive) ? "" : fieldFive;

            qrHash = item.ActionUID == null ? "" : item.ActionUID;
           

        

            TextOffset textOffset = TextOffset.OffsetFive;

            switch (dataOptionsLength)
            {
                case 1:
                    textOffset = TextOffset.OffsetOne;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    qrCommand = QRWrite(330, 270, 0, 5, QRErrorCorrection.ULTRA_HIGH, qrHash);
                    break;
                case 2:
                    textOffset = TextOffset.OffsetTwo;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    qrCommand = QRWrite(330, 270, 0, 5, QRErrorCorrection.ULTRA_HIGH, qrHash);
                    break;
                case 3:
                    textOffset = TextOffset.OffsetFive;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    tfs = Helpers.SetTextFontSize(fieldThree, TextField.FieldThree);
                    commandThree = TextWrite('0', (int)tfs, 125, 180 + (int)textOffset, 580, 1, fieldThree);

                    qrCommand = QRWrite(250, 250, 0, 5, QRErrorCorrection.ULTRA_HIGH, qrHash);
                    break;
                case 4:
                    textOffset = TextOffset.OffsetFive;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    tfs = Helpers.SetTextFontSize(fieldThree, TextField.FieldThree);
                    commandThree = TextWrite('0', (int)tfs, 125, 180 + (int)textOffset, 580, 1, fieldThree);

                    tfs = Helpers.SetTextFontSize(fieldFour, TextField.FieldFour);
                    commandFour = TextWrite('0', (int)tfs, 125, 280 + (int)textOffset, 440, 2, fieldFour);

                    qrCommand = QRWrite(570, 250, 0, 5, QRErrorCorrection.ULTRA_HIGH, qrHash);
                    break;
                case 5:
                    textOffset = TextOffset.OffsetFive;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    tfs = Helpers.SetTextFontSize(fieldThree, TextField.FieldThree);
                    commandThree = TextWrite('0', (int)tfs, 125, 180 + (int)textOffset, 580, 1, fieldThree);

                    tfs = Helpers.SetTextFontSize(fieldFour, TextField.FieldFour);
                    commandFour = TextWrite('0', (int)tfs, 125, 280 + (int)textOffset, 440, 2, fieldFour);

                    tfs = Helpers.SetTextFontSize(fieldFive, TextField.FieldFive);
                    commandFive = TextWrite('0', (int)tfs, 125, 350 + (int)textOffset, 440, 2, fieldFive);

                    qrCommand = QRWrite(570, 250, 0, 5, QRErrorCorrection.ULTRA_HIGH, qrHash);
                    break;
            }


            _printer.SendCommand(WholeCommand(qrCommand, commandOne, commandTwo, commandThree, commandFour, commandFive), "ISO-8859-1");
        }
       
        private void PrintLabelClear(PrintQueueItem item)
        {
            TextFontSize tfs;

          
            int dataOptionsLength = 0;
            foreach (string opt in GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption)
            {
                if (!string.IsNullOrEmpty(opt))
                {
                    dataOptionsLength++;
                }
            }

            string fieldOne = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[0], item);
            string fieldTwo = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[1], item);
            string fieldThree = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[2], item);
            string fieldFour = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[3], item);
            string fieldFive = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[4], item);

            fieldOne = string.IsNullOrEmpty(fieldOne) ? "": fieldOne;
            fieldTwo = string.IsNullOrEmpty(fieldTwo) ? "" : fieldTwo;
            fieldThree = string.IsNullOrEmpty(fieldThree) ? "" : fieldThree;
            fieldFour = string.IsNullOrEmpty(fieldFour) ? "" : fieldFour;
            fieldFive = string.IsNullOrEmpty(fieldFive) ? "" : fieldFive;

            string commandOne = "";
            string commandTwo = "";
            string commandThree = "";
            string commandFour = "";
            string commandFive = "";

            TextOffset textOffset = TextOffset.OffsetFive;

           switch(dataOptionsLength)
           {
                case 1:
                    textOffset = TextOffset.OffsetOne;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 4, fieldOne);
                    break;
                case 2:
                    textOffset = TextOffset.OffsetTwo;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 3, fieldTwo);
                    break;
                case 3:
                    textOffset = TextOffset.OffsetThree;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    tfs = Helpers.SetTextFontSize(fieldThree, TextField.FieldThree);
                    commandThree = TextWrite('0', (int)tfs, 125, 180 + (int)textOffset, 580, 3, fieldThree);
                    break;
                case 4:
                    textOffset = TextOffset.OffsetFour;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    tfs = Helpers.SetTextFontSize(fieldThree, TextField.FieldThree);
                    commandThree = TextWrite('0', (int)tfs, 125, 180 + (int)textOffset, 580, 1, fieldThree);

                    tfs = Helpers.SetTextFontSize(fieldFour, TextField.FieldFour);
                    commandFour = TextWrite('0', (int)tfs, 125, 280 + (int)textOffset, 580, 3, fieldFour);
                    break;
                case 5:
                    textOffset = TextOffset.OffsetFive;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    tfs = Helpers.SetTextFontSize(fieldThree, TextField.FieldThree);
                    commandThree = TextWrite('0', (int)tfs, 125, 180 + (int)textOffset, 580, 1, fieldThree);

                    tfs = Helpers.SetTextFontSize(fieldFour, TextField.FieldFour);
                    commandFour = TextWrite('0', (int)tfs, 125, 280 + (int)textOffset, 580, 2, fieldFour);

                    tfs = Helpers.SetTextFontSize(fieldFive, TextField.FieldFive);
                    commandFive = TextWrite('0', (int)tfs, 125, 350 + (int)textOffset, 580, 2, fieldFive);
                    break;
            }
           
            _printer.SendCommand(WholeCommand("", commandOne, commandTwo, commandThree, commandFour, commandFive), "ISO-8859-1");
        }

        private string QRWrite(int left, int top, int justification, int magnitude, QRErrorCorrection errorCorrection, string barcode)
        {
            string command = string.Format(
                                 "^FO{0},{1},{2}," +
                                 "^BQ,2,{3}" +
                                 "^FD72M,A{4}" +
                                 "^FS", (object)left, (object)top, (object)justification, (object)magnitude, (object)(char)errorCorrection, (object)barcode);

            return command;
        }

        private string TextWrite(char fontType, int fontSize, int left, int top,  int blokSize, int blockLines, string text)
        {
            string command = string.Format(
                                 "^CF{0},{1}" +
                                 "^FO{2},{3}" +
                                 "^TBN,{4},{5}" +
                                 "^FH"+
                                 "^FD{6}" +
                                 "^FS", (object)(char)fontType, (object)fontSize, (object)left, (object)top, (object)blokSize, (object)(blockLines* fontSize), (object)text);

            return command;
        }

        private string WholeCommand(string commandQr, string commandTextOne, string commandTextTwo, string commandTextThree, string commandTextFour, string commandTextFive)
        {
            return string.Format("^XA{0}{1}{2}{3}{4}{5}^XZ", commandQr, commandTextOne, commandTextTwo, commandTextThree, commandTextFour, commandTextFive);
        }
    }
}
