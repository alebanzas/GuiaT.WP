using System;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;

namespace GuiaTBAWP.Views.SUBE
{
    public partial class Mapa : PhoneApplicationPage
    {
        Pushpin PosicionActual;
        //public ObservableCollection<Lugar> EnLugar;

        public Mapa()
        {
            InitializeComponent();
            //Genero el evento asociados al cambio de posición
            //(App.Current as App).Ubicacion.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(Ubicacion_PositionChanged);

            //App.Map = MiMapa;
            DataContext = App.ViewModel;
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
            //App.CreateNewPushpin(App.ViewModel.CurrentLocation);

            //Ajusto el mapa para mostrar los items
            var x = from l in App.ViewModel.Pushpins
                    select l.Location;
            MiMapa.SetView(LocationRect.CreateLocationRect(x));

            //Si uso localizacion, agrego mi ubicación
            //if ((bool)IsolatedStorageSettings.ApplicationSettings["localizacion"])
            //    ActualizarUbicacion((App.Current as App).Ubicacion.Position.Location);
            //else
            //    ActualizarUbicacion(null);
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

        private void Pushpin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var pushpin = sender as Pushpin;

            // Center the map on a pushpin when touched.
            if (pushpin != null) App.ViewModel.Center = pushpin.Location;
        }
    }
}