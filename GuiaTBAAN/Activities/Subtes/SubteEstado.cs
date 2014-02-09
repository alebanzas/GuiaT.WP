using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using GuiaTBA.BLL.ViewModels;

namespace GuiaTBAAN.Activities.Subtes
{
    [Activity(Label = "SubteEstado", MainLauncher = true, Icon = "@drawable/icon")]
    public class SubteEstado : Activity
    {
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

