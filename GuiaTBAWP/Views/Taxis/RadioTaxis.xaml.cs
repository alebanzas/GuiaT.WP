using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Taxis
{
    public partial class RadioTaxis : PhoneApplicationPage
    {
        private static RadioTaxisViewModel _viewModel = new RadioTaxisViewModel();
        public static RadioTaxisViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new RadioTaxisViewModel()); }
        }

        public RadioTaxis()
        {
            InitializeComponent();

            Loaded += RadioTaxis_Loaded;

            StatusChecker.Check("Taxi.RadioTaxis");
        }

        private void RadioTaxis_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }

    public class RadioTaxisViewModel
    {
        public string Nombre { get; set; }

        public string Telefono { get; set; }
    }
}