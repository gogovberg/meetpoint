using MeetpointPrinterNew.CustomControls;
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
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
        public LogPage()
        {
            InitializeComponent();
            LogControl lc1 = new LogControl();
            LogControl lc2 = new LogControl();
            LogControl lc3 = new LogControl();
            LogControl lc4 = new LogControl();
            LogControl lc5 = new LogControl();

            icEventItems.Items.Add(lc1);
            icEventItems.Items.Add(lc2);
            icEventItems.Items.Add(lc3);
            icEventItems.Items.Add(lc4);
            icEventItems.Items.Add(lc5);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SetupPagePrintTemplate();
        }
    }
}
