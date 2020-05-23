using System;
using System.Collections.Generic;
using System.Reflection;
using UtilidadesParaIu;
using Gestor.Elementos.ModeloIu;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeCreacion<TElemento> : ControlHtml where TElemento : Elemento
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public BarraDeMenu<TElemento> MenuCreacion { get; private set; }
        public string htmlDeCreacionEspecifico { get; set; }

        public DescriptorDeCreacion(DescriptorDeCrud<TElemento> crud, string etiqueta)
        : base(
          padre: crud,
          id: $"{crud.Id}_{TipoControl.pnlCreador}",
          etiqueta: etiqueta,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.pnlCreador;
            MenuCreacion = new BarraDeMenu<TElemento>(creador: this);
            MenuCreacion.AnadirOpcionDeNuevoElemento();
            MenuCreacion.AnadirOpcionDeCancelarNuevo();
        }



        public override string RenderControl()
        {
            var htmContenedorCreacion =
                $@"
                   <Div id=¨{IdHtml}¨ class=¨div-no-visible¨ controlador=¨{Crud.Controlador}¨>
                     <h2>Div de creación</h2>
                     {MenuCreacion.RenderControl()}
                     {htmlRenderObjetoVacio()}
                     {htmlDeCreacionEspecifico}
                   </Div>
                ";

            return htmContenedorCreacion.Render();
        }

        

        protected virtual string htmlRenderObjetoVacio()
        {
            var tabla = new DescriptorDeTabla(typeof(TElemento), ModoDeTrabajo.Nuevo, Crud.Controlador);

            var htmlObjeto = @$"<table id=¨{tabla.IdHtml}¨ name=¨table_propiedad¨  class=¨tabla-creacion¨>
                                  htmlFilas
                                </table>
                               ";

            var htmlFilas = "";

            for (short i = 0; i < tabla.NumeroDeFilas; i++)
            {
                htmlFilas = htmlFilas + Environment.NewLine + RenderDto<TElemento>.RenderFilaParaElDto(tabla, i);
            }

            return htmlObjeto.Replace("htmlFilas", $"{htmlFilas}");
        }





    }
}