using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using GuiaTBA.BLL.ViewModels;
using GuiaTBAWP;
using GuiaTBAWP.Commons.Extensions;
using GuiaTBAWP.Commons.Helpers;
using GuiaTBAWP.Commons.Services;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using Microsoft.Phone.Shell;
using SubteBAWP.Extensions;

namespace SubteBAWP.Views.Subtes
{
    public partial class SubteEstado
    {
        WebRequest _httpReq;

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
            Unloaded += (sender, args) =>
                {
                    if(_httpReq != null)
                        _httpReq.Abort();
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

            var client = new HttpClient();
            _httpReq = client.Get("/api/subte".ToApiCallUri(alwaysRefresh: true));
            _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
        }

        private int ResetUI()
        {
            ConnectionError.Visibility = Visibility.Collapsed;
            Loading.Visibility = Visibility.Collapsed;
            ProgressBar.Hide();
            SetApplicationBarEnabled(true);
            return 0;
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
            catch (Exception ex)
            {
                ex.Log(ResetUI, () => { ConnectionError.Visibility = Visibility.Visible; return 0; });
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

            ViewModel.Actualizacion = string.Format("Actualizado hace {0}.", model.Actualizacion.ToUpdateDateTime());
        }
        
        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}