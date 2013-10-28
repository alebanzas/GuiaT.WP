using System;
using System.Windows.Controls;
using GuiaTBAWP.BusData;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Colectivos
{
    public partial class Colectivos : PhoneApplicationPage
    {
        public Colectivos()
        {
            InitializeComponent();

            DataColectivos.SetData(busesIUrb, DataColectivos.LoadBusesIUrb());
            DataColectivos.SetData(busesProv, DataColectivos.LoadBusesProv());
            DataColectivos.SetData(busesMuni, DataColectivos.LoadBusesMuni());
        }

        private void Buses_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var bus = ((Bus)e.AddedItems[0]).Title.Split(' ')[1];

                Uri uri = new Uri(String.Format("/Views/Colectivos/Mapa.xaml?linea={0}", bus), UriKind.Relative);
                NavigationService.Navigate(uri);
            }
            catch (Exception) {}
        }
    }
}