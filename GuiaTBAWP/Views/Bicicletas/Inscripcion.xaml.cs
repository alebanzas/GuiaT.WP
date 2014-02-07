using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Bicicletas
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