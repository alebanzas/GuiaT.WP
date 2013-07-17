using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Helpers;
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
            Loaded += (sender, args) =>
                {
                    var model = Config.Get<SubteStatusModel>() ?? new SubteStatusModel();
                    BindViewModel(model);

                    LoadData();
                };
        }
        
        public void LoadData()
        {
            ResetUI();
            if (!NetworkInterface.GetIsNetworkAvailable())
            {    
                ConnectionError.Visibility = Visibility.Visible;
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
                return;
            }
            
            if(ViewModel.Lineas.Count == 0) Loading.Visibility = Visibility.Visible;
            ProgressBar.Show("Obteniendo estado del servicio...");
            SetApplicationBarEnabled(false);
            
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
                ResetUI();
                Dispatcher.BeginInvoke(() => MessageBox.Show("Ocurrió un error al obtener el estado del servicio. Verifique su conexión a internet."));
            }
        }

        delegate void DelegateUpdateEstado(SubteStatusModel estado);
        private void UpdateEstadoServicio(SubteStatusModel model)
        {
            BindViewModel(model);

            ResetUI();

            Config.Set(model);
        }

        private void BindViewModel(SubteStatusModel model)
        {
            ViewModel.Lineas.Clear();
            foreach (var subteItemViewModel in model.Lineas)
            {
                ViewModel.AddLinea(new SubteItemViewModel
                    {
                        Detalles = subteItemViewModel.Detalles,
                        Nombre = subteItemViewModel.Nombre,
                    });
            }

            ViewModel.Actualizacion = string.Format("Ultima actualización: {0}", model.Actualizacion.ToLocalDateTime());
        }
        
        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}