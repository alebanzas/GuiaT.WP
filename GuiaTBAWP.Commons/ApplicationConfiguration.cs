using System;
using System.Data.Linq;
using System.Device.Location;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Commons.ViewModels;
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

            RadioTaxiDC.Destroy();
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("eee11015-d3aa-4f34-9ac4-223149a7d254"), Nombre = "24 Horas", Telefono = "4523-2222", Url = "http://www.radiotaxi24.com.ar/" });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("195911c6-8706-4f8b-a413-abf98a3e2810"), Nombre = "Ale", Telefono = "4983-3800", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("b421a7dc-af5a-489b-bc6d-1bea96b7c901"), Nombre = "Aló", Telefono = "4855-5555", Url = "http://www.radiotaxisalo.com.ar/" });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("57e78006-2eaa-4c06-b61e-66d2fca59245"), Nombre = "Amistax", Telefono = "4582-7774", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("09e8d3a9-d17a-4ccb-a28b-6c3d8417d450"), Nombre = "Buen Viaje", Telefono = "5252-9999", Url = "http://www.radiotaxibuenviaje.com/" });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("2560026b-1afa-489f-a4af-88e46f9c67aa"), Nombre = "Ciber Taxi", Telefono = "4921-6006", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("568164f1-d1e7-449d-b74f-80210d8d0c35"), Nombre = "City Tax", Telefono = "4585-5544", Url = "http://www.citytax.com.ar/" });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("5665d408-1100-4f9d-a455-2d8d40e08815"), Nombre = "Ciudad", Telefono = "4923-7007", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("1965e1c2-db4a-4ea7-a8c1-3e1449b0f79d"), Nombre = "Del Plata", Telefono = "4504-7776", Url = "http://www.delplataradiotaxi.com/" });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("4ddf21e9-71e3-4037-89f0-8a11aed59de4"), Nombre = "Diez", Telefono = "4585-5007", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("b019967d-249f-40e0-9082-f6058e5f81a8"), Nombre = "El", Telefono = "4922-9999", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("2a6a1d88-5ec7-4ca2-a524-0d0e5d5f8add"), Nombre = "Gold", Telefono = "4305-5050", Url = "http://www.radiotaxigoldsrl.com.ar/" });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("931d96ac-bbcd-4a2b-bdfa-e3a6662b1708"), Nombre = "Llamenos", Telefono = "4556-6666", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("d6622428-9e5e-4edf-81a7-6f16b5ae20ed"), Nombre = "Mi Radio Taxi", Telefono = "4931-1200", Url = "http://www.mitaxionline.com/" });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("72834a72-cf46-423b-8378-4dae55f26126"), Nombre = "Millenium", Telefono = "4829-0000", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("2b0474b9-29be-479e-9437-96504df554f3"), Nombre = "Mutax", Telefono = "4305-6446", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("da0db794-93ab-4c12-b6b3-1072f3a24fb6"), Nombre = "Pampa", Telefono = "4683-3003", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("c787c3ee-3168-4045-8857-f07818ecd8a8"), Nombre = "Pídalo", Telefono = "4956-1200", Url = "http://www.radiotaxipidalo.com.ar/" });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("e1ac3a39-e1b1-4703-8b66-c72d819c3c8b"), Nombre = "Por Buenos Aires", Telefono = "4566-7777", Url = "http://www.bairesradiotaxi.com.ar/" });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("23f5bdae-ab7b-4bd7-80a5-60d598a0939f"), Nombre = "Porteño", Telefono = "4566-5777", Url = "http://www.radiotaxiportenio.com.ar/" });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("8bae0725-5ab5-41a1-b861-60845b971255"), Nombre = "Premier", Telefono = "4858-0888", Url = "http://www.premieradiotaxi.com.ar/" });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("16da2d29-e965-417e-8625-c3aa221b58bf"), Nombre = "Premium", Telefono = "5238-0000", Url = "http://www.taxipremium.com/" });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("cc3673e1-2ffc-4af2-8e2c-1ae310c8dd7d"), Nombre = "Primer Nivel", Telefono = "4988-9999", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("595af820-0da0-4dc9-aef3-090b82f552dc"), Nombre = "Recoleta VIP", Telefono = "4983-0544", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("9c380268-0f24-4c32-8853-020d1d99b24c"), Nombre = "Tango", Telefono = "4862-2222", Url = null });
            RadioTaxiDC.Current.Lista.InsertOnSubmit(new RadioTaxiTable { Id = new Guid("8b00205e-bcc4-41b5-a165-27c282728d46"), Nombre = "Tiempo", Telefono = "4854-3838", Url = "http://www.radiotaxitiemposrl.com.ar/" });

            RadioTaxiDC.Current.SubmitChanges();
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