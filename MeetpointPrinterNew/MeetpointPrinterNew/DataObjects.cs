﻿using System;
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
        public int Status { set; get; }

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

        [XmlElement(ElementName = "TextAlignment")]
        public int TextAlignment { get; set; }
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

        [XmlElement(ElementName = "IsPrinterSet")]
        public bool IsPrinterSet { set; get; }

        [XmlElement(ElementName = "IsAccountsSet")]
        public bool IsAccountsSet { set; get; }

        [XmlElement(ElementName = "IsTemplateSet")]
        public bool IsTemplateSet { set; get; }
    }


    public enum WizardStep
    {
        Printer = 0,
        Account = 1,
        Template = 2
    }
    public enum WizardStepState
    {
        CurrentEmpty,
        Empty,
        CurrentFilled,
        Filled
    }
    public enum WizardStepLine
    {
        Empty=0,
        PrinterAccount=1,
        AccountTemplate=2
    }

    public enum PrintTemplateSize
    {
        TemplateSetupBig=0,
        TemplateSetupSmall=1,
        SettingsBig =2,
        SettingsSmall=3,
        PrintLogBig =4,
        PrintLogSmall=5
    }
    public enum TextLength
    {
        ExtraShort = 30,
        VeryShort = 34,
        Short = 30,
        Normal = 26,
        Long = 23,
        VeryLong = 21,
        ExtraLong= 19
    }
    public enum TextFontSize
    {
        ExtraSmall = 30,
        VerySmall  = 35,
        Small = 40,
        Normal = 45,
        Big = 50,
        VeryBig = 55,
        ExtraBig = 60
    }
    public enum TextField
    {
        FieldOne=1,
        FieldTwo=2,
        FieldThree=3,
        FieldFour=4,
        FieldFive=5
    }
    public enum TextOffset
    {
        OffsetOne = 150,
        OffsetTwo = 110,
        OffsetThree = 90,
        OffsetFour = 40,
        OffsetFive = 0
    }

}
