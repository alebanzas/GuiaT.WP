using System.Windows;
using Microsoft.Phone.Tasks;

namespace GuiaTBAWP.Views
{
    public partial class Opciones
    {
        public Opciones()
        {
            InitializeComponent();

            TglLocalizacion.IsChecked = App.Configuration.IsLocationEnabledByAppConfig;
            
            Localizacion_Changed(null, null);
        }

        private void Localizacion_Changed(object sender, RoutedEventArgs e)
        {
            var isChecked = TglLocalizacion.IsChecked;
            var activated = isChecked != null && (bool)isChecked;
            App.Configuration.IsLocationEnabledByAppConfig = activated;
            if (activated)
            {
                PositionService.Initialize();
            }
            else
            {
                PositionService.Stop();
            }

            TglLocalizacion.Content = activated ? "Activado" : "Desactivado";
        }

        private void ButtonDownloadMaps_OnClick(object sender, RoutedEventArgs e)
        {
            (new MapDownloaderTask()).Show();
        }
    }
}