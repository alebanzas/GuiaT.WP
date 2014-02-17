using System;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using GuiaTBAWP.Commons;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Core;
using Microsoft.Phone.Tasks;

namespace GuiaTBA.WP7.Views.Bicicletas
{
    public partial class LugarDetalles : PhoneApplicationPage
    {
        BicicletaEstacionTable _bicicletaEstacion;
        
        // Constructor
        public LugarDetalles()
        {
            InitializeComponent();
            Unloaded += Page_UnLoaded;
        }

        private void Page_UnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void UpdateLugar()
        {
            PageTitle.Text = _bicicletaEstacion.Nombre;
            Horario.Text = _bicicletaEstacion.Horario;
            Estado.Text = _bicicletaEstacion.Estado;
            Cantidad.Text = _bicicletaEstacion.Cantidad.ToString(CultureInfo.InvariantCulture);
            Distancia.Text = string.IsNullOrWhiteSpace(_bicicletaEstacion.Distance) ? string.Empty : "distancia: " + _bicicletaEstacion.Distance;

            var nuevoLugar = new Pushpin
                {
                    Content = _bicicletaEstacion.Nombre,
                    Location = new GeoCoordinate(_bicicletaEstacion.Latitud, _bicicletaEstacion.Longitud)
                };
            MiMapa.Children.Clear();
            MiMapa.Children.Add(nuevoLugar);

            MiMapa.Center = new GeoCoordinate(_bicicletaEstacion.Latitud, _bicicletaEstacion.Longitud);
            MiMapa.Mode = new AerialMode();
            MiMapa.ZoomLevel = 17;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Al navegar a la página, busco el lugar en base al id pasado y luego lo muestro.
            var id = Guid.Parse(Uri.EscapeUriString(NavigationContext.QueryString["id"]));
            var query = from miLugar in BicicletaEstacionDC.Current.Estaciones
                        where miLugar.Id == id
                        select miLugar;

            _bicicletaEstacion = query.FirstOrDefault();
            MiMapa.CredentialsProvider = new ApplicationIdCredentialsProvider(App.Configuration.BingMapApiKey);
            UpdateLugar();

            base.OnNavigatedTo(e);
        }
        
        private void SwitchView(object sender, EventArgs e)
        {
            MiMapa.Mode = (MiMapa.Mode is RoadMode) ? (MapMode) new AerialMode() : new RoadMode();
        }
        
        private void Pin(object sender, EventArgs e)
        {
            TileManager.Set(new Uri(string.Format("/Views/Bicicletas/LugarDetalles.xaml?id={0}", _bicicletaEstacion.Id), UriKind.Relative), _bicicletaEstacion.Nombre, new Uri("/Images/Home/bicicletas.png", UriKind.Relative));
        }
        
        private void Share(object sender, EventArgs e)
        {
            var shareLinkTask = new ShareLinkTask
                {
                    Title = _bicicletaEstacion.Nombre,
                    Message = _bicicletaEstacion.Horario,
                    LinkUri = new Uri(_bicicletaEstacion.Url, UriKind.Absolute)
                };
            shareLinkTask.Show();
        }

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void Directions(object sender, EventArgs e)
        {
            var bingMapsDirectionsTask = new BingMapsDirectionsTask
                {
                    End =
                        new LabeledMapLocation(
                            String.Format("Estación {0}", _bicicletaEstacion.Nombre),
                            new GeoCoordinate(_bicicletaEstacion.Latitud, _bicicletaEstacion.Longitud))
                };
            bingMapsDirectionsTask.Show();
        }

    }
}