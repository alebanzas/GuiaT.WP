using Microsoft.Phone.Controls;

namespace GuiaTBA.WP7.Views.Bicicletas
{
    public partial class Inscripcion : PhoneApplicationPage
    {
        public Inscripcion()
        {
            InitializeComponent();
            StatusChecker.Check("Bicicletas.Inscripcion");
        }
    }
}