using System.Data.Linq;
using System.Linq;

namespace GuiaTBAWP.Models
{
    public class TrenesLineaEstadoDC : DataContext
    {
        public Table<TrenesLineaEstadoTable> Lineas;

        private TrenesLineaEstadoDC(string connectionString)
            : base(connectionString)
        {
            
        }

        static TrenesLineaEstadoDC _dataContext;

        public void Truncate()
        {
            var query = from r in Lineas select r;
            _dataContext.Lineas.DeleteAllOnSubmit(query);
            _dataContext.SubmitChanges();
        }

        public static TrenesLineaEstadoDC Current
        {
            get
            {
                if (_dataContext == null)
                {
                    _dataContext = new TrenesLineaEstadoDC("isostore:/TrenesLineaEstadoTable.sdf");

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
