using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using GuiaTBAWP.ViewModels;

namespace GuiaTBAWP.Models
{
    public class TrenesRamalEstadoDC : DataContext
    {
        public Table<TrenesRamalEstadoTable> Ramales;

        private TrenesRamalEstadoDC(string connectionString)
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

        public List<TrenRamalItemViewModel> ByLinea(string lineaNickName)
        {
            return _dataContext.Ramales.Where(x => x.LineaNickName.Equals(lineaNickName)).Select(x => x.ConvertToTrenRamalItemViewModel()).ToList();
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
