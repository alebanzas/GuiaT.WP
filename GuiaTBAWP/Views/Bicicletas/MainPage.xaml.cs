using System;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.Globalization;

namespace WPLugares
{
    public partial class MainPage : PhoneApplicationPage
    {
        Pushpin PosicionActual;
        bool zoomAjustado;
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
            (App.Current as App).Ubicacion.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(Ubicacion_PositionChanged);
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
            if (!(App.Current as App).DataLoaded)
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

            var query = from l in LugarDC.Current.Lugares
                        orderby l.Id
                        select l;

            this.LstLugares.ItemsSource = query.ToList();


            if (this.LstLugares.Items.Count > 0)
            {
                ObservableCollection<Lugar> lugares = new ObservableCollection<Lugar>(query.ToList());
                foreach (Lugar lugar in lugares)
                {
                    Pushpin pushpin = new Pushpin();
                    pushpin.Content = lugar.Nombre;
                    pushpin.Location = new GeoCoordinate(lugar.Latitud, lugar.Longitud);
                    this.MiMapa.Children.Add(pushpin);
                }
            }

            //Si uso localizacion, agrego mi ubicación
            if ((bool)IsolatedStorageSettings.ApplicationSettings["localizacion"])
                ActualizarUbicacion((App.Current as App).Ubicacion.Position.Location);
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

        public void Cargar()
        {
            XDocument loadedData = XDocument.Load("Assets/lugares.xml");
            var filteredData = from c in loadedData.Descendants("lugar")
                               select new Lugar()
                               {
                                   Id = int.Parse(c.Element("id").Value),
                                   Nombre = c.Element("nombre").Value,
                                   Longitud = Convert.ToDouble(c.Element("longitud").Value, CultureInfo.InvariantCulture),
                                   Latitud = Convert.ToDouble(c.Element("latitud").Value, CultureInfo.InvariantCulture),
                                   Descripcion = c.Element("descripcion").Value,
                                   Imagen1 = c.Element("imagen1").Value,
                                   Imagen2 = c.Element("imagen2").Value,
                                   Url = c.Element("url").Value
                               };
            foreach (Lugar l in filteredData)
            {
                //si el elemento no está guardado en el isolated storage, lo guardo
                if (!LugarDC.Current.Lugares.Contains<Lugar>(l))
                    LugarDC.Current.Lugares.InsertOnSubmit(l);
            }
            LugarDC.Current.SubmitChanges();
            (App.Current as App).DataLoaded = true;
            MostrarLugares();
        }


        private void LstLugares_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            if (listBox != null && listBox.SelectedIndex != -1)
            {
                Lugar lugar = (Lugar)listBox.SelectedItem;

                Uri uri = new Uri(String.Format("/LugarDetalles.xaml?id={0}", lugar.Id), UriKind.Relative);
                NavigationService.Navigate(uri);

                //Vuelvo el indice del item seleccionado a -1 para que pueda hacer tap en el mismo item y navegarlo
                listBox.SelectedIndex = -1;
            }
        }

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Opciones.xaml", UriKind.Relative));
        }

        private void Acerca_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Acerca.xaml", UriKind.Relative));
        }

        private void MiMapa_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Mapa.xaml", UriKind.Relative));
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            (App.Current as App).TimerUsed = true;
            AjustarMapa();
        }

    }
}