using System;
using System.Collections.Generic;
using ModeloDeDto;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{

    public class BloqueDeFitro<TElemento> : ControlFiltroHtml where TElemento : ElementoDto
    {
        public TablaFiltro Tabla { get; set; }

        public ICollection<ControlFiltroHtml> Controles => Tabla.Controles;

        public bool HayExpansor { get; private set; } = false;

        public BloqueDeFitro(ZonaDeFiltro<TElemento> filtro, string titulo, Dimension dimension)
        : base(
          padre: filtro,
          id: $"{filtro.Id}_{filtro.Bloques.Count}_bloque",
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Bloque;
            Tabla = new TablaFiltro(this, dimension, new List<ControlFiltroHtml>());
            filtro.Bloques.Add(this);
        }


        public void AnadirControl(ControlFiltroHtml c)
        {
            Controles.Add(c);
        }

        public void AnadirControlEn(ControlFiltroHtml c)
        {
            Controles.Add(c);
            foreach (var control in Controles)
            {
                if (control.Id == c.Id)
                    continue;

                if (control.Posicion.fila >= c.Posicion.fila)
                {
                    if (control.Posicion.fila == c.Posicion.fila && control.Posicion.columna == c.Posicion.columna)
                       control.Posicion.fila++;
                }

                if (control.Posicion.fila >= Tabla.Dimension.Filas)
                    Tabla.Dimension.NumeroDeFilas(control.Posicion.fila + 1);
            }
        }

        public void AnadirSelectorElemento<t1>(ListaDeElemento<t1> s) where t1 : ElementoDto 
        {
            AnadirControl(s);
        }

        public void AnadirSelector<t1,t2>(SelectorDeFiltro<t1, t2> s) where t1:ElementoDto where t2:ElementoDto
        {
            AnadirControl(s);
            AnadirControl(s.Modal);
        }
        public ControlHtml ObtenerControl(string id)
        {

            foreach (ControlHtml c in Controles)
            {
                if (c.Id == id)
                    return c;
            }

            throw new Exception($"El control {id} no está en la zona de filtrado");
        }

        public string RenderModalesBloque()
        {
            var htmlModalesEnBloque = "";
            foreach (ControlHtml c in Controles)
            {
                if (c.Tipo == TipoControl.GridModal)
                    htmlModalesEnBloque =
                        $@"{htmlModalesEnBloque}{(htmlModalesEnBloque.IsNullOrEmpty() ? "" : Environment.NewLine)}" +
                        $"{c.RenderControl()}";

            }
            return htmlModalesEnBloque;
        }

        public override string RenderControl()
        {
            return $@"
                  <div id=¨mostrar.{IdHtml}¨ class=¨cuerpo-datos-filtro-bloque¨> 
                        <a id=¨mostrar.{IdHtml}.ref¨ 
                           style=¨margin-left: 10px;¨
                           href=¨javascript:Crud.{GestorDeEventos.EventosDelMantenimiento}('{TipoDeAccionDeMnt.OcultarMostrarBloque}', '{IdHtml}');¨>                           
                        bloque: {Etiqueta}
                        </a>
                        <input id=¨expandir.{IdHtml}.input¨ type=¨hidden¨ value=¨1¨> 
                        <div id=¨{IdHtml}¨  class=¨{Css.Render(enumCssDiv.DivVisible)}¨>
                          {Tabla.RenderControl()}
                        </div>
                   </div>";
        }

        internal ControlFiltroHtml BuscarControl(string propiedad)
        {
            foreach (ControlFiltroHtml c in Controles)
            {
                
                if (c.Id == $"{Id}_{c.Tipo}_{propiedad}")
                    return c;
            }
            return null;
        }
    }

}
