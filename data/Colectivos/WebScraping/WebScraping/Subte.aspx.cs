using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace WebScraping
{
    public partial class Subte : System.Web.UI.Page
    {
        public string Resultado { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Show();

        }
        
        protected void Show()
        {
            string result = string.Empty;

            HtmlNode html = new Scraper().GetNodes(new Uri("http://www.metrovias.com.ar/V2/InfoSubteSplash.asp"));

            string script = html.CssSelect("script").First().InnerText;

            var infos = script.Split(new[] { "if" }, StringSplitOptions.RemoveEmptyEntries)[0];

            foreach (var info in infos.Split(new[] { "pausecontent" }, StringSplitOptions.RemoveEmptyEntries).Skip(2))
            {
                var linea = info.Split(new[] { "] = '" }, StringSplitOptions.RemoveEmptyEntries);

                result += linea[1].Replace("';","") + "<br><br>";
            }

            Resultado = result;
        }

    }
}