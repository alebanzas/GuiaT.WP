using System;
using System.Device.Location;
using GuiaTBAWP.Helpers;

namespace GuiaTBAWP
{
    public class ApplicationConfiguration
    {
        public ApplicationConfiguration()
        {
            Ubicacion = new GeoPosition<GeoCoordinate>();
        }

        public void SetInitialConfiguration()
        {
            if (IsInitialized) return;

            InstallationId = Guid.NewGuid();
            IsLocationEnabled = true;
            IsInitialized = true;

            Config.Set(this);
        }

        /// <value>Registered ID used to access map control and Bing maps service.</value>
        public string BingMapApiKey { get { return "AgagZE2Ku0M0iPH8uolBeUSZUgHmGRrqbd-5etCjKym4dmTaH59yeS6Ka_kz_SDp"; } }

        public bool IsInitialized { get; set; }

        public bool IsLocationEnabled { get; set; }

        public Guid InstallationId { get; set; }

        public GeoPosition<GeoCoordinate> Ubicacion { get; set; }

        public double MinDiffGeography = 0.0001;

        public DateTime UltimaActualizacionBicicletas { get; set; }

        public bool InitialDataBicicletas { get; set; }

        public DateTime UltimaActualizacionTrenes { get; set; }

        public bool InitialDataTrenes { get; set; }

        public string Version
        {
            get
            {
                var v = "1.4.0.0";
#if DEBUG
                v += "d";
#endif
                return v;
            }
        }

        public string Name
        {
            get
            {
                return "GUIATBAWP";
            }
        }
    }
}