using System;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace GuiaTBAWP.Views.Bicicletas
{
    public partial class LugarDetalles : PhoneApplicationPage
    {
        Pushpin PosicionActual;
        BicicletaEstacionTable bicicletaEstacion;
        
        // Constructor
        public LugarDetalles()
        {
            InitializeComponent();
        }

        private void UpdateLugar()
        {
            PageTitle.Text = bicicletaEstacion.Nombre;
            MiDescripcion.Text = bicicletaEstacion.Horario;
            Pushpin NuevoLugar = new Pushpin();
            NuevoLugar.Content = bicicletaEstacion.Nombre;
            NuevoLugar.Location = new GeoCoordinate(bicicletaEstacion.Latitud, bicicletaEstacion.Longitud);
            MiMapa.Children.Clear();
            this.MiMapa.Children.Add(NuevoLugar);
            //this.MiImagen.Source = new BitmapImage(new Uri(bicicletaEstacion.Imagen1, UriKind.Absolute));
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = !String.IsNullOrEmpty(bicicletaEstacion.Url);
            ((ApplicationBarIconButton)ApplicationBar.Buttons[2]).IsEnabled = (ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(String.Format("LugarDetalles.xaml?id={0}", bicicletaEstacion.Id))) == null);

         
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
            Guid id = Guid.Parse(Uri.EscapeUriString(NavigationContext.QueryString["id"]));
            var query = from MiLugar in BicicletaEstacionDC.Current.Estaciones
                        where MiLugar.Id == id
                        select MiLugar;

            this.bicicletaEstacion = query.FirstOrDefault();
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
            if (!String.IsNullOrEmpty(bicicletaEstacion.Url))
            {
                // Creo una nueva tarea WebBrowserTask para navegar al item  
                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri(bicicletaEstacion.Url, UriKind.Absolute);
                webBrowserTask.Show();
            }
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
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.Title = bicicletaEstacion.Nombre;
            shareLinkTask.Message = bicicletaEstacion.Horario;
            shareLinkTask.LinkUri = new Uri(bicicletaEstacion.Url, UriKind.Absolute);
            shareLinkTask.Show();
        }

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Opciones.xaml", UriKind.Relative));
        }

        private void Directions(object sender, EventArgs e)
        {
            BingMapsDirectionsTask bingMapsDirectionsTask = new BingMapsDirectionsTask();
            bingMapsDirectionsTask.End = new LabeledMapLocation(String.Format("{0},{1}", bicicletaEstacion.Latitud, bicicletaEstacion.Longitud), new GeoCoordinate(bicicletaEstacion.Latitud, bicicletaEstacion.Longitud));
            bingMapsDirectionsTask.Show();
        }

    }
}