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

namespace MeetpointPrinterNew.CustomControls
{
    /// <summary>
    /// Interaction logic for SubHeaderControl.xaml
    /// </summary>
    public partial class SubHeaderControl : UserControl
    {
        public string EventName
        {
            get { return lblEventName.Content.ToString(); }
            set { lblEventName.Content = value; }
        }
        public string EventDateLocation
        {
            get { return lblEventDateLocation.Content.ToString(); }
            set { lblEventDateLocation.Content = value; }
        }
        public ImageSource EventLogo
        {
            get { return imgEventLogo.Source; }
            set { imgEventLogo.Source = value; }
        }
        public SubHeaderControl()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
