using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using GuiaTBAWP.Bing.Geocode;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using GuiaTBAWP.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NetworkInterface = System.Net.NetworkInformation.NetworkInterface;

namespace GuiaTBAWP.Views.Colectivos
{
    public partial class Cercanos : PhoneApplicationPage
    {
        private static ColectivoCercanoViewModel _viewModel;
        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static ColectivoCercanoViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                return _viewModel ?? (_viewModel = new ColectivoCercanoViewModel());
            }
        }

        private static bool _datosLoaded = false;

        readonly ProgressIndicator _progress = new ProgressIndicator();
        private HttpWebRequest _httpReq;

        public Cercanos()
        {
            InitializeComponent();
            
            Loaded += (s, e) =>
                {
                    ResetUI();
                    
                    DataContext = ViewModel;

                    _progress.IsVisible = true;
                    _progress.IsIndeterminate = true;

                    GetColectivosCercanos();
                    //SetProgressBar("Buscando posición...");
                    var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                    if (applicationBarIconButton != null)
                        applicationBarIconButton.IsEnabled = false;
                };
            Unloaded += (s, e) =>
            {
                CancelarRequest();
            };
        }
        
        private void SetProgressBar(string msj, bool showProgress = true)
        {
            if (string.IsNullOrEmpty(msj))
            {
                SystemTray.SetProgressIndicator(this, null);
            }
            else
            {
                _progress.Text = msj;
                _progress.IsIndeterminate = showProgress;
                SystemTray.SetIsVisible(this, true);
                SystemTray.SetProgressIndicator(this, _progress);
            }
        }

        void GetColectivosCercanos()
        {
            var e = App.Ubicacion;

            var location = new GeocodeLocation
            {
                Latitude = e.Position.Location.Latitude,
                Longitude = e.Position.Location.Longitude
            };

            if (_datosLoaded || _pendingRequests > 0)
            {
                if (Math.Abs(location.Latitude - ViewModel.CurrentLocation.Latitude) < App.Configuration.MinDiffGeography &&
                    Math.Abs(location.Longitude - ViewModel.CurrentLocation.Longitude) < App.Configuration.MinDiffGeography)
                {
                    ResetUI();
                    return;
                }
            }

            ViewModel.CurrentLocation = e.Position.Location;
            
            SetProgressBar(null);

            GetMasCercanos(location);
        }

        private int _pendingRequests;
        private void GetMasCercanos(GeocodeLocation location)
        {
            SetProgressBar("Buscando más cercano...");
            CancelarRequest();
            
            var param = new Dictionary<string, object>
                {
                    {"lat", location.Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")},
                    {"lon", location.Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")},
                };

            _httpReq = (HttpWebRequest)WebRequest.Create("/transporte/cercano".ToApiCallUri(param));
            _httpReq.Method = "GET";
            _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
            _pendingRequests++;
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(List<TransporteModel>));
                var o = (List<TransporteModel>)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateWebBrowser(UpdateWebBrowser), o);
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        Loading.Visibility = Visibility.Collapsed;
                        ConnectionError.Visibility = Visibility.Visible;
                        FinishRequest();
                    });
                //this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error... " + ex.Message));
            }
        }

        delegate void DelegateUpdateWebBrowser(List<TransporteModel> local);
        private void UpdateWebBrowser(List<TransporteModel> l)
        {
            ViewModel.Items.Clear();
            foreach (IGrouping<string, TransporteModel> transporteModel in l.Where(x => x.TipoNickName == "colectivo").GroupBy(x => x.Linea))
            {
                ViewModel.AddLinea(new ColectivoItemViewModel { 
                    Id = transporteModel.Key,
                    Nombre = "Línea " + transporteModel.Key, 
                    Detalles = SetDetalleByLinea(transporteModel.Key, l),
                    //Detalles = DataColectivos.ByCode(transporteModel.Key), 
                });
            }
            _datosLoaded = true;
            Loading.Visibility = Visibility.Collapsed;
            ConnectionError.Visibility = Visibility.Collapsed;
            FinishRequest();
        }

        private string SetDetalleByLinea(string key, IEnumerable<TransporteModel> transporteModels)
        {
            return string.Join("\n", transporteModels.Where(x => x.Linea.Equals(key)).OrderBy(x => x.Ramal).Select(x => x.Ramal));
        }

        private void FinishRequest()
        {
            _pendingRequests--;
            if (_pendingRequests != 0) return;

            ResetUI();
        }

        private void ResetUI()
        {
            if (_pendingRequests != 0) return;
            SetProgressBar(null);
            var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (applicationBarIconButton != null)
                applicationBarIconButton.IsEnabled = true;
        }
        
        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void Acerca_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Acerca.xaml", UriKind.Relative));
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            GetColectivosCercanos();
        }

        private void ListaColectivos_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox == null || listBox.SelectedIndex == -1) return;

            CancelarRequest();

            var colectivoItem = (ColectivoItemViewModel)listBox.SelectedItem;

            var uri = new Uri(String.Format("/Views/Colectivos/Detalle.xaml?id={0}", colectivoItem.Id), UriKind.Relative);
            NavigationService.Navigate(uri);

            //Vuelvo el indice del item seleccionado a -1 para que pueda hacer tap en el mismo item y navegarlo
            listBox.SelectedIndex = -1;
        }

        private void CancelarRequest()
        {
            if (_httpReq != null)
                _httpReq.Abort();
        }
    }
}