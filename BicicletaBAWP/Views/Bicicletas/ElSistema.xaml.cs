using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace BicicletaBAWP.Views.Bicicletas
{
    public partial class ElSistema : PhoneApplicationPage
    {
        public ElSistema()
        {
            InitializeComponent();
        }

        private void TurTel_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new PhoneCallTask();
            task.PhoneNumber = "0800-999-2838";
            task.Show();
        }

        private void GCBATel_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new PhoneCallTask();
            task.PhoneNumber = "147";
            task.Show();
        }
    }
}