using System.Data.Linq;
using System.Linq;

namespace GuiaTBAWP.Models
{
    public class TrenesRamalEstadoDC : DataContext
    {
        public Table<TrenesRamalEstadoTable> Estaciones;

        private TrenesRamalEstadoDC(string connectionString)
            : base(connectionString)
        {
            
        }

        static TrenesRamalEstadoDC _dataContext;

        public void Truncate()
        {
            var query = from r in Estaciones select r;
            _dataContext.Estaciones.DeleteAllOnSubmit(query);
            _dataContext.SubmitChanges();
        }

        public static TrenesRamalEstadoDC Current
        {
            get
            {
                if (_dataContext == null)
                {
                    _dataContext = new TrenesRamalEstadoDC("isostore:/TrenesRamalEstadoTable.sdf");

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
