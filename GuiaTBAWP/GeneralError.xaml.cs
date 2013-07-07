using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP
{
    public partial class GeneralError : PhoneApplicationPage
    {
        public GeneralError()
        {
            InitializeComponent();
        }

        public static Exception Exception;

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ErrorText.Text = Exception.ToString();
        }

        private void GoToHome_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
            }
            catch (Exception)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Por favor, vuelva a iniciar la aplicación manualmente."));
            }
        }

        private void SendErrorReport_OnClick(object sender, RoutedEventArgs e)
        {
            DvError.Visibility = Visibility.Collapsed;
            DvThanks.Visibility = Visibility.Visible;
        }
    }
}