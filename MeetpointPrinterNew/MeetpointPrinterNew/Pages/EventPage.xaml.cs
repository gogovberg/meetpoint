using MeetpointPrinterNew.CustomControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace MeetpointPrinterNew.Pages
{
    /// <summary>
    /// Interaction logic for EventPage.xaml
    /// </summary>
    public partial class EventPage : Page
    {
        private App _currentApp = ((App)Application.Current);

        private string _username = "";
        private string _accessToken = "";
        public EventPage(string username, string accessToken, string EventID)
        {
            InitializeComponent();
            _username = username;
            _accessToken = accessToken;
            GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;

            GlobalSettings.CurrentPageID = 1;

            _currentApp.CurrentUser = username;
            BitmapImage imgsrc = new BitmapImage(new Uri("/Images/icon_event_primary.png", UriKind.Relative));
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
                icEventItems.Items.Add(ec1);
            }

            foreach(EventControl ec in icEventItems.Items)
            {
                if(EventID.Equals(ec.EventID.ToString()))
                {
                    ec.IsSelected = true;
                }
            }
        }

        protected void Control_click(object sender, EventArgs e)
        {
            EventControl ec = (EventControl)sender;
            UserSettings us = Helpers.ReadUserSettings(_username, ec.EventID.ToString());
            if (us == null)
            {
                us = new UserSettings();
                us.Event = new Event();
                us.Event.EventName = ec.EventName;
                us.Event.EventID = ec.EventID;
                us.Event.EventStartDate = DateTime.MinValue;
                us.Event.EventEndDate = DateTime.MaxValue;
                us.Event.EventCreatedOn = DateTime.Parse(ec.EventCreatedDate);
                us.Event.EventLocation = ec.EventLocation;
                us.Accounts = new Accounts();
                us.Accounts.Account = new List<Account>();
                us.PrinterSetup = new PrinterSetup();
                us.PrinterSetup.DataOptions = new DataOptions();
                us.PrinterSetup.DataOptions.DataOption = new List<string>();
                us.Username = _username;
                us.AuthToken = _accessToken;
                Helpers.SaveUserSettings(us);
                GlobalSettings.ApplicationSettings = us;
            }
            else
            {
                us.Username = _username;
                us.AuthToken = _accessToken;
                Helpers.SaveUserSettings(us);
                GlobalSettings.ApplicationSettings = us;
            }
            _currentApp.CurrentEvent = ec.EventName;
            _currentApp.CurrentEventLocation = ec.EventDate + " " + ec.EventLocation;
            _currentApp.CurrentEventLocation = string.Format("{0}{1}{2}{3}{4}",
                us.Event.EventStartDate.ToShortDateString(),
                us.Event.EventStartDate != us.Event.EventEndDate ? " - " : "",
                us.Event.EventStartDate != us.Event.EventEndDate ? us.Event.EventEndDate.ToShortDateString() : "",
                string.IsNullOrEmpty(ec.EventLocation) ? "" : ", ",
                string.IsNullOrEmpty(ec.EventLocation) ? "" : ec.EventLocation
                );

            Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
            GlobalSettings.PreviousPageID = GlobalSettings.PreviousPageID;
            if (!string.IsNullOrWhiteSpace(GlobalSettings.ApplicationSettings.Printer) && GlobalSettings.ApplicationSettings.Accounts != null && GlobalSettings.ApplicationSettings.Accounts.Account.Count > 0)
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
