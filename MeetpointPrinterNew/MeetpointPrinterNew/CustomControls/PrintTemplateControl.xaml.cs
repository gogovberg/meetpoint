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
    /// Interaction logic for PrintTemplateControl.xaml
    /// </summary>
    public partial class PrintTemplateControl : UserControl
    {
        public PrintTemplateControl()
        {
            InitializeComponent();

            LayoutHalfRight();
        }


        //public void LayoutHalfLeft()
        //{
        //    gdrPreview.ColumnDefinitions.Clear();
        //    imgQRCode.Visibility = Visibility.Visible;
        //    ColumnDefinition gridCol1 = new ColumnDefinition();
        //    gridCol1.Width = new GridLength(4, GridUnitType.Star);
        //    ColumnDefinition gridCol2 = new ColumnDefinition();
        //    gridCol2.Width = new GridLength(6, GridUnitType.Star);

        //    gdrPreview.ColumnDefinitions.Add(gridCol1);
        //    gdrPreview.ColumnDefinitions.Add(gridCol2);
      
        //    Grid.SetColumn(imgQRCode, 0);
        //    Grid.SetColumn(spDataOptions, 1);
        //}
        public void LayoutHalfRight()
        {
            gdrPreview.ColumnDefinitions.Clear();
            imgQRCode.Visibility = Visibility.Visible;
            ColumnDefinition gridCol1 = new ColumnDefinition();
            gridCol1.Width = new GridLength(6, GridUnitType.Star);
            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol2.Width = new GridLength(4, GridUnitType.Star);

            gdrPreview.ColumnDefinitions.Add(gridCol1);
            gdrPreview.ColumnDefinitions.Add(gridCol2);

            Grid.SetColumn(spDataOptions, 0);
            Grid.SetColumn(imgQRCode, 1);
        }
        public void LayoutClean()
        {

            gdrPreview.ColumnDefinitions.Clear();
            ColumnDefinition gridCol1 = new ColumnDefinition();
            gridCol1.Width = new GridLength(4, GridUnitType.Star);
            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol2.Width = new GridLength(6, GridUnitType.Star);

            gdrPreview.ColumnDefinitions.Add(gridCol1);
            gdrPreview.ColumnDefinitions.Add(gridCol2);

            imgQRCode.Visibility = Visibility.Collapsed;
            Grid.SetColumn(spDataOptions, 0);
            Grid.SetColumnSpan(spDataOptions, 2);
        }
    }

}
