using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace GuiaTBAWP.Models
{
    public class BicicletaEstacionDC : DataContext
    {
        public Table<BicicletaEstacionTable> Estaciones;

        private BicicletaEstacionDC(string connectionString)
            : base(connectionString)
        {
            
        }

        static BicicletaEstacionDC dataContext = null;

        public static void Destroy()
        {
            dataContext = null;
        }
        
        public static List<BicicletaEstacionTable> GetAll()
        {
            try
            {
                var query = from l in BicicletaEstacionDC.Current.Estaciones
                            orderby l.Id
                            select l;

                return query.ToList();
            }
            catch (Exception)
            {
                //Ante un error en la query, reseteo todos.
                Current.DeleteDatabase();
                Destroy();
                App.Configuration.InitialDataBicicletas = false;
                App.Configuration.UltimaActualizacionBicicletas = DateTime.MinValue;
                return new List<BicicletaEstacionTable>();
            }
        }

        public void Truncate()
        {
            var query = from r in Estaciones select r;
            dataContext.Estaciones.DeleteAllOnSubmit(query);
            dataContext.SubmitChanges();
        }

        public static BicicletaEstacionDC Current
        {
            get
            {
                if (dataContext == null)
                {
                    dataContext = new BicicletaEstacionDC("isostore:/BicicletaEstacionTable.sdf");

                    if (!dataContext.DatabaseExists())
                    {
                        dataContext.CreateDatabase();
                    }
                }

                return dataContext;
            }
        }

    }
}
