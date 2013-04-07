using System.Linq;
using System.Windows;
using GuiaTBAWP.Models;
using GuiaTBAWP.ViewModels;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Trenes
{
    public partial class Roca : PhoneApplicationPage
    {
        public Roca()
        {
            InitializeComponent();
        
            DataContext = ViewModel;
            Loaded += Page_Loaded;
        }

        private static TrenLineaItemViewModel _viewModel = new TrenLineaItemViewModel();

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static TrenLineaItemViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                return _viewModel ?? (_viewModel = new TrenLineaItemViewModel());
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var query = TrenesRamalEstadoDC.Current.ByLinea("roca");
            ViewModel.Ramales.Clear();
            foreach (var estadoTable in query.ToList())
            {
                ViewModel.AddRamal(estadoTable.ConvertToTrenRamalItemViewModel());
            }
            ViewModel.Actualizacion = (App.Current as App).UltimaActualizacionTrenes;
        }
    }
}