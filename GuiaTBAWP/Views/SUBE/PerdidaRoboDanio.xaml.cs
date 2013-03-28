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
        }

        private void SUBETel_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new PhoneCallTask();
            task.PhoneNumber = "0800-777-7823";
            task.Show();
        }
    }
}