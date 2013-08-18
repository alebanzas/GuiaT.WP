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

    public class TipoTransporte
    {
        public Guid Id { get; set; }

    }

    public partial class Default : System.Web.UI.Page
    {
        public TipoTransporte Subte = new TipoTransporte
        {
            Id = new Guid("E74E932C-BF15-4ED6-ADA6-F0CBF0688B78"),
        };
        public TipoTransporte Colectivo = new TipoTransporte
        {
            Id = new Guid("8C9A672B-9103-47BF-A373-0648C0F10C5C"),
        };
        public TipoTransporte Tren = new TipoTransporte
        {
            Id = new Guid("440C21D3-71DE-4C94-849D-66139EADCE4C"),
        };

        public List<string> LineasSubte = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E",
                "H",
                "P",
            };

        public List<string> LineasTren = new List<string>
            {
                "roca",
                "sarmiento",
                "san-martin",
                "urquiza",
                "mitre",
                "belgrano-norte",
                "belgrano-sur",
            };

        /*public List<string> LineasColectivo = new List<string>
            {
                "1",
                "2",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10",
                "12",
                "15",
                "17",
                "19",
                "20",
                "21",
                "22",
                "23",
                "24",
                "25",
                "26",
                "28",
                "29",
                "32",
                "33",
                "34",
                "36",
                "37",
                "39",
                "41",
                "42",
                "44",
                "45",
                "46",
                "47",
                "49",
                "50",
                "51",
                "53",
                "55",
                "56",
                "57",
                "59",
                "60",
                "61",
                "62",
                "63",
                "64",
                "65",
                "67",
                "68",
                "70",
                "71",
                "74",
                "75",
                "76",
                "78",
                "79",
                "80",
                "84",
                "85",
                "86",
                "87",
                "88",
                "91",
                "92",
                "93",
                "95",
                "96",
                "97",
                "98",
                "99",
                "100",
                "101",
                "102",
                "103",
                "104",
                "105",
                "106",
                "107",
                "108",
                "109",
                "110",
                "111",
                "112",
                "113",
                "114",
                "115",
                "117",
                "118",
                "123",
                "124",
                "126",
                "127",
                "128",
                "129",
                "130",
                "132",
                "133",
                "134",
                "135",
                "136",
                "140",
                "141",
                "143",
                "146",
                "148",
                "150",
                "151",
                "152",
                "153",
                "154",
                "158",
                "159",
                "160",
                "161",
                "163",
                "165",
                "166",
                "168",
                "169",
                "172",
                "174",
                "175",
                "176",
                "177",
                "178",
                "179",
                "180",
                "181",
                "182",
                "184",
                "185",
                "188",
                "193",
                "194",
                "195"
            };*/

        //Update metrobus 9 de julio
        public List<string> LineasColectivo = new List<string>
            {
                "5",
                "6",
                "7",
                "9",
                "10",
                "17",
                "24",
                "26",
                "45",
                "53",
                "59",
                "67",
                "70",
                "75",
                "91",
                "98",
                "100",
                "111",
                "115",
                "129",
                "152",
                "195"
            };

        public List<string> LineasColectivoProv = new List<string>
            {
                "203",
"204A",
"204B",
"205", 
"218", 
"219", 
"228a",
"228b",
"228d",
"228e",
"228f",
"236",
"237",
"238",
"242",
"243",
"244",
"247",
"252",
"253",
"257",
"263",
"266",
"269",
"271",
"273",
"275",
"277",
"278",
"281",
"283",
"284",
"291",
"293",
"295",
"298",
"299",
"300",
"302",
"303",
"306",
"307",
"310",
"312",
"314",
"315",
"317",
"318",
"320",
"321",
"322",
"324",
"325",
"326",
"328",
"333",
"336",
"338",
"341",
"343",
"354",
"365",
"371",
"372",
"378",
"379",
"382",
"384",
"386",
"390",
"391",
"392",
"395",
"403",
"405",
"406",
"407",
"410",
"414",
"418",
"421",
"422",
"429",
"430",
"435",
"437",
"441",
"443",
"446",
"461",
"462",
"463",
"464",
"500"
            };

        public string Resultado { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ShowSubte();
            //ShowTren();
            
            //ShowSubte(true);
            //ShowTren(true);

            ShowColectivo(LineasColectivo);
            ShowColectivo(LineasColectivo, true);

            //ShowColectivo(LineasColectivoProv);
            //ShowColectivo(LineasColectivoProv, true);
        }

        protected void ShowSubte(bool regreso = false)
        {
            string result = string.Empty;

            foreach (var linea in LineasSubte)
            {
                HtmlNode html = new Scraper(Encoding.UTF7).GetNodes(new Uri("http://www.omnilineas.com.ar/buenos-aires/colectivo/linea-subte-" + linea + "/" + (regreso ? "&r=1" : "")));

                var ramales = html.CssSelect("ul").ToArray()[1].CssSelect("li").ToArray();

                string script = html.CssSelect("script").ToArray()[4].InnerText;

                var i = 0;
                foreach (var info in script.Split(new[] { "str2garr('" }, StringSplitOptions.RemoveEmptyEntries).Skip(1))
                {
                    string query = "INSERT INTO [GUIATBA_Transporte]" +
                                "([ID],[TipoTransporteID],[Nombre],[Codigo],[Ramal],[Ubicacion],[DescripcionRecorrido],[Regreso])" +
                                "VALUES (NEWID(),'{0}','Linea {1}','{1}','{2}','{3}','',{4})";

                    var item = info.Split(new[] { "')" }, StringSplitOptions.RemoveEmptyEntries);

                    result += string.Format(query, Subte.Id, linea, ramales[i].InnerText, GetLocationsFromString(item[0]), regreso ? "1" : "0") + "<br><br>";
                    i++;
                }
            }

            Resultado = result;
        }

        protected void ShowTren(bool regreso = false)
        {
            string result = string.Empty;

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            foreach (var linea in LineasTren)
            {
                HtmlNode html = new Scraper(Encoding.UTF7).GetNodes(new Uri("http://www.omnilineas.com.ar/buenos-aires/colectivo/linea-tren-" + linea + "/" + (regreso ? "&r=1" : "")));

                var ramales = html.CssSelect("ul").ToArray()[1].CssSelect("li").ToArray();

                string script = html.CssSelect("script").ToArray()[4].InnerText;

                var i = 0;
                foreach (var info in script.Split(new[] { "str2garr('" }, StringSplitOptions.RemoveEmptyEntries).Skip(1))
                {
                    string query = "INSERT INTO [GUIATBA_Transporte]" +
                                "([ID],[TipoTransporteID],[Nombre],[Codigo],[Ramal],[Ubicacion],[DescripcionRecorrido],[Regreso])" +
                                "VALUES (NEWID(),'{0}','Linea {1} {2}','{4}','{2}','{3}','',{5})";

                    var item = info.Split(new[] { "')" }, StringSplitOptions.RemoveEmptyEntries);

                    result += string.Format(query, Tren.Id, textInfo.ToTitleCase(linea), ramales[i].InnerText, GetLocationsFromString(item[0]), linea, regreso ? "1" : "0") + "<br><br>";
                    i++;
                }
            }

            Resultado = result;
        }

        protected void ShowColectivo(List<string> lineas, bool regreso = false)
        {
            string result = string.Empty;

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;


            Parallel.ForEach(lineas, linea =>
            {
                HtmlNode html = new Scraper(Encoding.UTF7).GetNodes(new Uri("http://www.omnilineas.com.ar/buenos-aires/colectivo/linea-" + linea + "/" + (regreso ? "&r=1" : "")));

                var ramales = html.CssSelect("ul").ToArray()[1].CssSelect("li").ToArray();

                string script = html.CssSelect("script").ToArray()[4].InnerText;

                var i = 0;
                foreach (var info in script.Split(new[] { "str2garr('" }, StringSplitOptions.RemoveEmptyEntries).Skip(1))
                {
                    string query = "INSERT INTO [GUIATBA_Transporte]" +
                                "([ID],[TipoTransporteID],[Nombre],[Codigo],[Ramal],[Ubicacion],[DescripcionRecorrido],[Regreso])" +
                                "VALUES (NEWID(),'{0}','Linea {1} {2}','{4}','{2}','{3}','',{5})";

                    var item = info.Split(new[] { "')" }, StringSplitOptions.RemoveEmptyEntries);

                    result += string.Format(query, Colectivo.Id, textInfo.ToTitleCase(linea), ramales[i].InnerText, GetLocationsFromString(item[0]), linea, regreso ? "1" : "0") + "<br><br>";
                    i++;
                }
            });

            Resultado = result;
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
        {			//decode polypoint of 18 chars, return array with lat and lng
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


            double reslat = rslat * (rablat + (double)rcdelat / 1000 + (double)rfghlat / 1000000);
            double reslng = rslng * (rablng + (double)rcdelng / 1000 + (double)rfghlng / 1000000);

            return string.Format("{0} {1}, <br>", reslng, reslat);
        }


    }

    public static class StringExtensions
    {
        public static IEnumerable<string> SplitByLength(this string str, int maxLength)
        {
            for (int index = 0; index < str.Length; index += maxLength)
            {
                yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
            }
        }

        /// <summary>
        /// Get the string slice between the two indexes.
        /// Inclusive for start index, exclusive for end index.
        /// </summary>
        public static string Slice(this string source, int start, int end)
        {
            if (end < 0) // Keep this for negative end support
            {
                end = source.Length + end;
            }
            int len = end - start;               // Calculate length
            return source.Substring(start, len); // Return Substring of length
        }
    }
}