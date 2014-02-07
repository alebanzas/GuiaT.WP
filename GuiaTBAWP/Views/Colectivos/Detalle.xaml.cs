using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GuiaTBAWP.BusData;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Colectivos
{
    public partial class Detalle : PhoneApplicationPage
    {
        public Detalle()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadData();
            base.OnNavigatedTo(e);
        }

        private void LoadData()
        {
            var linea = Uri.EscapeUriString(NavigationContext.QueryString["id"]);
            var colectivoRecorrido = DataColectivos.AllByCode(linea);

            GeneralPanorama.Title = "Línea " + linea;
            /*
             <controls:PanoramaItem Header="recorrido" HeaderTemplate="{StaticResource SmallHomePanoramaTitle}">
                <ScrollViewer Margin="12,0,0,0">
                    <TextBlock x:Name="TxtRecorrido"></TextBlock>
                </ScrollViewer>
            </controls:PanoramaItem>
             */

            foreach (var colectivo in colectivoRecorrido)
            {
                var text = new TextBlock { Text = colectivo.Value, TextWrapping = TextWrapping.Wrap };
                var scroll = new ScrollViewer { Margin = new Thickness(12, 0, 0, 40), Content = text };
                var item = new PanoramaItem { Header = colectivo.Key, Content = scroll };
                GeneralPanorama.Items.Add(item);
            }

        }

        private void VerEnMapa_Click(object sender, EventArgs e)
        {
            var bus = NavigationContext.QueryString["id"];

            Uri uri = new Uri(String.Format("/Views/Colectivos/Mapa.xaml?linea={0}", bus), UriKind.Relative);
            NavigationService.Navigate(uri);
        }
    }
}