﻿using System.Windows.Media;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Commons.ViewModels;
using System.Collections.Generic;

namespace GuiaTBAWP.BusData
{
    public class DataSubte
    {
        public static List<TrackLine> GetData()
        {
            var lineas = new List<TrackLine>
            {
                new TrackLine
                {
                    Nombre = "Linea D",
                    Trazado = new List<PuntoViewModel>
                    {
                        new PuntoViewModel {X = -34.555469, Y = -58.462521},
                        new PuntoViewModel {X = -34.561777, Y = -58.456943},
                        new PuntoViewModel {X = -34.56494 , Y = -58.454196},
                        new PuntoViewModel {X = -34.566549, Y = -58.45147 },
                        new PuntoViewModel {X = -34.574094, Y = -58.437072},
                        new PuntoViewModel {X = -34.576638, Y = -58.43173 },
                        new PuntoViewModel {X = -34.577891, Y = -58.427481},
                        new PuntoViewModel {X = -34.578546, Y = -58.425635},
                        new PuntoViewModel {X = -34.579624, Y = -58.423876},
                        new PuntoViewModel {X = -34.589109, Y = -58.4101  },
                        new PuntoViewModel {X = -34.591529, Y = -58.407353},
                        new PuntoViewModel {X = -34.59319 , Y = -58.405358},
                        new PuntoViewModel {X = -34.594003, Y = -58.403748},
                        new PuntoViewModel {X = -34.594551, Y = -58.401946},
                        new PuntoViewModel {X = -34.594921, Y = -58.401411},
                        new PuntoViewModel {X = -34.595452, Y = -58.401217},
                        new PuntoViewModel {X = -34.596865, Y = -58.400959},
                        new PuntoViewModel {X = -34.598171, Y = -58.400702},
                        new PuntoViewModel {X = -34.599089, Y = -58.399886},
                        new PuntoViewModel {X = -34.599708, Y = -58.398105},
                        new PuntoViewModel {X = -34.599584, Y = -58.393064},
                        new PuntoViewModel {X = -34.599372, Y = -58.389243},
                        new PuntoViewModel {X = -34.599513, Y = -58.387805},
                        new PuntoViewModel {X = -34.599938, Y = -58.38684 },
                        new PuntoViewModel {X = -34.600645, Y = -58.385939},
                        new PuntoViewModel {X = -34.601421, Y = -58.385167},
                        new PuntoViewModel {X = -34.60257 , Y = -58.383707},
                        new PuntoViewModel {X = -34.603965, Y = -58.380983},
                        new PuntoViewModel {X = -34.606067, Y = -58.377228},
                        new PuntoViewModel {X = -34.60786 , Y = -58.373579},
                    },
                    Color = Colors.Green,
                    Postas = new List<PuntoViewModel>
                    {
                        new PuntoViewModel { Name = "Catedral", X = -34.60786, Y = -58.373579},
                        new PuntoViewModel { Name = "9 de Julio", X = -34.603859, Y = -58.381241},
                        new PuntoViewModel { Name = "Tribunales", X = -34.602764, Y = -58.385208},
                        new PuntoViewModel { Name = "Callao", X = -34.599602, Y = -58.392979},
                        new PuntoViewModel { Name = "Facultad de Medicina", X = -34.599728, Y = -58.397224},
                        new PuntoViewModel { Name = "Pueyrredón", X = -34.594425, Y = -58.402374},
                        new PuntoViewModel { Name = "Agüero", X = -34.591656, Y = -58.407139},
                        new PuntoViewModel { Name = "Bulnes", X = -34.588085, Y = -58.411495},
                        new PuntoViewModel { Name = "Scalabrini Ortiz", X = -34.585152, Y = -58.416000},
                        new PuntoViewModel { Name = "Plaza Italia", X = -34.581249, Y = -58.421108},
                        new PuntoViewModel { Name = "Palermo", X = -34.578156, Y = -58.426601},
                        new PuntoViewModel { Name = "Ministro Carranza", X = -34.575119, Y = -58.434971},
                        new PuntoViewModel { Name = "Olleros", X = -34.570473, Y = -58.443939},
                        new PuntoViewModel { Name = "José Hernández", X = -34.566090, Y = -58.452393},
                        new PuntoViewModel { Name = "Juramento", X = -34.562042, Y = -58.456730},
                        new PuntoViewModel { Name = "Congreso de Tucumán", X = -34.555469, Y = -58.462521},
                    }
                },
                new TrackLine
                {
                    Nombre = "Linea E",
                    Trazado = new List<PuntoViewModel>
                    {
                        new PuntoViewModel {X = -34.608982, Y = -58.373451},
                        new PuntoViewModel {X = -34.609970, Y = -58.374458},
                        new PuntoViewModel {X = -34.610889, Y = -58.375488},
                        new PuntoViewModel {X = -34.612850, Y = -58.377529},
                        new PuntoViewModel {X = -34.617634, Y = -58.381584},
                        new PuntoViewModel {X = -34.622330, Y = -58.385853},
                        new PuntoViewModel {X = -34.622475, Y = -58.387161},
                        new PuntoViewModel {X = -34.622524, Y = -58.388493},
                        new PuntoViewModel {X = -34.622753, Y = -58.391563},
                        new PuntoViewModel {X = -34.622978, Y = -58.395870},
                        new PuntoViewModel {X = -34.623161, Y = -58.397892},
                        new PuntoViewModel {X = -34.623337, Y = -58.398663},
                        new PuntoViewModel {X = -34.623425, Y = -58.399437},
                        new PuntoViewModel {X = -34.623550, Y = -58.400444},
                        new PuntoViewModel {X = -34.623760, Y = -58.402184},
                        new PuntoViewModel {X = -34.623989, Y = -58.403900},
                        new PuntoViewModel {X = -34.624168, Y = -58.405640},
                        new PuntoViewModel {X = -34.624397, Y = -58.406967},
                        new PuntoViewModel {X = -34.624504, Y = -58.408020},
                        new PuntoViewModel {X = -34.624802, Y = -58.410530},
                        new PuntoViewModel {X = -34.625080, Y = -58.413059},
                        new PuntoViewModel {X = -34.625404, Y = -58.416107},
                        new PuntoViewModel {X = -34.625706, Y = -58.417953},
                        new PuntoViewModel {X = -34.625828, Y = -58.419048},
                        new PuntoViewModel {X = -34.625984, Y = -58.420143},
                        new PuntoViewModel {X = -34.626183, Y = -58.421261},
                        new PuntoViewModel {X = -34.626373, Y = -58.422783},
                        new PuntoViewModel {X = -34.626621, Y = -58.424114},
                        new PuntoViewModel {X = -34.626976, Y = -58.426796},
                        new PuntoViewModel {X = -34.627220, Y = -58.428188},
                        new PuntoViewModel {X = -34.627434, Y = -58.429607},
                        new PuntoViewModel {X = -34.627575, Y = -58.430935},
                        new PuntoViewModel {X = -34.627750, Y = -58.432095},
                        new PuntoViewModel {X = -34.627823, Y = -58.432697},
                        new PuntoViewModel {X = -34.627983, Y = -58.433876},
                        new PuntoViewModel {X = -34.631001, Y = -58.442181},
                        new PuntoViewModel {X = -34.633350, Y = -58.445354},
                        new PuntoViewModel {X = -34.635536, Y = -58.448425},
                        new PuntoViewModel {X = -34.636810, Y = -58.450420},
                        new PuntoViewModel {X = -34.640163, Y = -58.457825},
                        new PuntoViewModel {X = -34.641663, Y = -58.459904},
                        new PuntoViewModel {X = -34.642178, Y = -58.460590},
                        new PuntoViewModel {X = -34.642548, Y = -58.461170},
                        new PuntoViewModel {X = -34.643326, Y = -58.461834},
                    },
                    Color = Colors.Purple,
                    Postas = new List<PuntoViewModel>
                    {
                        new PuntoViewModel { Name = "Bolívar", X = -34.609070, Y = -58.373386},
                        new PuntoViewModel { Name = "Belgrano", X = -34.612865, Y = -58.377571},
                        new PuntoViewModel { Name = "Independencia", X = -34.617687, Y = -58.381542},
                        new PuntoViewModel { Name = "San José", X = -34.622402, Y = -58.385899},
                        new PuntoViewModel { Name = "Entre Ríos", X = -34.622753, Y = -58.391541},
                        new PuntoViewModel { Name = "Pichincha", X = -34.623234, Y = -58.397850},
                        new PuntoViewModel { Name = "Jujuy", X = -34.623798, Y = -58.402248},
                        new PuntoViewModel { Name = "General Urquiza", X = -34.624611, Y = -58.408920},
                        new PuntoViewModel { Name = "Boedo", X = -34.625439, Y = -58.416088},
                        new PuntoViewModel { Name = "Avenida La Plata", X = -34.627029, Y = -58.426773},
                        new PuntoViewModel { Name = "José María Moreno", X = -34.628052, Y = -58.433876},
                        new PuntoViewModel { Name = "Emilio Mitre", X = -34.631073, Y = -58.442139},
                        new PuntoViewModel { Name = "Medalla Milagrosa", X = -34.636879, Y = -58.450439},
                        new PuntoViewModel { Name = "Varela", X = -34.640163, Y = -58.457867},
                        new PuntoViewModel { Name = "Plaza de los Virreyes", X = -34.643360, Y = -58.461769},
                    }
                },
                new TrackLine
                {
                    Nombre = "Linea H",
                    Trazado = new List<PuntoViewModel>
                    {
                        new PuntoViewModel {X = -34.610039, Y = -58.405979},
                        new PuntoViewModel {X = -34.604530, Y = -58.405380},
                        new PuntoViewModel {X = -34.611130, Y = -58.405869},
                        new PuntoViewModel {X = -34.613022, Y = -58.405418},
                        new PuntoViewModel {X = -34.614868, Y = -58.404881},
                        new PuntoViewModel {X = -34.620220, Y = -58.403061},
                        new PuntoViewModel {X = -34.625591, Y = -58.401749},
                        new PuntoViewModel {X = -34.627800, Y = -58.401279},
                        new PuntoViewModel {X = -34.629990, Y = -58.400890},
                        new PuntoViewModel {X = -34.632069, Y = -58.400349},
                        new PuntoViewModel {X = -34.633270, Y = -58.399849},
                        new PuntoViewModel {X = -34.636028, Y = -58.399021},
                        new PuntoViewModel {X = -34.637093, Y = -58.405724},
                        new PuntoViewModel {X = -34.639210, Y = -58.411732},
                    },
                    Color = Colors.Yellow,
                    Postas = new List<PuntoViewModel>
                    {
                        new PuntoViewModel { Name = "Corrientes", X = -34.604813, Y = -58.405315},
                        new PuntoViewModel { Name = "Once", X = -34.610165, Y = -58.405895},
                        new PuntoViewModel { Name = "Venezuela", X = -34.615761, Y = -58.404610},
                        new PuntoViewModel { Name = "Humberto I", X = -34.622456, Y = -58.402504},
                        new PuntoViewModel { Name = "Inclan", X = -34.629978, Y = -58.400894},
                        new PuntoViewModel { Name = "Caseros", X = -34.635925, Y = -58.398922},
                        new PuntoViewModel { Name = "Parque Patricios", X = -34.637093, Y = -58.405807},
                        new PuntoViewModel { Name = "Hospitales", X = -34.639477, Y = -58.411709},
                    }
                },
            };

            return lineas;
        }
    }
}
