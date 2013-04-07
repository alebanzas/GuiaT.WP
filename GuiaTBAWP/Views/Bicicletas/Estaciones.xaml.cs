using System;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Views.Bicicletas
{
    public partial class Estaciones : PhoneApplicationPage
    {
        Pushpin _posicionActual;
        WebRequest _httpReq;
        readonly ProgressIndicator _progress = new ProgressIndicator();

        public Estaciones()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
            Unloaded += Page_UnLoaded;
        }
        
        void Page_UnLoaded(object sender, RoutedEventArgs e)
        {
            if (_httpReq != null)
                _httpReq.Abort();
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _progress.IsVisible = true;
            _progress.IsIndeterminate = true;

            UpdatedAt.Text = ToLocalDateTime((App.Current as App).UltimaActualizacionBicicletas);

            MostrarLugares();
            
            EndRequest();

            if (!_datosLoaded)
                Cargar();
        }

        void Ubicacion_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            ActualizarUbicacion(e.Position.Location);
        }

        private void ActualizarUbicacion(GeoCoordinate location)
        {
            MiMapa.Children.Remove(_posicionActual);

            if (location == null || location.IsUnknown) return;

            _posicionActual = new Pushpin
                {
                    Location = location,
                    Template = (ControlTemplate) (App.Current.Resources["locationPushpinTemplate"])
                };
            MiMapa.Children.Add(_posicionActual);
        }


        public void MostrarLugares()
        {
            //Limpio el mapa, tomo lugares de la tabla local y los agrego al mapa
            MiMapa.Children.Clear();

            var query = from l in BicicletaEstacionDC.Current.Estaciones
                        orderby l.Id
                        select l;

            LstLugares.ItemsSource = query.ToList();


            if (LstLugares.Items.Count > 0)
            {
                var lugares = new ObservableCollection<BicicletaEstacionTable>(query.ToList());
                foreach (var lugar in lugares)
                {
                    var pushpin = new Pushpin
                        {
                            Content = lugar.Nombre,
                            Location = new GeoCoordinate(lugar.Latitud, lugar.Longitud)
                        };
                    MiMapa.Children.Add(pushpin);
                }
            }

            //Si uso localizacion, agrego mi ubicación
            if ((bool)IsolatedStorageSettings.ApplicationSettings["localizacion"])
                ActualizarUbicacion((App.Current as App).Ubicacion);
            else
                ActualizarUbicacion(null);

            AjustarMapa();
        }

        private void AjustarMapa()
        {
            //Ajusto el mapa para mostrar los items
            var x = from l in MiMapa.Children
                    select (l as Pushpin).Location;
            MiMapa.SetView(LocationRect.CreateLocationRect(x));
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

        private static bool _datosLoaded = false;
        private bool CancelarRequest()
        {
            if (_datosLoaded)
                return false;

            if (MessageBox.Show(string.Format("¿Abortar la {0} de datos?", !(App.Current as App).InitialDataBicicletas ? "obtención" : "actualización"), "Estado del servicio", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return true;

            if (_httpReq != null)
                _httpReq.Abort();
            
            return false;
        }

        public void Cargar()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                SetProgressBar(MiMapa.Children.Any() ? "Actualizando estado..." : "Obteniendo estado...");

                _datosLoaded = false;
                _httpReq = WebRequest.Create(new Uri("http://servicio.abhosting.com.ar/bicicletas/?type=WP&version=1"));
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
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.RequestCanceled && (App.Current as App).InitialDataBicicletas)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(string.Format("La información del estado de servicio se actualizó por ultima vez el: {0}", ToLocalDateTime((App.Current as App).UltimaActualizacionBicicletas))));
                }
                EndRequest();
            }
            catch (Exception ex)
            {
                EndRequest();
                //this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error... " + ex.Message));
            }
        }

        delegate void DelegateUpdateWebBrowser(BicicletasStatusModel local);
        private void UpdateWebBrowser(BicicletasStatusModel l)
        {
            (App.Current as App).UltimaActualizacionBicicletas = l.Actualizacion;

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


            (App.Current as App).InitialDataBicicletas = true;
            _datosLoaded = true;

            UpdatedAt.Text = ToLocalDateTime(l.Actualizacion);

            MostrarLugares();
            EndRequest();
        }

        private static string ToLocalDateTime(DateTime dt)
        {
            return string.Format("{0} {1}", dt.ToLocalTime().ToShortDateString(), dt.ToLocalTime().ToShortTimeString());
        }

        private void EndRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = true;

                SystemTray.SetProgressIndicator(this, null);
            });
        }

        private void LstLugares_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox == null || listBox.SelectedIndex == -1) return;

            if (CancelarRequest()) return;

            var bicicletaEstacion = (BicicletaEstacionTable)listBox.SelectedItem;

            var uri = new Uri(String.Format("/Views/Bicicletas/LugarDetalles.xaml?id={0}", bicicletaEstacion.Id), UriKind.Relative);
            NavigationService.Navigate(uri);

            //Vuelvo el indice del item seleccionado a -1 para que pueda hacer tap en el mismo item y navegarlo
            listBox.SelectedIndex = -1;
        }

        private void Opciones_Click(object sender, EventArgs e)
        {
            if (CancelarRequest()) return;

            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void Acerca_Click(object sender, EventArgs e)
        {
            if (CancelarRequest()) return;

            NavigationService.Navigate(new Uri("/Views/Acerca.xaml", UriKind.Relative));
        }

        private void MiMapa_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (CancelarRequest()) return;

            NavigationService.Navigate(new Uri("/Views/Bicicletas/Mapa.xaml", UriKind.Relative));
        }
        
        private void ButtonGo_Click(object sender, EventArgs e)
        {
            Cargar();
        }
    }
}