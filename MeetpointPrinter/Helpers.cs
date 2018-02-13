using Com.SharpZebra.Printing;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetpointPrinter
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


    public class UserSettings
    {
        public string Username { set; get; }
        public string PrintTemplate { set; get; }
        public int TemplateHeight { set; get; }
        public int TemplateWidth { set; get; }
        public string PrintDevice { set; get; }
        public List<string> PrintUsers { set; get; }
       
    }

}
