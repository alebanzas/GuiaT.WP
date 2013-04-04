using System;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using WPLugares;

namespace GuiaTBAWP.Views.Bicicletas
{
    public partial class LugarDetalles : PhoneApplicationPage
    {
        Pushpin PosicionActual;
        Lugar lugar;
        
        // Constructor
        public LugarDetalles()
        {
            InitializeComponent();
        }

        private void UpdateLugar()
        {
            PageTitle.Text = lugar.Nombre;
            MiDescripcion.Text = lugar.Descripcion;
            Pushpin NuevoLugar = new Pushpin();
            NuevoLugar.Content = lugar.Nombre;
            NuevoLugar.Location = new GeoCoordinate(lugar.Latitud, lugar.Longitud);
            MiMapa.Children.Clear();
            this.MiMapa.Children.Add(NuevoLugar);
            this.MiImagen.Source = new BitmapImage(new Uri(lugar.Imagen1, UriKind.Absolute));
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = !String.IsNullOrEmpty(lugar.Url);
            ((ApplicationBarIconButton)ApplicationBar.Buttons[2]).IsEnabled = (ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(String.Format("LugarDetalles.xaml?id={0}", lugar.Id))) == null);

         
            //Ajusto el mapa a la ubicacion del lugar
            MiMapa.SetView(NuevoLugar.Location,17);
            MiMapa.Mode = new AerialMode();

            //Si uso localizacion, agrego mi ubicación
            if ((bool)IsolatedStorageSettings.ApplicationSettings["localizacion"])
                ActualizarUbicacion((App.Current as App).Ubicacion);
            else
                ActualizarUbicacion(null);
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Al navegar a la página, busco el lugar en base al id pasado y luego lo muestro.
            int id = int.Parse(Uri.EscapeUriString(NavigationContext.QueryString["id"]));
            var query = from MiLugar in LugarDC.Current.Lugares
                        where MiLugar.Id == id
                        select MiLugar;

            this.lugar = (Lugar)query.FirstOrDefault();
            UpdateLugar();

            base.OnNavigatedTo(e);
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

        private void SwitchView(object sender, EventArgs e)
        {
            if (MiMapa.Mode is RoadMode)
                MiMapa.Mode = new AerialMode();
            else
                MiMapa.Mode = new RoadMode();
        }

        private void Browse(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(lugar.Url))
            {
                // Creo una nueva tarea WebBrowserTask para navegar al item  
                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri(lugar.Url, UriKind.Absolute);
                webBrowserTask.Show();
            }
        }

        private void Pin(object sender, EventArgs e)
        {
            ShellTile toFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(String.Format("LugarDetalles.xaml?id={0}", lugar.Id)));

            if (toFind == null)
            {
                var frontFile = SaveTileAsImage("f", new TileData() { ImageSource = lugar.Imagen1, Text1 = lugar.Nombre});
                
                var data = new StandardTileData
                {
                    BackgroundImage = new Uri("isostore:" + frontFile, UriKind.Absolute),
                };

                ShellTile.Create(new Uri(String.Format("/LugarDetalles.xaml?id={0}", lugar.Id), UriKind.Relative), data);
            }
        }

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
            var filename = String.Format("/Shared/ShellContent/tile_{0}_{1}.jpg", lugar.Id, tileType);

            if (!isf.DirectoryExists("/Shared/ShellContent"))
                isf.CreateDirectory("/Shared/ShellContent");

            using (var stream = isf.OpenFile(filename, System.IO.FileMode.OpenOrCreate))
            {
                bmp.SaveJpeg(stream, 173, 173, 0, 100);
            }
            return filename;
        }

        private void Share(object sender, EventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.Title = lugar.Nombre;
            shareLinkTask.Message = lugar.Descripcion;
            shareLinkTask.LinkUri = new Uri(lugar.Url, UriKind.Absolute);
            shareLinkTask.Show();
        }

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Opciones.xaml", UriKind.Relative));
        }

        private void Directions(object sender, EventArgs e)
        {
            BingMapsDirectionsTask bingMapsDirectionsTask = new BingMapsDirectionsTask();
            bingMapsDirectionsTask.End = new LabeledMapLocation(String.Format("{0},{1}", lugar.Latitud, lugar.Longitud), new GeoCoordinate(lugar.Latitud, lugar.Longitud));
            bingMapsDirectionsTask.Show();
        }

    }
}