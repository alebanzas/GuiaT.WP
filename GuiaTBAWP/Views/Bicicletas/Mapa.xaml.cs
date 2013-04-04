using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;


namespace WPLugares
{
    public partial class Mapa : PhoneApplicationPage
    {
        Pushpin PosicionActual;
        public ObservableCollection<Lugar> EnLugar;

        public Mapa()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
            //Genero el evento asociados al cambio de posición
            (App.Current as App).Ubicacion.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(Ubicacion_PositionChanged);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarLugares();
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

        void MostrarLugares()
        {
            //Limpio el mapa, tomo lugares de la tabla local y los agrego al mapa
            MiMapa.Children.Clear();

            var query = from MiLugar in LugarDC.Current.Lugares
                        orderby MiLugar.Id
                        select MiLugar;

            EnLugar = new ObservableCollection<Lugar>(query.ToList());
            
            foreach (Lugar ML in EnLugar)
            {

                Pushpin NuevoLugar = new Pushpin();
                NuevoLugar.Content = ML.Nombre;
                NuevoLugar.Location = new GeoCoordinate(ML.Latitud, ML.Longitud);

                NuevoLugar.MouseLeftButtonUp += new MouseButtonEventHandler(NuevoLugar_MouseLeftPuttonUp);
                this.MiMapa.Children.Add(NuevoLugar);
            }

            //Ajusto el mapa para mostrar los items
            var x = from l in this.MiMapa.Children
                    select (l as Pushpin).Location;
            MiMapa.SetView(LocationRect.CreateLocationRect(x));

            //Si uso localizacion, agrego mi ubicación
            if ((bool)IsolatedStorageSettings.ApplicationSettings["localizacion"])
                ActualizarUbicacion((App.Current as App).Ubicacion.Position.Location);
            else
                ActualizarUbicacion(null);
        }

        private void NuevoLugar_MouseLeftPuttonUp(object sender, MouseButtonEventArgs e)
        {
            Pushpin pin = sender as Pushpin;

            if (pin != null)
            {
                var query = from l in LugarDC.Current.Lugares
                            where l.Latitud == pin.Location.Latitude
                            && l.Longitud == pin.Location.Longitude
                            && l.Nombre == pin.Content.ToString()
                            select l;
                            
                List<Lugar> listaLugares = query.ToList();
                Lugar lugar = listaLugares.FirstOrDefault();
                Uri uri = new Uri(String.Format("/LugarDetalles.xaml?id={0}", lugar.Id), UriKind.Relative);
                NavigationService.Navigate(uri);
            }
        }

        private void BtnAcercar_Click(object sender, EventArgs e)
        {
            MiMapa.ZoomLevel++;
        }

        private void BtnAlejar_Click(object sender, EventArgs e)
        {
            MiMapa.ZoomLevel--;
        }

        private void BtnVista_Click(object sender, EventArgs e)
        {
            if (MiMapa.Mode is RoadMode)
                MiMapa.Mode = new AerialMode();
            else
                MiMapa.Mode = new RoadMode();
        }

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Opciones.xaml", UriKind.Relative));
        }

    }
}