using System;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Core;
using Microsoft.Phone.Tasks;

namespace BicicletaBAWP.Views.Bicicletas
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
            MiMapa.Mode = (MiMapa.Mode is RoadMode) ? (MapMode)new AerialMode() : new RoadMode();
        }

        private void Pin(object sender, EventArgs e)
        {
            /*
            ShellTile toFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(String.Format("LugarDetalles.xaml?id={0}", bicicletaEstacion.Id)));

            if (toFind == null)
            {
                var frontFile = SaveTileAsImage("f", new TileData() { ImageSource = bicicletaEstacion.Imagen1, Text1 = bicicletaEstacion.Nombre});
                
                var data = new StandardTileData
                {
                    BackgroundImage = new Uri("isostore:" + frontFile, UriKind.Absolute),
                };

                ShellTile.Create(new Uri(String.Format("/LugarDetalles.xaml?id={0}", bicicletaEstacion.Id), UriKind.Relative), data);
            }
            */
        }

        /*
        private string SaveTileAsImage(String tileType, TileData tileData)
        {
            TileControl tile = new TileControl();
            tile.DataContext = tileData;

            tile.Measure(new Size(173, 173));
            tile.Arrange(new Rect(0, 0, 173, 173));
            var bmp = new WriteableBitmap(173, 173);
            bmp.Render(tile, null);
            bmp.Invalidate();

            var isf = IsolatedStorageFile.GetUserStoreForApplication();
            var filename = String.Format("/Shared/ShellContent/tile_{0}_{1}.jpg", bicicletaEstacion.Id, tileType);

            if (!isf.DirectoryExists("/Shared/ShellContent"))
                isf.CreateDirectory("/Shared/ShellContent");

            using (var stream = isf.OpenFile(filename, System.IO.FileMode.OpenOrCreate))
            {
                bmp.SaveJpeg(stream, 173, 173, 0, 100);
            }
            return filename;
        }
        */

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
                        String.Format("{0},{1}", _bicicletaEstacion.Latitud, _bicicletaEstacion.Longitud),
                        new GeoCoordinate(_bicicletaEstacion.Latitud, _bicicletaEstacion.Longitud))
            };
            bingMapsDirectionsTask.Show();
        }

    }
}