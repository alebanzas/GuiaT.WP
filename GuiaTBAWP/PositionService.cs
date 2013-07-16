using System;
using System.Device.Location;
using System.Windows;

namespace GuiaTBAWP
{
    public class PositionService
    {
        public static void Initialize()
        {
            if (App.Configuration.IsLocationEnabled)
            {
                if (Ubicacion == null)
                {
                    Ubicacion = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                    Ubicacion.StatusChanged += Ubicacion_StatusChanged;
                    Ubicacion.PositionChanged += Ubicacion_PositionChanged;
                    Ubicacion.MovementThreshold = 250;
                    Ubicacion.Start();
                }
                else
                {
                    Ubicacion.TryStart(true, TimeSpan.FromSeconds(4));
                }
            }
            else
            {
                MessageBox.Show("El servicio de localización se encuentra deshabilitado. Por favor asegúrese de habilitarlo en las Opciones de la aplicación para utilizar las caracteristicas que lo requeran.");
            }
        }

        public static void Destroy()
        {
            Ubicacion.Dispose();
        }

        private static GeoCoordinateWatcher Ubicacion { get; set; }
        private static void Ubicacion_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            App.Configuration.Ubicacion = e.Position;
        }

        private static void Ubicacion_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    MessageBox.Show(Ubicacion.Permission == GeoPositionPermission.Denied
                                        ? "El servicio de localización se encuentra deshabilitado. Por favor asegúrese de habilitarlo en las Opciones del dispositivo para utilizar las caracteristicas que lo requeran."
                                        : "El servicio de localización se encuentra sin funcionamiento.");
                    Ubicacion.Stop();
                    break;

                case GeoPositionStatus.Initializing: //Estado: Inicializando

                    break;

                case GeoPositionStatus.NoData: //Estado: Datos no disponibles
                    MessageBox.Show("El servicio de localización no puede obtener su posición.");
                    Ubicacion.Stop();
                    break;

                case GeoPositionStatus.Ready: //Estado: Servicio de localización disponible

                    break;
            }
        }

        public static GeoPosition<GeoCoordinate> GetCurrentLocation()
        {
            if (double.IsNaN(Ubicacion.Position.Location.Latitude) ||
                double.IsNaN(Ubicacion.Position.Location.Longitude))
            {
                return null;
            }

            return Ubicacion.Position;
        }
    }
}
