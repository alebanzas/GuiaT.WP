using System;
using System.Linq;
using System.Windows;
using GuiaTBA.Common;
using GuiaTBAWP.Commons;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using GuiaTBAWP.Services;
using Microsoft.Phone.Shell;
using GuiaTBA.Common.Models;

namespace GuiaTBAWP.Views.Trenes
{
    public partial class Sarmiento
    {
        public Sarmiento()
        {
            InitializeComponent();
        
            DataContext = ViewModel;
            Loaded += Page_Loaded;
            Unloaded += (sender, args) =>
            {
                DataService.CancelRequest();
                _dataRealTimeService.CancelRequest();
            };

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

        private static TrenRealTimeStatusService _dataRealTimeService;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DataService.DatosLoaded)
                DataService.LoadData();
            FillViewModel();
            _dataRealTimeService = new TrenRealTimeStatusService(20, "sarmiento")
            {
                EndRequest = model =>
                {
                    FillRealTimeModel(model);
                    return model;
                }
            };
        }

        private void FillRealTimeModel(LiveTrenModel model)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                //TEV_Actualizacion.Text = model.Actualizacion.ToUpdateDateTime();
                var estaciones = model.Estaciones.ToArray();

                TEV_0_1.Text = GetTimeStringIda(estaciones[0]);

                TEV_1_1.Text = GetTimeStringVuelta(estaciones[1]);
                TEV_1_2.Text = GetTimeStringIda(estaciones[1]);

                TEV_2_1.Text = GetTimeStringVuelta(estaciones[2]);
                TEV_2_2.Text = GetTimeStringIda(estaciones[2]);

                TEV_3_1.Text = GetTimeStringVuelta(estaciones[3]);
                TEV_3_2.Text = GetTimeStringIda(estaciones[3]);

                TEV_4_1.Text = GetTimeStringVuelta(estaciones[4]);
                TEV_4_2.Text = GetTimeStringIda(estaciones[4]);

                TEV_5_1.Text = GetTimeStringVuelta(estaciones[5]);
                TEV_5_2.Text = GetTimeStringIda(estaciones[5]);

                TEV_6_1.Text = GetTimeStringVuelta(estaciones[6]);
                TEV_6_2.Text = GetTimeStringIda(estaciones[6]);

                TEV_7_1.Text = GetTimeStringVuelta(estaciones[7]);
                TEV_7_2.Text = GetTimeStringIda(estaciones[7]);

                TEV_8_1.Text = GetTimeStringVuelta(estaciones[8]);
                TEV_8_2.Text = GetTimeStringIda(estaciones[8]);

                TEV_9_1.Text = GetTimeStringVuelta(estaciones[9]);
                TEV_9_2.Text = GetTimeStringIda(estaciones[9]);

                TEV_10_1.Text = GetTimeStringVuelta(estaciones[10]);
                TEV_10_2.Text = GetTimeStringIda(estaciones[10]);

                TEV_11_1.Text = GetTimeStringVuelta(estaciones[11]);
                TEV_11_2.Text = GetTimeStringIda(estaciones[11]);

                TEV_12_1.Text = GetTimeStringVuelta(estaciones[12]);
                TEV_12_2.Text = GetTimeStringIda(estaciones[12]);

                TEV_13_1.Text = GetTimeStringVuelta(estaciones[13]);
                TEV_13_2.Text = GetTimeStringIda(estaciones[13]);

                TEV_14_1.Text = GetTimeStringVuelta(estaciones[14]);
                TEV_14_2.Text = GetTimeStringIda(estaciones[14]);

                TEV_15_1.Text = GetTimeStringVuelta(estaciones[15]);
            });
        }

        private static string GetTimeStringIda(LiveTrenItemModel estaciones)
        {
            if (estaciones.Ida1 == -1 && estaciones.Ida2 == -1)
            {
                return "no hay servicios";
            }

            var i = "prox. a Moreno en ";
            i += (estaciones.Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones.Ida1));
            if (estaciones.Ida2 == -1) return i;
            if (estaciones.Ida2 != 0)
                i += string.Format(" y en {0} min", estaciones.Ida2);
            return i;
        }

        private static string GetTimeStringVuelta(LiveTrenItemModel estaciones)
        {
            if (estaciones.Vuelta1 == -1 && estaciones.Vuelta2 == -1)
            {
                return "no hay servicios";
            }

            var i = "prox. a Once en ";
            i += (estaciones.Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones.Vuelta1));
            if (estaciones.Vuelta2 == -1) return i;
            if (estaciones.Vuelta2 != 0)
                i += string.Format(" y en {0} min", estaciones.Vuelta2);
            return i;
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
            TrenMapModel.SetMapModel("sarmiento");

            NavigationService.Navigate(new Uri("/Views/Mapa.xaml", UriKind.Relative));
        }

        private void Pin_Click(object sender, EventArgs e)
        {
            TileManager.Set(new Uri("/Views/Trenes/Sarmiento.xaml", UriKind.Relative), "Sarmiento", new Uri("/Images/Home/trenes.png", UriKind.Relative));
        }
    }
}