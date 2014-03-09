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
                TEV_0_1.Text = estaciones[0].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[0].Ida1);
                TEV_0_2.Text = estaciones[0].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[0].Ida2);


                TEV_1_1.Text = estaciones[1].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[1].Ida1);
                TEV_1_2.Text = estaciones[1].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[1].Ida2);
                TEV_1_3.Text = estaciones[1].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[1].Vuelta1);
                TEV_1_4.Text = estaciones[1].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[1].Vuelta2);

                TEV_2_1.Text = estaciones[2].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[2].Ida1);
                TEV_2_2.Text = estaciones[2].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[2].Ida2);
                TEV_2_3.Text = estaciones[2].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[2].Vuelta1);
                TEV_2_4.Text = estaciones[2].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[2].Vuelta2);

                TEV_3_1.Text = estaciones[3].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[3].Ida1);
                TEV_3_2.Text = estaciones[3].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[3].Ida2);
                TEV_3_3.Text = estaciones[3].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[3].Vuelta1);
                TEV_3_4.Text = estaciones[3].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[3].Vuelta2);

                TEV_4_1.Text = estaciones[4].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[4].Ida1);
                TEV_4_2.Text = estaciones[4].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[4].Ida2);
                TEV_4_3.Text = estaciones[4].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[4].Vuelta1);
                TEV_4_4.Text = estaciones[4].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[4].Vuelta2);

                TEV_5_1.Text = estaciones[5].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[5].Ida1);
                TEV_5_2.Text = estaciones[5].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[5].Ida2);
                TEV_5_3.Text = estaciones[5].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[5].Vuelta1);
                TEV_5_4.Text = estaciones[5].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[5].Vuelta2);

                TEV_6_1.Text = estaciones[6].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[6].Ida1);
                TEV_6_2.Text = estaciones[6].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[6].Ida2);
                TEV_6_3.Text = estaciones[6].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[6].Vuelta1);
                TEV_6_4.Text = estaciones[6].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[6].Vuelta2);

                TEV_7_1.Text = estaciones[7].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[7].Ida1);
                TEV_7_2.Text = estaciones[7].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[7].Ida2);
                TEV_7_3.Text = estaciones[7].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[7].Vuelta1);
                TEV_7_4.Text = estaciones[7].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[7].Vuelta2);

                TEV_8_1.Text = estaciones[8].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[8].Ida1);
                TEV_8_2.Text = estaciones[8].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[8].Ida2);
                TEV_8_3.Text = estaciones[8].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[8].Vuelta1);
                TEV_8_4.Text = estaciones[8].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[8].Vuelta2);

                TEV_9_1.Text = estaciones[9].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[9].Ida1);
                TEV_9_2.Text = estaciones[9].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[9].Ida2);
                TEV_9_3.Text = estaciones[9].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[9].Vuelta1);
                TEV_9_4.Text = estaciones[9].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[9].Vuelta2);

                TEV_10_1.Text = estaciones[10].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[10].Ida1);
                TEV_10_2.Text = estaciones[10].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[10].Ida2);
                TEV_10_3.Text = estaciones[10].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[10].Vuelta1);
                TEV_10_4.Text = estaciones[10].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[10].Vuelta2);

                TEV_11_1.Text = estaciones[11].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[11].Ida1);
                TEV_11_2.Text = estaciones[11].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[11].Ida2);
                TEV_11_3.Text = estaciones[11].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[11].Vuelta1);
                TEV_11_4.Text = estaciones[11].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[11].Vuelta2);

                TEV_12_1.Text = estaciones[12].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[12].Ida1);
                TEV_12_2.Text = estaciones[12].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[12].Ida2);
                TEV_12_3.Text = estaciones[12].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[12].Vuelta1);
                TEV_12_4.Text = estaciones[12].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[12].Vuelta2);

                TEV_13_1.Text = estaciones[13].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[13].Ida1);
                TEV_13_2.Text = estaciones[13].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[13].Ida2);
                TEV_13_3.Text = estaciones[13].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[13].Vuelta1);
                TEV_13_4.Text = estaciones[13].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[13].Vuelta2);

                TEV_14_1.Text = estaciones[14].Ida1 == 0 ? "andén" : string.Format("{0} min", estaciones[14].Ida1);
                TEV_14_2.Text = estaciones[14].Ida2 == 0 ? "andén" : string.Format("{0} min", estaciones[14].Ida2);
                TEV_14_3.Text = estaciones[14].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[14].Vuelta1);
                TEV_14_4.Text = estaciones[14].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[14].Vuelta2);

                TEV_15_3.Text = estaciones[15].Vuelta1 == 0 ? "andén" : string.Format("{0} min", estaciones[15].Vuelta1);
                TEV_15_4.Text = estaciones[15].Vuelta2 == 0 ? "andén" : string.Format("{0} min", estaciones[15].Vuelta2);
            });
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