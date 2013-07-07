using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GuiaTBAWP.BusData;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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
                var text = new TextBlock();
                text.Text = colectivo.Value;
                text.TextWrapping = TextWrapping.Wrap;

                var scroll = new ScrollViewer();
                scroll.Margin = new Thickness(12,0,0,40);
                scroll.Content = text;

                var item = new PanoramaItem();
                item.Header = colectivo.Key;
                item.Content = scroll;

                GeneralPanorama.Items.Add(item);
            }

        }
    }
}