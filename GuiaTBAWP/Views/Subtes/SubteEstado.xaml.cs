using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using GuiaTBAWP.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Views.Subtes
{
    public partial class SubteEstado : PhoneApplicationPage
    {
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
        
        public SubteEstado()
        {
            ViewModel.Lineas.Clear();

            InitializeComponent();
            
            // Set the data context of the listbox control to the sample data
            DataContext = ViewModel;
            Loaded += MainPage_Loaded;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }


        #region DataInit


        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                Loading.Visibility = Visibility.Visible;
                ConnectionError.Visibility = Visibility.Collapsed;
                ProgressBar.Show("Obteniendo estado del servicio...");

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                    if (applicationBarIconButton != null)
                        applicationBarIconButton.IsEnabled = false;
                });

                var httpReq = (HttpWebRequest)WebRequest.Create("/subte".ToApiCallUri());
                httpReq.Method = "GET";
                httpReq.BeginGetResponse(HTTPWebRequestCallBack, httpReq);
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

                var serializer = new DataContractJsonSerializer(typeof(SubteStatusModel));
                var o = (SubteStatusModel)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateEstado(UpdateEstadoServicio), o);
            }
            catch (Exception)
            {
                EndRequest();
                //this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error.. " + ex.Message));
                Dispatcher.BeginInvoke(() => MessageBox.Show("Ocurrió un error al obtener el estado del servicio. Verifique su conexión a internet."));
            }
        }

        delegate void DelegateUpdateEstado(SubteStatusModel estado);
        private void UpdateEstadoServicio(SubteStatusModel model)
        {
            var result = new Collection<SubteItemViewModel>();

            foreach (var item in model.Lineas)
            {
                result.Add(new SubteItemViewModel
                {
                    Nombre = item.Nombre,
                    Detalles = item.Detalles,
                });
            }

            BindViewModel(result);

            EndRequest();
        }

        private void BindViewModel(IEnumerable<SubteItemViewModel> result)
        {
            _viewModel.Lineas.Clear();
            foreach (var subteItemViewModel in result)
            {
                _viewModel.AddLinea(new SubteItemViewModel
                    {
                        Detalles = subteItemViewModel.Detalles,
                        Nombre = subteItemViewModel.Nombre,
                    });
            }

            _viewModel.Actualizacion = DateTime.UtcNow.ToShortTimeString();

        }

        private void EndRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = true;

                Loading.Visibility = Visibility.Collapsed;
                ProgressBar.Hide();
            });
        }

        private void ShowErrorConnection()
        {
            //Luego le aviso al usuario que no se pudo cargar nueva información.
            ViewModel.Lineas.Clear();
            ConnectionError.Visibility = Visibility.Visible;
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
        }

        #endregion

        private void ButtonGo_Click(object sender, EventArgs e)
        {
            ViewModel.Lineas.Clear();
            LoadData();
        }
    }
}