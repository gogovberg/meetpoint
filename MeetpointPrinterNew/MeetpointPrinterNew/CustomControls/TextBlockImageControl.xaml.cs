using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MeetpointPrinterNew.CustomControls
{
    /// <summary>
    /// Interaction logic for TextBlockImageControl.xaml
    /// </summary>
    public partial class TextBlockImageControl : UserControl
    {
        private DateTime downTime;
        private object downSender;
        public event EventHandler Control_Click;
        public event EventHandler Control_UnClick;

        public string ContentID { set; get; }

        public ImageSource ContentImageSource
        {
            get { return imgContentImage.Source; }
            set { imgContentImage.Source = value; }
        }
        public string ContentText
        {
            get { return tblContentText.Text; }
            set { tblContentText.Text = value; }
        }

        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(TextBlockImageControl), new PropertyMetadata(false));

        public TextBlockImageControl()
        {
            InitializeComponent();
        }

        protected void TextBlockImageBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.downSender = sender;
                this.downTime = DateTime.Now;
            }
        }

        protected void TextBlockImageBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Released && sender == this.downSender)
            {
                TimeSpan timeSinceDown = DateTime.Now - this.downTime;
                if (timeSinceDown.TotalMilliseconds < 500)
                {
                    if(!IsSelected)
                    {
                        this.SetValue(IsSelectedProperty, true);
                        if (this.Control_Click != null)
                        {
                            this.Control_Click(this, e);
                        }
                    }
                    else
                    {
                        this.SetValue(IsSelectedProperty, false);
                        if (this.Control_UnClick != null)
                        {
                            this.Control_UnClick(this, e);
                        }
                    }

                }
            }

        }
    }
}
