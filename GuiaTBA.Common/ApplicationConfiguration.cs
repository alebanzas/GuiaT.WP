using System;
using System.Collections.Generic;
using System.Device.Location;
using GuiaTBA.Common.Models;
using GuiaTBAWP.Commons.Helpers;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Commons.Services;
using GuiaTBAWP.Models;

namespace GuiaTBA.Common
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
            
            //Ante un cambio de version reinicializo los datos
            if (version.Equals(Version))
            {
                SetInitialData();
            }

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

            BicicletaEstacionDC.Destroy();
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 1, Latitud = -34.583271, Longitud = -58.392579, Nombre = "Facultad de Derecho" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 2, Latitud = -34.592966, Longitud = -58.374379, Nombre = "Retiro" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 3, Latitud = -34.611376, Longitud = -58.369009, Nombre = "Aduana" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 4, Latitud = -34.601714, Longitud = -58.369202, Nombre = "Plaza Roma" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 5, Latitud = -34.580563, Longitud = -58.420605, Nombre = "Plaza Italia" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 6, Latitud = -34.628233, Longitud = -58.369606, Nombre = "Parque Lezama" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 7, Latitud = -34.605997, Longitud = -58.38098, Nombre = "Obelisco " });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 8, Latitud = -34.609733, Longitud = -58.389253, Nombre = "Congreso" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 9, Latitud = -34.58529, Longitud = -58.407598, Nombre = "Parque Las Heras" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 10, Latitud = -34.616106, Longitud = -58.366017, Nombre = "Madero UCA" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 11, Latitud = -34.601343, Longitud = -58.38525, Nombre = "Tribunales" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 12, Latitud = -34.592583, Longitud = -58.388965, Nombre = "Plaza Vicente López" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 13, Latitud = -34.609881, Longitud = -58.406346, Nombre = "Once" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 14, Latitud = -34.578112, Longitud = -58.426416, Nombre = "Pacífico" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 16, Latitud = -34.610158, Longitud = -58.374814, Nombre = "Legislatura" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 17, Latitud = -34.606463, Longitud = -58.418705, Nombre = "Plaza Almagro" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 18, Latitud = -34.61769, Longitud = -58.380473, Nombre = "Independencia" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 19, Latitud = -34.595699, Longitud = -58.377203, Nombre = "Plaza San Martín" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 20, Latitud = -34.579996, Longitud = -58.441615, Nombre = "Distrito Audiovisual" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 21, Latitud = -34.640112, Longitud = -58.406552, Nombre = "Parque Patricios" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 22, Latitud = -34.594087, Longitud = -58.382662, Nombre = "Arenales" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 23, Latitud = -34.60014, Longitud = -58.379679, Nombre = "Suipacha" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 24, Latitud = -34.610656, Longitud = -58.380845, Nombre = "Alsina" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 25, Latitud = -34.589746, Longitud = -58.4165, Nombre = "Plaza Güemes" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 26, Latitud = -34.600525, Longitud = -58.363645, Nombre = "Juana Manso" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 27, Latitud = -34.599506, Longitud = -58.389902, Nombre = "Montevideo" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 28, Latitud = -34.622995, Longitud = -58.41258, Nombre = "Plaza Boedo" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 29, Latitud = -34.607873, Longitud = -58.43354, Nombre = "Parque Centenario " });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 30, Latitud = -34.590586, Longitud = -58.397434, Nombre = "Peña" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 31, Latitud = -34.603173, Longitud = -58.439635, Nombre = "Padilla" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 32, Latitud = -34.607559, Longitud = -58.3736, Nombre = "Catedral" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 33, Latitud = -34.597211, Longitud = -58.398887, Nombre = "Facultad de Medicina" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 34, Latitud = -34.599097, Longitud = -58.37416, Nombre = "Galerías Pacífico" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 35, Latitud = -34.596146, Longitud = -58.371839, Nombre = "Ingeniero Butty" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 36, Latitud = -34.604841, Longitud = -58.376584, Nombre = "Maipú" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 37, Latitud = -34.607948, Longitud = -58.377993, Nombre = "Piedras" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 38, Latitud = -34.596969, Longitud = -58.382798, Nombre = "Libertad" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 40, Latitud = -34.604436, Longitud = -58.373785, Nombre = "Sarmiento" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 41, Latitud = -34.636356, Longitud = -58.40766, Nombre = "Urquiza" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 42, Latitud = -34.60472, Longitud = -58.37967, Nombre = "Diagonal Norte" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 43, Latitud = -34.599755, Longitud = -58.397834, Nombre = "Plaza Houssay" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 44, Latitud = -34.575548, Longitud = -58.413716, Nombre = "Zoológico" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 46, Latitud = -34.616327, Longitud = -58.374491, Nombre = "Chile" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 47, Latitud = -34.611002, Longitud = -58.373413, Nombre = "Colegio Nacional Buenos Aires" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 48, Latitud = -34.605429, Longitud = -58.371528, Nombre = "Perón" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 50, Latitud = -34.583712, Longitud = -58.401047, Nombre = "Hospital Rivadavia" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 51, Latitud = -34.601347, Longitud = -58.382268, Nombre = "Tucumán" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 53, Latitud = -34.595389, Longitud = -58.374469, Nombre = "Ricardo Rojas" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 55, Latitud = -34.59939, Longitud = -58.369413, Nombre = "Bouchard" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 56, Latitud = -34.589416, Longitud = -58.425143, Nombre = "Plaza Palermo Viejo" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 57, Latitud = -34.613024, Longitud = -58.370706, Nombre = "Belgrano" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 58, Latitud = -34.575139, Longitud = -58.434778, Nombre = "Ministro Carranza" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 59, Latitud = -34.590202, Longitud = -58.411614, Nombre = "Coronel Díaz" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 60, Latitud = -34.601586, Longitud = -58.371224, Nombre = "25 de Mayo" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 61, Latitud = -34.609017, Longitud = -58.370908, Nombre = "Ministerio de Economía" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 62, Latitud = -34.598506, Longitud = -58.376938, Nombre = "Córdoba" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 63, Latitud = -34.599506, Longitud = -58.372761, Nombre = "Reconquista" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 64, Latitud = -34.594263, Longitud = -58.39433, Nombre = "Riobamba" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 65, Latitud = -34.587184, Longitud = -58.415815, Nombre = "Julián Álvarez" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 66, Latitud = -34.594615, Longitud = -58.413818, Nombre = "Billinghurst" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 68, Latitud = -34.606681, Longitud = -58.385455, Nombre = "Rivarola" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 69, Latitud = -34.595984, Longitud = -58.404459, Nombre = "Ecuador" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 70, Latitud = -34.592667, Longitud = -58.425827, Nombre = "Araoz" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 71, Latitud = -34.603027, Longitud = -58.382824, Nombre = "Cerrito" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 72, Latitud = -34.613635, Longitud = -58.374675, Nombre = "Venezuela" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 74, Latitud = -34.604646, Longitud = -58.434769, Nombre = "Instituto Leloir" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 76, Latitud = -34.607551, Longitud = -58.394959, Nombre = "Ayacucho" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 78, Latitud = -34.591493, Longitud = -58.380276, Nombre = "Arroyo" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 81, Latitud = -34.615746, Longitud = -58.370755, Nombre = "Balcarce" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 82, Latitud = -34.608095, Longitud = -58.426184, Nombre = "Hospital Italiano" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 84, Latitud = -34.602192, Longitud = -58.395588, Nombre = "Lavalle" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 85, Latitud = -34.594522, Longitud = -58.409038, Nombre = "Agüero" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 88, Latitud = -34.612886, Longitud = -58.404286, Nombre = "Misiones" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 91, Latitud = -34.617276, Longitud = -58.397672, Nombre = "Pasco" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 94, Latitud = -34.591811, Longitud = -58.449442, Nombre = "Guzmán" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 95, Latitud = -34.60221, Longitud = -58.378212, Nombre = "Esmeralda" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 96, Latitud = -34.602914, Longitud = -58.411465, Nombre = "Carlos Gardel" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 98, Latitud = -34.611711, Longitud = -58.378285, Nombre = "Moreno" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 99, Latitud = -34.596331, Longitud = -58.435565, Nombre = "Malabia" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 114, Latitud = -34.594811, Longitud = -58.372649, Nombre = "Della Paolera" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 115, Latitud = -34.58766, Longitud = -58.390828, Nombre = "Quintana" });
            BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(new BicicletaEstacionTable { ExternalId = 121, Latitud = -34.60081, Longitud = -58.428378, Nombre = "Yatay" });
            BicicletaEstacionDC.Current.SubmitChanges();

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
        public string BingMapApiKey => "AgagZE2Ku0M0iPH8uolBeUSZUgHmGRrqbd-5etCjKym4dmTaH59yeS6Ka_kz_SDp";

        public bool IsInitialized { get; set; }

        public bool IsLocationEnabled => IsLocationEnabledByPhone && IsLocationEnabledByAppConfig;

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

        public bool IsRated { get; set; }
    }
}