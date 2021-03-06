﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MeetpointPrinterNew.CustomControls
{
    /// <summary>
    /// Interaction logic for EventControl.xaml
    /// </summary>
    public partial class EventControl : UserControl
    {
        private DateTime downTime;
        private object downSender;
        public event EventHandler Control_Click;

        public int EventID { set; get; }

        public ImageSource EventLogoSource
        {
            get { return imgEventLogo.Source; }
            set { imgEventLogo.Source = value; }
        }
        public string EventName
        {
            get { return tblEventName.Text; }
            set { tblEventName.Text = value; }
        }
        public string EventDate
        {
            get { return tblEventDate.Text; }
            set { tblEventDate.Text = value; }
        }
        public string EventLocation
        {
            get { return tblEventLocation.Text; }
            set { tblEventLocation.Text = value; }
        }
        public string EventCreatedLabel
        {
            get { return tblCreatedLabel.Text; }
            set { tblCreatedLabel.Text = value; }
        }
        public string EventCreatedDate
        {
            get { return tblCreatedDate.Text; }
            set { tblCreatedDate.Text = value; }
        }

        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(EventControl), new PropertyMetadata(false));


        public EventControl()
        {
            InitializeComponent();
        }
      
        protected void EventBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.downSender = sender;
                this.downTime = DateTime.Now;
            }
        }

        protected void EventBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
          
            if (e.LeftButton == MouseButtonState.Released && sender == this.downSender)
            {
                TimeSpan timeSinceDown = DateTime.Now - this.downTime;
                if (timeSinceDown.TotalMilliseconds < 500)
                {

                    if (this.Control_Click != null)
                    {
                        this.Control_Click(this, e);
                    }

                }
            }

        }

    }
}
