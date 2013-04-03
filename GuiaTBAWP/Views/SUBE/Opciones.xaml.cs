using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.SUBE
{
    public partial class Opciones : PhoneApplicationPage
    {
        public Opciones()
        {
            InitializeComponent();
            if (IsolatedStorageSettings.ApplicationSettings.Contains("localizacion"))
                this.TglLocalizacion.IsChecked = (bool)IsolatedStorageSettings.ApplicationSettings["localizacion"];
            
            Localizacion_Changed(null, null);
        }

        private void Localizacion_Changed(object sender, RoutedEventArgs e)
        {
            bool activated = (bool)this.TglLocalizacion.IsChecked;
            IsolatedStorageSettings.ApplicationSettings["localizacion"] = activated;
            if (activated)
            {
                //if ((App.Current as App).Ubicacion.Status != GeoPositionStatus.Disabled)
                //    (App.Current as App).Ubicacion.Start();
                this.TglLocalizacion.Content = "Activado";
            }
            else
            {
                //if ((App.Current as App).Ubicacion.Status != GeoPositionStatus.Disabled)
                //    (App.Current as App).Ubicacion.Stop();
                this.TglLocalizacion.Content = "Desactivado";
            }
        }

    }
}