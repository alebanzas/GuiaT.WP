using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using GuiaTBA.Common;
using GuiaTBA.WP7.Extensions;
using GuiaTBAWP.Commons.Extensions;
using GuiaTBAWP.Commons.Helpers;
using GuiaTBAWP.Commons.Services;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using Microsoft.Phone.Shell;

namespace GuiaTBA.WP7.Views.Aviones
{
    public partial class Arribos
    {
        WebRequest _httpReq;

        private bool _isSearching;

        private static AirportStatusViewModel _viewModel = new AirportStatusViewModel();
        public static AirportStatusViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new AirportStatusViewModel()); }
        }

        public Arribos()
        {
            InitializeComponent();
            
            DataContext = ViewModel;
            Loaded += (sender, args) => LoadData();
            Unloaded += (sender, args) =>
                {
                    if(_httpReq != null)
                        _httpReq.Abort();
                };

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var titulo = NavigationContext.QueryString["titulo"];
            var aeroestacion = NavigationContext.QueryString["aeroestacion"];
            var nickname = NavigationContext.QueryString["nickname"];
            var tipo = NavigationContext.QueryString["tipo"];

            ViewModel.Titulo = titulo;
            ViewModel.Aeroestacion = aeroestacion;
            ViewModel.NickName = nickname;
            ViewModel.Tipo = tipo;
            ViewModel.Vuelos.Clear();
            ViewModel.VuelosFiltrados.Clear();

            base.OnNavigatedTo(e);
        }

        public void LoadData()
        {
            ResetUI();
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                ConnectionError.Visibility = Visibility.Visible;
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
                return;
            }
            
            StartUI();

            var param = new Dictionary<string, object>
                {
                    { "t", ViewModel.NickName },
                };

            var client = new HttpClient();
            _httpReq = client.Get(string.Format("/api/avion/{0}", ViewModel.Tipo).ToApiCallUri(param, alwaysRefresh: true));
            _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
        }

        private void StartUI()
        {
            ProgressBar.Show(string.Format("Obteniendo {0}...", ViewModel.Titulo));
            ViewModel.Actualizacion = "Aguarde...";
            Refreshing.Visibility = Visibility.Visible;
            SetApplicationBarEnabled(false);
            ViewModel.Vuelos.Clear();
            ViewModel.VuelosFiltrados.Clear();
        }

        private int ResetUI()
        {
            NoResults.Visibility = Visibility.Collapsed;
            Refreshing.Visibility = Visibility.Collapsed;
            ConnectionError.Visibility = Visibility.Collapsed;
            ProgressBar.Hide();
            SetApplicationBarEnabled(true);
            return 0;
        }

        void SetApplicationBarEnabled(bool isEnabled)
        {
            var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (applicationBarIconButton != null)
                applicationBarIconButton.IsEnabled = isEnabled;

            applicationBarIconButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            if (applicationBarIconButton != null)
                applicationBarIconButton.IsEnabled = isEnabled;
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest) result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof (AvionesTerminalStatusModel));
                var o = (AvionesTerminalStatusModel) serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateEstado(UpdateEstadoServicio), o);
            }
            catch (Exception ex)
            {
                ex.Log(ResetUI, () => { ConnectionError.Visibility = Visibility.Visible; return 0; });
            }
        }

        delegate void DelegateUpdateEstado(AvionesTerminalStatusModel estado);
        private void UpdateEstadoServicio(AvionesTerminalStatusModel model)
        {
            BindViewModel(model);

            ResetUI();

            Config.Set(model);
        }

        private void BindViewModel(AvionesTerminalStatusModel model)
        {
            ViewModel.Vuelos.Clear();
            ViewModel.VuelosFiltrados.Clear();
            foreach (var itemViewModel in model.Arribos.Where(x => x.Hora > DateTime.UtcNow.AddHours(-5) || !"aterrizado".Equals(x.Estado.ToLowerInvariant())))
            {
                ViewModel.AddVuelo(new AirportStatusItemViewModel
                {
                    Estado = GetEstadoByStatusModel(itemViewModel),
                    Nombre = itemViewModel.Nombre,
                    Ciudad = itemViewModel.Origen.SanitizeHtml(),
                    Terminal = string.Format("terminal {0}", itemViewModel.Terminal),
                    Horario = string.Format("{0} {1}", itemViewModel.Hora.ToString("dd/MM"), itemViewModel.Hora.ToString("HH:mm")),
                    Aerolinea = itemViewModel.Linea,
                });
            }
            foreach (var itemViewModel in model.Partidas.Where(x => x.Hora > DateTime.UtcNow.AddHours(-5) || !"despegado".Equals(x.Estado.ToLowerInvariant())))
            {
                ViewModel.AddVuelo(new AirportStatusItemViewModel
                {
                    Estado = GetEstadoByStatusModel(itemViewModel),
                    Nombre = itemViewModel.Nombre,
                    Ciudad = itemViewModel.Destino.SanitizeHtml(),
                    Terminal = string.Format("terminal {0}", itemViewModel.Terminal),
                    Horario = string.Format("{0} {1}", itemViewModel.Hora.ToString("dd/MM"), itemViewModel.Hora.ToString("HH:mm")),
                    Aerolinea = itemViewModel.Linea,
                });
            }

            ViewModel.Actualizacion = string.Format("Actualizado hace {0}.", model.Actualizacion.ToUpdateDateTime());
        }

        private string GetEstadoByStatusModel(VueloArriboModel itemViewModel)
        {
            if (string.IsNullOrWhiteSpace(itemViewModel.Estado))
            {
                return "no informa";
            }
            if (itemViewModel.Estado.ToLowerInvariant().Contains("estima") && itemViewModel.Estima.HasValue)
            {
                return string.Format("estima ({0})", itemViewModel.Estima.Value.ToString("HH:mm"));
            }

            return itemViewModel.Estado.SanitizeHtml().ToLowerInvariant();
        }

        private string GetEstadoByStatusModel(VueloPartidaModel itemViewModel)
        {
            if (string.IsNullOrWhiteSpace(itemViewModel.Estado))
            {
                return "no informa";
            }
            if (itemViewModel.Estado.ToLowerInvariant().Contains("estima") && itemViewModel.Estima.HasValue)
            {
                return string.Format("estima ({0})", itemViewModel.Estima.Value.ToString("HH:mm"));
            }

            return itemViewModel.Estado.SanitizeHtml().ToLowerInvariant();
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void AcBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            AcBox.Text = string.Empty;
        }

        private void AcBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcesarBusqueda(AcBox.Text);
            }
        }

        private void ButtonBuscar_OnClick(object sender, RoutedEventArgs e)
        {
            ProcesarBusqueda(AcBox.Text);
        }

        private void ProcesarBusqueda(string pattern)
        {
            ViewModel.FiltrarVuelos(pattern);
            NoResults.Visibility = !ViewModel.VuelosFiltrados.Any() ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            SearchPanel.Margin = new Thickness(0, -79, 0, 0);
            AcBox.Focus();
            _isSearching = true;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (_isSearching)
            {
                SearchPanel.Margin = new Thickness(0, -280, 0, 0);
                _isSearching = false;
                ProcesarBusqueda(null);
                e.Cancel = true;
            }
        }
    }
}