using System.Linq;
using System.Windows;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls;
using TrenesBAWP;

namespace GuiaTBAWP.Views.Trenes
{
    public partial class BelgranoSur : PhoneApplicationPage
    {
        public BelgranoSur()
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
            var query = TrenesRamalEstadoDC.Current.ByLinea("belgrano sur");
            ViewModel.Ramales.Clear();
            foreach (var estadoTable in query.ToList())
            {
                ViewModel.AddRamal(estadoTable.ConvertToTrenRamalItemViewModel());
            }
            ViewModel.Actualizacion = string.Format("Actualizado hace {0}.", App.Configuration.UltimaActualizacionTrenes.ToUpdateDateTime());
        }
    }
}