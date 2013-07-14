using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GuiaTBAWP;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;


namespace WPLugares
{
    public partial class Mapa : PhoneApplicationPage
    {
        Pushpin PosicionActual;
        public ObservableCollection<BicicletaEstacionTable> EnLugar;

        public Mapa()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MostrarLugares();
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

            var query = from MiLugar in BicicletaEstacionDC.Current.Estaciones
                        orderby MiLugar.Id
                        select MiLugar;

            EnLugar = new ObservableCollection<BicicletaEstacionTable>(query.ToList());

            foreach (BicicletaEstacionTable ML in EnLugar)
            {

                Pushpin NuevoLugar = new Pushpin();
                NuevoLugar.Content = ML.Nombre;
                NuevoLugar.Location = new GeoCoordinate(ML.Latitud, ML.Longitud);

                NuevoLugar.MouseLeftButtonUp += NuevoLugar_MouseLeftPuttonUp;
                this.MiMapa.Children.Add(NuevoLugar);
            }

            //Ajusto el mapa para mostrar los items
            var x = from l in this.MiMapa.Children
                    select (l as Pushpin).Location;
            MiMapa.SetView(LocationRect.CreateLocationRect(x));

            //Si uso localizacion, agrego mi ubicación
            if ((bool)IsolatedStorageSettings.ApplicationSettings["localizacion"])
                ActualizarUbicacion(App.Configuration.Ubicacion);
            else
                ActualizarUbicacion(null);
        }

        private void NuevoLugar_MouseLeftPuttonUp(object sender, MouseButtonEventArgs e)
        {
            Pushpin pin = sender as Pushpin;

            if (pin != null)
            {
                var query = from l in BicicletaEstacionDC.Current.Estaciones
                            where l.Latitud == pin.Location.Latitude
                            && l.Longitud == pin.Location.Longitude
                            && l.Nombre == pin.Content.ToString()
                            select l;

                List<BicicletaEstacionTable> listaLugares = query.ToList();
                BicicletaEstacionTable bicicletaEstacion = listaLugares.FirstOrDefault();
                Uri uri = new Uri(String.Format("/Views/Bicicletas/LugarDetalles.xaml?id={0}", bicicletaEstacion.Id), UriKind.Relative);
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
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

    }
}