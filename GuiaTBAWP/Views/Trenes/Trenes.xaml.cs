using System;
using System.Windows;
using GuiaTBAWP.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Views.Trenes
{
    public partial class Trenes : PhoneApplicationPage
    {
        public Trenes()
        {
            InitializeComponent();

            DataContext = ViewModel;
            Loaded += MainPage_Loaded;

            ViewModel.Lineas.Clear();
            _dataService.EndRequest = EndRequest;
            _dataService.StartRequest = StartRequest;
        }

        private void Button_Click_BelgranoNorte(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/BelgranoNorte.xaml", UriKind.Relative));
        }

        private void Button_Click_BelgranoSur(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/BelgranoSur.xaml", UriKind.Relative));
        }

        private void Button_Click_Mitre(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/Mitre.xaml", UriKind.Relative));
        }

        private void Button_Click_Roca(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/Roca.xaml", UriKind.Relative));
        }

        private void Button_Click_SanMartin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/SanMartin.xaml", UriKind.Relative));
        }

        private void Button_Click_Sarmiento(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/Sarmiento.xaml", UriKind.Relative));
        }

        private void Button_Click_Urquiza(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Trenes/Urquiza.xaml", UriKind.Relative));
        }
        
        #region Data

        private static SubteStatusViewModel _viewModel = new SubteStatusViewModel();

        private static TrenStatusService _dataService = new TrenStatusService();

        public static SubteStatusViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                return _viewModel ?? (_viewModel = new SubteStatusViewModel());
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_dataService.DatosLoaded)
                _dataService.LoadData();
        }

        private int EndRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = true;

                ProgressBar.Hide();
            });
            return 0;
        }

        private int StartRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = false;

                ProgressBar.Show("Actualizando estado del servicio...");
            });
            return 0;
        }

        private void ButtonGo_Click(object sender, EventArgs e)
        {
            _dataService.LoadData();
        }
        #endregion
    }
}