using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using GuiaTBAWP.Commons.Extensions;
using GuiaTBAWP.Commons.Services;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBA.WP7.Extensions;
using Microsoft.Phone.Controls;

namespace GuiaTBA.WP7.Views.Ruta
{
    public partial class Home : PhoneApplicationPage
    {
        WebRequest _httpReq;

        private static SubteStatusViewModel _viewModel = new SubteStatusViewModel();
        public static SubteStatusViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new SubteStatusViewModel()); }
        }
        
        public Home()
        {
            InitializeComponent();
            
            DataContext = ViewModel;
            Loaded += (sender, args) => { };
            Unloaded += (sender, args) =>
                {
                    if(_httpReq != null)
                        _httpReq.Abort();
                };
            
            StatusChecker.Check("HomeRuta");
        }

        public void Buscar()
        {
            var param = new Dictionary<string, object>
                {
                    {"id", TxtBuscar.Text},
                };

            var client = new HttpClient();
            _httpReq = client.Get("/api/geocoder".ToApiCallUri(param));
            _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(List<GeocoderResult>));
                var o = (List<GeocoderResult>)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdate(UpdateSugerencias), o);
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        delegate void DelegateUpdate(List<GeocoderResult> result);
        private void UpdateSugerencias(List<GeocoderResult> model)
        {
            ViewModel.Lineas.Clear();
            foreach (var geocoderResult in model)
            {
                ViewModel.AddLinea(new SubteItemViewModel
                {
                    Nombre = geocoderResult.Nombre, 
                    Detalles = string.Format("X:{0}, Y:{1}", geocoderResult.X, geocoderResult.Y),
                });   
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Buscar();
        }
    }

    public class GeocoderResult
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Nombre { get; set; }
    }
}