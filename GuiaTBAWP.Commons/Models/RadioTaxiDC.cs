using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace GuiaTBAWP.Commons.Models
{
    public class RadioTaxiDC : DataContext
    {
        public Table<RadioTaxiTable> Lista;

        private RadioTaxiDC(string connectionString)
            : base(connectionString)
        {
            
        }

        static RadioTaxiDC _dataContext;

        public static void Destroy()
        {
            Current.DeleteDatabase();
            _dataContext = null;
        }

        public static List<RadioTaxiTable> GetAll()
        {
            try
            {
                var query = from l in Current.Lista
                            orderby l.Id
                            select l;

                return query.ToList();
            }
            catch (Exception)
            {
                //Ante un error en la query, reseteo todos.
                Destroy();
                return new List<RadioTaxiTable>();
            }
        }

        public void Truncate()
        {
            var query = from r in Lista select r;
            _dataContext.Lista.DeleteAllOnSubmit(query);
            _dataContext.SubmitChanges();
        }

        public static RadioTaxiDC Current
        {
            get
            {
                if (_dataContext == null)
                {
                    _dataContext = new RadioTaxiDC("isostore:/RadioTaxiTable.sdf");

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
