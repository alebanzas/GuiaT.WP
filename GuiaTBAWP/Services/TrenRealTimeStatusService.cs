using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Threading;
using GuiaTBA.Common.Models;
using GuiaTBAWP.Commons.Extensions;
using GuiaTBAWP.Commons.Services;
using GuiaTBAWP.Extensions;

namespace GuiaTBAWP.Services
{
    public class TrenRealTimeStatusService
    {
        public TrenRealTimeStatusService(int seconds, string key)
        {
            SetRealTimeInterval(seconds, key);
        }

        private bool _datosLoaded;
        public bool DatosLoaded
        {
            get { return _datosLoaded || App.Configuration.UltimaActualizacionTrenes > DateTime.UtcNow.AddMinutes(-5).ToLocalTime(); }
            set { _datosLoaded = value; }
        }

        public WebRequest Request { get; set; }
        public bool Canceled { get; private set; }
        public Func<LiveTrenModel, LiveTrenModel> EndRequest { get; set; }
        private DispatcherTimer _dt = new DispatcherTimer();

        private void SetRealTimeInterval(int seconds, string key)
        {
            _dt = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(seconds),
            };
            _dt.Tick += (sender, args) => LoadData(key);
            _dt.Start(); 
            LoadData(key);
        }
        
        private void LoadData(string key)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
                return;
            }

            DatosLoaded = false;
            Canceled = false;
            if (Request != null) Request.Abort();
            var client = new HttpClient();
            var param = new Dictionary<string, object> {{"id", key}};
            Request = client.Get("/api/livetren".ToApiCallUri(param, true));
            Request.BeginGetResponse(HTTPWebRequestCallBack, Request);
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(LiveTrenModel));
                var o = (LiveTrenModel)serializer.ReadObject(stream);
                DatosLoaded = true;
                EndRequest(o);
            }
            catch (Exception ex)
            {
                if (!Canceled)
                {
                    ex.Log(false);
                }
            }
        }

        public void CancelRequest()
        {
            _dt.Stop();
            Canceled = true;
        }
    }
}
