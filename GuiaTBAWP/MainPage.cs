using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using GuiaTBAWP.Helpers;
using GuiaTBAWP.Models;
using GuiaTBAWP.Source;

namespace GuiaTBAWP
{
    /// <summary>
    /// This partial class contains startup code for implementing the bing maps lab,
    /// so you can concentrate on the Bing map control and services and not on the
    /// UI behavior or application flow.
    /// </summary>
    /// <remarks>
    /// User code should be added to the MainPage.xaml.cs file only.
    /// </remarks>
    public partial class MainPage : INotifyPropertyChanged
    {
        #region Fields

        /// <value>Helper for handling mouse (touch) events.</value>
        private readonly TouchBehavior _touchBehavior;

        /// <value>Helper for handling visual states.</value>
        private readonly VisualStates _visualStates;

        #endregion

        #region Ctor

        public MainPage()
        {
            InitializeDefaults();
            InitializeComponent();
            
            SetLocationService(GeoPositionAccuracy.High);

            _touchBehavior = new TouchBehavior(this);
            _touchBehavior.Tap += touchBehavior_Tap;
            _touchBehavior.Move += touchBehavior_Move;
            _touchBehavior.Hold += touchBehavior_Hold;

            _visualStates = new VisualStates(this);

            DataContext = this;
        }

        #endregion

        #region Event Handlers

        private void touchBehavior_Tap(object sender, TouchBehaviorEventArgs e)
        {
            // Hide pushpins panel.
            _visualStates.PushpinsState = VisualStates.HidePushpins;
        }

        private void touchBehavior_Move(object sender, TouchBehaviorEventArgs e)
        {
            // Hide pushpins panel.
            _visualStates.PushpinsState = VisualStates.HidePushpins;
        }

        private void touchBehavior_Hold(object sender, TouchBehaviorEventArgs e)
        {
            CenterPushpinsPopup(e.TouchPoint);

            // Show pushpins panel.
            _visualStates.PushpinsState = VisualStates.ShowPushpins;
        }

        private void ListBoxPushpinCatalog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = sender as Selector;
            if (selector == null || selector.SelectedItem == null)
            {
                return;
            }

            CreateNewPushpin(selector.SelectedItem, _touchBehavior.LastTouchPoint);

            // Reset the selected item so we can pick it again next time.
            selector.SelectedItem = null;

            // Hide pushpins panel.
            _visualStates.PushpinsState = VisualStates.HidePushpins;
        }

        #region Button Click Events

        private void ButtonLocation_Click(object sender, EventArgs e)
        {
            // Hide pushpins panel.
            _visualStates.PushpinsState = VisualStates.HidePushpins;

            StartLocationService();
            //CenterLocation();
        }

        private void ButtonRoute_Click(object sender, EventArgs e)
        {
            // Display or hide route panel.
            _visualStates.RouteState = _visualStates.RouteState == VisualStates.HideRoute ? VisualStates.ShowRoute : VisualStates.HideRoute;

            // Hide pushpins panel.
            _visualStates.PushpinsState = VisualStates.HidePushpins;
        }

        private void ButtonDirections_Click(object sender, EventArgs e)
        {
            // Display or hide directions panel.
            if (_visualStates.DirectionsState == VisualStates.HideDirections)
            {
                if (HasDirections)
                {
                    _visualStates.DirectionsState = VisualStates.ShowDirections;
                }
            }
            else
            {
                _visualStates.DirectionsState = VisualStates.HideDirections;
            }

            // Hide pushpins panel.
            _visualStates.PushpinsState = VisualStates.HidePushpins;
        }

        private void ButtonMode_Click(object sender, EventArgs e)
        {
            SwapMapMode();

            // Hide pushpins panel.
            _visualStates.PushpinsState = VisualStates.HidePushpins;
        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            //HttpWebRequest httpReq = (HttpWebRequest)HttpWebRequest.Create(new Uri(string.Format("http://localhost/local/near/?lat={0}&lon={1}", CurrentLocation.Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", "."), CurrentLocation.Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", "."))));
            //httpReq.Method = "POST";
            //httpReq.BeginGetResponse(HTTPWebRequestCallBack, httpReq);

            ShowNewRoute();

            // Hide pushpins panel.
            _visualStates.PushpinsState = VisualStates.HidePushpins;
        }

        private void ShowNewRoute()
        {
            LocalModel local = LocalesSource.GetInstance().GetNearest(new Point(CurrentLocation.Latitude, CurrentLocation.Longitude));
            
            ToLocation = new GeoCoordinate(local.Latitud, local.Longitud);
            To = string.Format("{0}, Argentina", local.Direccion2);
            CalculateRoute();
        }

        private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)result.AsyncState;
            WebResponse response = httpRequest.EndGetResponse(result);
            Stream stream = response.GetResponseStream();

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<LocalServiceModel>));
            var local = (List<LocalServiceModel>)serializer.ReadObject(stream);

            this.Dispatcher.BeginInvoke(new delegateUpdateWebBrowser(updateWebBrowser), local);
        }

        delegate void delegateUpdateWebBrowser(List<LocalServiceModel> local);
        private void updateWebBrowser(List<LocalServiceModel> l)
        {
            LocalServiceModel local = l[0];

            ToLocation = new GeoCoordinate(local.Latitud, local.Longitud);
            To = string.Format("{0}, {1}, {2}, {3}, Argentina", local.Direccion, local.Barrio, local.Ciudad, local.Provincia);
            CalculateRoute();
        }




        private void ButtonZoomIn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Zoom += 1;

            // Hide pushpins panel.
            _visualStates.PushpinsState = VisualStates.HidePushpins;
        }

        private void ButtonZoomOut_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Zoom -= 1;

            // Hide pushpins panel.
            _visualStates.PushpinsState = VisualStates.HidePushpins;
        }

        #endregion

        #endregion

        #region Privates

        /// <summary>
        /// Hides the route view and display the directions view.
        /// </summary>
        private void ShowDirectionsView()
        {
            _visualStates.RouteState = VisualStates.HideRoute;

            if (HasDirections)
            {
                _visualStates.DirectionsState = VisualStates.ShowDirections;
            }
        }

        #endregion

        #region Property Changed

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Visual States
        /// <summary>
        /// An internal helper class for handling MainPage visual state transitions.
        /// </summary>
        private class VisualStates
        {
            #region Predefined Visual States
            // Route States
            public const string ShowRoute = "ShowRoute";
            public const string HideRoute = "HideRoute";

            // Directions States
            public const string ShowDirections = "ShowDirections";
            public const string HideDirections = "HideDirections";

            // Pushpins States
            public const string ShowPushpins = "ShowPushpins";
            public const string HidePushpins = "HidePushpins";
            #endregion

            #region Fields
            private readonly Control _control;
            private string _routeState = HideRoute;
            private string _directionsState = HideDirections;
            private string _pushpinsState = HidePushpins;
            #endregion

            #region Properties
            /// <summary>
            /// Change the route panel visual state.
            /// </summary>
            public string RouteState
            {
                get { return _routeState; }
                set
                {
                    if (_routeState != value)
                    {
                        _routeState = value;
                        VisualStateManager.GoToState(_control, value, true);
                    }
                }
            }

            /// <summary>
            /// Change the directions panel visual state.
            /// </summary>
            public string DirectionsState
            {
                get { return _directionsState; }
                set
                {
                    if (_directionsState != value)
                    {
                        _directionsState = value;
                        VisualStateManager.GoToState(_control, value, true);
                    }
                }
            }

            /// <summary>
            /// Change the pushpins popup visual state.
            /// </summary>
            public string PushpinsState
            {
                get { return _pushpinsState; }
                set
                {
                    if (_pushpinsState != value)
                    {
                        _pushpinsState = value;
                        VisualStateManager.GoToState(_control, value, true);
                    }
                }
            }
            #endregion

            #region Ctor
            /// <summary>
            /// Initializes a new instance of this class.
            /// </summary>
            /// <param name="control">The target control.</param>
            public VisualStates(Control control)
            {
                _control = control;
            }
            #endregion
        }
        #endregion
    }
}
