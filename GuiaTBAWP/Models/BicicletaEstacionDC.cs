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
