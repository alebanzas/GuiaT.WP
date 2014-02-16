using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using GuiaTBAWP.Commons.Extensions;
using GuiaTBAWP.Commons.Services;
using GuiaTBA.WP7.Extensions;
using GuiaTBA.WP7.Models;

namespace GuiaTBA.WP7.Services
{
    public class TrenStatusService
    {
        private bool _datosLoaded;
        public bool DatosLoaded
        {
            get { return _datosLoaded || App.Configuration.UltimaActualizacionTrenes > DateTime.UtcNow.AddMinutes(-5).ToLocalTime(); }
            set { _datosLoaded = value; }
        }

        public WebRequest Request { get; set; }
        public bool Canceled { get; private set; }

        public Func<int> EndRequest { get; set; }
        public Func<int> StartRequest { get; set; }

        public bool LoadData()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
                return false;
            }

            StartRequest();
            DatosLoaded = false;
            Canceled = false;
            var client = new HttpClient();
            Request = client.Get("/api/tren".ToApiCallUri());
            Request.BeginGetResponse(HTTPWebRequestCallBack, Request);
            return true;
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

                Deployment.Current.Dispatcher.BeginInvoke(new DelegateUpdateWebBrowser(UpdateStatus), o);
            }
            catch (Exception ex)
            {
                if (!Canceled)
                {
                    ex.Log(EndRequest);
                }
            }
        }

        delegate void DelegateUpdateWebBrowser(TrenesStatusModel local);
        private void UpdateStatus(TrenesStatusModel model)
        {
            App.Configuration.UltimaActualizacionTrenes = model.Actualizacion;

            var trenService = new TrenDataService();
            trenService.UpdateStatus(model);

            DatosLoaded = true;

            EndRequest();
        }

        public void CancelRequest()
        {
            Canceled = true;
        }
    }
}
