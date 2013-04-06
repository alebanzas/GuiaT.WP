using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using GuiaTBAWP.Models;
using GuiaTBAWP.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Views.Trenes
{
    public partial class Trenes : PhoneApplicationPage
    {
        public Trenes()
        {
            ViewModel.Lineas.Clear();

            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = ViewModel;
            Loaded += MainPage_Loaded;
            Unloaded += MainPage_Unloaded;

            _progress.IsVisible = true;
            _progress.IsIndeterminate = true;
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if(_httpReq != null)
                _httpReq.Abort();
        }

        private bool CancelarRequest()
        {
            if (_datosLoaded)
                return false;

            if (MessageBox.Show(string.Format("¿Abortar la {0} de datos?", !(App.Current as App).InitialDataTrenes ? "obtención" : "actualización"), "Estado del servicio", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return true;

            _httpReq.Abort();
            return false;
        }

        private void Button_Click_BelgranoNorte(object sender, RoutedEventArgs e)
        {
            if (CancelarRequest()) return;
            
            NavigationService.Navigate(new Uri("/Views/Trenes/BelgranoNorte.xaml", UriKind.Relative));
        }

        private void Button_Click_BelgranoSur(object sender, RoutedEventArgs e)
        {
            if (CancelarRequest()) return;

            NavigationService.Navigate(new Uri("/Views/Trenes/BelgranoSur.xaml", UriKind.Relative));
        }

        private void Button_Click_Mitre(object sender, RoutedEventArgs e)
        {
            if (CancelarRequest()) return;

            NavigationService.Navigate(new Uri("/Views/Trenes/Mitre.xaml", UriKind.Relative));
        }

        private void Button_Click_Roca(object sender, RoutedEventArgs e)
        {
            if (CancelarRequest()) return;

            NavigationService.Navigate(new Uri("/Views/Trenes/Roca.xaml", UriKind.Relative));
        }

        private void Button_Click_SanMartin(object sender, RoutedEventArgs e)
        {
            if (CancelarRequest()) return;

            NavigationService.Navigate(new Uri("/Views/Trenes/SanMartin.xaml", UriKind.Relative));
        }

        private void Button_Click_Sarmiento(object sender, RoutedEventArgs e)
        {
            if (CancelarRequest()) return;

            NavigationService.Navigate(new Uri("/Views/Trenes/Sarmiento.xaml", UriKind.Relative));
        }

        private void Button_Click_Urquiza(object sender, RoutedEventArgs e)
        {
            if (CancelarRequest()) return;

            NavigationService.Navigate(new Uri("/Views/Trenes/Urquiza.xaml", UriKind.Relative));
        }

        private static SubteStatusViewModel _viewModel = new SubteStatusViewModel();

        private static bool _datosLoaded = false;

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static SubteStatusViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                return _viewModel ?? (_viewModel = new SubteStatusViewModel());
            }
        }


        readonly ProgressIndicator _progress = new ProgressIndicator();
        
        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_datosLoaded)
                LoadData();
        }


        #region DataInit

        private WebRequest _httpReq;

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                _progress.Text = string.Format("{0} estado del servicio...", !(App.Current as App).InitialDataTrenes ? "Obteniendo" : "Actualizando");
                SystemTray.SetIsVisible(this, true);
                SystemTray.SetProgressIndicator(this, _progress);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                    if (applicationBarIconButton != null)
                        applicationBarIconButton.IsEnabled = false;
                });

                _datosLoaded = false;
                _httpReq = WebRequest.Create(new Uri("http://servicio.abhosting.com.ar/trenes/?type=WP&version=1"));
                _httpReq.Method = "GET";
                _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
            }
            else
            {
                ShowErrorConnection();
            }
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(TrenesStatusModel));
                var o = (TrenesStatusModel)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateWebBrowser(UpdateStatus), o);
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.RequestCanceled && (App.Current as App).InitialDataTrenes)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(string.Format("La información del estado de servicio se actualizó por ultima vez el: {0}", ToLocalDateTime((App.Current as App).UltimaActualizacionTrenes))));    
                }
                EndRequest();
            }
            catch (Exception ex)
            {
                EndRequest();
                //this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error.. " + ex.Message));
                Dispatcher.BeginInvoke(() => MessageBox.Show("Ocurrió un error al obtener las cotizaciones. Verifique su conexión a internet."));
            }
        }

        private static string ToLocalDateTime(DateTime dt)
        {
            return string.Format("{0} {1}", dt.ToLocalTime().ToShortDateString(), dt.ToLocalTime().ToShortTimeString());
        }

        delegate void DelegateUpdateWebBrowser(TrenesStatusModel local);
        private void UpdateStatus(TrenesStatusModel model)
        {
            (App.Current as App).UltimaActualizacionTrenes = model.Actualizacion;

            foreach (var ltm in model.Lineas)
            {
                var lineaTrenModel = ltm.ConvertToTrenesLineaEstadoTable();

                if (TrenesLineaEstadoDC.Current.Lineas.Contains(lineaTrenModel))
                {
                    var linea = TrenesLineaEstadoDC.Current.Lineas.FirstOrDefault(x => x.Equals(lineaTrenModel));
                    if (linea != null)
                    {
                        linea.Estado = lineaTrenModel.Estado;
                    }
                }
                else
                {
                    TrenesLineaEstadoDC.Current.Lineas.InsertOnSubmit(lineaTrenModel);
                }

                foreach (var ramalTrenModel in ltm.Ramales.ConvertToTrenesRamalEstadoTable(lineaTrenModel.NickName))
                {
                    if (TrenesRamalEstadoDC.Current.Ramales.Contains(ramalTrenModel))
                    {
                        var ramal = TrenesRamalEstadoDC.Current.Ramales.FirstOrDefault(x => x.Equals(ramalTrenModel));
                        if (ramal != null)
                        {
                            ramal.Estado = ramalTrenModel.Estado;
                            ramal.MasInfo = ramalTrenModel.MasInfo;
                        }
                    }
                    else
                    {
                        TrenesRamalEstadoDC.Current.Ramales.InsertOnSubmit(ramalTrenModel);
                    }
                }
            }

            TrenesLineaEstadoDC.Current.SubmitChanges();
            TrenesRamalEstadoDC.Current.SubmitChanges();

            (App.Current as App).InitialDataTrenes = true;
            _datosLoaded = true;

            EndRequest();
        }
        
        private void EndRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = true;

                SystemTray.SetProgressIndicator(this, null);
            });
        }

        private void ShowErrorConnection()
        {
            //Luego le aviso al usuario que no se pudo cargar nueva información.
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
        }

        #endregion

        private void ButtonGo_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}