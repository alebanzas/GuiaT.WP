using System.Data.Linq;

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

        public static BicicletaEstacionDC Current
        {
            get
            {
                if (dataContext == null)
                {
                    dataContext = new BicicletaEstacionDC("isostore:/BicicletaEstacionDC.sdf");

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
