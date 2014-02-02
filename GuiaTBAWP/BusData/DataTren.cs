using System.Windows.Media;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Commons.ViewModels;
using System.Collections.Generic;

namespace GuiaTBAWP.BusData
{
    public class DataTren
    {
        public static List<TrackLine> GetData()
        {
            var lineas = new List<TrackLine>
            {
                new TrackLine
                {
                    Nombre = "Pampa",
                    Trazado = new List<PuntoViewModel>
                    {
                        new PuntoViewModel {X = -34.5844731848619, Y = -58.4880995750427},
                        new PuntoViewModel {X = -34.5836693829063, Y = -58.4867906570435},
                        new PuntoViewModel {X = -34.5818320920995, Y = -58.4836578369141},
                        new PuntoViewModel {X = -34.5792439164329, Y = -58.4791839122772},
                        new PuntoViewModel {X = -34.5759136196232, Y = -58.4733152389526},
                        new PuntoViewModel {X = -34.5706838126772, Y = -58.4641313552856},
                    },
                    Color = Colors.Green,
                    Postas = new List<PuntoViewModel>
                    {
                        new PuntoViewModel { Name = "Probando", X = -34.5844731848619, Y = -58.4880995750427},
                    }
                },
            };

            return lineas;
        }
    }
}
