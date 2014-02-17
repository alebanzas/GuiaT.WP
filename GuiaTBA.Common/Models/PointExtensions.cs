using System;
using System.Windows;

namespace GuiaTBAWP.Models
{
    public static class PointExtensions
    {
        /// <summary>
        /// Distances from point 1 to point 2
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <returns></returns>
        public static double DistanceFrom(this Point p1, Point p2)
        {
            var dX = p2.X - p1.X;
            var dY = p2.Y - p1.Y;
            return Math.Sqrt(dX * dX + dY * dY);
        }
    }
}
