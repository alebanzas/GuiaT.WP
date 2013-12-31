using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBAWP.Extensions;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Views.Colectivos
{
    public partial class Mapa : PhoneApplicationPage
    {
        Pushpin _posicionActual;

        public string Linea { get; set; }
        
        public Mapa()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                MiMapa.CredentialsProvider = new ApplicationIdCredentialsProvider(App.Configuration.BingMapApiKey);
                GetColectivo();
            };
            Unloaded += (s, e) => CancelarRequest();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string linea;
            if (NavigationContext.QueryString.TryGetValue("linea", out linea))
            {
                Linea = linea;
            }
        }

        private HttpWebRequest _httpReq;

        
        void SetApplicationBarEnabled(bool isEnabled)
        {
            var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (applicationBarIconButton != null)
                applicationBarIconButton.IsEnabled = isEnabled;
        }

        void GetColectivo()
        {
            ResetUI();
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
                return;
            }

            GeoPosition<GeoCoordinate> currentLocation = PositionService.GetCurrentLocation();

            if (!App.Configuration.IsLocationEnabled)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar recorrido de colectivos, por favor, active la función de localización en la configuración de la aplicación."));
                return;
            }
            if (currentLocation == null)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar recorrido de colectivos, por favor, active la función de localización."));
                return;
            }
            
            ProgressBar.Show(string.Format("Obteniendo recorrido línea {0}...", Linea));
            SetApplicationBarEnabled(false);
            CancelarRequest();

            var param = new Dictionary<string, object>
                {
                    //TODO: API con otros parametros
                    //{"lat", currentLocation.Location.Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")},
                    //{"lon", currentLocation.Location.Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")},
                    {"linea", Linea},
                    {"puntos", true},
                };

            var client = new HttpClient { HTTPWebRequestCallBack = HTTPWebRequestCallBack };
            var request = client.Get("/api/transporte".ToApiCallUri(param));

            request.BeginGetResponse(HTTPWebRequestCallBack, request);
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(List<TransporteViewModel>));
                var o = (List<TransporteViewModel>)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateList(UpdateList), o);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.RequestCanceled) return;

                Dispatcher.BeginInvoke(() =>
                {
                    ResetUI();
#if DEBUG
                        MessageBox.Show(ex.ToString());
#endif
                    MessageBox.Show("Ocurrió un error al obtener el recorrido. Verifique su conexión a internet.");
                });
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    ResetUI();
#if DEBUG
                        MessageBox.Show(ex.ToString());
#endif
                    MessageBox.Show("Ocurrió un error al obtener el recorrido. Verifique su conexión a internet.");
                });
            }
        }

        delegate void DelegateUpdateList(List<TransporteViewModel> local);
        private void UpdateList(List<TransporteViewModel> ls)
        {
            //Limpio el mapa, tomo lugares de la tabla local y los agrego al mapa
            MiMapa.Children.Clear();

            foreach (var transporteViewModel in ls)
            {
                var routeColor = GetRandomColor();
                var routeBrush = new SolidColorBrush(routeColor);

                var routeLine = new MapPolyline
                    {
                        Locations = new LocationCollection(),
                        Stroke = routeBrush,
                        Opacity = 0.65,
                        StrokeThickness = 5.0,
                    };

                foreach (var location in transporteViewModel.Puntos)
                {
                    routeLine.Locations.Add(new GeoCoordinate(location.Y, location.X));
                }

                MiMapa.Children.Add(routeLine);
            }

            //Si uso localizacion, agrego mi ubicación
            ActualizarUbicacion(App.Configuration.IsLocationEnabled ? App.Configuration.Ubicacion : null);

            var x = new List<GeoCoordinate>();
            //Ajusto el mapa para mostrar los items
            foreach (var child in MiMapa.Children)
            {
                var pushpin = child as Pushpin;
                if (pushpin != null)
                {
                    x.Add(pushpin.Location);
                }

                var line = child as MapPolyline;
                if (line != null)
                {
                    x.AddRange(line.Locations);
                }
            }

            MiMapa.SetView(LocationRect.CreateLocationRect(x));

            ResetUI();
        }

        private readonly Random _random = new Random();
        private Color GetRandomColor()
        {
            var red = _random.Next(1, 175);
            var green = _random.Next(1, 175);
            var blue = _random.Next(1, 175);

            return Color.FromArgb(255, (byte)red, (byte)green, (byte)blue);
        }

        private void CancelarRequest()
        {
            if (_httpReq != null)
                _httpReq.Abort();
        }

        private void ResetUI()
        {
            ProgressBar.Hide();
            SetApplicationBarEnabled(true);
        }

        
        private void ActualizarUbicacion(GeoPosition<GeoCoordinate> location)
        {
            MiMapa.Children.Remove(_posicionActual);
            if (location == null || location.Location.IsUnknown) return;

            _posicionActual = new Pushpin
                {
                    Location = location.Location,
                    Template = (ControlTemplate) (Application.Current.Resources["locationPushpinTemplate"])
                };
            MiMapa.Children.Add(_posicionActual);
        }
        
        private void BtnAcercar_Click(object sender, EventArgs e)
        {
            MiMapa.ZoomLevel++;
        }

        private void BtnAlejar_Click(object sender, EventArgs e)
        {
            MiMapa.ZoomLevel--;
        }

        private void BtnVista_Click(object sender, EventArgs e)
        {
            if (MiMapa.Mode is RoadMode)
                MiMapa.Mode = new AerialMode();
            else
                MiMapa.Mode = new RoadMode();
        }

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

    }

    internal class HttpClient
    {
        public AsyncCallback HTTPWebRequestCallBack { get; set; }

        private string appKey = "8463adb194f44436a046ac36229f1571";
        private string appSecret = "6855502326fe42f2b8ff63a9cbba52c8";

        public HttpWebRequest Get(Uri requestUrl)
        {
            return SendRequestGetResponse(requestUrl);
        }

        private HttpWebRequest SendRequestGetResponse(Uri requestUrl)
        {
            var request = (HttpWebRequest)WebRequest.Create(requestUrl);

            request.Method = "GET";
            //request.ContentType = "application/json";
            //http://www.w3.org/Protocols/rfc2616/rfc2616-sec3.html#sec3.3.1
            request.Headers["X-ABS-Date"] = DateTime.UtcNow.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture);
            
            var signature = GetSignature(appSecret, request);
            request.Headers[HttpRequestHeader.Authorization] = string.Format("{0} {1}", "ABS-H", string.Format("{0}:{1}", appKey, signature));

            return request;
        }

        private string GetSignature(string secret, HttpWebRequest request)
        {
            string canonicalizedMessage = string.Join("\n", GetCanonicalParts(request));
            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(canonicalizedMessage))
            {
                return "";
            }
            byte[] secretBytes = Encoding.UTF8.GetBytes(secret);
            byte[] valueBytes = Encoding.UTF8.GetBytes(canonicalizedMessage);
            string signature;

            using (var hmac = new HMACSHA256(secretBytes))
            {
                byte[] hash = hmac.ComputeHash(valueBytes);
                signature = Convert.ToBase64String(hash);
            }
            return signature;
        }

        private string[] GetCanonicalParts(HttpWebRequest request)
        {
            var result = new List<string>
                {
                    request.Method
                };

            var httpRequestHeaders = request.Headers;
            result.Add(httpRequestHeaders[HttpRequestHeader.ContentMd5]);
            result.Add(httpRequestHeaders[HttpRequestHeader.ContentType] ?? string.Empty);
            result.Add(httpRequestHeaders[HttpRequestHeader.Date] ?? string.Empty);

            foreach (var key in httpRequestHeaders.AllKeys)
            {
                if (!key.StartsWith("X-ABS", StringComparison.InvariantCultureIgnoreCase))
                    continue;
                result.Add(string.Format("{0}:{1}", key.ToLowerInvariant().Trim(), httpRequestHeaders[key]));
            }

            result.Add(request.RequestUri.LocalPath + request.RequestUri.Query);

            return result.ToArray();
        }

        //public string ContentMd5HashForHeader(string input)
        //{
        //    // http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.15
        //    MD5 md5 = MD5.Create();
        //    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        //    byte[] hashBytes = md5.ComputeHash(inputBytes);
        //
        //    return Convert.ToBase64String(hashBytes);
        //}

    }
}