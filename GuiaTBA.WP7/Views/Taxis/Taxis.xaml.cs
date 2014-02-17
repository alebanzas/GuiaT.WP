using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBA.WP7.Views.Taxis
{
    public partial class Taxis : PhoneApplicationPage
    {
        public Taxis()
        {
            InitializeComponent();
            MobFoxAdControl.PublisherID = App.Configuration.MobFoxID;
            MobFoxAdControl.TestMode = App.Configuration.MobFoxInTestMode;

            StatusChecker.Check("Taxis");
        }

        private void Button_Click_Tarifas(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Taxis/Tarifas.xaml", UriKind.Relative));
        }

        private void Button_Click_RadioTaxis(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Taxis/RadioTaxis.xaml", UriKind.Relative));
        }
    }
}