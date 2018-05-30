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
        private App _currentApp = ((App)Application.Current);
       

        public EventPage(string username, string accessToken)
        {
            InitializeComponent();
            GlobalSettings.CurrentPageID = 1;

            _currentApp.CurrentUser = username;
            BitmapImage imgsrc = new BitmapImage(new Uri("/Images/icon_event_primary.png", UriKind.Relative));
            Style eventStyle = (Style)FindResource("EventBorderStyle");
            List<EventDataEvent> edv = Helpers.GetEvents(accessToken);

            foreach(EventDataEvent item in edv)
            {
                EventControl ec1 = new EventControl();
                ec1.EventName = item.EventName;
                ec1.EventDate = item.DtStart.ToShortDateString() +" "+item.DtEnd.ToShortDateString();
                ec1.EventLocation = item.Location;
                ec1.EventCreatedDate = item.DtStart.ToShortDateString();
                ec1.EventLogoSource = imgsrc;
                ec1.EventCreatedLabel = "CREATED ON";
                ec1.EventID = item.EventID;
                ec1.Control_Click += new EventHandler(Control_click);
                ec1.Style = eventStyle;
                icEventItems.Items.Add(ec1);
            }

            foreach(EventControl ec in icEventItems.Items)
            {
                if(GlobalSettings.ApplicationSettings.Event.EventID == ec.EventID)
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

            GlobalSettings.ApplicationSettings.Event = eve;
            _currentApp.CurrentEvent = ec.EventName;
            _currentApp.CurrentEventLocation = ec.EventDate +" " + ec.EventLocation;

            Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);

            GlobalSettings.PreviousPageID = GlobalSettings.PreviousPageID;

            if (!string.IsNullOrWhiteSpace(GlobalSettings.ApplicationSettings.Printer) && GlobalSettings.ApplicationSettings.Accounts!=null && GlobalSettings.ApplicationSettings.Accounts.Account.Count>0)
            {
                Application.Current.MainWindow.Content = new SettingsPage(GlobalSettings.ApplicationSettings);
            }
            else
            {
                
                Application.Current.MainWindow.Content = new SetupPage(GlobalSettings.ApplicationSettings, 0);
            }
            
           
        }

       
      
    }
}
