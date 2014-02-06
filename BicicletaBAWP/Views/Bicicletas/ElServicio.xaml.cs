using Microsoft.Phone.Controls;

namespace BicicletaBAWP.Views.Bicicletas
{
    public partial class ElServicio : PhoneApplicationPage
    {
        public ElServicio()
        {
            InitializeComponent();
            StatusChecker.Check("Bicicletas.ElServicio");
        }
    }
}