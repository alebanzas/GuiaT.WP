using Microsoft.Phone.Controls;

namespace GuiaTBA.WP7.Views.Bicicletas
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