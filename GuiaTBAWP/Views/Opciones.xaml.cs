using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views
{
    public partial class Opciones : PhoneApplicationPage
    {
        public Opciones()
        {
            InitializeComponent();

            TglLocalizacion.IsChecked = App.Configuration.IsLocationEnabled;
            
            Localizacion_Changed(null, null);
        }

        private void Localizacion_Changed(object sender, RoutedEventArgs e)
        {
            var isChecked = TglLocalizacion.IsChecked;
            var activated = isChecked != null && (bool)isChecked;
            App.Configuration.IsLocationEnabled = activated;
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

    }
}