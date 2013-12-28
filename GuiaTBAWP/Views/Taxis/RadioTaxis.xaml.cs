using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Taxis
{
    public partial class RadioTaxis : PhoneApplicationPage
    {
        public RadioTaxis()
        {
            InitializeComponent();

            StatusChecker.Check("Taxi.RadioTaxis");
        }
    }
}