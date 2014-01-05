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

        static RadioTaxiDC dataContext = null;

        public static void Destroy()
        {
            dataContext = null;
        }

        public static List<RadioTaxiTable> GetAll()
        {
            try
            {
                var query = from l in RadioTaxiDC.Current.Lista
                            orderby l.Id
                            select l;

                return query.ToList();
            }
            catch (Exception)
            {
                //Ante un error en la query, reseteo todos.
                Current.DeleteDatabase();
                Destroy();
                //TODO: refactor
                return new List<RadioTaxiTable>();
            }
        }

        public void Truncate()
        {
            var query = from r in Lista select r;
            dataContext.Lista.DeleteAllOnSubmit(query);
            dataContext.SubmitChanges();
        }

        public static RadioTaxiDC Current
        {
            get
            {
                if (dataContext == null)
                {
                    dataContext = new RadioTaxiDC("isostore:/RadioTaxiTable.sdf");

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
