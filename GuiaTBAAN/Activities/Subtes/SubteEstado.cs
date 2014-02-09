using System.Collections.Generic;
using Android.App;
using Android.OS;
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

            list.Adapter = new ArrayAdapter<SubteItemViewModel>(this, Android.Resource.Layout.SimpleListItem1, items);

        }
    }
}

