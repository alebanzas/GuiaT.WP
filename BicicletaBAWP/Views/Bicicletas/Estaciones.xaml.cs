using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using BicicletaBAWP.Extensions;
using GuiaTBAWP.Commons;
using GuiaTBAWP.Commons.Extensions;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Commons.Services;
using GuiaTBAWP.Extensions;
using GuiaTBAWP.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ProgressBar = GuiaTBAWP.ProgressBar;

namespace BicicletaBAWP.Views.Bicicletas
{
    public partial class Estaciones
    {
        private static EstacionesStatusViewModel _viewModel = new EstacionesStatusViewModel();
        public static EstacionesStatusViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new EstacionesStatusViewModel()); }
        }

        WebRequest _httpReq;

        public Estaciones()
        {
            InitializeComponent();
            DataContext = ViewModel;
            Loaded += Page_Loaded;
            Unloaded += (sender, args) =>
            {
                if (_httpReq != null)
                    _httpReq.Abort();
            };
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Actualizacion = string.Format("Actualizado hace {0}.", App.Configuration.UltimaActualizacionBicicletas.ToUpdateDateTime());

            MostrarLugares();
            
            Cargar();
        }
        
        public void MostrarLugares()
        {
            ViewModel.Estaciones.Clear();

            var bicicletaEstacionTables = BicicletaEstacionDC.GetAll();

            foreach (var bicicletaEstacionTable in bicicletaEstacionTables)
            {
                bicicletaEstacionTable.Distance = App.Configuration.IsLocationEnabled ? GetMeasureString(bicicletaEstacionTable) : string.Empty;
            }

            var list = App.Configuration.IsLocationEnabled ?
                bicicletaEstacionTables.OrderBy(x => x.GetDistanceTo(PositionService.GetCurrentLocation())) :
                bicicletaEstacionTables.OrderBy(x => x.Nombre);
            
            var lugares = new ObservableCollection<BicicletaEstacionTable>(list);
            foreach (var lugar in lugares)
            {
                if (!App.Configuration.IsLocationEnabled)
                {
                    lugar.Distance = string.Empty;
                }
                ViewModel.AddEstacion(lugar);
            }
        }
        
        public void Cargar(bool refresh = false)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                ProgressBar.Show("Actualizando estado...");

                var client = new HttpClient();
                _httpReq = client.Get("/api/bicicleta".ToApiCallUri(refresh));
                _httpReq.BeginGetResponse(HTTPWebRequestCallBack, _httpReq);
            }
            else
            {
                ShowErrorConnection();
            }
        }

        private void ShowErrorConnection()
        {
            //Luego le aviso al usuario que no se pudo cargar nueva información.
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(BicicletasStatusModel));
                var o = (BicicletasStatusModel)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateWebBrowser(UpdateWebBrowser), o);
            }
            catch (Exception ex)
            {
                ex.Log(EndRequest);
            }
        }

        delegate void DelegateUpdateWebBrowser(BicicletasStatusModel local);
        private void UpdateWebBrowser(BicicletasStatusModel l)
        {
            App.Configuration.UltimaActualizacionBicicletas = l.Actualizacion;

            foreach (BicicletaEstacionTable ll in l.Estaciones.ConvertToBicicletaEstacionTable())
            {
                ll.Distance = App.Configuration.IsLocationEnabled ? GetMeasureString(ll) : string.Empty;
                if (BicicletaEstacionDC.Current.Estaciones.Contains(ll))
                {
                    var estacion = BicicletaEstacionDC.Current.Estaciones.FirstOrDefault(x => x.Equals(ll));
                    if (estacion != null)
                    {
                        estacion.Cantidad = ll.Cantidad;
                        estacion.Horario = ll.Horario;
                        estacion.Estado = ll.Estado;
                        estacion.Distance = ll.Distance;
                    }
                }
                else
                {
                    BicicletaEstacionDC.Current.Estaciones.InsertOnSubmit(ll);
                }   
            }
            BicicletaEstacionDC.Current.SubmitChanges();

            App.Configuration.UltimaActualizacionBicicletas = l.Actualizacion;
            
            ViewModel.Actualizacion = string.Format("Actualizado hace {0}.", l.Actualizacion.ToUpdateDateTime());

            MostrarLugares();
            EndRequest();
        }

        private string GetMeasureString(BicicletaEstacionTable ll)
        {
            var currentLocation = PositionService.GetCurrentLocation();
            return StringExtensions.GetMeasureString(ll.Latitud, ll.Longitud, 
                                        currentLocation.Location.Latitude, currentLocation.Location.Longitude);
        }

        private int EndRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = true;

                ProgressBar.Hide();
            });
            return 0;
        }

        private void LstLugares_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox == null || listBox.SelectedIndex == -1) return;

            var bicicletaEstacion = (BicicletaEstacionTable)listBox.SelectedItem;

            var uri = new Uri(string.Format("/Views/Bicicletas/LugarDetalles.xaml?id={0}", bicicletaEstacion.Id), UriKind.Relative);
            NavigationService.Navigate(uri);

            //Vuelvo el indice del item seleccionado a -1 para que pueda hacer tap en el mismo item y navegarlo
            listBox.SelectedIndex = -1;
        }

        private void Opciones_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Opciones.xaml", UriKind.Relative));
        }

        private void Acerca_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Acerca.xaml", UriKind.Relative));
        }
        
        private void ButtonGo_Click(object sender, EventArgs e)
        {
            Cargar(true);
        }

        private void PinToStart_Click(object sender, RoutedEventArgs e)
        {
            if (List.ItemContainerGenerator == null) return;
            var selectedListBoxItem = List.ItemContainerGenerator.ContainerFromItem(((MenuItem)sender).DataContext) as ListBoxItem;
            if (selectedListBoxItem == null) return;
            var selectedIndex = List.ItemContainerGenerator.IndexFromContainer(selectedListBoxItem);
            
            var item = (BicicletaEstacionTable)List.Items[selectedIndex];
            var uri = new Uri(string.Format("/Views/Bicicletas/LugarDetalles.xaml?id={0}", item.Id), UriKind.Relative);
            TileManager.Set(uri, string.Format("Estación {0}", item.Nombre.ToLowerInvariant()), new Uri("/Images/Home/bicicletas.png", UriKind.Relative));
        }
    }

    public class EstacionesStatusViewModel: INotifyPropertyChanged
    {
        public EstacionesStatusViewModel()
        {
            Estaciones = new ObservableCollection<BicicletaEstacionTable>();
        }

        private string _actualizacion;
        public string Actualizacion
        {
            get { return _actualizacion; }
            set
            {
                _actualizacion = value;
                NotifyPropertyChanged("Actualizacion");
            }
        }

        private ObservableCollection<BicicletaEstacionTable> _estaciones;
        public ObservableCollection<BicicletaEstacionTable> Estaciones
        {
            get { return _estaciones; }
            private set { 
                _estaciones = value;
                NotifyPropertyChanged("Estaciones");
            }
        }

        public void AddEstacion(BicicletaEstacionTable estacion)
        {
            Estaciones.Add(estacion);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}