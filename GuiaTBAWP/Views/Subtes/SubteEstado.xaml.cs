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
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Views.Subtes
{
    public partial class SubteEstado
    {
        private static SubteStatusViewModel _viewModel = new SubteStatusViewModel();
        public static SubteStatusViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new SubteStatusViewModel()); }
        }
        
        public SubteEstado()
        {
            InitializeComponent();
            
            DataContext = ViewModel;
            Loaded += (sender, args) => LoadData();
        }
        
        public void LoadData()
        {
            ResetUI();
            if (NetworkInterface.GetIsNetworkAvailable())
            {    
                ConnectionError.Visibility = Visibility.Visible;
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
                return;
            }
            
            Loading.Visibility = Visibility.Visible;
            ProgressBar.Show("Obteniendo estado del servicio...");
            SetApplicationBarEnabled(true);
            
            var httpReq = (HttpWebRequest)WebRequest.Create("/subte".ToApiCallUri(alwaysRefresh: true));
            httpReq.Method = "GET";
            httpReq.BeginGetResponse(HTTPWebRequestCallBack, httpReq);
        }

        private void ResetUI()
        {
            ConnectionError.Visibility = Visibility.Collapsed;
            Loading.Visibility = Visibility.Collapsed;
            ProgressBar.Hide();
            SetApplicationBarEnabled(true);
        }

        void SetApplicationBarEnabled(bool isEnabled)
        {
            var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (applicationBarIconButton != null)
                applicationBarIconButton.IsEnabled = isEnabled;
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
                Dispatcher.BeginInvoke(() => MessageBox.Show("Ocurrió un error al obtener el estado del servicio. Verifique su conexión a internet."));
            }
        }

        delegate void DelegateUpdateEstado(SubteStatusModel estado);
        private void UpdateEstadoServicio(SubteStatusModel model)
        {
            ViewModel.Actualizacion = model.ActualizacionStr;
            ViewModel.Lineas.Clear();

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
        
        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            ViewModel.Lineas.Clear();
            LoadData();
        }
    }
}