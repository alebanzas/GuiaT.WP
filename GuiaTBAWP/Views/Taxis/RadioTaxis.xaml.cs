using System.Windows;
using System.Windows.Controls;
using GuiaTBAWP.Commons.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

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

            DataContext = ViewModel;
            Loaded += RadioTaxis_Loaded;

            StatusChecker.Check("Taxi.RadioTaxis");
        }

        private void RadioTaxis_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "Premium", Telefono = "5238-0000", Url = "http://www.taxipremium.com/" });
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "Premier", Telefono = "4858-0888", Url = "http://www.premieradiotaxi.com.ar/" });
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "Por Buenos Aires", Telefono = "4566-7777", Url = "http://www.bairesradiotaxi.com.ar/" });
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "Pídalo", Telefono = "4956-1200", Url = "http://www.radiotaxipidalo.com.ar/" });
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "Mi Radio Taxi", Telefono = "4931-1200", Url = "http://www.mitaxionline.com/" });
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "Tiempo", Telefono = "4854-3838", Url = "http://www.radiotaxitiemposrl.com.ar/" });
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "City Tax", Telefono = "4585-5544", Url = "http://www.citytax.com.ar/" });
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "Porteño", Telefono = "4566-5777", Url = "http://www.radiotaxiportenio.com.ar/" });
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "Buen Viaje", Telefono = "5252-9999", Url = "http://www.radiotaxibuenviaje.com/" });
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "Aló", Telefono = "4855-5555", Url = "http://www.radiotaxisalo.com.ar/" });
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "Del Plata", Telefono = "4504-7776", Url = "http://www.delplataradiotaxi.com/" });
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "24 Horas", Telefono = "4523-2222", Url = "http://www.radiotaxi24.com.ar/" });
            ViewModel.Lista.Add(new RadioTaxisItemViewModel { Nombre = "Gold", Telefono = "4305-5050", Url = "http://www.radiotaxigoldsrl.com.ar/" });
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lb = (ListBox) sender;
            if (lb.SelectedItems != null)
            {
                var item = (RadioTaxisItemViewModel) lb.SelectedItems[0];

                var task = new PhoneCallTask();
                task.DisplayName = item.Nombre;
                task.PhoneNumber = item.Telefono;
                task.Show();
            }
        }
    }
}