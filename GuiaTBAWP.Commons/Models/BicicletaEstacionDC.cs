using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace GuiaTBAWP.Commons.Models
{
    public class BicicletaEstacionDC : DataContext
    {
        public Table<BicicletaEstacionTable> Estaciones;

        private BicicletaEstacionDC(string connectionString)
            : base(connectionString)
        {
            
        }

        static BicicletaEstacionDC _dataContext;

        public static void Destroy()
        {
            Current.DeleteDatabase();
            _dataContext = null;
        }
        
        public static List<BicicletaEstacionTable> GetAll()
        {
            try
            {
                var query = from l in Current.Estaciones
                            orderby l.Id
                            select l;

                return query.ToList();
            }
            catch (Exception)
            {
                //Ante un error en la query, reseteo todos.
                Destroy();
                return new List<BicicletaEstacionTable>();
            }
        }

        public void Truncate()
        {
            var query = from r in Estaciones select r;
            _dataContext.Estaciones.DeleteAllOnSubmit(query);
            _dataContext.SubmitChanges();
        }

        public static BicicletaEstacionDC Current
        {
            get
            {
                if (_dataContext == null)
                {
                    _dataContext = new BicicletaEstacionDC("isostore:/BicicletaEstacionTable.sdf");

                    if (!_dataContext.DatabaseExists())
                    {
                        _dataContext.CreateDatabase();
                    }
                }

                return _dataContext;
            }
        }

    }
}
