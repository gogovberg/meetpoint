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
    /// Interaction logic for EventPage.xaml
    /// </summary>
    public partial class EventPage : Page
    {
        private App _currentApp = (Application.Current as App);

        public EventPage(string username, string accessToken)
        {
            InitializeComponent();
            _currentApp.CurrentUser = username;
          

            BitmapImage imgsrc = new BitmapImage(new Uri("/Images/icon_event_primary.png", UriKind.Relative));

            Style eventStyle = (Style)FindResource("EventBorderStyle");

            EventControl ec1 = new EventControl();
            ec1.EventName = "Test event 1";
            ec1.EventDate = "01.03.2018 - 02.03.2018";
            ec1.EventLocation = "Location number one";
            ec1.EventCreatedDate = "01.03.2018";
            ec1.EventLogoSource = imgsrc;
            ec1.EventCreatedLabel = "CREATED ON";
            ec1.EventID = 1;
            ec1.Control_Click += new EventHandler(Control_click);
            ec1.Style = eventStyle;
           


            EventControl ec2 = new EventControl();
            ec2.EventName = "Test event 2";
            ec2.EventDate = "02.03.2018 - 03.03.2018";
            ec2.EventLocation = "Location number two";
            ec2.EventCreatedDate = "02.03.2018";
            ec2.EventLogoSource = imgsrc;
            ec2.EventCreatedLabel = "CREATED ON";
            ec2.EventID = 2;
            ec2.Control_Click += new EventHandler(Control_click);
            ec2.Style = eventStyle;

            EventControl ec3 = new EventControl();
            ec3.EventName = "Test event 3";
            ec3.EventDate = "03.03.2018 - 04.03.2018";
            ec3.EventLocation = "Location number three";
            ec3.EventCreatedDate = "03.03.2018";
            ec3.EventLogoSource = imgsrc;
            ec3.EventCreatedLabel = "CREATED ON";
            ec3.EventID = 3;
            ec3.Control_Click += new EventHandler(Control_click);

            ec3.Style = eventStyle;

            EventControl ec4 = new EventControl();
            ec4.EventName = "Test event 4";
            ec4.EventDate = "04.03.2018 - 05.03.2018";
            ec4.EventLocation = "Location number four";
            ec4.EventCreatedDate = "044.03.2018";
            ec4.EventLogoSource = imgsrc;
            ec4.EventCreatedLabel = "CREATED ON";
            ec4.EventID = 4;
            ec4.Control_Click += new EventHandler(Control_click);
            ec4.Style = eventStyle;

            EventControl ec5 = new EventControl();
            ec5.EventName = "Test event 5";
            ec5.EventDate = "05.03.2018 - 06.03.2018";
            ec5.EventLocation = "Location number five";
            ec5.EventCreatedDate = "05.03.2018";
            ec5.EventLogoSource = imgsrc;
            ec5.EventCreatedLabel = "CREATED ON";
            ec5.EventID = 5;
            ec5.Control_Click += new EventHandler(Control_click);
            ec5.Style = eventStyle;

            icEventItems.Items.Add(ec1);
            icEventItems.Items.Add(ec2);
            icEventItems.Items.Add(ec3);
            icEventItems.Items.Add(ec4);
            icEventItems.Items.Add(ec5);

            
            foreach(EventControl ec in icEventItems.Items)
            {
                if(_currentApp.ApplicationSettings.Event.EventID == ec.EventID)
                {
                    ec.IsSelected = true;
                }
            }


        }

        protected void Control_click(object sender, EventArgs e)
        {
            EventControl ec = (EventControl)sender;

            Event eve = new Event();
            eve.EventName = ec.EventName;
            eve.EventID = ec.EventID;
            eve.EventStartDate = DateTime.MinValue;
            eve.EventEndDate = DateTime.MaxValue;
            eve.EventCreatedOn = DateTime.Parse(ec.EventCreatedDate);
            eve.EventLocation = ec.EventLocation;

            _currentApp.ApplicationSettings.Event = eve;
            _currentApp.CurrentEvent = ec.EventName;
            _currentApp.CurrentEventLocation = ec.EventDate +" " + ec.EventLocation;

            Helpers.SaveUserSettings(_currentApp.ApplicationSettings);

            Application.Current.MainWindow.Content = new SetupPage(0);
           
        }

       
      
    }
}
