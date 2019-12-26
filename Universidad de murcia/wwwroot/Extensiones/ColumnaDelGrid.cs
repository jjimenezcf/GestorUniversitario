using System;

namespace Extensiones.Html
{
    public class ColumnaDelGrid
    {
        private Aliniacion _alineada;
        private string _titulo;

        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Titulo { get { return _titulo == null ? Nombre : _titulo; } set { _titulo = value; } }
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

        internal string AlineacionCss()
        {
            switch (Alineada)
            {
                case Aliniacion.izquierda:
                    return "text-left";
                case Aliniacion.derecha:
                    return "text-right";
                case Aliniacion.centrada:
                    return "text-center";
                case Aliniacion.justificada:
                    return "text-justify";
                default:
                    return "text-left";
            }
        }

    }
}