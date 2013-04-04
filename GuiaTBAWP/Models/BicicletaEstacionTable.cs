﻿using System;
using System.Data.Linq.Mapping;

namespace GuiaTBAWP.Models
{
    [Table]
    public class BicicletaEstacionTable
    {
        private Guid id;
        [Column(IsPrimaryKey = true)]
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        private string nombre;
        [Column]
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private double longitud;
        [Column]
        public double Longitud
        {
            get { return longitud; }
            set { longitud = value; }
        }

        private double latitud;
        [Column]
        public double Latitud
        {
            get { return latitud; }
            set { latitud = value; }
        }

        private string _estado;
        [Column]
        public string Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }

        private int _cantidad;
        [Column]
        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        private string _horario;
        [Column]
        public string Horario
        {
            get { return _horario; }
            set { _horario = value; }
        }

        private string url;
        [Column]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string Descripcion
        {
            get { return string.Format("{0} | {1}", Horario, Cantidad); }
        }

        public BicicletaEstacionTable()
        {
        }

        public BicicletaEstacionTable(Guid id, string nombre, double longitud, double latitud, string estado, int cantidad, string horario, string url)
        {
            this.Id = id; 
            this.Nombre = nombre;
            this.Longitud = longitud;
            this.Latitud = latitud;
            this.Estado = estado;
            this.Cantidad = cantidad;
            this.Horario = horario;
            this.Url = url;
        }

    }
    
}