using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace WebScraping
{
    public partial class Default : System.Web.UI.Page
    {

        public List<string> LineasBici = new List<string>
        {
            "ukra9kx2rzt",
            "l1ov51ds16",
            "line28",
            "lqd53i7qhph9f6",
            "br2629xk5ut052",
            "line3",
            "line4",
            "line5",
            "g4a8p7nhca24kj",
            "vuac457fario1o",
            "line8",
            "paan4nf24bmrhp",
            "line10",
            "line12",
            "line13",
            "line14",
            "line16",
            "line17",
            "line18",
            "line19",
            "line20",
            "line21",
            "line23",
            "line24",
            "line27",
            "line29",
            "line30",
            "line31",
            "line32",
            "line34",
            "line35",
            "line36",
            "line2",
            "wt81hktlbass8a",
            "hqg5lh277e6d2t",
            "l5czfnojg6k9y6",
            "gzhds5hhn2ricn",
            "o02viz25cint57",
            "f2mbcu6t70t3xr",
            "gdaj5hl4bg6faj",
            "76r9uudsa0mgqf",
            "2kayk99fvstf0f",
            "kn6chowks5hlg1",
            "oug3igdd8amm42",
            "d0jroybrwrn3ik",
            "4n0tgr1mos6pqf",
            "m0jt4qf0i4gw4s",
            "7ayjil8mcguba9",
            "hcwd4yhv0gpkfb",
            "0gslo6ywmcu6us",
            "line22",
            "roflqt6yg7ouso",
            "line25",
            "8txw8uzzl9sve7",
            "ow1xlgkoe6i27q",
            "g54xbisclu8zol",
            "ei4e6ww9m9yynw",
            "rhi1n3utnblqh0",
            "line11",
            "line9",
            "line2a",
            "line7",
            "line6",
            "hmrvisy9x4gf1o",
            "iy5t5mvojzzd7v",
            "ue0c7l06cxamj9",
        };

        public string Resultado { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ShowBici();
        }

        protected void ShowBici()
        {
            string result = string.Empty;

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            foreach (var linea in LineasBici)
            {
                BiciResult r = new JsonResult<BiciResult>().GetNodes(
                    new Uri("http://190.210.190.48/bikepath/route/?bikepath_code=" + linea));

                var initT = "lineas.Add(new BikeLine<br>" +
                            "            {<br>" +
                            "                Nombre = \"" + r.bikepath_name + "\",<br>" +
                            "                Puntos = new List&lt;PuntoViewModel&gt;<br>" +
                            "                {<br>";

                foreach (var route in r.route)
                {
                    initT += "                    new PuntoViewModel{ X = " + route.latitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".") + ", Y = " + route.longitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".") + "},<br>";
                }

                initT += "                }<br>" +
                        "            });<br>" +
                        "            <br>";
                
                result += initT;

            }

            Resultado = result;
        }

        public class BiciResult
        {
            public BiciResult()
            {
                route = new List<BiciRouteItem>();
            }

            public string bikepath_name { get; set; }
            public string bikepath_code { get; set; }
            public List<BiciRouteItem> route { get; set; }
        }

        public class BiciRouteItem
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
        }

        private string GetLocationsFromString(string line)
        {
            var result = "LINESTRING ( <br>";
            var stringPoints = line.SplitByLength(18);
            foreach (var stringPoint in stringPoints)
            {
                result += DecodePoint(stringPoint);
            }
            return result.Remove(result.Length - 7) + "<br> )";
        }

        public string DecodePoint(string pdat)
        {
            //decode polypoint of 18 chars, return array with lat and lng
            if (pdat.Length != 18) return string.Empty;
            var datlat = pdat.Slice(0, 9);
            var datlng = pdat.Slice(9, 18);

            var slat = datlat.Slice(0, 1);
            var slng = datlng.Slice(0, 1);
            var ablat = datlat.Slice(1, 3);
            var ablng = datlng.Slice(1, 3);
            var cdelat = datlat.Slice(3, 6);
            var cdelng = datlng.Slice(3, 6);
            var fghlat = datlat.Slice(6, 9);
            var fghlng = datlng.Slice(6, 9);

            var rslat = 1;
            if (slat == "k") rslat = -1;
            var rslng = 1;
            if (slng == "k") rslng = -1;

            var rablat = int.Parse(ablat, NumberStyles.HexNumber);
            var rablng = int.Parse(ablng, NumberStyles.HexNumber);

            var rcdelat = int.Parse(cdelat, NumberStyles.HexNumber);
            var rcdelng = int.Parse(cdelng, NumberStyles.HexNumber);

            var rfghlat = int.Parse(fghlat, NumberStyles.HexNumber);
            var rfghlng = int.Parse(fghlng, NumberStyles.HexNumber);


            double reslat = rslat*(rablat + (double) rcdelat/1000 + (double) rfghlat/1000000);
            double reslng = rslng*(rablng + (double) rcdelng/1000 + (double) rfghlng/1000000);

            return string.Format("{0} {1}, <br>", reslng, reslat);
        }


    }
}