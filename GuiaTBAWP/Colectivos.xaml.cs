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

namespace GuiaTBAWP
{
    public partial class Colectivos : PhoneApplicationPage
    {
        public Colectivos()
        {
            InitializeComponent();

            LoadBusesIUrb();
            LoadBusesMuni();
            LoadBusesProv();
        }

        private void LoadBusesIUrb()
        {
            var buss = new List<Bus>();




            var byCategory = from Bus in buss
                             group Bus by Bus.Category into c
                             //orderby c.Key
                             select new PublicGrouping<string, Bus>(c);

            busesIUrb.ItemsSource = byCategory;
        }

        private void LoadBusesMuni()
        {
            var buss = new List<Bus>();




            var byCategory = from Bus in buss
                             group Bus by Bus.Category into c
                             //orderby c.Key
                             select new PublicGrouping<string, Bus>(c);

            busesMuni.ItemsSource = byCategory;
        }

        private void LoadBusesProv()
        {
            var buss = new List<Bus>();




            var byCategory = from Bus in buss
                             group Bus by Bus.Category into c
                             //orderby c.Key
                             select new PublicGrouping<string, Bus>(c);

            busesProv.ItemsSource = byCategory;
        }
    }
}