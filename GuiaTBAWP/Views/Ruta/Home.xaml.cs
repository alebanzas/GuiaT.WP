using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Ruta
{
    public partial class Home : PhoneApplicationPage
    {
        public Home()
        {
            InitializeComponent();
            
            StatusChecker.Check("HomeRuta");
        }
    }
}