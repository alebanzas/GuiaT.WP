using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using GuiaTBA.BLL.Models;
using GuiaTBA.BLL.ViewModels;
using GuiaTBAAN.Extensions;
using GuiaTBAAN.Services;
using Newtonsoft.Json;

namespace GuiaTBAAN.Activities.Subtes
{
    [Activity(Label = "SubteEstado", MainLauncher = true, Icon = "@drawable/icon")]
    public class SubteEstado : Activity
    {
        WebRequest _httpReq;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.SubteEstado);

            var list = FindViewById<ListView>(Resource.Id.listView);
            var items = new List<SubteItemViewModel>
            {
                new SubteItemViewModel {Nombre = "Línea A", Detalles = "obteniendo información..."},
                new SubteItemViewModel {Nombre = "Línea B", Detalles = "obteniendo información..."},
                new SubteItemViewModel {Nombre = "Línea C", Detalles = "obteniendo información..."},
                new SubteItemViewModel {Nombre = "Línea D", Detalles = "obteniendo información..."},
                new SubteItemViewModel {Nombre = "Línea E", Detalles = "obteniendo información..."},
                new SubteItemViewModel {Nombre = "Línea H", Detalles = "obteniendo información..."},
                new SubteItemViewModel {Nombre = "Línea P", Detalles = "obteniendo información..."}
            };

            list.Adapter = new SubteEstadoAdapter(this, items);

            LoadData();
        }

        public void LoadData()
        {
            //ResetUI();
            //if (!NetworkInterface.GetIsNetworkAvailable())
            //{
            //    ConnectionError.Visibility = Visibility.Visible;
            //    Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
            //    return;
            //}
            //
            //if (ViewModel.Lineas.Count == 0) Loading.Visibility = Visibility.Visible;
            //ProgressBar.Show("Obteniendo estado del servicio...");
            //SetApplicationBarEnabled(false);

            var client = new HttpClient();
            _httpReq = client.Get("/api/subte".ToApiCallUri(alwaysRefresh: true));
            _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var r = new SubteStatusModel();

                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    var str = stream.ReadToEnd();

                    r = JsonConvert.DeserializeObject<SubteStatusModel>(str);
                }


            }
            catch (Exception ex)
            {
                //ex.Log(ResetUI, () => { ConnectionError.Visibility = Visibility.Visible; return 0; });
            }
        }
    }

    public class SubteEstadoAdapter : BaseAdapter<SubteItemViewModel>
    {
        List<SubteItemViewModel> items;
        Activity context;
        public SubteEstadoAdapter(Activity context, List<SubteItemViewModel> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override SubteItemViewModel this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];

            View view = convertView ?? context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.Nombre;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = item.Detalles;
            //TODO: agregar imagen

            return view;
        }
    }
}

