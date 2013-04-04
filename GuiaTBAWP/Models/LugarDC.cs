using System.Data.Linq;

namespace WPLugares
{
    public class LugarDC : DataContext
    {
        public Table<Lugar> Lugares;

        private LugarDC(string connectionString)
            : base(connectionString)
        {
        }

        static LugarDC dataContext = null;

        public static LugarDC Current
        {
            get
            {
                if (dataContext == null)
                {
                    dataContext = new LugarDC("isostore:/LugarBD.sdf");

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
