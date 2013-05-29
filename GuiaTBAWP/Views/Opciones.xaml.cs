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
            if (IsolatedStorageSettings.ApplicationSettings.Contains("localizacion"))
                TglLocalizacion.IsChecked = (bool)IsolatedStorageSettings.ApplicationSettings["localizacion"];
            
            Localizacion_Changed(null, null);
        }

        private void Localizacion_Changed(object sender, RoutedEventArgs e)
        {
            var isChecked = TglLocalizacion.IsChecked;
            var activated = isChecked != null && (bool)isChecked;
            IsolatedStorageSettings.ApplicationSettings["localizacion"] = activated;
            TglLocalizacion.Content = activated ? "Activado" : "Desactivado";
        }

    }
}