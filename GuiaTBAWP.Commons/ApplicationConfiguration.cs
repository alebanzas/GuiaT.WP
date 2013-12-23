using System;
using System.Device.Location;
using GuiaTBAWP.Helpers;
using GuiaTBAWP.Models;
using System.Collections.Generic;
using GuiaTBAWP.Commons;
using GuiaTBAWP.Commons.Services;

namespace GuiaTBAWP
{
    public class ApplicationConfiguration
    {
        public ApplicationConfiguration()
        {
            Ubicacion = SetUbicacionDefault();
        }

        public ApplicationConfiguration(string name, string version)
        {
            Name = name;
            Version = version;
            Ubicacion = SetUbicacionDefault();
        }

        private static GeoPosition<GeoCoordinate> SetUbicacionDefault()
        {
            //obelisco de buenos aires
            return new GeoPosition<GeoCoordinate>(DateTimeOffset.UtcNow, new GeoCoordinate(-34.603722, -58.381594));
        }

        public void SetInitialConfiguration(string name, string version)
        {
            OpenCount++;
            Name = name;
            Version = version;

            if (IsInitialized) return;

            OpenCount = 1;
            Ubicacion = SetUbicacionDefault();
            InstallationId = Guid.NewGuid();
            IsLocationEnabledByPhone = true;
            IsLocationEnabledByAppConfig = true;
            IsInitialized = true;

            SetInitialData();

            Config.Set(this);
        }

        private void SetInitialData()
        {
            //Subte
            var subteStatusModel = new SubteStatusModel();
            subteStatusModel.Lineas.Add(new SubteStatusItem { Nombre = "Línea A", Detalles = "obteniendo información..."});
            subteStatusModel.Lineas.Add(new SubteStatusItem { Nombre = "Línea B", Detalles = "obteniendo información..."});
            subteStatusModel.Lineas.Add(new SubteStatusItem { Nombre = "Línea C", Detalles = "obteniendo información..."});
            subteStatusModel.Lineas.Add(new SubteStatusItem { Nombre = "Línea D", Detalles = "obteniendo información..."});
            subteStatusModel.Lineas.Add(new SubteStatusItem { Nombre = "Línea E", Detalles = "obteniendo información..."});
            subteStatusModel.Lineas.Add(new SubteStatusItem { Nombre = "Línea H", Detalles = "obteniendo información..."});
            subteStatusModel.Lineas.Add(new SubteStatusItem { Nombre = "Línea P", Detalles = "obteniendo información..."});
            subteStatusModel.Lineas.Add(new SubteStatusItem { Nombre = "Línea U", Detalles = "obteniendo información..."});
            subteStatusModel.Actualizacion = DateTime.UtcNow;
            
            Config.Set(subteStatusModel);


            //Trenes
            var trenesStatusModel = new TrenesStatusModel();
            trenesStatusModel.Lineas.Add(new LineaTrenModel { Estado = string.Empty, Nombre = "Belgrano Norte", Ramales = new List<RamalTrenModel> {
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Retiro-Villa Rosa" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Villa Rosa-Retiro" },
            } });
            trenesStatusModel.Lineas.Add(new LineaTrenModel { Estado = string.Empty, Nombre = "Belgrano Sur", Ramales = new List<RamalTrenModel> {
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Buenos Aires-González Catán" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "González Catán-Buenos Aires" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "M del Crucero Gral Belgrano-Bs As" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Bs As-M del Crucero Gral Belgrano" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Pte. Alsina-Aldo Bonzi" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Aldo Bonzi-Pte. Alsina" },
            } });
            trenesStatusModel.Lineas.Add(new LineaTrenModel { Estado = string.Empty, Nombre = "Gral. Roca", Ramales = new List<RamalTrenModel> {
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Pza. Constitución-Ezeiza" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Ezeiza-Pza. Constitución" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Pza. Constitución-Bosques" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Bosques-Pza. Constitución" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Pza. Constitución-Bosques/Gutiérrez" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Pza. Constitución-Claypole" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Pza. Constitución-Alejandro Korn" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Alejandro Korn-Pza. Constitución" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Temperley-Haedo" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Haedo-Temperley" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Pza. Constitución-La Plata" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "La Plata-Pza. Constitución" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Gutiérrez/Bosques-Pza. Constitución" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Claypole-Pza. Constitución" },
            } });
            trenesStatusModel.Lineas.Add(new LineaTrenModel { Estado = string.Empty, Nombre = "Mitre", Ramales = new List<RamalTrenModel> {
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Bartolomé Mitre- Retiro" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Retiro-Bartolomé Mitre" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "José León Suárez-Retiro" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Retiro-José León Suárez" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Tigre-Retiro" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Retiro-Tigre" },
            } });
            trenesStatusModel.Lineas.Add(new LineaTrenModel { Estado = string.Empty, Nombre = "Pto. Madero", Ramales = new List<RamalTrenModel> {
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Castelar-Puerto Madero" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Puerto Madero-Castelar" },
            } });
            trenesStatusModel.Lineas.Add(new LineaTrenModel { Estado = string.Empty, Nombre = "San Martín", Ramales = new List<RamalTrenModel> {
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Pilar-Retiro" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Retiro-Pilar" },
            } });
            trenesStatusModel.Lineas.Add(new LineaTrenModel { Estado = string.Empty, Nombre = "Sarmiento", Ramales = new List<RamalTrenModel> {
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Moreno-Once" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Once-Moreno" },
            } });
            trenesStatusModel.Lineas.Add(new LineaTrenModel { Estado = string.Empty, Nombre = "Urquiza", Ramales = new List<RamalTrenModel> {
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Federico Lacroze-Gral. Lemos" },
                new RamalTrenModel { MasInfo = "", Estado = "obteniendo información...", Nombre = "Gral. Lemos-Federico Lacroze" },
            } });
            trenesStatusModel.Actualizacion = DateTime.UtcNow;

            var trenService = new TrenDataService();
            trenService.UpdateStatus(trenesStatusModel);

        }

        /// <value>Registered ID used to access map control and Bing maps service.</value>
        public string BingMapApiKey { get { return "AgagZE2Ku0M0iPH8uolBeUSZUgHmGRrqbd-5etCjKym4dmTaH59yeS6Ka_kz_SDp"; } }

        public bool IsInitialized { get; set; }

        public bool IsLocationEnabled { get { return IsLocationEnabledByPhone && IsLocationEnabledByAppConfig; } }

        public bool IsLocationEnabledByPhone { get; set; }

        public bool IsLocationEnabledByAppConfig { get; set; }

        public Guid InstallationId { get; set; }

        private GeoPosition<GeoCoordinate> _ubicacion;
        public GeoPosition<GeoCoordinate> Ubicacion
        {
            get { return _ubicacion ?? (_ubicacion = new GeoPosition<GeoCoordinate>()); }
            set { _ubicacion = value; }
        }

        public double MinDiffGeography = 0.0001;

        public DateTime UltimaActualizacionBicicletas { get; set; }

        public bool InitialDataBicicletas { get; set; }

        public DateTime UltimaActualizacionTrenes { get; set; }

        public bool InitialDataTrenes { get; set; }

        private string _version;
        public string Version
        {
            private set
            {
                var v = value;
#if DEBUG
                v += "d";
#endif
                _version = v;
            }

            get { return _version; }
        }

        public string Name { get; private set; }

        public int OpenCount { get; set; }

        public string MobFoxID = "5e96acfbe5d7e4b856ec3a22c975aa63";

        public bool MobFoxInTestMode
        {
            get
            {
#if DEBUG
                return true;
#endif
                return false;
            }
        }

    }
}