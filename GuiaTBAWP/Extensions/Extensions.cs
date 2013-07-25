// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GuiaTBAWP.Extensions
{
    public static class UriExtensions
    {
        private static Uri ToApiCallUri(this string source, string param, bool alwaysRefresh = false)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                param = "&" + param;
            }
            else
            {
                param = string.Empty;
            }
            var refresh = string.Empty;
            if (alwaysRefresh)
            {
                refresh = string.Format("&t={0}.{1}", DateTime.UtcNow.Hour, DateTime.UtcNow.Minute);
            }
            try
            {
                if (!param.Contains("&lat=") && !param.Contains("&lon="))
                {
                    var currentPosition = PositionService.GetCurrentLocation();
                    if (currentPosition != null)
                    {
                        param += "&lat=" + currentPosition.Location.Latitude;
                        param += "&lon=" + currentPosition.Location.Longitude;
                    }
                }
            }
            catch
            {
                //no me importa si pincha.
            }
            
            return new Uri(string.Format("http://servicio.abhosting.com.ar{0}/?appId={1}&versionId={2}&installationId={3}{4}{5}",
                source,
                App.Configuration.Name,
                App.Configuration.Version,
                App.Configuration.InstallationId,
                refresh,
                param));
        }

        public static Uri ToApiCallUri(this string source, Dictionary<string, object> param, bool alwaysRefresh = false)
        {
            var pp = string.Join("&", param.Select(x => x.Key + "=" + x.Value));

            return ToApiCallUri(source, pp, alwaysRefresh);
        }

        public static Uri ToApiCallUri(this string source, bool alwaysRefresh = false)
        {
            return ToApiCallUri(source, string.Empty, alwaysRefresh);
        }
    }

    public static class DateTimeExtensions
    {
        public static string ToLocalDateTime(this DateTime source)
        {
            return string.Format("{0} {1}", source.ToLocalTime().ToShortDateString(), source.ToLocalTime().ToShortTimeString());
        }

        public static string ToUpdateDateTime(this DateTime source)
        {
            var localTime = DateTime.Now;
            var lastTime = source.ToLocalTime();
            var difference = localTime.Subtract(lastTime);

            if (difference.Days == 1)
            {
                return "1 día";
            }
            if (difference.Days > 1)
            {
                return string.Format("{0} días", difference.Days);
            }
            if (difference.Hours == 1)
            {
                return "1 hora";
            }
            if (difference.Hours > 1)
            {
                return string.Format("{0} horas", difference.Hours);
            }
            if (difference.Minutes == 1)
            {
                return "1 minuto";
            }
            if (difference.Minutes > 1)
            {
                return string.Format("{0} minutos", difference.Minutes);
            }
            //if (difference.Seconds == 1)
            //{
            //    return "1 segundo";
            //}
            //if (difference.Seconds > 1)
            //{
            //    return string.Format("{0} segundos", difference.Seconds);
            //}
            return "pocos segundos";
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Return a random item from a list.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="rnd">The Random instance.</param>
        /// <param name="list">The list to choose from.</param>
        /// <returns>A randomly selected item from the list.</returns>
        public static T Next<T>(this Random rnd, IList<T> list)
        {
            return list[rnd.Next(list.Count)];
        }
    }

    /// <summary>
    /// A class used to expose the Key property on a dynamically-created Linq grouping.
    /// The grouping will be generated as an internal class, so the Key property will not
    /// otherwise be available to databind.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TElement">The type of the items.</typeparam>
    public class PublicGrouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private readonly IGrouping<TKey, TElement> _internalGrouping;

        public PublicGrouping(IGrouping<TKey, TElement> internalGrouping)
        {
            _internalGrouping = internalGrouping;
        }

        public override bool Equals(object obj)
        {
            PublicGrouping<TKey, TElement> that = obj as PublicGrouping<TKey, TElement>;

            return (that != null) && (this.Key.Equals(that.Key));
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        #region IGrouping<TKey,TElement> Members

        public TKey Key
        {
            get { return _internalGrouping.Key; }
        }

        #endregion

        #region IEnumerable<TElement> Members

        public IEnumerator<TElement> GetEnumerator()
        {
            return _internalGrouping.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _internalGrouping.GetEnumerator();
        }

        #endregion
    }

    public class CommandButton : Button
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandButton), new PropertyMetadata(OnCommandChanged));

        private static void OnCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((CommandButton)obj).OnCommandChanged(e);
        }

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandButton), new PropertyMetadata(OnCommandParameterChanged));

        private static void OnCommandParameterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((CommandButton)obj).UpdateIsEnabled();
        }

        private void OnCommandChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ICommand command = e.OldValue as ICommand;
                if (command != null)
                {
                    command.CanExecuteChanged -= CommandCanExecuteChanged;
                }
            }

            if (e.NewValue != null)
            {
                ICommand command = e.NewValue as ICommand;
                if (command != null)
                {
                    command.CanExecuteChanged += CommandCanExecuteChanged;
                }
            }

            UpdateIsEnabled();
        }

        private void CommandCanExecuteChanged(object sender, EventArgs e)
        {
            UpdateIsEnabled();
        }

        private void UpdateIsEnabled()
        {
            IsEnabled = Command != null ? Command.CanExecute(CommandParameter) : false;
        }

        protected override void OnClick()
        {
            base.OnClick();

            if (Command != null)
            {
                Command.Execute(CommandParameter);
            }
        }
    }
}
