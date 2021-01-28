using System;
using ModeloDeDto;
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

    public class ColumnaDelGrid<TElemento> where TElemento : ElementoDto
    {
        public ZonaDeDatos<TElemento> ZonaDeDatos { get; set; }

        private Aliniacion _alineada;
        private string _titulo;
        public string Propiedad { get; set; } 
        public string Id => $"{ZonaDeDatos.IdHtml}_c_tr_0.{Propiedad}";
        public string IdHtml => Id.ToLower();
        public string Titulo { get { return _titulo == null ? Propiedad : _titulo; } set { _titulo = value; } }
        public Type Tipo { get; set; } = typeof(string);
        public int PorAnchoMnt { get; set; } = 0;

        private int _PorAnchoSel;
        public int PorAnchoSel { get { return _PorAnchoSel == 0 ? PorAnchoMnt : _PorAnchoSel; } set { _PorAnchoSel = value; } }
        public bool Ordenar { get; set; } = false;

        public enumCssOrdenacion cssOrdenacion { get; set; } = enumCssOrdenacion.SinOrden;
        public bool Visible { get; set; } = true;
        public bool Editable { get; set; } = false;
        public Aliniacion Alineada
        {
            get
            {
                if (_alineada != Aliniacion.no_definida)
                    return _alineada;

                if (Tipo == typeof(int) || Tipo == typeof(decimal))
                        return Aliniacion.derecha;
                if (Tipo == typeof(DateTime))
                    return Aliniacion.centrada;

                return Aliniacion.izquierda;
            }

            set { _alineada = value; }
        }

        public string AlineacionCss => HtmlRender.AlineacionCss(Alineada);

        public HtmlDescriptorCabecera descriptor { get; set; }

        public ColumnaDelGrid()
        {
            //Visible =(bool)Elemento.ValorDelAtributo(typeof(TElemento), IUPropiedadAttribute.,  nameof(IUPropiedadAttribute.Visible));
            descriptor = new HtmlDescriptorCabecera();
        }
    }
}