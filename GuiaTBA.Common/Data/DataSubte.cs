﻿using System.Collections.Generic;
using System.Windows.Media;
using GuiaTBAWP.Commons.Models;
using GuiaTBAWP.Commons.ViewModels;

namespace GuiaTBAWP.Commons.Data
{
    public class DataSubte
    {
        public static List<TrackLine> GetData()
        {
            var lineas = new List<TrackLine>
            {
                new TrackLine
                {
                    Nombre = "Linea A",
                    Trazado = new List<PuntoViewModel>
                    {
                        new PuntoViewModel {X = -34.630737, Y = -58.469753},
                        new PuntoViewModel {X = -34.629181, Y = -58.463917},
                        new PuntoViewModel {X = -34.625790, Y = -58.454731},
                        new PuntoViewModel {X = -34.624870, Y = -58.452579},
                        new PuntoViewModel {X = -34.623390, Y = -58.448631},
                        new PuntoViewModel {X = -34.622120, Y = -58.445030},
                        new PuntoViewModel {X = -34.620491, Y = -58.441509},
                        new PuntoViewModel {X = -34.616638, Y = -58.432709},
                        new PuntoViewModel {X = -34.616249, Y = -58.431999},
                        new PuntoViewModel {X = -34.615440, Y = -58.430050},
                        new PuntoViewModel {X = -34.613731, Y = -58.426170},
                        new PuntoViewModel {X = -34.611450, Y = -58.421169},
                        new PuntoViewModel {X = -34.611149, Y = -58.420120},
                        new PuntoViewModel {X = -34.610069, Y = -58.405998},
                        new PuntoViewModel {X = -34.609348, Y = -58.394390},
                        new PuntoViewModel {X = -34.609138, Y = -58.392200},
                        new PuntoViewModel {X = -34.609428, Y = -58.386559},
                        new PuntoViewModel {X = -34.608528, Y = -58.374950},
                        new PuntoViewModel {X = -34.608910, Y = -58.372162},
                    },
                    Color = Colors.Cyan,
                    Postas = new List<PuntoViewModel>
                    {
                        new PuntoViewModel { Name = "Plaza de Mayo", X = -34.608982, Y = -58.372166},
                        new PuntoViewModel { Name = "Perú", X = -34.608627, Y = -58.374973},
                        new PuntoViewModel { Name = "Piedras", X = -34.608788, Y = -58.377831},
                        new PuntoViewModel { Name = "Lima", X = -34.609104, Y = -58.381969},
                        new PuntoViewModel { Name = "Saenz Peña", X = -34.609512, Y = -58.387272},
                        new PuntoViewModel { Name = "Congreso", X = -34.609226, Y = -58.391991},
                        new PuntoViewModel { Name = "Pasco", X = -34.609600, Y = -58.397591},
                        new PuntoViewModel { Name = "Alberti", X = -34.609917, Y = -58.401840},
                        new PuntoViewModel { Name = "Plaza Miserere", X = -34.610077, Y = -58.406109},
                        new PuntoViewModel { Name = "Loria", X = -34.610729, Y = -58.414436},
                        new PuntoViewModel { Name = "Castro Barros", X = -34.611435, Y = -58.421085},
                        new PuntoViewModel { Name = "Río de Janeiro", X = -34.615425, Y = -58.430058},
                        new PuntoViewModel { Name = "Acoyte", X = -34.618340, Y = -58.436474},
                        new PuntoViewModel { Name = "Primera Junta", X = -34.620567, Y = -58.441429},
                        new PuntoViewModel { Name = "Puan", X = -34.623550, Y = -58.448681},
                        new PuntoViewModel { Name = "Carabobo", X = -34.626568, Y = -58.456879},
                        new PuntoViewModel { Name = "San José de Flores", X = -34.629272, Y = -58.464153},
                        new PuntoViewModel { Name = "San Pedrito", X = -34.630772, Y = -58.469711},
                    }
                },
                new TrackLine
                {
                    Nombre = "Linea B",
                    Trazado = new List<PuntoViewModel>
                    {
                        new PuntoViewModel {X = -34.602951, Y = -58.370220},
                        new PuntoViewModel {X = -34.603249, Y = -58.374062},
                        new PuntoViewModel {X = -34.603619, Y = -58.380482},
                        new PuntoViewModel {X = -34.603680, Y = -58.380970},
                        new PuntoViewModel {X = -34.603600, Y = -58.381569},
                        new PuntoViewModel {X = -34.603748, Y = -58.382648},
                        new PuntoViewModel {X = -34.604149, Y = -58.388721},
                        new PuntoViewModel {X = -34.604469, Y = -58.392269},
                        new PuntoViewModel {X = -34.604439, Y = -58.393242},
                        new PuntoViewModel {X = -34.604488, Y = -58.396172},
                        new PuntoViewModel {X = -34.604752, Y = -58.403622},
                        new PuntoViewModel {X = -34.604511, Y = -58.406109},
                        new PuntoViewModel {X = -34.604080, Y = -58.408909},
                        new PuntoViewModel {X = -34.604019, Y = -58.410782},
                        new PuntoViewModel {X = -34.604172, Y = -58.413231},
                        new PuntoViewModel {X = -34.603909, Y = -58.415291},
                        new PuntoViewModel {X = -34.603580, Y = -58.416801},
                        new PuntoViewModel {X = -34.603470, Y = -58.417770},
                        new PuntoViewModel {X = -34.602032, Y = -58.432190},
                        new PuntoViewModel {X = -34.599861, Y = -58.438770},
                        new PuntoViewModel {X = -34.596432, Y = -58.442539},
                        new PuntoViewModel {X = -34.595428, Y = -58.443539},
                        new PuntoViewModel {X = -34.593288, Y = -58.445839},
                        new PuntoViewModel {X = -34.591721, Y = -58.447651},
                        new PuntoViewModel {X = -34.587139, Y = -58.455261},
                        new PuntoViewModel {X = -34.584141, Y = -58.466171},
                        new PuntoViewModel {X = -34.583599, Y = -58.467918},
                        new PuntoViewModel {X = -34.581860, Y = -58.473221},
                        new PuntoViewModel {X = -34.578316, Y = -58.480183},
                        new PuntoViewModel {X = -34.573971, Y = -58.486835},
                    },
                    Color = Colors.Red,
                    Postas = new List<PuntoViewModel>
                    {
                        new PuntoViewModel { Name = "Leandro N. Alem", X = -34.603012, Y = -58.370190},
                        new PuntoViewModel { Name = "Florida", X = -34.603313, Y = -58.375233},
                        new PuntoViewModel { Name = "Carlos Pelegrini", X = -34.603699, Y = -58.380962},
                        new PuntoViewModel { Name = "Uruguay", X = -34.604019, Y = -58.386581},
                        new PuntoViewModel { Name = "Callao", X = -34.604511, Y = -58.392269},
                        new PuntoViewModel { Name = "Pasteur", X = -34.604652, Y = -58.399479},
                        new PuntoViewModel { Name = "Pueyrredón", X = -34.604603, Y = -58.405529},
                        new PuntoViewModel { Name = "Carlos Gardel", X = -34.604088, Y = -58.411774},
                        new PuntoViewModel { Name = "Medrano", X = -34.603207, Y = -58.421001},
                        new PuntoViewModel { Name = "Ángel Gallardo", X = -34.602039, Y = -58.432137},
                        new PuntoViewModel { Name = "Malabia", X = -34.599018, Y = -58.439796},
                        new PuntoViewModel { Name = "Dorrego", X = -34.591778, Y = -58.447651},
                        new PuntoViewModel { Name = "Federico Lacroze", X = -34.587132, Y = -58.455269},
                        new PuntoViewModel { Name = "Tronador", X = -34.584076, Y = -58.466297},
                        new PuntoViewModel { Name = "Los Incas", X = -34.581303, Y = -58.474300},
                        new PuntoViewModel { Name = "Echeverría", X = -34.578403, Y = -58.480160},
                        new PuntoViewModel { Name = "Juan Manuel de Rosas", X = -34.573990, Y = -58.486813},
                    }
                },
                new TrackLine
                {
                    Nombre = "Linea C",
                    Trazado = new List<PuntoViewModel>
                    {
                        new PuntoViewModel {X = -34.592522, Y = -58.375381},
                        new PuntoViewModel {X = -34.595165, Y = -58.378536},
                        new PuntoViewModel {X = -34.596371, Y = -58.378429},
                        new PuntoViewModel {X = -34.598873, Y = -58.378323},
                        new PuntoViewModel {X = -34.600258, Y = -58.378216},
                        new PuntoViewModel {X = -34.601616, Y = -58.378151},
                        new PuntoViewModel {X = -34.602459, Y = -58.378128},
                        new PuntoViewModel {X = -34.605003, Y = -58.379654},
                        new PuntoViewModel {X = -34.609154, Y = -58.380726},
                        new PuntoViewModel {X = -34.610477, Y = -58.380619},
                        new PuntoViewModel {X = -34.611942, Y = -58.380554},
                        new PuntoViewModel {X = -34.613640, Y = -58.380466},
                        new PuntoViewModel {X = -34.616077, Y = -58.380360},
                        new PuntoViewModel {X = -34.617702, Y = -58.380276},
                        new PuntoViewModel {X = -34.620373, Y = -58.380123},
                        new PuntoViewModel {X = -34.621552, Y = -58.380081},
                        new PuntoViewModel {X = -34.622066, Y = -58.380058},
                        new PuntoViewModel {X = -34.622616, Y = -58.380341},
                        new PuntoViewModel {X = -34.627789, Y = -58.382229},
                    },
                    Color = Colors.Blue,
                    Postas = new List<PuntoViewModel>
                    {
                        new PuntoViewModel { Name = "Retiro", X = -34.592537, Y = -58.375340},
                        new PuntoViewModel { Name = "General San Martín", X = -34.595161, Y = -58.378548},
                        new PuntoViewModel { Name = "Lavalle", X = -34.602375, Y = -58.378128},
                        new PuntoViewModel { Name = "Diagonal Norte", X = -34.604866, Y = -58.379524},
                        new PuntoViewModel { Name = "Avenida de Mayo", X = -34.609016, Y = -58.380703},
                        new PuntoViewModel { Name = "Moreno", X = -34.611843, Y = -58.380554},
                        new PuntoViewModel { Name = "Independencia", X = -34.617619, Y = -58.380234},
                        new PuntoViewModel { Name = "San Juan", X = -34.622120, Y = -58.380039},
                        new PuntoViewModel { Name = "Constitución", X = -34.627804, Y = -58.382229},
                    }
                },
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
                        new PuntoViewModel {X = -34.587454, Y = -58.397295},
                        new PuntoViewModel {X = -34.587454, Y = -58.397295},
                        new PuntoViewModel {X = -34.58903, Y = -58.399661},
                        new PuntoViewModel {X = -34.58903, Y = -58.399661},
                        new PuntoViewModel {X = -34.589335999999996, Y = -58.399957},
                        new PuntoViewModel {X = -34.589335999999996, Y = -58.399957},
                        new PuntoViewModel {X = -34.59122, Y = -58.400884999999995},
                        new PuntoViewModel {X = -34.59122, Y = -58.400884999999995},
                        new PuntoViewModel {X = -34.59418, Y = -58.402205},
                        new PuntoViewModel {X = -34.59418, Y = -58.402205},
                        new PuntoViewModel {X = -34.598348, Y = -58.403684999999996},
                        new PuntoViewModel {X = -34.598348, Y = -58.403684999999996},
                        new PuntoViewModel {X = -34.601138999999996, Y = -58.404628},
                        new PuntoViewModel {X = -34.601138999999996, Y = -58.404628},
                        new PuntoViewModel {X = -34.604194, Y = -58.405422},
                        new PuntoViewModel {X = -34.607655, Y = -58.406196},
                        new PuntoViewModel {X = -34.610110999999996, Y = -58.406002},
                        new PuntoViewModel {X = -34.611276999999994, Y = -58.405895},
                        new PuntoViewModel {X = -34.614560999999995, Y = -58.404972},
                        new PuntoViewModel {X = -34.621094, Y = -58.402783},
                        new PuntoViewModel {X = -34.628687, Y = -58.401088},
                        new PuntoViewModel {X = -34.631689, Y = -58.400508},
                        new PuntoViewModel {X = -34.635926999999995, Y = -58.398943},
                        new PuntoViewModel {X = -34.636597, Y = -58.399006},
                        new PuntoViewModel {X = -34.63702, Y = -58.399414},
                        new PuntoViewModel {X = -34.63725, Y = -58.400058},
                        new PuntoViewModel {X = -34.637585, Y = -58.401839},
                        new PuntoViewModel {X = -34.637462, Y = -58.402504},
                        new PuntoViewModel {X = -34.636968, Y = -58.403298},
                        new PuntoViewModel {X = -34.636862, Y = -58.404049},
                        new PuntoViewModel {X = -34.637269, Y = -58.406279999999995},
                        new PuntoViewModel {X = -34.637577, Y = -58.408228},
                        new PuntoViewModel {X = -34.637912, Y = -58.410276999999994},
                        new PuntoViewModel {X = -34.638186999999995, Y = -58.410835999999996},
                        new PuntoViewModel {X = -34.638539, Y = -58.411276},
                        new PuntoViewModel {X = -34.639192, Y = -58.411608},
                        new PuntoViewModel {X = -34.640588, Y = -58.412219},
                        new PuntoViewModel {X = -34.641459999999995, Y = -58.412573},
                    },
                    Color = Colors.Yellow,
                    Postas = new List<PuntoViewModel>
                    {
                        new PuntoViewModel { Name = "Las Heras", X = -34.587534, Y = -58.397334},
                        new PuntoViewModel { Name = "Santa Fe", X = -34.594524, Y = -58.402403},
                        new PuntoViewModel { Name = "Córdoba", X = -34.598432, Y = -58.403658},
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
