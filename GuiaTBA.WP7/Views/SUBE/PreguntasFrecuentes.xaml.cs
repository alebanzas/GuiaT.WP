using Microsoft.Phone.Controls;

namespace GuiaTBA.WP7.Views.SUBE
{
    public partial class PreguntasFrecuentes : PhoneApplicationPage
    {
        public PreguntasFrecuentes()
        {
            InitializeComponent();
            StatusChecker.Check("SUBE.PreguntasFrecuentes");
        }
    }
}