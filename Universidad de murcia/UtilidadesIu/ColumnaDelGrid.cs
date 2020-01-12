﻿using System;

namespace UtilidadesParaIu
{
    public class HtmlDescriptorCabecera
    {
        public string id { get; set; }
        public string propiedad { get; set; }
        public string visible { get; set; }
        public string alineada { get; set; }
        public string valor { get; set; }
    }

    public class ColumnaDelGrid
    {
        private Aliniacion _alineada;
        private string _titulo;
        private string _id;
        private string _nombre;

        public string Id { get { return _id.ToLower(); } set { _id = value; descriptor.id = _id;  } }
        public string Nombre { get { return _nombre; } set { _nombre = value; descriptor.propiedad = _nombre.ToLower(); } }
        public string Propiedad => Nombre.ToLower();
        public string Titulo { get { return _titulo == null ? _nombre : _titulo; } set { _titulo = value; } }
        public Type Tipo { get; set; } = typeof(string);
        public int Ancho { get; set; } = 0;
        public bool Ordenar { get; set; } = false;
        public string OrdenPor => $"ordenoPor{Nombre}";
        public string Sentido = "Asc";
        public bool Visible { get; set; } = true;
        public bool Editable { get; set; } = false;
        public IFormatProvider Mascara { get; set; } = null;
        public Aliniacion Alineada
        {
            get
            {
                return _alineada == Aliniacion.no_definida
                       ? (Tipo == typeof(int) || (Tipo == typeof(decimal)) || (Tipo == typeof(DateTime))
                          ? Aliniacion.derecha
                          : Aliniacion.izquierda)
                       : _alineada;
            }
            set { _alineada = value; }
        }

        public string Ruta { get; set; }
        public string Accion { get; set; }

        public string AlineacionCss => HtmlRender.AlineacionCss(Alineada);
        public string AlineacionTabulator => HtmlRender.AlineacionTabulator(Alineada);

        public HtmlDescriptorCabecera descriptor { get; set; }

        public ColumnaDelGrid()
        {
            descriptor = new HtmlDescriptorCabecera();
        }
    }
}