using Microsoft.Phone.Controls;

namespace BicicletaBAWP.Views.Bicicletas
{
    public partial class ElSistema : PhoneApplicationPage
    {
        public ElSistema()
        {
            InitializeComponent();
            StatusChecker.Check("Bicicletas.ElSistema");
        }
    }
}