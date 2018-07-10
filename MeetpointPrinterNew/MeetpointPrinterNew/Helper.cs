using Com.SharpZebra;
using Com.SharpZebra.Printing;
using hgi.Environment;
using MeetpointPrinterNew.CustomControls;
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
using Zebra.Sdk.Printer;

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

        public static void SetPrintTemplateSize(PrintTemplateControl printTemplate, PrintTemplateSize size)
        {
            if(printTemplate!=null)
            {
                switch (size)
                {
                    case PrintTemplateSize.TemplateSetupBig:
                        printTemplate.Width = 281;
                        printTemplate.Height = 188;
                        printTemplate.tbOptOne.FontSize = 24;
                        printTemplate.tbOptTwo.FontSize = 24;
                        printTemplate.tbOptThree.FontSize = 22;
                        printTemplate.tbOptFour.FontSize = 16;
                        printTemplate.tbOptFive.FontSize = 16;
                        break;
                    case PrintTemplateSize.TemplateSetupSmall:
                        printTemplate.Width = 300;
                        printTemplate.Height = 150;
                        printTemplate.tbOptOne.FontSize = 24;
                        printTemplate.tbOptTwo.FontSize = 24;
                        printTemplate.tbOptThree.FontSize = 22;
                        printTemplate.tbOptFour.FontSize = 16;
                        printTemplate.tbOptFive.FontSize = 16;
                        break;
                    case PrintTemplateSize.SettingsBig:
                        printTemplate.Width = 188;
                        printTemplate.Height = 125;
                        printTemplate.tbOptOne.FontSize = 16;
                        printTemplate.tbOptTwo.FontSize = 16;
                        printTemplate.tbOptThree.FontSize = 14;
                        printTemplate.tbOptFour.FontSize = 8;
                        printTemplate.tbOptFive.FontSize = 8;
                        break;
                    case PrintTemplateSize.SettingsSmall:
                        printTemplate.Width = 200;
                        printTemplate.Height = 100;
                        printTemplate.tbOptOne.FontSize = 16;
                        printTemplate.tbOptTwo.FontSize = 16;
                        printTemplate.tbOptThree.FontSize = 14;
                        printTemplate.tbOptFour.FontSize = 8;
                        printTemplate.tbOptFive.FontSize = 8;
                        break;
                    case PrintTemplateSize.PrintLogBig:
                        printTemplate.Width = 281;
                        printTemplate.Height = 188;
                        printTemplate.tbOptOne.FontSize = 24;
                        printTemplate.tbOptTwo.FontSize = 24;
                        printTemplate.tbOptThree.FontSize = 22;
                        printTemplate.tbOptFour.FontSize = 16;
                        printTemplate.tbOptFive.FontSize = 16;
                        break;
                    case PrintTemplateSize.PrintLogSmall:
                        printTemplate.Width = 300;
                        printTemplate.Height = 150;
                        printTemplate.tbOptOne.FontSize = 24;
                        printTemplate.tbOptTwo.FontSize = 24;
                        printTemplate.tbOptThree.FontSize = 22;
                        printTemplate.tbOptFour.FontSize = 16;
                        printTemplate.tbOptFive.FontSize = 16;
                        break;
                }
            }
        }

       

        public static TextFontSize SetTextFontSize(string text, TextField textField)
        {
            TextFontSize fs = TextFontSize.Normal;
            int textLength = text.Length;
            switch(textField)
            {
                case TextField.FieldOne:
                    if(textLength<19)
                    {
                        fs = TextFontSize.ExtraBig;
                    }
                    else if(textLength>=19 && textLength < 21)
                    {
                        fs = TextFontSize.VeryBig;
                    }
                    else if(textLength >= 21 && textLength < 23)
                    {
                        fs = TextFontSize.Big;
                    }
                    else if (textLength >= 23)
                    {
                        fs = TextFontSize.Normal;

                    }
                    break;
                case TextField.FieldTwo:
                    if (textLength < 21)
                    {
                        fs = TextFontSize.VeryBig;
                    }
                    else if (textLength >= 21 && textLength < 23)
                    {
                        fs = TextFontSize.Big;
                    }
                    else if (textLength >= 23 && textLength < 26)
                    {
                        fs = TextFontSize.Normal;
                    }
                    else if (textLength >= 26)
                    {
                        fs = TextFontSize.Small;
                       
                    }
                    break;
                case TextField.FieldThree:
                    if (textLength < 23)
                    {
                        fs = TextFontSize.Big;
                    }
                    else if (textLength >= 23 && textLength < 26)
                    {
                        fs = TextFontSize.Normal;
                    }
                    else if (textLength >= 26 && textLength <30)
                    {
                        fs = TextFontSize.Small;
                    }
                    else if(textLength>=30)
                    {
                        fs = TextFontSize.VerySmall;
                        
                    }
                    break;
                case TextField.FieldFour:
                    fs = TextFontSize.ExtraSmall;
                   
                    break;
                case TextField.FieldFive:
                    fs = TextFontSize.ExtraSmall;
                   
                    break;
            }

            return fs;
        }

        public static TextOffset SetTextOffsetHeight(int dataOptionsLength)
        {
            TextOffset tof = TextOffset.OffsetFive;

            switch(dataOptionsLength)
            {
                case 1:
                    tof = TextOffset.OffsetOne;
                    break;
                case 2:
                    tof = TextOffset.OffsetTwo;
                    break;
                case 3:
                    tof = TextOffset.OffsetThree;
                    break;
                case 4:
                    tof = TextOffset.OffsetFour;
                    break;
                case 5:
                    tof = TextOffset.OffsetFive;
                    break;
                default:
                    tof = TextOffset.OffsetFive;
                    break;
            }

            return tof;
        }
       
        public static string ReturnDataOptionsName(string cbName)
        {
            switch(cbName)
            {
                case "cbName":
                    return "First name";
                case "cbSurName":
                    return "Last name";
                case "cbCompanyName":
                    return "Company";
                case "cbFunctionName":
                    return "Job position";
                case "cbCountryName":
                    return "Country";
                case "cbGroupName":
                    return "Group";
                default:
                    return "";
            }
          
        }


        public static void PrintLabelQRight(PrintQueueItem item, ZebraPrinter printer)
        {
            TextFontSize tfs;

            string commandOne = "";
            string commandTwo = "";
            string commandThree = "";
            string commandFour = "";
            string commandFive = "";
            string qrHash = "";
            string qrCommand = "";
            int dataOptionsLength = 0;

            foreach (string opt in GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption)
            {
                if (!string.IsNullOrEmpty(opt))
                {
                    dataOptionsLength++;
                }
            }

            string fieldOne = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[0], item);
            string fieldTwo = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[1], item);
            string fieldThree = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[2], item);
            string fieldFour = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[3], item);
            string fieldFive = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[4], item);

            fieldOne = string.IsNullOrEmpty(fieldOne) ? "" : fieldOne;
            fieldTwo = string.IsNullOrEmpty(fieldTwo) ? "" : fieldTwo;
            fieldThree = string.IsNullOrEmpty(fieldThree) ? "" : fieldThree;
            fieldFour = string.IsNullOrEmpty(fieldFour) ? "" : fieldFour;
            fieldFive = string.IsNullOrEmpty(fieldFive) ? "" : fieldFive;

            qrHash = item.ActionUID == null ? "" : item.ActionUID;

            TextOffset textOffset = TextOffset.OffsetFive;

            switch (dataOptionsLength)
            {
                case 1:
                    textOffset = TextOffset.OffsetThree;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    qrCommand = QRWrite(125, 200, 0, 5, QRErrorCorrection.ULTRA_HIGH, qrHash);
                    break;
                case 2:
                    textOffset = TextOffset.OffsetFour;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    qrCommand = QRWrite(125, 220, 0, 5, QRErrorCorrection.ULTRA_HIGH, qrHash);
                    break;
                case 3:
                    textOffset = TextOffset.OffsetFive;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    tfs = Helpers.SetTextFontSize(fieldThree, TextField.FieldThree);
                    commandThree = TextWrite('0', (int)tfs, 125, 180 + (int)textOffset, 580, 1, fieldThree);

                    qrCommand = QRWrite(125, 250, 0, 5, QRErrorCorrection.ULTRA_HIGH, qrHash);
                    break;
                case 4:
                    textOffset = TextOffset.OffsetFive;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    tfs = Helpers.SetTextFontSize(fieldThree, TextField.FieldThree);
                    commandThree = TextWrite('0', (int)tfs, 125, 180 + (int)textOffset, 580, 1, fieldThree);

                    tfs = Helpers.SetTextFontSize(fieldFour, TextField.FieldFour);
                    commandFour = TextWrite('0', (int)tfs, 125, 260 + (int)textOffset, 420, 2, fieldFour);

                    qrCommand = QRWrite(550, 250, 0, 5, QRErrorCorrection.ULTRA_HIGH, qrHash);
                    break;
                case 5:
                    textOffset = TextOffset.OffsetFive;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    tfs = Helpers.SetTextFontSize(fieldThree, TextField.FieldThree);
                    commandThree = TextWrite('0', (int)tfs, 125, 180 + (int)textOffset, 580, 1, fieldThree);

                    tfs = Helpers.SetTextFontSize(fieldFour, TextField.FieldFour);
                    commandFour = TextWrite('0', (int)tfs, 125, 260 + (int)textOffset, 420, 2, fieldFour);

                    tfs = Helpers.SetTextFontSize(fieldFive, TextField.FieldFive);
                    commandFive = TextWrite('0', (int)tfs, 125, 330 + (int)textOffset, 420, 2, fieldFive);

                    qrCommand = QRWrite(550, 250, 0, 5, QRErrorCorrection.ULTRA_HIGH, qrHash);
                    break;
            }


            printer.SendCommand(WholeCommand(qrCommand, commandOne, commandTwo, commandThree, commandFour, commandFive), "ISO-8859-1");
        }

        public static void PrintLabelClear(PrintQueueItem item, ZebraPrinter printer)
        {
            TextFontSize tfs;


            int dataOptionsLength = 0;
            foreach (string opt in GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption)
            {
                if (!string.IsNullOrEmpty(opt))
                {
                    dataOptionsLength++;
                }
            }

            string fieldOne = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[0], item);
            string fieldTwo = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[1], item);
            string fieldThree = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[2], item);
            string fieldFour = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[3], item);
            string fieldFive = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[4], item);

            fieldOne = string.IsNullOrEmpty(fieldOne) ? "" : fieldOne;
            fieldTwo = string.IsNullOrEmpty(fieldTwo) ? "" : fieldTwo;
            fieldThree = string.IsNullOrEmpty(fieldThree) ? "" : fieldThree;
            fieldFour = string.IsNullOrEmpty(fieldFour) ? "" : fieldFour;
            fieldFive = string.IsNullOrEmpty(fieldFive) ? "" : fieldFive;

            string commandOne = "";
            string commandTwo = "";
            string commandThree = "";
            string commandFour = "";
            string commandFive = "";

            TextOffset textOffset = TextOffset.OffsetFive;

            switch (dataOptionsLength)
            {
                case 1:
                    textOffset = TextOffset.OffsetOne;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 4, fieldOne);
                    break;
                case 2:
                    textOffset = TextOffset.OffsetTwo;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 3, fieldTwo);
                    break;
                case 3:
                    textOffset = TextOffset.OffsetThree;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    tfs = Helpers.SetTextFontSize(fieldThree, TextField.FieldThree);
                    commandThree = TextWrite('0', (int)tfs, 125, 180 + (int)textOffset, 580, 3, fieldThree);
                    break;
                case 4:
                    textOffset = TextOffset.OffsetFour;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    tfs = Helpers.SetTextFontSize(fieldThree, TextField.FieldThree);
                    commandThree = TextWrite('0', (int)tfs, 125, 180 + (int)textOffset, 580, 1, fieldThree);

                    tfs = Helpers.SetTextFontSize(fieldFour, TextField.FieldFour);
                    commandFour = TextWrite('0', (int)tfs, 125, 260 + (int)textOffset, 580, 3, fieldFour);
                    break;
                case 5:
                    textOffset = TextOffset.OffsetFive;

                    tfs = Helpers.SetTextFontSize(fieldOne, TextField.FieldOne);
                    commandOne = TextWrite('0', (int)tfs, 125, 40 + (int)textOffset, 580, 1, fieldOne);

                    tfs = Helpers.SetTextFontSize(fieldTwo, TextField.FieldTwo);
                    commandTwo = TextWrite('0', (int)tfs, 125, 110 + (int)textOffset, 580, 1, fieldTwo);

                    tfs = Helpers.SetTextFontSize(fieldThree, TextField.FieldThree);
                    commandThree = TextWrite('0', (int)tfs, 125, 180 + (int)textOffset, 580, 1, fieldThree);

                    tfs = Helpers.SetTextFontSize(fieldFour, TextField.FieldFour);
                    commandFour = TextWrite('0', (int)tfs, 125, 260 + (int)textOffset, 580, 2, fieldFour);

                    tfs = Helpers.SetTextFontSize(fieldFive, TextField.FieldFive);
                    commandFive = TextWrite('0', (int)tfs, 125, 330 + (int)textOffset, 580, 2, fieldFive);
                    break;
            }

            printer.SendCommand(WholeCommand("", commandOne, commandTwo, commandThree, commandFour, commandFive), "ISO-8859-1");
        }

        private static string QRWrite(int left, int top, int justification, int magnitude, QRErrorCorrection errorCorrection, string barcode)
        {
            string command = string.Format(
                                 "^FO{0},{1},{2}," +
                                 "^BQ,2,{3}" +
                                 "^FD72M,A{4}" +
                                 "^FS", (object)left, (object)top, (object)justification, (object)magnitude, (object)(char)errorCorrection, (object)barcode);

            return command;
        }

        private static string TextWrite(char fontType, int fontSize, int left, int top, int blokSize, int blockLines, string text)
        {
            string command = string.Format(
                                 "^CF{0},{1}" +
                                 "^FO{2},{3}" +
                                 "^TBN,{4},{5}" +
                                 "^FH" +
                                 "^FD{6}" +
                                 "^FS", (object)(char)fontType, (object)fontSize, (object)left, (object)top, (object)blokSize, (object)(blockLines * fontSize), (object)text);

            return command;
        }

        private static string WholeCommand(string commandQr, string commandTextOne, string commandTextTwo, string commandTextThree, string commandTextFour, string commandTextFive)
        {
            return string.Format("^XA{0}{1}{2}{3}{4}{5}^XZ", commandQr, commandTextOne, commandTextTwo, commandTextThree, commandTextFour, commandTextFive);
        }
    }
}
