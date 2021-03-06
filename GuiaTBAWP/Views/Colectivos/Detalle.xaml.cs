﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GuiaTBAWP.BusData;
using Microsoft.Phone.Controls;

namespace GuiaTBAWP.Views.Colectivos
{
    public partial class Detalle : PhoneApplicationPage
    {
        private readonly GetColectivoMapService _getColectivoMapService;

        public Detalle()
        {
            InitializeComponent();
            _getColectivoMapService = new GetColectivoMapService();
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
             <phone:PanoramaItem Header="recorrido" HeaderTemplate="{StaticResource SmallHomePanoramaTitle}">
                <ScrollViewer Margin="12,0,0,0">
                    <TextBlock x:Name="TxtRecorrido"></TextBlock>
                </ScrollViewer>
            </phone:PanoramaItem>
             */

            foreach (var colectivo in colectivoRecorrido)
            {
                var text = new TextBlock { Text = colectivo.Value, TextWrapping = TextWrapping.Wrap };
                var scroll = new ScrollViewer { Margin = new Thickness(12, 0, 0, 40), Content = text };
                var item = new PanoramaItem { HeaderTemplate = Application.Current.Resources["SmallHomePanoramaTitle"] as DataTemplate, Header = colectivo.Key, Content = scroll };
                GeneralPanorama.Items.Add(item);
            }

        }

        private void VerEnMapa_Click(object sender, EventArgs e)
        {
            App.MapViewModel.Reset();
            var bus = NavigationContext.QueryString["id"];
            _getColectivoMapService.SuccessFunc = () =>
            {
                NavigationService.Navigate(new Uri("/Views/Mapa.xaml", UriKind.Relative));
                return 0;
            };
            _getColectivoMapService.GetColectivo(bus);
        }
    }
}