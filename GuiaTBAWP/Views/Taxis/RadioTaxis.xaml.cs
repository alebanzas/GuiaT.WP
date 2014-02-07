using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GuiaTBAWP.Commons.Models;
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
            ViewModel.Lista.Clear();
            foreach (var radioTaxiTable in RadioTaxiDC.Current.Lista.OrderBy(x => x.Nombre))
            {
                ViewModel.Lista.Add(new RadioTaxisItemViewModel
                {
                    Id = radioTaxiTable.Id,
                    Nombre = radioTaxiTable.Nombre,
                    Telefono = radioTaxiTable.Telefono,
                    Url = radioTaxiTable.Url,
                    Detalles = radioTaxiTable.Detalles,
                });
            }

        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox == null || listBox.SelectedIndex == -1) return;

            var bicicletaEstacion = (RadioTaxisItemViewModel)listBox.SelectedItem;

            var uri = new Uri(string.Format("/Views/Taxis/RadioTaxiDetalles.xaml?id={0}", bicicletaEstacion.Id), UriKind.Relative);
            NavigationService.Navigate(uri);

            //Vuelvo el indice del item seleccionado a -1 para que pueda hacer tap en el mismo item y navegarlo
            listBox.SelectedIndex = -1;
        }
    }
}