using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MeetpointPrinterNew
{

    #region EVENT OBJECT


    [XmlRoot(ElementName = "result")]
    public class EventDataResponse
    {
        [XmlAttribute(AttributeName = "serviceStatus")]
        public string serviceStatus { get; set; }

        [XmlElement(ElementName = "data")]
        public EventData data { get; set; }
    }


    [XmlRoot(ElementName = "data")]
    public class EventData
    {
        [XmlAttribute(AttributeName = "status")]
        public string Status { set; get; }

        [XmlAttribute(AttributeName = "message")]
        public string Message { set; get; }

        [XmlElement(ElementName = "events")]
        public List<EventDataEvent> events { set; get; }
    }

    [XmlRoot(ElementName = "events")]
    public class EventDataEvent
    {
        [XmlAttribute(AttributeName = "EventID")]
        public int EventID { set; get; }

        [XmlAttribute(AttributeName = "EventName")]
        public string EventName { set; get; }

        [XmlAttribute(AttributeName = "Tags")]
        public string Tags { set; get; }

        [XmlAttribute(AttributeName = "Location")]
        public string Location { set; get; }

        [XmlAttribute(AttributeName = "Organiser")]
        public string Organiser { set; get; }

        [XmlAttribute(AttributeName = "Slug")]
        public string Slug { set; get; }

        [XmlAttribute(AttributeName = "DtStart")]
        public DateTime DtStart { set; get; }

        [XmlAttribute(AttributeName = "DtEnd")]
        public DateTime DtEnd { set; get; }

        [XmlElement(ElementName = "programs")]
        public List<EventDataEventProgram> programs { set; get; }
    }

    [XmlRoot(ElementName = "programs")]
    public class EventDataEventProgram
    {
        [XmlAttribute(AttributeName = "ProgramName")]
        public string ProgramName { set; get; }

        [XmlAttribute(AttributeName = "EventProgramID")]
        public int EventProgramID { set; get; }

        [XmlAttribute(AttributeName = "DtStart")]
        public DateTime DtStart { set; get; }

        [XmlAttribute(AttributeName = "DtEnd")]
        public DateTime DtEnd { set; get; }

        [XmlAttribute(AttributeName = "ProgramLocation")]
        public string ProgramLocation { set; get; }

        [XmlAttribute(AttributeName = "MaxParticipants")]
        public int MaxParticipants { set; get; }
    }

    #endregion EVENT OBJECT


    public class Data
    {
        public string authToken { get; set; }
        public string authStatus { get; set; }
        public object authStatusDescription { get; set; }
    }

    public class ServiceResponse
    {
        public string serviceStatus { get; set; }
        public Data data { get; set; }
    }

    public class PrintQueueItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string JobPosition { get; set; }
        public string Country { get; set; }

        public int PrintUserID { get; set; }
        public string ActionUID { get; set; }

        public string EventPosition { get; set; }


    }

    public class PrintQueue
    {
        public List<PrintQueueItem> Item { get; set; }
    }

    public class PrintQueueResponse
    {
        public string serviceStatus { get; set; }
        public PrintQueue data { get; set; }
    }

    public class User
    {
        public int key { get; set; }
        public string value { get; set; }
    }

    public class UserData
    {
        public List<User> Rows { get; set; }
    }

    public class GetUserDataResponse
    {
        public string serviceStatus { get; set; }
        public UserData data { get; set; }
    }


    [XmlRoot(ElementName = "Event")]
    public class Event
    {
        [XmlElement(ElementName = "EventID")]
        public int EventID { get; set; }
        [XmlElement(ElementName = "EventName")]
        public string EventName { get; set; }
        [XmlElement(ElementName = "EventLocation")]
        public string EventLocation { get; set; }
        [XmlElement(ElementName = "EventStartDate")]
        public DateTime EventStartDate { get; set; }
        [XmlElement(ElementName = "EventEndDate")]
        public DateTime EventEndDate { get; set; }
        [XmlElement(ElementName = "EventCreatedOn")]
        public DateTime EventCreatedOn { get; set; }
    }

    [XmlRoot(ElementName = "Account")]
    public class Account
    {
        [XmlElement(ElementName = "AccountID")]
        public string AccountID { get; set; }
        [XmlElement(ElementName = "AccountName")]
        public string AccountName { get; set; }
    }

    [XmlRoot(ElementName = "Accounts")]
    public class Accounts
    {
        [XmlElement(ElementName = "Account")]
        public List<Account> Account { get; set; }
    }

    [XmlRoot(ElementName = "DataOptions")]
    public class DataOptions
    {
        [XmlElement(ElementName = "DataOption")]
        public List<string> DataOption { get; set; }
    }

    [XmlRoot(ElementName = "PrinterSetup")]
    public class PrinterSetup
    {
        [XmlElement(ElementName = "LayoutSizeID")]
        public string LayoutSizeID { get; set; }
        [XmlElement(ElementName = "LayoutWidth")]
        public double LayoutWidth { get; set; }
        [XmlElement(ElementName = "LayoutHeight")]
        public double LayoutHeight { get; set; }

        [XmlElement(ElementName = "LayoutTemplate")]
        public string LayoutTemplate { get; set; }
        [XmlElement(ElementName = "DataOptions")]
        public DataOptions DataOptions { get; set; }
    }

    [XmlRoot(ElementName = "UserSettings")]
    public class UserSettings
    {
        [XmlElement(ElementName = "AuthToken")]
        public string AuthToken { get; set; }

        [XmlElement(ElementName = "Username")]
        public string Username { get; set; }

        [XmlElement(ElementName = "Printer")]
        public string Printer { get; set; }

        [XmlElement(ElementName = "Event")]
        public Event Event { get; set; }
        [XmlElement(ElementName = "Accounts")]
        public Accounts Accounts { get; set; }
      
        [XmlElement(ElementName = "PrinterSetup")]
        public PrinterSetup PrinterSetup { get; set; }
    }

}
