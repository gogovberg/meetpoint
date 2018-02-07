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

        public SettingsPage()
        {
            InitializeComponent();
            (Application.Current as App).CurrentPage = "Settings page";
            cbTemplateSize.Items.Add("150x150");
            cbTemplateSize.Items.Add("170x170");
            cbTemplateSize.Items.Add("150x170");
            cbTemplateSize.Items.Add("170x200");
            cbTemplateSize.Items.Add("150x200");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //save logic
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
            if (e.LeftButton == MouseButtonState.Released &&
                sender == this.downSender)
            {
                TimeSpan timeSinceDown = DateTime.Now - this.downTime;
                if (timeSinceDown.TotalMilliseconds < 500)
                {
                    // Do click
                }
            }
        }
    }
}
