using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Taxis
{
    public partial class Taxis : PhoneApplicationPage
    {
        public Taxis()
        {
            InitializeComponent();

            StatusChecker.Check("Taxis");

            MobFoxAdControl.PublisherID = App.Configuration.MobFoxID;
            MobFoxAdControl.TestMode = App.Configuration.MobFoxInTestMode;
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