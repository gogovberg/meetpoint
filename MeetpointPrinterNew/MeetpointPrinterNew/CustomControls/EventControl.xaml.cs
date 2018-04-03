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
    /// Interaction logic for EventControl.xaml
    /// </summary>
    public partial class EventControl : UserControl
    {
        public EventControl()
        {
            InitializeComponent();
        }
        public ImageSource EventLogoSource
        {
            get { return imgEventLogo.Source; }
            set { imgEventLogo.Source = value; }
        }
        public string EventName
        {
            get { return tblEventName.Text; }
            set { tblEventName.Text = value; }
        }
        public string EventDate
        {
            get { return tblEventDate.Text; }
            set { tblEventDate.Text = value; }
        }
        public string EventLocation
        {
            get { return tblEventLocation.Text; }
            set { tblEventLocation.Text = value; }
        }
        public string EventCreatedLabel
        {
            get { return tblCreatedLabel.Text; }
            set { tblCreatedLabel.Text = value; }
        }
        public string EventCreatedDate
        {
            get { return tblCreatedDate.Text; }
            set { tblCreatedDate.Text = value; }
        }

    }
}
