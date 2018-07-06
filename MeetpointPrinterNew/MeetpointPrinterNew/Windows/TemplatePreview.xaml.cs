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

namespace MeetpointPrinterNew.Windows
{
    /// <summary>
    /// Interaction logic for TemplatePriview.xaml
    /// </summary>
    public partial class TemplatePreview : Window
    {
        public TemplatePreview()
        {
            InitializeComponent();
          
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void PrivewTemplateLogic(PrintQueueItem item)
        {

            printTemplate.tbOptOne.Text = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[0], item);
            printTemplate.tbOptTwo.Text = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[1], item);
            printTemplate.tbOptThree.Text = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[2], item);
            printTemplate.tbOptFour.Text = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[3], item);
            printTemplate.tbOptFive.Text = Helpers.GetDataOptionsFiled(GlobalSettings.ApplicationSettings.PrinterSetup.DataOptions.DataOption[4], item);

            Helpers.SetPrintTemplateSize(printTemplate, PrintTemplateSize.PrintLogBig);
            if (GlobalSettings.ApplicationSettings.PrinterSetup.LayoutSizeID.Equals("cbSizeTwo"))
            {
                Helpers.SetPrintTemplateSize(printTemplate, PrintTemplateSize.PrintLogSmall);
            }
         

            switch (GlobalSettings.ApplicationSettings.PrinterSetup.LayoutTemplate)
            {
             
                case "cbLayoutHR":
                    printTemplate.LayoutHalfRight();
                    break;  
                case "cbLayoutClean":
                    printTemplate.LayoutClean();
                    break;
            }
        }

       
    }
}
