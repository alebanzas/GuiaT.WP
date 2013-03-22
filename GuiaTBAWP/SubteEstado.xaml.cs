using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NetworkInterface = System.Net.NetworkInformation.NetworkInterface;

namespace GuiaTBAWP
{
    public partial class SubteEstado : PhoneApplicationPage
    {
        private static SubteStatusViewModel _viewModel = new SubteStatusViewModel();

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static SubteStatusViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                return _viewModel ?? (_viewModel = new SubteStatusViewModel());
            }
        }


        readonly ProgressIndicator _progress = new ProgressIndicator();
        

        public SubteEstado()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = ViewModel;
            Loaded += MainPage_Loaded;

            _progress.IsVisible = true;
            _progress.IsIndeterminate = true;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }


        #region DataInit


        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                ConnectionError.Visibility = Visibility.Collapsed;
                _progress.Text = "Buscando cotizaciones";
                SystemTray.SetIsVisible(this, true);
                SystemTray.SetProgressIndicator(this, _progress);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                    if (applicationBarIconButton != null)
                        applicationBarIconButton.IsEnabled = false;
                });

                var httpReq = (HttpWebRequest)WebRequest.Create(new Uri("http://servicio.abhosting.com.ar/subte/?type=WP&version=1"));
                httpReq.Method = "GET";
                httpReq.BeginGetResponse(HTTPWebRequestCallBack, httpReq);
            }
            else
            {
                ShowErrorConnection();
            }
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            try
            {
                var httpRequest = (HttpWebRequest)result.AsyncState;
                var response = httpRequest.EndGetResponse(result);
                var stream = response.GetResponseStream();

                var serializer = new DataContractJsonSerializer(typeof(SubteStatusModel));
                var o = (SubteStatusModel)serializer.ReadObject(stream);

                Dispatcher.BeginInvoke(new DelegateUpdateWebBrowser(UpdateCotizaciones), o);
            }
            catch (Exception)
            {
                EndRequest();
                //this.Dispatcher.BeginInvoke(() => MessageBox.Show("Error.. " + ex.Message));
                Dispatcher.BeginInvoke(() => MessageBox.Show("Ocurrió un error al obtener las cotizaciones. Verifique su conexión a internet."));
            }
        }

        delegate void DelegateUpdateWebBrowser(SubteStatusModel local);
        private void UpdateCotizaciones(SubteStatusModel model)
        {
            var result = new Collection<SubteItemViewModel>();

            foreach (var item in model.Lineas)
            {
                result.Add(new SubteItemViewModel
                {
                    Nombre = item.Nombre,
                    Detalles = item.Detalles,
                });
            }

            BindViewModel(result);

            EndRequest();
        }

        private void BindViewModel(IEnumerable<SubteItemViewModel> result)
        {
            _viewModel.Lineas.Clear();
            foreach (var subteItemViewModel in result)
            {
                _viewModel.AddLinea(new SubteItemViewModel
                    {
                        Detalles = subteItemViewModel.Detalles,
                        Nombre = subteItemViewModel.Nombre,
                    });
            }

            _viewModel.Actualizacion = DateTime.UtcNow.ToShortTimeString();

        }

        private void EndRequest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var applicationBarIconButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (applicationBarIconButton != null)
                    applicationBarIconButton.IsEnabled = true;

                Loading.Visibility = Visibility.Collapsed;
                SystemTray.SetProgressIndicator(this, null);
            });
        }

        private void ShowErrorConnection()
        {
            //Luego le aviso al usuario que no se pudo cargar nueva información.
            ConnectionError.Visibility = Visibility.Visible;
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Ha habido un error intentando acceder a los nuevos datos o no hay conexiones de red disponibles.\nPor favor asegúrese de contar con acceso de red y vuelva a intentarlo."));
        }

        #endregion

        private void ButtonGo_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }

    public class SubteItemViewModel
    {
        public string Nombre { get; set; }

        public string Detalles { get; set; }
    }

    public class SubteStatusViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SubteItemViewModel> _lineas;

        public SubteStatusViewModel()
        {
            Lineas = new ObservableCollection<SubteItemViewModel>();
        }

        public ObservableCollection<SubteItemViewModel> Lineas
        {
            get { return _lineas; }
            private set { 
                    _lineas = value;
                    NotifyPropertyChanged("SampleProperty");
                }
        }

        public void AddLinea(SubteItemViewModel linea)
        {
            Lineas.Add(linea);
        }

        public string Actualizacion { get; set; }

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

    public class SubteStatusModel
    {
        public SubteStatusModel()
        {
            Lineas = new List<SubteStatusItem>();
        }

        public IList<SubteStatusItem> Lineas { get; set; }

        public DateTime Actualizacion { get; set; }

        public string ActualizacionStr
        {
            get { return string.Format("{0} {1}", Actualizacion.ToLongDateString(), Actualizacion.ToLongTimeString()); }
        }
    }

    public class SubteStatusItem
    {
        public string Nombre { get; set; }

        public string Detalles { get; set; }
    }
}