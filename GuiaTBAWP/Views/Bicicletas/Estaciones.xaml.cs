using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Views.Bicicletas
{
    public partial class Estaciones
    {
        private static EstacionesStatusViewModel _viewModel = new EstacionesStatusViewModel();
        public static EstacionesStatusViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new EstacionesStatusViewModel()); }
        }

        Pushpin _posicionActual;
        WebRequest _httpReq;

        public Estaciones()
        {
            InitializeComponent();
            DataContext = ViewModel;
            Loaded += Page_Loaded;
            Unloaded += Page_UnLoaded;
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Actualizacion = string.Format("Actualizado hace {0}.", App.Configuration.UltimaActualizacionBicicletas.ToUpdateDateTime());

            MiMapa.CredentialsProvider = new ApplicationIdCredentialsProvider(App.Configuration.BingMapApiKey);

            MostrarLugares();
            
            Cargar();
        }

        void Page_UnLoaded(object sender, RoutedEventArgs e)
        {
            CancelarRequest();
        }

        private void ActualizarUbicacion(GeoPosition<GeoCoordinate> location)
        {
            MiMapa.Children.Remove(_posicionActual);

            if (location == null) return;

            _posicionActual = new Pushpin
                {
                    Location = location.Location,
                    Template = (ControlTemplate) (Application.Current.Resources["locationPushpinTemplate"])
                };
            MiMapa.Children.Add(_posicionActual);
        }


        public void MostrarLugares()
        {
            MiMapa.Children.Clear();
            ViewModel.Estaciones.Clear();
            
            var list = BicicletaEstacionDC.GetAll().OrderBy(x => x.Nombre);
            
            var lugares = new ObservableCollection<BicicletaEstacionTable>(list);
            foreach (var lugar in lugares)
            {
                var pushpin = new Pushpin
                    {
                        Content = lugar.Nombre,
                        Location = new GeoCoordinate(lugar.Latitud, lugar.Longitud)
                    };
                MiMapa.Children.Add(pushpin);
                ViewModel.AddEstacion(lugar);
            }

            //Si uso localizacion, agrego mi ubicación
            ActualizarUbicacion(App.Configuration.IsLocationEnabled ? PositionService.GetCurrentLocation() : null);

            AjustarMapa();
        }

        private void AjustarMapa()
        {
            //Ajusto el mapa para mostrar los items
            var x = from l in MiMapa.Children
                    let pushpin = l as Pushpin
                    where pushpin != null
                    select pushpin.Location;
            MiMapa.SetView(LocationRect.CreateLocationRect(x));
        }

        private void CancelarRequest()
        {
            if (_httpReq != null)
                _httpReq.Abort();
        }

        public void Cargar(bool refresh = false)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                ProgressBar.Show(MiMapa.Children.Any() ? "Actualizando estado..." : "Obteniendo estado...");

                _httpReq = WebRequest.Create("/bicicletas".ToApiCallUri(true));
                _httpReq.Method = "GET";
                _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
            }
            else
            {
                ShowErrorConnection();
            }
        }

        private void ShowErrorConnection()
        {
            //Luego le aviso al usuario que no se pudo cargar nueva información.
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(BicicletasStatusModel));
                var o = (BicicletasStatusModel)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateWebBrowser(UpdateWebBrowser), o);
            }
            catch (Exception)
            {
                EndRequest();
            }
        }

        delegate void DelegateUpdateWebBrowser(BicicletasStatusModel local);
        private void UpdateWebBrowser(BicicletasStatusModel l)
        {
            App.Configuration.UltimaActualizacionBicicletas = l.Actualizacion;

            foreach (BicicletaEstacionTable ll in l.Estaciones.ConvertToBicicletaEstacionTable())
            {
                if (BicicletaEstacionDC.Current.Estaciones.Contains(ll))
                {
                    var estacion = BicicletaEstacionDC.Current.Estaciones.FirstOrDefault(x => x.Equals(ll));
                    if (estacion != null)
                    {
                        estacion.Cantidad = ll.Cantidad;
                        estacion.Horario = ll.Horario;
                        estacion.Estado = ll.Estado;
                    }
                }
                else
                {
                    BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(ll);
                }   
            }
            BicicletaEstacionDC.Current.SubmitChanges();

            App.Configuration.UltimaActualizacionBicicletas = l.Actualizacion;
            App.Configuration.InitialDataBicicletas = true;
            
            ViewModel.Actualizacion = string.Format("Actualizado hace {0}.", l.Actualizacion.ToUpdateDateTime());

            MostrarLugares();
            EndRequest();
        }

        private void EndRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = true;

                ProgressBar.Hide();
            });
        }

        private void LstLugares_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox == null || listBox.SelectedIndex == -1) return;

            var bicicletaEstacion = (BicicletaEstacionTable)listBox.SelectedItem;

            var uri = new Uri(String.Format("/Views/Bicicletas/LugarDetalles.xaml?id={0}", bicicletaEstacion.Id), UriKind.Relative);
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

        private void MiMapa_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Bicicletas/Mapa.xaml", UriKind.Relative));
        }
        
        private void ButtonGo_Click(object sender, EventArgs e)
        {
            Cargar(true);
        }
    }

    public class EstacionesStatusViewModel: INotifyPropertyChanged
    {
        public EstacionesStatusViewModel()
        {
            Estaciones = new ObservableCollection<BicicletaEstacionTable>();
        }

        private string _actualizacion;
        public string Actualizacion
        {
            get { return _actualizacion; }
            set
            {
                _actualizacion = value;
                NotifyPropertyChanged("Actualizacion");
            }
        }

        private ObservableCollection<BicicletaEstacionTable> _estaciones;
        public ObservableCollection<BicicletaEstacionTable> Estaciones
        {
            get { return _estaciones; }
            private set { 
                _estaciones = value;
                NotifyPropertyChanged("Estaciones");
            }
        }

        public void AddEstacion(BicicletaEstacionTable estacion)
        {
            Estaciones.Add(estacion);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}