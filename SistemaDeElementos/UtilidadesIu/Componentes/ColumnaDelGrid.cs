using System;
using MVCSistemaDeElementos.Descriptores;

namespace UtilidadesParaIu
{
    public class HtmlDescriptorCabecera
    {
        public string id { get; set; }
        public string idHtml { get; set; }
        public string visible { get; set; }
        public string alineada { get; set; }
        public string valor { get; set; }
    }

    public class ColumnaDelGrid<TElemento>
    {
        public ZonaDeDatos<TElemento> ZonaDeDatos { get; set; }
        public string idGridHtml => ZonaDeDatos.IdHtml;

        private Aliniacion _alineada;
        private string _titulo;
        public string Propiedad { get; set; } //crud_usuario_mantenimiento_grid_c_tr_0.login
        public string Id => $"{idGridHtml}_c_tr_0.{Propiedad}";
        public string IdHtml => Id.ToLower();
        public string Titulo { get { return _titulo == null ? Propiedad : _titulo; } set { _titulo = value; } }
        public Type Tipo { get; set; } = typeof(string);
        public int PorAncho { get; set; } = 0;
        public bool Ordenar { get; set; } = false;
        public string OrdenPor => $"ordenoPor{Propiedad}";
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