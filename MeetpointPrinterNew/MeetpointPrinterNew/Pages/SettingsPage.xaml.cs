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
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            Style cbStyle = (Style)FindResource("ChecBoxPrinterStyle");

            CheckBox cb1 = new CheckBox();
            cb1.Style = cbStyle;
            cb1.Content = "Printer name 1";
            CheckBox cb2= new CheckBox();
            cb2.Style = cbStyle;
            cb2.Content = "Printer name 2";
            CheckBox cb3= new CheckBox();
            cb3.Style = cbStyle;
            cb3.Content = "Printer name 3";
            CheckBox cb4 = new CheckBox();
            cb4.Style = cbStyle;
            cb4.Content = "Printer name 4";
            CheckBox cb5 = new CheckBox();
            cb5.Style = cbStyle;
            cb5.Content = "Printer name 5";

            icPrinterItems.Items.Add(cb1);
            icPrinterItems.Items.Add(cb2);
            icPrinterItems.Items.Add(cb3);
            icPrinterItems.Items.Add(cb4);
            icPrinterItems.Items.Add(cb5);
        }
    }
}
