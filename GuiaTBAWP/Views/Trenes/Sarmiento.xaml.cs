﻿using System;
using System.Linq;
using System.Windows;
using GuiaTBAWP.Commons.ViewModels;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using GuiaTBAWP.Services;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Views.Trenes
{
    public partial class Sarmiento
    {
        public Sarmiento()
        {
            InitializeComponent();
        
            DataContext = ViewModel;
            Loaded += Page_Loaded;
            Unloaded += (sender, args) => DataService.CancelRequest();

            ViewModel.Ramales.Clear();
            DataService.EndRequest = EndRequest;
            DataService.StartRequest = StartRequest;
        }

        private static TrenLineaItemViewModel _viewModel = new TrenLineaItemViewModel();
        public static TrenLineaItemViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new TrenLineaItemViewModel()); }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DataService.DatosLoaded)
                DataService.LoadData();
            FillViewModel();
        }

        private static void FillViewModel()
        {
            var query = TrenesRamalEstadoDC.Current.ByLineas(new[] {"sarmiento", "pto. madero"});
            ViewModel.Ramales.Clear();
            foreach (var estadoTable in query.ToList())
            {
                ViewModel.AddRamal(estadoTable.ConvertToTrenRamalItemViewModel());
            }
            ViewModel.Actualizacion = string.Format("Actualizado hace {0}.",
                App.Configuration.UltimaActualizacionTrenes.ToUpdateDateTime());
        }

        #region Data

        private static readonly TrenStatusService DataService = new TrenStatusService();

        private int EndRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = true;

                ProgressBar.Hide();

                FillViewModel();
            });
            return 0;
        }

        private int StartRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = false;

                ProgressBar.Show("Actualizando estado del servicio...");
            });
            return 0;
        }

        private void ButtonGo_Click(object sender, EventArgs e)
        {
            DataService.LoadData();
        }
        #endregion
    }
}