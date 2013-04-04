using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace WebScraping
{
    public partial class Default : System.Web.UI.Page
    {
        public string Resultado { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Show();

        }
        
        protected void Show()
        {
            string result = string.Empty;

            HtmlNode html = new Scraper().GetNodes(new Uri("http://servicios.lanacion.com.ar/transito/"));

            var cssSelect = html.CssSelect("section.trenes");
            var script = cssSelect.CssSelect("nav ul li");
            
            List<LineaTrenModel> lineas = new List<LineaTrenModel>();

            foreach (var linea in script)
            {
                var classNumber = linea.GetAttributeValue("class");

                var aux = cssSelect.CssSelect("div.ramales ul." + classNumber);

                var ramales = aux.CssSelect("li");

                List<RamalTrenModel> r = new List<RamalTrenModel>();
                foreach (var ramal in ramales)
                {
                    /*
                     <li>
                     * Retiro-Villa Rosa<br>
                     * <a href="#" class="normal" alt="Normal" title="Normal">Normal</a>
                     * <span>
                     * <b class="color">Retiro-Villa Rosa</b>
                     * <br><b class="normal">Normal</b>
                     * <div class="separador"></div></span></li>
                     */

                    var ra = new RamalTrenModel();
                    ra.Nombre = ramal.InnerHtml.Split(new[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    ra.Estado = ramal.CssSelect("a").FirstOrDefault().InnerText;
                    ra.MasInfo = "";

                    r.Add(ra);
                }

                
                var l = new LineaTrenModel();
                l.Nombre = linea.CssSelect("a").FirstOrDefault().InnerText;
                l.Ramales = r;

                lineas.Add(l);
            }
            
            Resultado = result;
        }

    }
    
    public class LineaTrenModel
    {
        public string Nombre { get; set; }

        /// <summary>
        /// TODO: el peor de los estados de sus ramales
        /// </summary>
        public string Estado { get; set; }

        public List<RamalTrenModel> Ramales { get; set; }
    }

    public class RamalTrenModel
    {
        public string Nombre { get; set; }

        public string Estado { get; set; }

        public string MasInfo { get; set; }
    }
}