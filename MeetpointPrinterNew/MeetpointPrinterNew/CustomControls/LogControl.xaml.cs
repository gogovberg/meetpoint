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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeetpointPrinterNew.CustomControls
{
    /// <summary>
    /// Interaction logic for LogControl.xaml
    /// </summary>
    public partial class LogControl : UserControl
    {
        public event EventHandler Preview_Click;
        public event EventHandler PrintAgain_Click;

        public ImageSource UsernameLogoSource
        {
            get { return imgName.Source; }
            set { imgName.Source = value; }
        }
        public ImageSource StatusLogoSource
        {
            get { return imgStatus.Source; }
            set { imgStatus.Source = value; }
        }
        public string ButtonPrivewContent
        {
            get { return (string)btnPreview.Content; }
            set { btnPreview.Content = value; }
        }
        public string ButtonPrintAgainContent
        {
            get { return (string)btnPrintAgain.Content; }
            set { btnPrintAgain.Content = value; }
        }
        public string LogUsername
        {
            get { return tbName.Text; }
            set { tbName.Text = value; }
        }
        public string LogStatus
        {
            get { return tbStatus.Text; }
            set { tbStatus.Text = value; }
        }

        public PrintQueueItem PrintQueueItem { set; get; }

        public LogControl()
        {
            InitializeComponent();
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (this.Preview_Click != null)
            {
                this.Preview_Click(this, e);
            }
           
        }

        private void btnPrintAgain_Click(object sender, RoutedEventArgs e)
        {
            if (this.PrintAgain_Click != null)
            {
                this.PrintAgain_Click(this, e);
            }
            
        }
    }
}
