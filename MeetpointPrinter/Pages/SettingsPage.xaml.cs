using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace MeetpointPrinter.Pages
{

    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Window
    {
        private DateTime downTime;
        private object downSender;
        private string _accessToken;
        private string _printTemplate;
        private List<Border> _borderList;
        private  int _templateHeight = 0;
        private int _templateWidth = 0;
        private string _printDevice = "";
        private  System.Collections.Specialized.StringCollection _users;
        private List<string> _userlist;
        private char _delimiter = ';';
        private char _sizeDelimiter = 'x';

        public SettingsPage(string accessToken)
        {
           
            this._accessToken = accessToken;
            InitializeComponent();
            LoadCustomerUsers();

            this.ddAvailablePrinters.ItemsSource = Helpers.GetConnectedPrinters();

            (Application.Current as App).CurrentPage = "Settings page";

            cbTemplateSize.Items.Add("150"+ _sizeDelimiter + "150");
            cbTemplateSize.Items.Add("170" + _sizeDelimiter + "170");
            cbTemplateSize.Items.Add("150" + _sizeDelimiter + "170");
            cbTemplateSize.Items.Add("170" + _sizeDelimiter + "200");
            cbTemplateSize.Items.Add("150" + _sizeDelimiter + "200");

            _borderList = new List<Border>();
            _borderList.Add(tmp_1);
            _borderList.Add(tmp_2);
            _borderList.Add(tmp_3);
            _borderList.Add(tmp_4);
            _borderList.Add(tmp_5);
            _borderList.Add(tmp_6);

            RetrieveSettings();
            ReadUserSettings();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //save logic
            _users = new System.Collections.Specialized.StringCollection();
            _userlist = new List<string>();

            _printTemplate = "tmp_5";

            if (cbTemplateSize.SelectedIndex>-1)
            {
                string text = (cbTemplateSize as ComboBox).SelectedItem as string;
                string[] size = text.Split('x');
                _templateHeight = int.Parse(size[0]);
                _templateWidth = int.Parse(size[1]);
            }

            if(ddAvailablePrinters.SelectedIndex>-1)
            {
                _printDevice = (ddAvailablePrinters as ComboBox).SelectedItem as string;
            }

            if(cbUsers.SelectedItems.Count > 0)
            {
                foreach(KeyValuePair<int,string> item in cbUsers.SelectedItems)
                {
                    _users.Add(item.Key.ToString() + _delimiter + item.Value.ToString());
                    _userlist.Add(item.Key.ToString() + _delimiter + item.Value.ToString());
                }
                
            }
            SaveUserSettings();


            Properties.Settings.Default.PrintDevice = _printDevice;
            Properties.Settings.Default.PrintTemplateHeight = _templateHeight;
            Properties.Settings.Default.PrintTemplateWidth = _templateWidth;
            Properties.Settings.Default.PrintUsers = _users;
            Properties.Settings.Default.PrintTemplate = _printTemplate;
            Properties.Settings.Default.Save();
        }

        private void cbTemplateSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string text = (sender as ComboBox).SelectedItem as string;
            string[] size = text.Split('x');
            imgPriview.Height = int.Parse(size[0]);
            imgPriview.Width = int.Parse(size[1]);

        }
       
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.downSender = sender;
                this.downTime = DateTime.Now;
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Border borderObj = (Border)sender;
            Image borderChild = (Image)borderObj.Child;
            if (e.LeftButton == MouseButtonState.Released &&
                sender == this.downSender)
            {
                TimeSpan timeSinceDown = DateTime.Now - this.downTime;
                if (timeSinceDown.TotalMilliseconds < 500)
                {
                    // Do click
                    BorderClickLogic(borderObj.Name);
                    imgPriview.Source = borderChild.Source;

                }
            }
        }

        private void LoadCustomerUsers()
        {
            var usrList = Helpers.GetCustomerUsers(this._accessToken);
            Dictionary<int, string> data = new Dictionary<int, string>();
            foreach (User u in usrList.Rows)
            {
                if(data.ContainsKey(u.key))
                {
                    data[u.key] = u.value;
                }
                else
                {
                    data.Add(u.key, u.value);
                }
               
            }
            cbUsers.ItemsSource = data;
            cbUsers.DisplayMemberPath = "Value";
            cbUsers.ValueMemberPath = "Key";
        }

        private void RetrieveSettings()
        {
            _users = new System.Collections.Specialized.StringCollection();
            _users = Properties.Settings.Default.PrintUsers;
            _printDevice = Properties.Settings.Default.PrintDevice;
            _printTemplate = Properties.Settings.Default.PrintTemplate;
            _templateHeight = Properties.Settings.Default.PrintTemplateHeight;
            _templateWidth = Properties.Settings.Default.PrintTemplateWidth;

            //privew image
            Border tempBorder= (Border)this.FindName(_printTemplate);

            if(tempBorder != null)
            {
                imgPriview.Height = _templateHeight;
                imgPriview.Width = _templateWidth;
                imgPriview.Source = ((Image)tempBorder.Child).Source;
            }
           

            //print device
            if(!string.IsNullOrEmpty(_printDevice))
            {
                foreach (String item in ddAvailablePrinters.Items)
                {
                    if (item.ToString().Equals(_printDevice))
                    {
                        ddAvailablePrinters.SelectedItem = item;
                    }
                }
            }
          
            if(_users!=null)
            {
                foreach (string users in _users)
                {
                    string[] pair = users.Split(_delimiter);

                    if (pair.Length > 1)
                    {
                        KeyValuePair<int, string> pairD = new KeyValuePair<int, string>(int.Parse(pair[0]),pair[1]);
                        cbUsers.SelectedItems.Add(pairD);                       
                    }
                }
            }
         

            if(_templateHeight!=0 && _templateWidth!=0)
            {
                string size = _templateHeight.ToString() + _sizeDelimiter +  _templateWidth.ToString();
                foreach (String item in cbTemplateSize.Items)
                {
                    if (item.ToString().Equals(size))
                    {
                        cbTemplateSize.SelectedItem = item;
                    }
                }
            }
         



        }

        private void BorderClickLogic(string borderName)
        {
            foreach(Border bor in _borderList)
            {
                if(bor.Name.Equals(borderName))
                {
                    _printTemplate = bor.Name;
                    bor.BorderBrush = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    bor.Background = Brushes.Transparent;
                    bor.BorderBrush = new SolidColorBrush();
                }
            }
        }

        private void SaveUserSettings()
        {
            UserSettings up = new UserSettings();

            up.PrintDevice = _printDevice;
            up.TemplateHeight = _templateHeight;
            up.TemplateWidth = _templateWidth;
            up.PrintUsers = _userlist;
            up.PrintTemplate = _printTemplate;
          

            XmlSerializer mySerializer = new XmlSerializer(typeof(UserSettings));
            StreamWriter myWriter = new StreamWriter("c:/prefs.xml");
            mySerializer.Serialize(myWriter, up);
            myWriter.Close();
        }
        private void ReadUserSettings()
        {
            try
            {
                UserSettings up;

                XmlSerializer mySerializer = new XmlSerializer(typeof(UserSettings));
                FileStream myFileStream = new FileStream("c:/prefs.xml", FileMode.Open);

                up = (UserSettings)mySerializer.Deserialize(myFileStream);
            }
            catch
            {

            }
           
        }
    }
}
