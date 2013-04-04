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
        Pushpin PosicionActual;
        bool zoomAjustado;
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        readonly ProgressIndicator _progress = new ProgressIndicator();

        public Estaciones()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
            this.MiMapa.ViewChangeEnd += MiMapa_ViewChangeEnd;
        }

        void MiMapa_ViewChangeEnd(object sender, MapEventArgs e)
        {
            if (!zoomAjustado)
                this.MiMapa.ZoomLevel--;
            zoomAjustado = true;
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarLugares();
            Cargar();
        }

        void Ubicacion_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            ActualizarUbicacion(e.Position.Location);
        }

        private void ActualizarUbicacion(GeoCoordinate location)
        {
            MiMapa.Children.Remove(PosicionActual);

            if (location != null && !location.IsUnknown)
            {
                PosicionActual = new Pushpin();
                PosicionActual.Location = location;
                PosicionActual.Template = (ControlTemplate)(App.Current.Resources["locationPushpinTemplate"]);
                MiMapa.Children.Add(PosicionActual);
            }
        }


        public void MostrarLugares()
        {
            //Limpio el mapa, tomo lugares de la tabla local y los agrego al mapa
            MiMapa.Children.Clear();

            var query = from l in BicicletaEstacionDC.Current.Estaciones
                        orderby l.Id
                        select l;

            this.LstLugares.ItemsSource = query.ToList();


            if (this.LstLugares.Items.Count > 0)
            {
                ObservableCollection<BicicletaEstacionTable> lugares = new ObservableCollection<BicicletaEstacionTable>(query.ToList());
                foreach (BicicletaEstacionTable lugar in lugares)
                {
                    Pushpin pushpin = new Pushpin();
                    pushpin.Content = lugar.Nombre;
                    pushpin.Location = new GeoCoordinate(lugar.Latitud, lugar.Longitud);
                    this.MiMapa.Children.Add(pushpin);
                }
            }

            //Si uso localizacion, agrego mi ubicación
            if ((bool)IsolatedStorageSettings.ApplicationSettings["localizacion"])
                ActualizarUbicacion((App.Current as App).Ubicacion);
            else
                ActualizarUbicacion(null);

            //Ajusto los márgenes del mapa
            if (this.LstLugares.Items.Count > 0 && !(App.Current as App).TimerUsed)
            {
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Tick += timer_Tick;
                timer.Start();
            }
        }

        private void AjustarMapa()
        {
            //Ajusto el mapa para mostrar los items
            var x = from l in this.MiMapa.Children
                    select (l as Pushpin).Location;
            this.MiMapa.SetView(LocationRect.CreateLocationRect(x));
            this.zoomAjustado = false;
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
            SetProgressBar("Buscando estaciones...");

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
                //si el elemento no está guardado en el isolated storage, lo guardo
                if (!BicicletaEstacionDC.Current.Estaciones.Contains<BicicletaEstacionTable>(ll))
                    BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(ll);
            }
            BicicletaEstacionDC.Current.SubmitChanges();

            UpdatedAt.Text = string.Format("Ultima actualizacion: {0}", l.ActualizacionStr);

            MostrarLugares();
            FinishRequest();
        }

        private void FinishRequest()
        {
            
        }

        private void LstLugares_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            if (listBox != null && listBox.SelectedIndex != -1)
            {
                BicicletaEstacionTable bicicletaEstacion = (BicicletaEstacionTable)listBox.SelectedItem;

                Uri uri = new Uri(String.Format("/Views/Bicicletas/LugarDetalles.xaml?id={0}", bicicletaEstacion.Id), UriKind.Relative);
                NavigationService.Navigate(uri);

                //Vuelvo el indice del item seleccionado a -1 para que pueda hacer tap en el mismo item y navegarlo
                listBox.SelectedIndex = -1;
            }
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
            timer.Stop();
            (App.Current as App).TimerUsed = true;
            AjustarMapa();
        }

    }
}