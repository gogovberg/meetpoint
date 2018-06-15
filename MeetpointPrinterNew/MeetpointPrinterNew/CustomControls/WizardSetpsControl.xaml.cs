using MeetpointPrinterNew.Pages;
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
    /// Interaction logic for WizardSetpsControl.xaml
    /// </summary>
    public partial class WizardSetpsControl : UserControl
    {
        private  BrushConverter _converter = new BrushConverter();
        private Brush _previousBrush;

        public WizardSetpsControl()
        {
            InitializeComponent();
        }

        private void btnSelectPrinter_Click(object sender, RoutedEventArgs e)
        {
            if(GlobalSettings.CurrentPageID!=2)
            {
                Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
                GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;
                Application.Current.MainWindow.Content = new SetupPage(GlobalSettings.ApplicationSettings, 0);
            }
            

        }

        private void btnSelectAccounts_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalSettings.CurrentPageID != 3 && GlobalSettings.IsPrinterSet)
            {
                Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
                GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;
                Application.Current.MainWindow.Content = new SetupPage(GlobalSettings.ApplicationSettings, 1);
            }
        }

        private void btnSelectTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalSettings.CurrentPageID != 4 && GlobalSettings.IsPrinterSet && GlobalSettings.IsAccountSet)
            {
                Helpers.SaveUserSettings(GlobalSettings.ApplicationSettings);
                GlobalSettings.PreviousPageID = GlobalSettings.CurrentPageID;
                Application.Current.MainWindow.Content = new SetupPagePrintTemplate(GlobalSettings.ApplicationSettings);
            }
        }

        public void SetWizardStepState(WizardStep step, WizardStepState state)
        {
           

            switch (step)
            {
                case WizardStep.Printer:
                    BitmapImage imgPrinterSource = new BitmapImage(new Uri("/Images/icon_printer.png", UriKind.Relative));
                    BitmapImage imgPrinterSourcePrimary = new BitmapImage(new Uri("/Images/icon_printer_primary.png", UriKind.Relative));
                    linePrintAccount.Stroke = (Brush)_converter.ConvertFromString("#D3D4D6");
                    lineAccountTemplate.Stroke = (Brush)_converter.ConvertFromString("#D3D4D6");
                   

                    switch (state)
                    {
                        case WizardStepState.CurrentEmpty:
                            btnSelectPrinter.BorderBrush = (Brush)_converter.ConvertFromString("#1FE6A5");
                            btnSelectPrinter.Background = (Brush)_converter.ConvertFromString("#D3D4D6");
                            imgSelectPrinter.Source = imgPrinterSource;
                       
                            break;
                        case WizardStepState.Empty:
                            btnSelectPrinter.BorderBrush = (Brush)_converter.ConvertFromString("#D3D4D6");
                            btnSelectPrinter.Background = (Brush)_converter.ConvertFromString("#D3D4D6");
                            imgSelectPrinter.Source = imgPrinterSource;
                            break;
                        case WizardStepState.CurrentFilled:
                            btnSelectPrinter.BorderBrush = (Brush)_converter.ConvertFromString("#1FE6A5");
                            btnSelectPrinter.Background = (Brush)_converter.ConvertFromString("#C6DBF3");
                            btnSelectTemplate.BorderThickness = new Thickness(2);
                            imgSelectPrinter.Source = imgPrinterSourcePrimary;
                            break;
                        case WizardStepState.Filled:
                            btnSelectPrinter.BorderBrush = (Brush)_converter.ConvertFromString("#C6DBF3");
                            btnSelectPrinter.Background = (Brush)_converter.ConvertFromString("#C6DBF3");
                            imgSelectPrinter.Source = imgPrinterSourcePrimary;
                            break;
                    }
                    break;
                case WizardStep.Account:
                    BitmapImage imgAccountSource = new BitmapImage(new Uri("/Images/icon_users.png", UriKind.Relative));
                    BitmapImage imgAccounSourcePrimary = new BitmapImage(new Uri("/Images/icon_users_primary.png", UriKind.Relative));
                    linePrintAccount.Stroke = (Brush)_converter.ConvertFromString("#C6DBF3");
                    lineAccountTemplate.Stroke = (Brush)_converter.ConvertFromString("#D3D4D6");

                    switch (state)
                    {
                        case WizardStepState.CurrentEmpty:
                            btnSelectAccounts.BorderBrush = (Brush)_converter.ConvertFromString("#1FE6A5");
                            btnSelectAccounts.Background = (Brush)_converter.ConvertFromString("#D3D4D6");
                            imgSelectAccounts.Source = imgAccountSource;
                            break;
                        case WizardStepState.Empty:
                            btnSelectAccounts.BorderBrush = (Brush)_converter.ConvertFromString("#D3D4D6");
                            btnSelectAccounts.Background = (Brush)_converter.ConvertFromString("#D3D4D6");
                            imgSelectAccounts.Source = imgAccountSource;
                            break;
                        case WizardStepState.CurrentFilled:
                            btnSelectAccounts.BorderBrush = (Brush)_converter.ConvertFromString("#1FE6A5");
                            btnSelectAccounts.Background = (Brush)_converter.ConvertFromString("#C6DBF3");
                            btnSelectTemplate.BorderThickness = new Thickness(2);
                            imgSelectAccounts.Source = imgAccounSourcePrimary;
                            break;
                        case WizardStepState.Filled:
                            btnSelectAccounts.BorderBrush = (Brush)_converter.ConvertFromString("#C6DBF3");
                            btnSelectAccounts.Background = (Brush)_converter.ConvertFromString("#C6DBF3");
                            imgSelectAccounts.Source = imgAccounSourcePrimary;
                            break;
                    }
                    break;
                case WizardStep.Template:
                    BitmapImage imgTemplateSource = new BitmapImage(new Uri("/Images/icon_qr_code.png", UriKind.Relative));
                    BitmapImage imgTemplateSourcePrimary = new BitmapImage(new Uri("/Images/icon_qr_code_big.png", UriKind.Relative));
                    linePrintAccount.Stroke = (Brush)_converter.ConvertFromString("#C6DBF3");
                    lineAccountTemplate.Stroke = (Brush)_converter.ConvertFromString("#C6DBF3");

                    switch (state)
                    {
                        case WizardStepState.CurrentEmpty:
                            btnSelectTemplate.BorderBrush = (Brush)_converter.ConvertFromString("#1FE6A5");
                            btnSelectTemplate.Background = (Brush)_converter.ConvertFromString("#D3D4D6");
                            imgSelectTemplate.Source = imgTemplateSource;
                            break;
                        case WizardStepState.Empty:
                            btnSelectTemplate.BorderBrush = (Brush)_converter.ConvertFromString("#D3D4D6");
                            btnSelectTemplate.Background = (Brush)_converter.ConvertFromString("#D3D4D6");
                            imgSelectTemplate.Source = imgTemplateSource;
                            break;
                        case WizardStepState.CurrentFilled:
                            btnSelectTemplate.BorderBrush = (Brush)_converter.ConvertFromString("#1FE6A5");
                            btnSelectTemplate.Background = (Brush)_converter.ConvertFromString("#C6DBF3");
                            btnSelectTemplate.BorderThickness = new Thickness(2);
                            imgSelectTemplate.Source = imgTemplateSourcePrimary;
                            break;
                        case WizardStepState.Filled:
                            btnSelectTemplate.BorderBrush = (Brush)_converter.ConvertFromString("#C6DBF3");
                            btnSelectTemplate.Background = (Brush)_converter.ConvertFromString("#C6DBF3");
                            imgSelectTemplate.Source = imgTemplateSourcePrimary;
                            break;
                    }
                    break;
            }
        }

        private void btnSelect_MouseEnter(object sender, RoutedEventArgs e)
        {

            Button temp = (Button)sender;
            _previousBrush = temp.BorderBrush;
             temp.BorderBrush = (Brush)_converter.ConvertFromString("#1FE6A5");
        }
        private void btnSelect_MouseLeave(object sender, RoutedEventArgs e)
        {

            Button temp = (Button)sender;
            temp.BorderBrush = _previousBrush;
        }
    }

}
