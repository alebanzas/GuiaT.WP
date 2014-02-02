using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Bicicletas
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