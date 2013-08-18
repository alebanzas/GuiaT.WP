﻿using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Shell;

namespace GuiaTBAWP.Commons
{
    public static class TileManager
    {
        public static void Set(Uri navigationUrl, string title, Uri backgroundImage)
        {
            var standardTileData = new StandardTileData
            {
                BackgroundImage = backgroundImage,
                Title = title,
            };

            ShellTile tiletopin = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(navigationUrl.ToString()));

            if (tiletopin == null)
            {
                ShellTile.Create(navigationUrl, standardTileData);
            }
            else
            {
                MessageBox.Show("Ya está anclado.");
            }
        }
    }
}