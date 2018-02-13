using Com.SharpZebra;
using Com.SharpZebra.Commands;
using Com.SharpZebra.Printing;
using MeetpointPrinter.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MeetpointPrinter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string accessToken;
        private bool timerStarted;
        private DataTable dtList;
        private Timer printTimer;
        private bool isEventHandler = false;
       
        private int _templateHeight = 0;
        private int _templateWidth = 0;
        private string _printDevice = "";
        private System.Collections.Specialized.StringCollection _users;
        private string _printTemplate;
        private char _delimiter = ';';
        private char _sizeDelimiter = 'x';

        public MainWindow(string accessToken)
        {
            InitializeComponent();

            this.accessToken = accessToken;
            printTimer = new System.Timers.Timer();
            printTimer.Elapsed += new ElapsedEventHandler(PrintTimer_Tick);
            printTimer.Interval = 2000; // 1000 ms => 1 second
            printTimer.Enabled = true;

            lblRefresh.Text = "";
           
            this.timerStarted = false;
          
            dtList = new DataTable();
            dtList.Columns.Add("Name");
            dtList.Columns.Add("LastName");
            dtList.Columns.Add("Company");
            dtList.Columns.Add("ActionUID");

            dataGridView.DataContext = dtList;
            (Application.Current as App).CurrentPage = "Main windown";

            _users = new System.Collections.Specialized.StringCollection();
            _users = Properties.Settings.Default.PrintUsers;
            _printDevice = Properties.Settings.Default.PrintDevice;
            _printTemplate = Properties.Settings.Default.PrintTemplate;
            _templateHeight = Properties.Settings.Default.PrintTemplateHeight;
            _templateWidth = Properties.Settings.Default.PrintTemplateWidth;
        }

      

        private void PrintTimer_Tick(object sender, EventArgs e)
        {
            if (!isEventHandler)
            {
                isEventHandler = true;
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action<TextBlock>(SetValue), lblRefresh);

                PrintQueue a = Helpers.GetPrintQueue(this.accessToken);

                PrinterSettings ps = new PrinterSettings();

                ps.PrinterName = _printDevice;
                ps.Width = (int)(203 * 3);
                ps.Length = (int)(203 * 1);
                ps.Darkness = 30;

                if (a != null)
                {
                    foreach (var b in a.Item)
                    {
                        DataRow r = dtList.NewRow();
                        r["Name"] = b.FirstName;
                        r["LastName"] = b.LastName;
                        r["Company"] = b.Company;
                        r["ActionUID"] = b.ActionUID;
                        dtList.Rows.Add(r);

                        List<byte> page = new List<byte>();
                        page.AddRange(ZPLCommands.ClearPrinter(ps));
                        page.AddRange(ZPLCommands.TextWrite(20, 25, ElementDrawRotation.NO_ROTATION, ZebraFont.CUSTOM_U, 15, 15, Helpers.ToTitleCase(b.FirstName)));
                        page.AddRange(ZPLCommands.TextWrite(20, 100, ElementDrawRotation.NO_ROTATION, ZebraFont.CUSTOM_U, 30, 30, Helpers.ToTitleCase(b.LastName)));
                        page.AddRange(ZPLCommands.TextWrite(20, 200, ElementDrawRotation.NO_ROTATION, ZebraFont.CUSTOM_S, 20, 15, string.Concat(b.Company)));
                        page.AddRange(ZPLCommands.TextWrite(20, 250, ElementDrawRotation.NO_ROTATION, ZebraFont.CUSTOM_S, 20, 15, string.Concat(b.EventPosition)));
                        page.AddRange(ZPLCommands.PrintBuffer(1));
                        new SpoolPrinter(ps).Print(page.ToArray());
                    }
                }
                isEventHandler = false;
            }
        }


        private static void SetValue(TextBlock txt)
        {
            txt.Text = string.Format("Last refresh: {0}:{1}:{2}",
            DateTime.Now.TimeOfDay.Hours,
            DateTime.Now.TimeOfDay.Minutes,
            DateTime.Now.TimeOfDay.Seconds);
        }
        IEnumerable<string> SplitToLines(string stringToSplit, int maximumLineLength)
        {
            var words = stringToSplit.Split(' ').Concat(new[] { "" });
            return
                words
                    .Skip(1)
                    .Aggregate(
                        words.Take(1).ToList(),
                        (a, w) =>
                        {
                            var last = a.Last();
                            while (last.Length > maximumLineLength)
                            {
                                a[a.Count() - 1] = last.Substring(0, maximumLineLength);
                                last = last.Substring(maximumLineLength);
                                a.Add(last);
                            }
                            var test = last + " " + w;
                            if (test.Length > maximumLineLength)
                            {
                                a.Add(w);
                            }
                            else
                            {
                                a[a.Count() - 1] = test;
                            }
                            return a;
                        });
        }


        public void StartTimer(bool start = true)
        {
            if (start)
                this.printTimer.Start();
            else
                this.printTimer.Stop();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = _printDevice;
            ps.Width = (int)(203 * 3);
            ps.Length = (int)(203 * 1);
            ps.Darkness = 30;
            List<byte> page = new List<byte>();
            page.AddRange(ZPLCommands.ClearPrinter(ps));
            page.AddRange(ZPLCommands.TextWrite(15, 75, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_LARGEST, 45, 30, "Anže Kravanja"));
            page.AddRange(ZPLCommands.TextWrite(15, 150, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_NORMAL, 30, 20, "Analytics Pros"));
            page.AddRange(ZPLCommands.TextWrite(15, 225, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_NORMAL, 20, 15, "Data Scientist"));
            page.AddRange(ZPLCommands.PrintBuffer(1));
            new SpoolPrinter(ps).Print(page.ToArray());
        }

        private void btnStartTimer_Click(object sender, EventArgs e)
        {
            if (this.timerStarted)
            {
                this.StartTimer(start: false);
                this.btnStartTimer.Content = "Start printing";
                this.timerStarted = false;
            }
            else
            {
                this.StartTimer();
                this.btnStartTimer.Content = "Stop printing";
                this.timerStarted = true;
            }
        }

  
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage objSettings = new SettingsPage(this.accessToken);
            objSettings.Show();
            
        }
    }
    

}
