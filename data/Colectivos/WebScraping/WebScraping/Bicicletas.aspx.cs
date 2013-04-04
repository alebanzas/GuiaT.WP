using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace WebScraping
{
    public partial class Bicicletas : System.Web.UI.Page
    {
        public string Resultado { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Show();

        }
        
        protected void Show()
        {
            string result = string.Empty;

            HtmlNode html = new Scraper().GetNodes(new Uri("http://www.bicicletapublica.com.ar/mapa.aspx"));

            var cssSelect = html.CssSelect("script");
            var script = cssSelect.Skip(1).FirstOrDefault().InnerText;
            
            foreach (var posta in script.Split(new[] { "new GLatLng(" }, StringSplitOptions.RemoveEmptyEntries).Skip(2))
            {
                var a = posta.Split(new[] {"openInfoWindowHtml('"}, StringSplitOptions.RemoveEmptyEntries);

                //-34.592308,-58.37501
                string arg0 = a[0].Split(new[] { ")," }, StringSplitOptions.RemoveEmptyEntries)[0];

                var lat = double.Parse(arg0.Split(',')[0].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture);
                var lon = double.Parse(arg0.Split(',')[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture);
                
                //<div style="height:100px;"><span class="style1">RETIRO
                //<br>Cerrado. Horario de atención: Lun a Vie de 8 a 20. Sáb 9 a 15.</span>
                //<br><span class="style2">Cant. Bicicletas disponibles: 8</span><br></div>
                string arg1 = a[1].Split(new[] { "'," }, StringSplitOptions.RemoveEmptyEntries)[0];

                var arg2 = arg1.Split(new[] {"<br>"}, StringSplitOptions.RemoveEmptyEntries);

                var nombre = arg2[0].Split('>')[2].Trim();

                var estado = arg2[1].Split('.')[0].Trim();

                var horario = arg2[1].Split(':')[1].Split('<')[0].Trim();

                var cantidad = int.Parse(arg2[2].Split(':')[1].Split('<')[0].Trim());
                
                var estacion = new BicicletaEstacion();
                estacion.Latitud = lat;
                estacion.Longitud = lon;
                estacion.Nombre = nombre;
                estacion.Estado = estado;
                estacion.Horario = horario;
                estacion.Cantidad = cantidad;


                result += string.Format("{0} <br><br> {1} <br><br> {2} <br>", arg0, arg1, string.Empty);
            }
            
            Resultado = result;
        }

    }

    public class BicicletaEstacion
    {
        public double Latitud { get; set; }

        public double Longitud { get; set; }

        public string Nombre { get; set; }

        public string Estado { get; set; }

        public string Horario { get; set; }

        public int Cantidad { get; set; }
    }
}