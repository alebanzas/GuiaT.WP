using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using GuiaTBAWP.Commons.Services;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;

namespace GuiaTBAWP.Services
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

        public Func<int> EndRequest { get; set; }
        public Func<int> StartRequest { get; set; }

        public bool LoadData()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                StartRequest();
                DatosLoaded = false;
                Request = WebRequest.Create("/trenes".ToApiCallUri());
                Request.Method = "GET";
                Request.BeginGetResponse(HTTPWebRequestCallBack, Request);
                return true;
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
                return false;
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

                Deployment.Current.Dispatcher.BeginInvoke(new DelegateUpdateWebBrowser(UpdateStatus), o);
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.RequestCanceled)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(string.Format("La información del estado de servicio se actualizó por ultima vez el: {0}", App.Configuration.UltimaActualizacionTrenes.ToLocalDateTime())));
                }
                EndRequest();
            }
            catch (Exception ex)
            {
                EndRequest();
                //this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error.. " + ex.Message));
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ocurrió un error al obtener el estado del servicio. Verifique su conexión a internet."));
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
    }
}
