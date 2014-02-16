using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using GuiaTBA.WP7.Commons.Extensions;
using GuiaTBA.WP7.Commons.Services;
using GuiaTBA.WP7.Commons.ViewModels;
using GuiaTBA.WP7.Extensions;
using GuiaTBA.WP7.Models;
using Microsoft.Phone.Shell;
using NetworkInterface = System.Net.NetworkInformation.NetworkInterface;

namespace GuiaTBA.WP7.Views.Colectivos
{
    public partial class Cercanos
    {
        private static ColectivoCercanoViewModel _viewModel;
        public static ColectivoCercanoViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new ColectivoCercanoViewModel()); }
        }

        private HttpWebRequest _httpReq;

        public Cercanos()
        {
            InitializeComponent();
            
            Loaded += (s, e) =>
                {
                    DataContext = ViewModel;
                    
                    GetColectivosCercanos();
                };
            Unloaded += (s, e) => CancelarRequest();
        }
        
        void SetApplicationBarEnabled(bool isEnabled)
        {
            var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (applicationBarIconButton != null)
                applicationBarIconButton.IsEnabled = isEnabled;
        }

        void GetColectivosCercanos()
        {
            ResetUI();
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                ConnectionError.Visibility = Visibility.Visible;
                Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
                return;
            }

            GeoPosition<GeoCoordinate> currentLocation = PositionService.GetCurrentLocation();

            if (!App.Configuration.IsLocationEnabled)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar colectivos cercanos, por favor, active la función de localización en la configuración de la aplicación."));
                return;
            }
            if (currentLocation == null)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar colectivos cercanos, por favor, active la función de localización."));
                return;
            }
            
            ProgressBar.Show("Buscando más cercanos...");
            if (ViewModel.Items.Count == 0) Refreshing.Visibility = Visibility.Visible;
            SetApplicationBarEnabled(false);
            CancelarRequest();
            
            var client = new HttpClient();
            _httpReq = client.Get("/transporte/cercano".ToApiCallUri());
            _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
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

                Dispatcher.BeginInvoke(new DelegateUpdateList(UpdateList), o);
            }
            catch (Exception ex)
            {
                ex.Log(ResetUI, () => { ConnectionError.Visibility = Visibility.Visible; return 0; });
            }
        }

        delegate void DelegateUpdateList(List<TransporteModel> local);
        private void UpdateList(List<TransporteModel> l)
        {
            ViewModel.Items.Clear();
            foreach (IGrouping<string, TransporteModel> transporteModel in l.Where(x => x.TipoNickName == "colectivo").GroupBy(x => x.Linea))
            {
                ViewModel.AddLinea(new ColectivoItemViewModel { 
                    Id = transporteModel.Key,
                    Nombre = "Línea " + transporteModel.Key, 
                    Detalles = SetDetalleByLinea(transporteModel.Key, l),
                });
            }
            ResetUI();
            if (ViewModel.Items.Count == 0)
            {
                NoResults.Visibility = Visibility.Visible;
            }
        }

        private string SetDetalleByLinea(string key, IEnumerable<TransporteModel> transporteModels)
        {
            return string.Join("\n", transporteModels.Where(x => x.Linea.Equals(key)).OrderBy(x => x.Ramal).Select(x => x.Ramal));
        }

        private void CancelarRequest()
        {
            if (_httpReq != null)
                _httpReq.Abort();
        }

        private int ResetUI()
        {
            Refreshing.Visibility = Visibility.Collapsed;
            ConnectionError.Visibility = Visibility.Collapsed;
            NoResults.Visibility = Visibility.Collapsed;
            ProgressBar.Hide();
            SetApplicationBarEnabled(true);
            return 0;
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
    }
}