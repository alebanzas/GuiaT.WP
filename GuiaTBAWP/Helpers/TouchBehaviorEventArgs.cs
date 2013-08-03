using System;
using System.Windows;

namespace GuiaTBAWP.Helpers
{
    /// <summary>
    /// Touch behavior event arguments.
    /// </summary>
    public class TouchBehaviorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the touch point.
        /// </summary>
        public Point TouchPoint { get; private set; }

        /// <summary>
        /// Initializes a new instance of this type.
        /// </summary>
        /// <param name="touchPoint">The touch point.</param>
        public TouchBehaviorEventArgs(Point touchPoint)
        {
            TouchPoint = touchPoint;
        }
    }
}
