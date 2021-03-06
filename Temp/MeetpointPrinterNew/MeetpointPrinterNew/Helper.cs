﻿using Com.SharpZebra.Printing;
using hgi.Environment;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;

namespace MeetpointPrinterNew
{
    public static class Helpers
    {
        private static string _host = "http://data.meetpoint.si/";
        private static double _milimeterInPixels = 3.7795275591;
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

       

        public static List<EventDataEvent> GetEvents(string AuthToken)
        {
            List<EventDataEvent> edv = new List<EventDataEvent>();
            try
            {
                var client = new RestClient(_host+"rest/v1/DataAPI/GetEventProgram/xml");
                var request = new RestRequest(Method.POST);

                request.AddHeader("content-type", "multipart/form-data;");
                request.AddParameter("AuthToken", AuthToken);
                request.AddParameter("Tags", "");
                IRestResponse response = client.Execute(request);


                EventDataResponse ev = new EventDataResponse();

                ev = (EventDataResponse)XmlToObject(response.Content, ev.GetType());
                if(ev!=null && ev.serviceStatus.Equals("OK") && ev.data.Status.Equals("OK"))
                {
                    edv = ev.data.events;
                }


            }
            catch(Exception ex)
            {
                Debug.Log("MeetpointPrinter", ex.ToString());
            }


            return edv;
        }
        public static List<User> GetCustomerUsers(string AuthToken)
        {
            List<User> users = new List<User>();
            try
            {
                var client = new RestClient(_host + "rest/v1/DataAPI/GetCustomerUsers/json");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "multipart/form-data;");
                request.AddParameter("AuthToken", AuthToken);
                IRestResponse response = client.Execute(request);

                var res = SimpleJson.DeserializeObject<GetUserDataResponse>(response.Content);
                if (res !=null && res.serviceStatus == "OK" && res.data!=null) 
                {
                    users = res.data.Rows;
                }


            }
            catch (Exception ex)
            {
                Debug.Log("MeetpointPrinter", ex.ToString());
            }
            return users;
        }

        public static List<PrintQueueItem> GetPrintQueue(string AuthToken,string Users)
        {
            List<PrintQueueItem> queue = new List<PrintQueueItem>();
            try
            {
                var client = new RestClient(_host + "rest/v1/DataAPI/GetLabelPrintQueueForUsers/json");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "multipart/form-data;");
                request.AddParameter("AuthToken", AuthToken);
                request.AddParameter("UserIDs", Users);
                request.AddParameter("EventID",GlobalSettings.ApplicationSettings.Event.EventID);
                IRestResponse response = client.Execute(request);

                var res = SimpleJson.DeserializeObject<PrintQueueResponse>(response.Content);
                if (res != null && res.serviceStatus == "OK" && res.data != null)
                {
                    queue = res.data.Item;
                }


            }
            catch (Exception ex)
            {
                Debug.Log("MeetpointPrinter", ex.ToString());
            }
            return queue;
        }

        public static string ToTitleCase(this string s)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public static void SaveUserSettings(UserSettings up)
        {
            try
            {
                Debug.Log("MeetpointPrinter", "Save user settings");
                if (up != null)
                {
                    string path = @"UserSettings/" + up.Username + "_"+up.Event.EventID+"_settings.config";
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

        public static UserSettings ReadUserSettings(string Username,string EventID)
        {
            UserSettings us = null;
            try
            {

                Debug.Log("MeetpointPrinter", "Read user settings");

                string path = @"UserSettings/" + Username + "_" + EventID + "_settings.config";
                if (File.Exists(path))
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(UserSettings));
                    FileStream myFileStream = new FileStream(path, FileMode.Open);
                    us = (UserSettings)mySerializer.Deserialize(myFileStream);
                    myFileStream.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.Log("MeetpointPrinter", ex.ToString());

            }
            return us;

        }

        public static void RemoveAccountItem(List<Account> items, string cbTag)
        {
            try
            {
                int removeindex = -1;
                if (items != null && !string.IsNullOrEmpty(cbTag))
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i].AccountID.Equals(cbTag))
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

        public static string ObjectToXml(object o)
        {
            StringWriter sw = new Utf8StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception e)
            {
                Debug.Log("MeetpointPrinter", e.ToString());
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }

        public static Object XmlToObject(string xml, Type objectType)
        {
            StringReader strReader = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;
            Object obj = null;
            try
            {
                strReader = new StringReader(xml);
                serializer = new XmlSerializer(objectType);
                xmlReader = new XmlTextReader(strReader);
                obj = serializer.Deserialize(xmlReader);
            }
            catch (Exception e)
            {
                Debug.Log("MeetpointPrinter", e.ToString());
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                if (strReader != null)
                {
                    strReader.Close();
                }
            }
            return obj;
        }

        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

        public static bool IsComplete(int currentPageID)
        {

      
            bool isComplete = false;

            switch (currentPageID)
            {
                case 2:
                    if (!string.IsNullOrEmpty(GlobalSettings.ApplicationSettings.Printer))
                    {
                        isComplete = true;
                    }
                    break;
                case 3:
                    if (GlobalSettings.ApplicationSettings.Accounts.Account.Count > 0)
                    {
                        isComplete = true;
                    }
                    break;
                case 4:
                    int options = 0;
                    foreach(string option in GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption)
                    {
                        if(!string.IsNullOrEmpty(option))
                        {
                            options++;
                        }
                    }
                    if (options > 0 &&
                        !string.IsNullOrEmpty(GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate) &&
                        !string.IsNullOrEmpty(GlobalSettings.ApplicationSettings.PrinterSetup.LayoutSizeID)
                        )
                    {
                        isComplete = true;
                    }
                    break;
            }


            return isComplete;
        }

        public static string GetDataOptionsFiled(string FieldName, PrintQueueItem item)
        {

            string value = "";
            switch (FieldName)
            {
                case "cbName":
                    value = item.FirstName;
                    break;
                case "cbSurName":
                    value = item.LastName;
                    break;
                case "cbCompanyName":
                    value = item.Company;
                    break;
                case "cbFunctionName":
                    value = item.JobPosition;
                    break;
                case "cbCountryName":
                    value = item.Country;
                    break;
                case "cbGroupName":
                    value = "";
                    break;
                default:
                    value = "";
                    break;
            }

            return value;

        }

        public static double MilimeterPixelConverter(double value, bool direction=true)
        {

            double result = 0;

            //convert milimiters in pixels
            if(direction)
            {
                result = value * _milimeterInPixels;
            }
            else
            {
                // convert pixels in milimiters
                result = value / _milimeterInPixels;
            }
            return result;
        }
    }
  

}
