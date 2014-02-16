using System;
using System.Linq;
using System.Windows;
using GuiaTBA.Common;
using GuiaTBAWP;
using GuiaTBAWP.Commons;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using Microsoft.Phone.Shell;
using TrenesBAWP.Services;

namespace TrenesBAWP.Views.Trenes
{
    public partial class Sarmiento
    {
        public Sarmiento()
        {
            InitializeComponent();
        
            DataContext = ViewModel;
            Loaded += Page_Loaded;
            Unloaded += (sender, args) => DataService.CancelRequest();

            StatusChecker.Check("Trenes.Sarmiento");

            ViewModel.Ramales.Clear();
            DataService.EndRequest = EndRequest;
            DataService.StartRequest = StartRequest;
        }

        private static TrenLineaItemViewModel _viewModel = new TrenLineaItemViewModel();
        public static TrenLineaItemViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new TrenLineaItemViewModel()); }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DataService.DatosLoaded)
                DataService.LoadData();
            FillViewModel();
        }

        private static void FillViewModel()
        {
            var query = TrenesRamalEstadoDC.Current.ByLineas(new[] {"sarmiento"});
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
            NavigationService.Navigate(new Uri("/Views/Trenes/Mapa.xaml?linea=sarmiento", UriKind.Relative));
        }

        private void Pin_Click(object sender, EventArgs e)
        {
            TileManager.Set(new Uri("/Views/Trenes/Sarmiento.xaml", UriKind.Relative), "Sarmiento", new Uri("/Images/Home/trenes.png", UriKind.Relative));
        }
    }
}