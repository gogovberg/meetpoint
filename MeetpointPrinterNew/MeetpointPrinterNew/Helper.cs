using Com.SharpZebra.Printing;
using hgi.Environment;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace MeetpointPrinterNew
{
    public static class Helpers
    {
        public static List<string> GetConnectedPrinters()
        {
            List<string> printers = new List<string>();

            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                printers.Add(printer);
            }

            return printers;
        }

        public static string GetDefaultPrinter()
        {
            PrinterSettings s = new PrinterSettings();
            return s.PrinterName;
        }

        public static PrintQueue GetPrintQueue(string AuthToken)

        {
            var client = new RestClient("http://data.meetpoint.si/rest/v1/DataAPI/GetLabelPrintQueue/json");
            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "multipart/form-data;");
            //request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\""+username+"\"\r\n\r\nrok\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"Password\"\r\n\r\nrok\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
            request.AddParameter("AuthToken", AuthToken);

            IRestResponse response = client.Execute(request);

            var res = SimpleJson.DeserializeObject<PrintQueueResponse>(response.Content);
            if (res.serviceStatus == "OK")
            {
                //if (res.data.authStatus == "OK")
                //    return Task.FromResult(res.data.authToken);
                //else
                //    return Task.FromResult(default(string));


                return res.data;

            }
            else

                return null;
        }

        public static string ToTitleCase(this string s)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public static UserData GetCustomerUsers(string AuthToken)
        {
            var client = new RestClient("http://data.meetpoint.si/rest/v1/DataAPI/GetCustomerUsers/json");
            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "multipart/form-data;");
            //request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\""+username+"\"\r\n\r\nrok\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"Password\"\r\n\r\nrok\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
            request.AddParameter("AuthToken", AuthToken);

            IRestResponse response = client.Execute(request);

            var res = SimpleJson.DeserializeObject<GetUserDataResponse>(response.Content);
            if (res.serviceStatus == "OK")
            {
                //if (res.data.authStatus == "OK")
                //    return Task.FromResult(res.data.authToken);
                //else
                //    return Task.FromResult(default(string));


                return res.data;

            }
            else

                return null;
        }

        public static void SaveUserSettings(UserSettings up)
        {
            try
            {
                Debug.Log("MeetpointPrinter", "Save user settings");
                if (up != null)
                {
                    string path = @"UserSettings/" + up.Username + "_settings.config";
                    XmlSerializer mySerializer = new XmlSerializer(typeof(UserSettings));
                    StreamWriter myWriter = new StreamWriter(path);
                    mySerializer.Serialize(myWriter, up);
                    myWriter.Close();

                }
            
            }
            catch (Exception ex)
            {
                Debug.Log("MeetpointPrinter", ex.ToString());

            }

        }

        public static UserSettings ReadUserSettings(string Username)
        {
            UserSettings up = null;
            try
            {
                Debug.Log("MeetpointPrinter", "Read user settings");

                string path = @"UserSettings/" + Username + "_settings.config";
                if (File.Exists(path))
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(UserSettings));
                    FileStream myFileStream = new FileStream(path, FileMode.Open);

                    up = (UserSettings)mySerializer.Deserialize(myFileStream);
                }
            }
            catch (Exception ex)
            {
                Debug.Log("MeetpointPrinter", ex.ToString());

            }

            return up;

        }

        public static void RemoveListItem(List<string> items, string cbName)
        {
            try
            {
                int removeindex = -1;
                if (items != null && !string.IsNullOrEmpty(cbName))
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i].Equals(cbName))
                        {
                            removeindex = i;
                            break;
                        }
                    }

                    if (removeindex > -1)
                    {
                        items.RemoveAt(removeindex);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.Log("MeetpointPrinter", ex.ToString());
            }
           
        }
    }
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
        [XmlElement(ElementName ="EventLocation")]
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
        public List<string> Account { get; set; }
    }

    [XmlRoot(ElementName = "Printer")]
    public class Printer
    { 
        [XmlElement(ElementName = "PrinterID")]
        public string PrinterID { get; set; }
        [XmlElement(ElementName = "PrinetrName")]
        public string PrinetrName { get; set; }
    }

    [XmlRoot(ElementName = "Printers")]
    public class Printers
    {
        [XmlElement(ElementName = "Printer")]
        public List<string> Printer { get; set; }
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
        public string LayoutSizeID{ get; set; }
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
        [XmlElement(ElementName = "Event")]
        public Event Event { get; set; }
        [XmlElement(ElementName = "Accounts")]
        public Accounts Accounts { get; set; }
        [XmlElement(ElementName = "Printers")]
        public Printers Printers { get; set; }
        [XmlElement(ElementName = "PrinterSetup")]
        public PrinterSetup PrinterSetup { get; set; }
    }

}
