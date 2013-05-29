using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using GuiaTBAWP.ViewModels;

namespace GuiaTBAWP.Models
{
    public class TrenesRamalEstadoDC : DataContext
    {
        public Table<TrenesRamalEstadoTable> Ramales;

        public TrenesRamalEstadoDC(string connectionString)
            : base(connectionString)
        {
            
        }

        static TrenesRamalEstadoDC _dataContext;
        
        public void Truncate()
        {
            var query = from r in Ramales select r;
            _dataContext.Ramales.DeleteAllOnSubmit(query);
            _dataContext.SubmitChanges();
        }

        public IQueryable<TrenesRamalEstadoTable> ByLinea(string lineaNickName)
        {
            var list = from ramal in Current.Ramales
                       where ramal.LineaNickName.Equals(lineaNickName)
                       select ramal;
            return list;

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
