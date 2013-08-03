using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace WebScraping
{
    public partial class Colectivos : System.Web.UI.Page
    {
        public string Resultado { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Show();

        }

        public RangosCategorias rangos = new RangosCategorias();





        protected void Show()
        {
            string result = string.Empty;

            HtmlNode html = new Scraper().GetNodes(new Uri("http://www.transporte.gob.ar/content/servicios-colectivos/"));

            IEnumerable<HtmlNode> grupos = html.CssSelect("div.recuadro_titularV2");

            foreach (var grupo in grupos)
            {
                IEnumerable<HtmlNode> lineas = grupo.CssSelect("div.content_title");
                foreach (var linea in lineas)
                {
                    try
                    {
                        string titulo = linea.CssSelect("a").FirstOrDefault().InnerHtml;
                        HtmlNode node = linea.SelectNodes("following-sibling::*").TakeWhile(tag => tag.Name == "div" && tag.Attributes["class"].Value == "content_box").FirstOrDefault();
                        string descripcion = node.InnerHtml.Replace("\"", "'");

                        var d1 = new List<string>();

                        



                        var rango = RangosCategorias.GetRango(int.Parse(titulo.Split(' ')[1]
                            .Replace("A","")
                            .Replace("B", "")
                            .Replace("C", "")
                            .Replace("D", "")));

                        result += "buss.Add(new Bus(\"Línea " + rango.Item1 + " a " + rango.Item2 + "\")";
                        result += "<br>";
                        result += "{";
                        result += "<br>";
                        result += "    Title = \"" + titulo + "\",";
                        result += "<br>";
                        //result += "    Description = \"" + System.Web.HttpUtility.HtmlDecode(descripcion) + "\"";
                        var desc = d1.ToArray();
                        for (int i = 0; i < desc.Length; i++)
                        {
                            result += "    Description" + i + " = @\"" + desc[i] + "\"";
                        }
                        result += "<br>";
                        result += "});";

                        result += "<br><br>";
                    }
                    catch (Exception e)
                    {
                        result +=
                            "<br><br>############################################################################################";
                        result += "<br>" + e.Message;
                        result +=
                            "<br><br>############################################################################################";
                    }
                }
                result += "<br><br>############################################################################################";
            }

            Resultado = result;
        }

    }

    public class RangosCategorias
    {
        public static IList<Tuple<int, int>> rangos = new Collection<Tuple<int, int>>();

        public RangosCategorias()
        {
            rangos.Add(new Tuple<int,int>(1, 10));
            rangos.Add(new Tuple<int,int>(11, 20));
            rangos.Add(new Tuple<int,int>(21, 30));
            rangos.Add(new Tuple<int,int>(31, 40));
            rangos.Add(new Tuple<int,int>(41, 50));
            rangos.Add(new Tuple<int,int>(51, 60));
            rangos.Add(new Tuple<int,int>(61, 70));
            rangos.Add(new Tuple<int,int>(71, 80));
            rangos.Add(new Tuple<int,int>(81, 90));
            rangos.Add(new Tuple<int,int>(91, 100));
            rangos.Add(new Tuple<int,int>(101, 110));
            rangos.Add(new Tuple<int,int>(111, 120));
            rangos.Add(new Tuple<int,int>(121, 130));
            rangos.Add(new Tuple<int,int>(131, 140));
            rangos.Add(new Tuple<int,int>(141, 150));
            rangos.Add(new Tuple<int,int>(151, 160));
            rangos.Add(new Tuple<int,int>(161, 170));
            rangos.Add(new Tuple<int,int>(171, 180));
            rangos.Add(new Tuple<int,int>(181, 190));
            rangos.Add(new Tuple<int,int>(191, 200));
            rangos.Add(new Tuple<int,int>(201, 210));
            rangos.Add(new Tuple<int,int>(211, 220));
            rangos.Add(new Tuple<int,int>(221, 230));
            rangos.Add(new Tuple<int,int>(231, 240));
            rangos.Add(new Tuple<int,int>(241, 250));
            rangos.Add(new Tuple<int,int>(251, 260));
            rangos.Add(new Tuple<int,int>(261, 270));
            rangos.Add(new Tuple<int,int>(271, 280));
            rangos.Add(new Tuple<int,int>(281, 290));
            rangos.Add(new Tuple<int,int>(291, 300));
            rangos.Add(new Tuple<int,int>(301, 310));
            rangos.Add(new Tuple<int,int>(311, 320));
            rangos.Add(new Tuple<int,int>(321, 330));
            rangos.Add(new Tuple<int,int>(331, 340));
            rangos.Add(new Tuple<int,int>(341, 350));
            rangos.Add(new Tuple<int,int>(351, 360));
            rangos.Add(new Tuple<int,int>(361, 370));
            rangos.Add(new Tuple<int,int>(371, 380));
            rangos.Add(new Tuple<int,int>(381, 390));
            rangos.Add(new Tuple<int,int>(391, 400));
            rangos.Add(new Tuple<int,int>(401, 410));
            rangos.Add(new Tuple<int,int>(411, 420));
            rangos.Add(new Tuple<int,int>(421, 430));
            rangos.Add(new Tuple<int,int>(431, 440));
            rangos.Add(new Tuple<int,int>(441, 450));
            rangos.Add(new Tuple<int,int>(451, 460));
            rangos.Add(new Tuple<int,int>(461, 470));
            rangos.Add(new Tuple<int,int>(471, 480));
            rangos.Add(new Tuple<int,int>(481, 490));
            rangos.Add(new Tuple<int,int>(491, 500));
            rangos.Add(new Tuple<int,int>(501, 510));
            rangos.Add(new Tuple<int,int>(511, 520));
            rangos.Add(new Tuple<int,int>(521, 530));
            rangos.Add(new Tuple<int,int>(531, 540));
            rangos.Add(new Tuple<int,int>(541, 550));
            rangos.Add(new Tuple<int,int>(551, 560));
            rangos.Add(new Tuple<int,int>(561, 570));
            rangos.Add(new Tuple<int,int>(571, 580));
            rangos.Add(new Tuple<int,int>(581, 590));
            rangos.Add(new Tuple<int,int>(591, 600));
            rangos.Add(new Tuple<int,int>(601, 610));
            rangos.Add(new Tuple<int,int>(611, 620));
            rangos.Add(new Tuple<int,int>(621, 630));
            rangos.Add(new Tuple<int,int>(631, 640));
            rangos.Add(new Tuple<int,int>(641, 650));
            rangos.Add(new Tuple<int,int>(651, 660));
            rangos.Add(new Tuple<int,int>(661, 670));
            rangos.Add(new Tuple<int,int>(671, 680));
            rangos.Add(new Tuple<int,int>(681, 690));
            rangos.Add(new Tuple<int,int>(691, 700));
            rangos.Add(new Tuple<int,int>(701, 710));
            rangos.Add(new Tuple<int,int>(711, 720));
            rangos.Add(new Tuple<int,int>(721, 730));
            rangos.Add(new Tuple<int,int>(731, 740));
            rangos.Add(new Tuple<int,int>(741, 750));
            rangos.Add(new Tuple<int,int>(751, 760));
            rangos.Add(new Tuple<int,int>(761, 770));
            rangos.Add(new Tuple<int,int>(771, 780));
            rangos.Add(new Tuple<int,int>(781, 790));
            rangos.Add(new Tuple<int,int>(791, 800));
            rangos.Add(new Tuple<int,int>(801, 810));
            rangos.Add(new Tuple<int,int>(811, 820));
            rangos.Add(new Tuple<int,int>(821, 830));
            rangos.Add(new Tuple<int,int>(831, 840));
            rangos.Add(new Tuple<int,int>(841, 850));
            rangos.Add(new Tuple<int,int>(851, 860));
            rangos.Add(new Tuple<int,int>(861, 870));
            rangos.Add(new Tuple<int,int>(871, 880));
            rangos.Add(new Tuple<int,int>(881, 890));
            rangos.Add(new Tuple<int,int>(891, 900));
            rangos.Add(new Tuple<int,int>(901, 910));
            rangos.Add(new Tuple<int,int>(911, 920));
            rangos.Add(new Tuple<int,int>(921, 930));
            rangos.Add(new Tuple<int,int>(931, 940));
            rangos.Add(new Tuple<int,int>(941, 950));
            rangos.Add(new Tuple<int,int>(951, 960));
            rangos.Add(new Tuple<int,int>(961, 970));
            rangos.Add(new Tuple<int,int>(971, 980));
            rangos.Add(new Tuple<int,int>(981, 990));
        }

        public static Tuple<int, int> GetRango(int valor)
        {
            foreach (var rango in rangos.Where(rango => rango.Item1 <= valor && valor <= rango.Item2))
            {
                return new Tuple<int, int>(rango.Item1, rango.Item2);
            }
            return new Tuple<int, int>(0,0);
        }
    }
}