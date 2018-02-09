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
using System.Windows.Shapes;

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
        public SettingsPage(string accessToken)
        {
           
            this._accessToken = accessToken;
            InitializeComponent();
            LoadCustomerUsers();

            this.ddAvailablePrinters.ItemsSource = Helpers.GetConnectedPrinters();

            (Application.Current as App).CurrentPage = "Settings page";

            cbTemplateSize.Items.Add("150x150");
            cbTemplateSize.Items.Add("170x170");
            cbTemplateSize.Items.Add("150x170");
            cbTemplateSize.Items.Add("170x200");
            cbTemplateSize.Items.Add("150x200");

            _borderList = new List<Border>();
            _borderList.Add(tmp_1);
            _borderList.Add(tmp_2);
            _borderList.Add(tmp_3);
            _borderList.Add(tmp_4);
            _borderList.Add(tmp_5);
            _borderList.Add(tmp_6);

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //save logic
           
            int templateHeight = 0;
            int templateWidth = 0;
            string printDevice = "";
            System.Collections.Specialized.StringCollection users = new System.Collections.Specialized.StringCollection();
            if(cbTemplateSize.SelectedIndex>-1)
            {
                string text = (cbTemplateSize as ComboBox).SelectedItem as string;
                string[] size = text.Split('x');
                templateHeight = int.Parse(size[0]);
                templateWidth = int.Parse(size[1]);
            }
            if(ddAvailablePrinters.SelectedIndex>-1)
            {
                printDevice = (ddAvailablePrinters as ComboBox).SelectedItem as string;
            }
            if(cbUsers.SelectedItems.Count > 0)
            {
                foreach(var item in cbUsers.SelectedItems)
                {
                    users.Add(item.ToString());
                }
                
            }
            Properties.Settings.Default.PrintDevice = printDevice;
            Properties.Settings.Default.PrintTemplateHeight = templateHeight;
            Properties.Settings.Default.PrintTemplateWidth = templateWidth;
            Properties.Settings.Default.PrintUsers = users;
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
    }
}
