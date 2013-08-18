using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using GuiaTBAWP;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using GuiaTBAWP.ViewModels;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace TrenesBAWP
{
    public partial class Home
    {
        public Home()
        {
            InitializeComponent();
            
            // Set the data context of the listbox control to the sample data
            DataContext = ViewModel;
            Loaded += MainPage_Loaded;
            Unloaded += MainPage_Unloaded;
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            //if(_httpReq != null)
            //    _httpReq.Abort();
        }

        private static SubteStatusViewModel _viewModel = new SubteStatusViewModel();
        
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

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            TxtVersion.Text = string.Format("Versión {0}", App.Configuration.Version);

            ViewModel.Lineas.Clear();
            
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
                ProgressBar.Show(string.Format("{0} estado del servicio...", !App.Configuration.InitialDataTrenes ? "Obteniendo" : "Actualizando"));

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                    if (applicationBarIconButton != null)
                        applicationBarIconButton.IsEnabled = false;
                });

                _httpReq = WebRequest.Create("/trenes".ToApiCallUri());
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
                if (e.Status == WebExceptionStatus.RequestCanceled && App.Configuration.InitialDataTrenes)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(string.Format("La información del estado de servicio se actualizó por ultima vez el: {0}", ToLocalDateTime(App.Configuration.UltimaActualizacionTrenes))));
                }
                EndRequest();
            }
            catch (Exception ex)
            {
                EndRequest();
                //this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error.. " + ex.Message));
                Dispatcher.BeginInvoke(() => MessageBox.Show("Ocurrió un error al obtener el estado del servicio. Verifique su conexión a internet."));
            }
        }

        private static string ToLocalDateTime(DateTime dt)
        {
            return string.Format("{0} {1}", dt.ToLocalTime().ToShortDateString(), dt.ToLocalTime().ToShortTimeString());
        }

        delegate void DelegateUpdateWebBrowser(TrenesStatusModel local);
        private void UpdateStatus(TrenesStatusModel model)
        {
            App.Configuration.UltimaActualizacionTrenes = model.Actualizacion;

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

            App.Configuration.InitialDataTrenes = true;

            EndRequest();
        }

        private void EndRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = true;

                ProgressBar.Hide();
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

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void Acerca_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Acerca.xaml", UriKind.Relative));
        }

        private void RateReview_Click(object sender, EventArgs e)
        {
            var marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }

        private void FullApp_OnClick(object sender, EventArgs e)
        {
            //Show an application, using the default ContentType.
            var marketplaceDetailTask = new MarketplaceDetailTask();

            marketplaceDetailTask.ContentIdentifier = "98250785-9804-4439-af3f-63ef88c5998c";
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

            marketplaceDetailTask.Show();
        }
    }
}