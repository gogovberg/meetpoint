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
        private System.Printing.PrintQueue _printQueue;

        public SettingsPage(UserSettings settings)
        {
          
            InitializeComponent();
            GlobalSettings.CurrentPageID = 5;

            _settings = settings;
            _printTimer = 10000;
            GlobalSettings.ApplicationSettings.Event = settings.Event;
            _currentApp.CurrentEvent = settings.Event.EventName;
            _currentApp.CurrentEventLocation =  settings.Event.EventStartDate.ToShortDateString()+" "+
                                                settings.Event.EventEndDate.ToShortDateString()+" "+
                                                settings.Event.EventLocation;
            string _tempPrintName = _settings.Printer;
            foreach (var item in _settings.Accounts.Account)
            {
                icAccountItem.Items.Add(item.AccountName);
            }

            _timerPrint = new Timer();
            _timerPrint.Elapsed += new ElapsedEventHandler(PrintQueueLabels);
            _timerPrint.Interval = _printTimer; // 1000 ms => 1 second
            _timerPrint.Enabled = false;
            
            lock(_printObject)
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

            cbInactive.IsChecked = true;


        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new LoginPage();
        }

        private void btneditPrinter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SetupPage(_settings,0);
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

            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = _settings.Printer;
            ps.Width = (int)(203 * 3);
            ps.Length = (int)(203 * 1);
            ps.Darkness = 30;

            List<byte> page = new List<byte>();
            page.AddRange(ZPLCommands.ClearPrinter(ps));
      

            page.AddRange(ZPLCommands.TextWrite(15, 75, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_LARGEST, 45, 30, "Mahatma Gandhi"));
            page.AddRange(ZPLCommands.TextWrite(15, 150, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_NORMAL, 30, 20, "Indian activist"));
            page.AddRange(ZPLCommands.TextWrite(15, 225, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_NORMAL, 20, 15, "Assassinated: January 30, 1948, New Delhi, India"));

            page.AddRange(ZPLCommands.PrintBuffer(1));

            new SpoolPrinter(ps).Print(page.ToArray());
        }

        private void btnPrintTest_Click(object sender, RoutedEventArgs e)
        {
            Windows.MessageBox mb = new Windows.MessageBox();
            mb.Show();
            mb.Activate();
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

                    foreach(PrintQueueItem item in items)
                    {
                        PrintLabelStickers(_settings.PrinterSetup.LayoutTemplate,item.FirstName,item.Company,item.PrintUserID.ToString());
                    }

                }
                catch (Exception ex)
                {
                  
                    Debug.Log("WatchForReaders", ex.ToString());
                }
            }

        }

        private void PrintLabelStickers(string templateType, string fieldOne, string fieldTwo, string fieldThree)
        {
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = _settings.Printer;
            ps.Width = (int)(203 * 3);
            ps.Length = (int)(203 * 1);
            ps.Darkness = 30;

            List<byte> page = new List<byte>();
            page.AddRange(ZPLCommands.ClearPrinter(ps));


            page.AddRange(ZPLCommands.TextWrite(15, 75, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_LARGEST, 45, 30, fieldOne));
            page.AddRange(ZPLCommands.TextWrite(15, 150, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_NORMAL, 30, 20, fieldTwo));
            page.AddRange(ZPLCommands.TextWrite(15, 225, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_NORMAL, 20, 15, fieldThree));

            page.AddRange(ZPLCommands.PrintBuffer(1));

            new SpoolPrinter(ps).Print(page.ToArray());
        }

        private void btnEvents_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
            GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;
            Application.Current.MainWindow.Content = new EventPage(GlobalSettings.ApplicationSettings.Username, GlobalSettings.ApplicationSettings.AuthToken, GlobalSettings.ApplicationSettings.Event.EventID.ToString());
        }
    }
}
