using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace WebScraping
{
    public partial class Dolar : System.Web.UI.Page
    {
        public string Resultado { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Show();

        }
        
        protected void Show()
        {
            string result = string.Empty;

            for (int i = 1; i <= 80; i++)
            {
                HtmlNode html = new Scraper().GetNodes(new Uri(string.Format("http://www.dolarsi.com/cotizaciones_anteriores.asp?page={0}&zona=1&dia=1&mes=1&anio=2002&dia1=24&mes1=3&anio1=2013", i.ToString())));

                var cssSelect = html.CssSelect("table[width=400] tr");
                var count = cssSelect.Count();
                var script = cssSelect.Skip(2).Take(count - 3);

                foreach (var htmlNode in script)
                {
                    var datos = htmlNode.CssSelect("td");

                    var auxFecha = datos.ElementAt(0).CssSelect("div font").FirstOrDefault().InnerText.Split('/');
                    DateTime fecha = new DateTime(int.Parse(auxFecha[2]), int.Parse(auxFecha[1]), int.Parse(auxFecha[0]));
                    var fechasql = string.Format("{0}-{1}-{2}", auxFecha[2], auxFecha[1], auxFecha[0]);

                    var auxCompra = datos.ElementAt(1).CssSelect("div").FirstOrDefault().InnerText;
                    var compra = float.Parse(auxCompra.Replace(',','.'), NumberStyles.Any, CultureInfo.InvariantCulture);

                    var auxVenta = datos.ElementAt(2).CssSelect("div").FirstOrDefault().InnerText;
                    var venta = float.Parse(auxVenta.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture);

                    //result += string.Format("{0} | {1} | {2} <br>", fecha.ToLongDateString(), compra, venta);
                    result += string.Format("INSERT INTO [DOLAR_Historico] ([ID],[date],[valorCompra],[valorVenta],[tipoMoneda]) VALUES (NEWID(),'{0}',{1},{2},1) <br>", fechasql, compra.ToString(CultureInfo.InvariantCulture), venta.ToString(CultureInfo.InvariantCulture));
                }
            }

            

            Resultado = result;
        }

    }
}