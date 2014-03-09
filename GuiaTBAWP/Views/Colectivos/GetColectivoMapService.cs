using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Media;
using GuiaTBA.Common;
using GuiaTBAWP.Commons.Extensions;
using GuiaTBAWP.Commons.Helpers;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Commons.Services;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBAWP.Extensions;
using Microsoft.Phone.Maps.Controls;

namespace GuiaTBAWP.Views.Colectivos
{
    public class GetColectivoMapService
    {
        private readonly Random _random = new Random();
        private HttpWebRequest _httpReq;
        private string _linea;
        public Func<int> SuccessFunc { get; set; }
        public Func<int> ErrorFunc { get; set; }
        
        public void GetColectivo(string linea)
        {
            _linea = linea;
            //Esta en file system
            if (Config.Get<List<TransporteViewModel>>("linea-" + _linea) != null)
            {
                UpdateList(Config.Get<List<TransporteViewModel>>("linea-" + _linea));
                return;
            }

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
                return;
            }

            GeoPosition<GeoCoordinate> currentLocation = PositionService.GetCurrentLocation();

            if (!App.Configuration.IsLocationEnabled)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar recorrido de colectivos, por favor, active la función de localización en la configuración de la aplicación."));
                return;
            }
            if (currentLocation == null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Para buscar recorrido de colectivos, por favor, active la función de localización."));
                return;
            }

            ProgressBar.Show(string.Format("Obteniendo recorrido línea {0}...", _linea));
            
            var param = new Dictionary<string, object>
            {
                {"linea", _linea},
                {"puntos", true},
            };

            var client = new HttpClient();
            _httpReq = client.Get("/api/transporte".ToApiCallUri(param));
            _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
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

                Deployment.Current.Dispatcher.BeginInvoke(new DelegateUpdateList(UpdateList), o);
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }

        private delegate void DelegateUpdateList(List<TransporteViewModel> local);

        private void UpdateList(List<TransporteViewModel> ls)
        {
            if (Config.Get<List<TransporteViewModel>>("linea-" + _linea) == null && ls.Any())
                Config.Set("linea-" + _linea, ls);

            ls = ls.OrderBy(y => y.Nombre).ToList();

            try
            {
                for (int index = 0; index < ls.Count; index++)
                {
                    TransporteViewModel transporteViewModel = ls[index];
                    var routeLine = new MapPolyline
                    {
                        Path = new GeoCoordinateCollection(),
                        StrokeColor = GetRandomColor(index),
                        StrokeThickness = 5.0,
                    };

                    foreach (var location in transporteViewModel.Puntos)
                    {
                        routeLine.Path.Add(new GeoCoordinate(location.Y, location.X));
                    }
                    App.MapViewModel.AddElement(new MapReference { Nombre = transporteViewModel.Nombre }, routeLine);
                }

                SuccessFunc();
            }
            catch
            {
                Config.Remove("linea-" + _linea);
                ErrorFunc();
            }
        }

        private Color GetRandomColor(int index)
        {
            var colors = new[] { 
                Colors.Red, 
                Colors.Blue, 
                Colors.Yellow,
                Colors.Orange, 
                Colors.Magenta, 
                Colors.Cyan, 
                ColorTranslator.FromHtml("#FF3C00"),
                ColorTranslator.FromHtml("#33FF00"),
                ColorTranslator.FromHtml("#FF0055")
            };

            if (index < colors.Length)
            {
                return colors[index];
            }

            var red = _random.Next(1, 175);
            var green = _random.Next(1, 175);
            var blue = _random.Next(1, 175);

            return Color.FromArgb(255, (byte)red, (byte)green, (byte)blue);
        }
    }
}