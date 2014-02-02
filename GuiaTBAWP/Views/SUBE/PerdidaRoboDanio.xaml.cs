using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace GuiaTBAWP.Views.SUBE
{
    public partial class PerdidaRoboDanio : PhoneApplicationPage
    {
        public PerdidaRoboDanio()
        {
            InitializeComponent();
            StatusChecker.Check("SUBE.PerdidaRoboDanio");
        }

        private void SUBETel_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new PhoneCallTask
            {
                PhoneNumber = "0800-777-7823"
            };
            task.Show();
        }
    }
}