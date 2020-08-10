using System.Linq;
using Gestor.Errores;
using ModeloDeDto;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public class ListaDeElemento<TElemento> : ControlFiltroHtml where TElemento : ElementoDto 
    {

        public string GuardarEn { get; private set; }
        public string SeleccionarDe { get; private set; }
        public string MostrarPropiedad { get; private set; }
        public bool CargaDinamica { get; private set; }

        public ListaDeElemento(BloqueDeFitro<TElemento> padre, string propiedad, Posicion posicion)
        : base(
            padre: padre
          , id: $"{padre.Id}_{TipoControl.ListaDeElemento}_{propiedad}" 
          , ""
          , propiedad
          , ""
          , posicion
          )
        {

            var propiedades = typeof(TElemento).GetProperties();
            var p = propiedades.FirstOrDefault(x => x.Name == propiedad);
            IUPropiedadAttribute atributos = ElementoDto.ObtenerAtributos(p);

            if (atributos.Etiqueta.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.Etiqueta)} de la propiedad {propiedad}");

            if (atributos.Ayuda.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.Ayuda)} de la propiedad {propiedad}");

            if (atributos.GuardarEn.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.GuardarEn)} de la propiedad {propiedad}");

            if (atributos.SeleccionarDe.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.SeleccionarDe)} de la propiedad {propiedad}");

            Etiqueta = atributos.Etiqueta;
            Ayuda = atributos.Ayuda;
            SeleccionarDe = atributos.SeleccionarDe;
            GuardarEn = atributos.GuardarEn;
            CargaDinamica = atributos.CargaDinamica;
            MostrarPropiedad = atributos.MostrarPropiedad.IsNullOrEmpty() ? propiedad : atributos.MostrarPropiedad;

            Tipo = atributos.TipoDeControl;
            Criterio = TipoCriterio.igual.ToString();
            padre.AnadirSelectorElemento(this);
        }

        public override string RenderControl()
        {
            if (CargaDinamica)
                return RenderListaDinamica();

            return RenderListaDeElementos();
        }

        private string RenderListaDinamica()
        {
            var htmlSelect = $@"<div id=¨div-{IdHtml}¨  class=¨contenedor-selector¨>
                                    <input id=¨{IdHtml}¨ class=¨{TipoControl.ListaDinamica}¨ {RenderAtributos()} />
                                    <datalist id=¨{IdHtml}-lista¨>
                                    </datalist>
                                </div>";
            
            //var htmlSelect2 = $@"<div id=¨div-{IdHtml}-select2¨  class=¨contenedor-selector¨>
            //                        <select class=¨select2¨ data-width=¨100%¨ data-minimum-results-for-search=¨Infinity¨ />
            //                    </div>";
            //var scriptSelect2 = $@"<script>$(document).ready(function() {{$(¨.select2¨).select2();}}); </script>";

            return htmlSelect;
        }

        
        public override string RenderAtributos(string atributos = "")
        {
            atributos = base.RenderAtributos(atributos);
            atributos = $@"{atributos} clase-elemento=¨{SeleccionarDe}¨ 
                                       guardar-en=¨{GuardarEn}¨ 
                                       mostrar-propiedad=¨{MostrarPropiedad.ToLower()}¨ ";
            if (CargaDinamica)
                atributos += $@"carga-dinamica=¨{(CargaDinamica ? 'S' : 'N')}¨ 
                                oninput=¨Crud.ListaDeElementos('cargar',this)¨ 
                                placeholder=¨Seleccionar ...¨ 
                                list=¨{IdHtml}-lista¨
                               ";
            return atributos;
        }

        private string RenderListaDeElementos()
        {
            var htmlSelect = $@"<div id=¨div_{IdHtml}¨  class=¨contenedor-selector¨>
                                    <select id=¨{IdHtml}¨ class=¨{TipoControl.ListaDeElemento}¨ {RenderAtributos()} >
                                         <option value=¨0¨>Seleccionar ...</option>
                                    </select>
                                </div>";
            return htmlSelect;
        }
    }
}


