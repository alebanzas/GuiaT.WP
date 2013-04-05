using System;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
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
        bool _zoomAjustado;
        readonly System.Windows.Threading.DispatcherTimer _timer = new System.Windows.Threading.DispatcherTimer();

        readonly ProgressIndicator _progress = new ProgressIndicator();

        public Estaciones()
        {
            InitializeComponent();
            Loaded += Page_Loaded;
            MiMapa.ViewChangeEnd += MiMapa_ViewChangeEnd;
        }

        void MiMapa_ViewChangeEnd(object sender, MapEventArgs e)
        {
            if (!_zoomAjustado)
                MiMapa.ZoomLevel--;
            _zoomAjustado = true;
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _progress.IsVisible = true;
            _progress.IsIndeterminate = true;

            MostrarLugares();
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

            //Ajusto los márgenes del mapa
            if (LstLugares.Items.Count <= 0 || (App.Current as App).TimerUsed) return;

            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += timer_Tick;
            _timer.Start();
        }

        private void AjustarMapa()
        {
            //Ajusto el mapa para mostrar los items
            var x = from l in MiMapa.Children
                    let pushpin = l as Pushpin
                    where pushpin != null
                    select pushpin.Location;
            MiMapa.SetView(LocationRect.CreateLocationRect(x));
            _zoomAjustado = false;
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

        public void Cargar()
        {
            SetProgressBar("Obteniendo estado...");

            var httpReq = (HttpWebRequest)WebRequest.Create(new Uri("http://servicio.abhosting.com.ar/bicicletas/"));
            httpReq.Method = "GET";
            httpReq.BeginGetResponse(HTTPWebRequestCallBack, httpReq);
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
            catch (Exception ex)
            {
                FinishRequest();
                //this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error... " + ex.Message));
            }
        }

        delegate void DelegateUpdateWebBrowser(BicicletasStatusModel local);
        private void UpdateWebBrowser(BicicletasStatusModel l)
        {
            foreach (BicicletaEstacionTable ll in l.Estaciones.ConvertToBicicletaEstacionTable())
            {
                if (BicicletaEstacionDC.Current.Estaciones.Contains(ll))
                {
                    var estacion = BicicletaEstacionDC.Current.Estaciones.FirstOrDefault(x => x.Equals(ll));
                    if (estacion != null)
                    {
                        estacion.Cantidad = ll.Cantidad;
                        estacion.Horario = ll.Horario;
                    }
                }
                else
                {
                    BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(ll);
                }   
            }
            BicicletaEstacionDC.Current.SubmitChanges();

            UpdatedAt.Text = string.Format("{0} {1}", l.Actualizacion.ToShortDateString(), l.Actualizacion.ToShortTimeString());

            MostrarLugares();
            FinishRequest();
        }

        private void FinishRequest()
        {
            Dispatcher.BeginInvoke(() => SetProgressBar(null));
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

        void timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            (App.Current as App).TimerUsed = true;
            AjustarMapa();
        }

    }
}