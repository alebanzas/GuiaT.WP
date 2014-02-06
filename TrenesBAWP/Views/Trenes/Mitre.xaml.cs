using System;
using System.Linq;
using System.Windows;
using GuiaTBAWP;
using GuiaTBAWP.Commons;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TrenesBAWP.Services;

namespace TrenesBAWP.Views.Trenes
{
    public partial class Mitre : PhoneApplicationPage
    {
        public Mitre()
        {
            InitializeComponent();

            DataContext = ViewModel;
            Loaded += Page_Loaded;
            Unloaded += (sender, args) => DataService.CancelRequest();

            StatusChecker.Check("Trenes.Mitre");

            ViewModel.Ramales.Clear();
            DataService.EndRequest = EndRequest;
            DataService.StartRequest = StartRequest;
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
            if (!DataService.DatosLoaded)
                DataService.LoadData();
            FillViewModel();
        }

        private static void FillViewModel()
        {
            var query = TrenesRamalEstadoDC.Current.ByLinea("mitre");
            ViewModel.Ramales.Clear();
            foreach (var estadoTable in query.ToList())
            {
                ViewModel.AddRamal(estadoTable.ConvertToTrenRamalItemViewModel());
            }
            ViewModel.Actualizacion = string.Format("Actualizado hace {0}.",
                App.Configuration.UltimaActualizacionTrenes.ToUpdateDateTime());
        }

        #region Data

        private static readonly TrenStatusService DataService = new TrenStatusService();

        private int EndRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = true;

                ProgressBar.Hide();

                FillViewModel();
            });
            return 0;
        }

        private int StartRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = false;

                ProgressBar.Show("Actualizando estado del servicio...");
            });
            return 0;
        }

        private void ButtonGo_Click(object sender, EventArgs e)
        {
            DataService.LoadData();
        }
        #endregion


        private void VerEnMapa_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/Mapa.xaml?linea=mitre", UriKind.Relative));
        }

        private void Pin_Click(object sender, EventArgs e)
        {
            TileManager.Set(new Uri("/Views/Trenes/Mitre.xaml", UriKind.Relative), "Mitre", new Uri("/Images/Home/trenes.png", UriKind.Relative));
        }
    }
}