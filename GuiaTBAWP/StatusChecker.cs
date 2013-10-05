using System;
using System.Collections.Generic;
using System.Net;
using GuiaTBAWP.Extensions;

namespace GuiaTBAWP
{
    public static class StatusChecker
    {
        public static void Check()
        {
            try
            {
                (new WebClient()).DownloadStringAsync("/status/check".ToApiCallUri());
            }
            catch (Exception)
            { }
        }

        public static void Check(string name)
        {
            try
            {
                var param = new Dictionary<string, object> {{"n", name}};
                (new WebClient()).DownloadStringAsync("/status/check".ToApiCallUri(param));
            }
            catch (Exception)
            { }
        }
    }
}
