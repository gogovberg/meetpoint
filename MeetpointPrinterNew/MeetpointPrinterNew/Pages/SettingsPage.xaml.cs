using Com.SharpZebra;
using Com.SharpZebra.Commands;
using Com.SharpZebra.Printing;
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
    /// Interaction logic for PrintSettingsPage.xaml
    /// </summary>
    /// 

    public partial class SettingsPage : Page
    {
        private App _currentApp = ((App)Application.Current);
        private UserSettings _settings;
        public SettingsPage(UserSettings settings)
        {
            _settings = settings;
            InitializeComponent();

            _currentApp.ApplicationSettings.Event = settings.Event;
            _currentApp.CurrentEvent = settings.Event.EventName;
            _currentApp.CurrentEventLocation =  settings.Event.EventStartDate.ToShortDateString()+" "+
                                                settings.Event.EventEndDate.ToShortDateString()+" "+
                                                settings.Event.EventLocation;
            foreach (var item in _settings.Accounts.Account)
            {
                icAccountItem.Items.Add(item);
            }
            
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
            ps.PrinterName = _settings.Printers.Printer[0];
            ps.Width = (int)(203 * 3);
            ps.Length = (int)(203 * 1);
            ps.Darkness = 30;

            List<byte> page = new List<byte>();
            page.AddRange(ZPLCommands.ClearPrinter(ps));
      

            page.AddRange(ZPLCommands.TextWrite(15, 75, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_LARGEST, 45, 30, "Рок Куштер"));
            page.AddRange(ZPLCommands.TextWrite(15, 150, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_NORMAL, 30, 20, "Почетник програмер"));
            page.AddRange(ZPLCommands.TextWrite(15, 225, ElementDrawRotation.NO_ROTATION, ZebraFont.STANDARD_NORMAL, 20, 15, "ЈаваСкрипт - Педер"));

            page.AddRange(ZPLCommands.PrintBuffer(1));

            new SpoolPrinter(ps).Print(page.ToArray());
        }

        private void btnPrintTest_Click(object sender, RoutedEventArgs e)
        {
            List<PrintQueueItem> items = Helpers.GetPrintQueue(_settings.AuthToken);
        }

        private void btnViewLog_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new LogPage(_settings);
        }

        private void cbInactive_Checked(object sender, RoutedEventArgs e)
        {
            cbActive.IsChecked = false;
        }

        private void cbActive_Checked(object sender, RoutedEventArgs e)
        {
            cbInactive.IsChecked = false;
        }
    }
}
