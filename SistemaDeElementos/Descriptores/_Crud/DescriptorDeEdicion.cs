using System;
using System.Collections.Generic;
using Enumerados;
using ModeloDeDto;
using ServicioDeDatos.Seguridad;
using Utilidades;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeEdicion<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public DescriptorDeMantenimiento<TElemento> Mnt => Crud.Mnt;
        public ZonaDeMenu<TElemento> MenuDeEdicion { get; private set; }
        public bool AbrirEnModal { set; get; }

        public List<DescriptorDeExpansor> Expanes = new List<DescriptorDeExpansor>();


        public DescriptorDeEdicion(DescriptorDeCrud<TElemento> crud, string etiqueta)
        : base(
          padre: crud,
          id: $"{crud.Id}_{enumTipoControl.pnlEditor.Render()}",
          etiqueta: etiqueta,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = enumTipoControl.pnlEditor;

            MenuDeEdicion = new ZonaDeMenu<TElemento>(editor: this);
            MenuDeEdicion.AnadirOpcionDeModificarElemento();
            MenuDeEdicion.AnadirOpcionDeCancelarEdicion();
        }

        public string RenderDeEdicion()
        {
            return RenderControl();
        }

        public override string RenderControl()
        {
            var tabla = new DescriptorDeTabla(typeof(TElemento), enumModoDeTrabajo.Edicion, Crud.Controlador);
            string htmContenedorEdt;
            if (AbrirEnModal)
            {
                htmContenedorEdt = RendelModal(tabla);
            }
            else
            {
                if (!Crud.NegocioActivo)
                    MenuDeEdicion.QuitarOpcionDeMenu(TipoDeAccionDeEdicion.ModificarElemento);

                htmContenedorEdt =
                $@"
                   <div id=¨{IdHtml}¨ 
                        class=¨{Css.Render(enumCssDiv.DivOculto)} {Css.Render(enumCssEdicion.CuerpoDeEdicion)}¨
                        controlador=¨{Crud.Controlador}¨>
                           {RenderContenedorDeEdicionCabecera()}
                           {RenderContenedorDeEdicionCuerpo(tabla)}
                           {RenderContenedorDeEdicionPie(tabla)}
                   </div>
                ";
            }

            return htmContenedorEdt.Render();
        }

        private string RendelModal(DescriptorDeTabla tabla)
        {
            var htmlModal = RenderizarModal(
                idHtml: IdHtml
                , controlador: Crud.Controlador
                , tituloH2: "Edición"
                , cuerpo: RenderContenedorDeEdicionCuerpo(tabla) + RenderContenedorDeEdicionPie(tabla)
                , idOpcion: $"{IdHtml}-modificar"
                , opcion: Crud.NegocioActivo ? "Modificar" : ""
                , accion: Crud.NegocioActivo ? $"Crud.EventosModalDeEdicion('{TipoDeAccionDeEdicion.ModificarElemento}')" : ""
                , cerrar: "Crud.EventosModalDeEdicion('cerrar-modal')"
                , navegador: HtmlRenderNavegadorDeSeleccionados()
                , claseBoton: enumCssOpcionMenu.DeElemento
                , permisosNecesarios: enumModoDeAccesoDeDatos.Gestor);

            return htmlModal;
        }

        private string RenderContenedorDeEdicionCabecera()
        {
            var htmlModal = $@"<div id=¨contenedor_edicion_cabecera_{IdHtml}¨ class=¨{Css.Render(enumCssEdicion.ContenedorDeEdicionCabecera)}¨>
                           <h2>Edición</h2> 
                              {MenuDeEdicion.RenderControl()}
                           </div>";
            return htmlModal;
        }

        private string RenderContenedorDeEdicionCuerpo(DescriptorDeTabla tabla)
        {
            var htmlExpanes = RenderExpanes();
            var htmlModal = $@"<div id=¨contenedor_edicion_cuerpo_{IdHtml}¨ class=¨{Css.Render(enumCssEdicion.ContenedorDeEdicionCuerpo)}¨>
                                 {htmlRenderObjetoVacio(tabla)}
                                 {htmlExpanes}
                               </div>";
            return htmlModal;
        }

        private string RenderExpanes()
        {
            string html = "";
            foreach(var expan in Expanes)
            {
                html = $"{(html.IsNullOrEmpty()? "": html + Environment.NewLine)}{expan.RenderControl()}";
            }
            return html;
        }

        private string RenderContenedorDeEdicionPie(DescriptorDeTabla tabla)
        {
            var htmlModal = $@"<div id=¨contenedor_edicion_pie_{IdHtml}¨ class=¨{Css.Render(enumCssEdicion.ContenedorDeEdicionPie)}¨>
                                  {htmlRenderPie(tabla)}
                                  {(AbrirEnModal ? "" : HtmlRenderNavegadorDeSeleccionados())}
                               </div>";
            return htmlModal;
        }

        private string HtmlRenderNavegadorDeSeleccionados()
        {
            var clase = AbrirEnModal ? "contenido-modal-pie-navegador" : "contenedor-pie-navegador";
            var htmlNavegadorGrid = $@"
                <div id= ¨pie-edicion-{IdHtml}-navegador¨ class = ¨{clase}¨>
                        <img src=¨/images/paginaInicial.png¨ alt=¨Primera página¨ title=¨Primer elemento¨ onclick=¨Crud.EventosDeEdicion('mostrar-primero')¨>

                        <input type=¨text¨ 
                               id=¨{IdHtml}-posicionador¨ 
                               value=¨0¨ 
                               title=¨Elemento editado¨
                               readonly/>

                        <input type=¨text¨ 
                               id=¨{IdHtml}-total-seleccionados¨ 
                               value=¨0¨ 
                               title=¨Elementos seleccionados¨
                               readonly/>

                        <img src=¨/images/paginaAnterior.png¨ alt=¨Primera página¨ title=¨Elemento anterior¨ onclick=¨Crud.EventosDeEdicion('mostrar-anterior')¨>
                        <img src=¨/images/paginaSiguiente.png¨ alt=¨Siguiente página¨ title=¨Elemento siguiente¨ onclick=¨Crud.EventosDeEdicion('mostrar-siguiente')¨>
                        <img src=¨/images/paginaUltima.png¨ alt=¨Última página¨ title=¨Último elemento¨ onclick=¨Crud.EventosDeEdicion('mostrar-ultimo')¨>
                </div>
            ";

            return htmlNavegadorGrid;
        }

        private object htmlRenderPie(DescriptorDeTabla tabla)
        {
            var htmContenedorPie =
                   $@"
                   <Div id=¨{IdHtml}¨ class=¨{Css.Render(enumCssEdicion.ContenedorId)}¨>
                     {RenderInputId(tabla)}
                  </Div>
                ";
            return htmContenedorPie;
        }

        private string RenderInputId(DescriptorDeTabla tabla)
        {
            var htmdDescriptorControl = $@"<input id=¨{tabla.IdHtml}_idElemento¨ 
                                             propiedad=¨{nameof(ElementoDto.Id).ToLower()}¨ 
                                             class=¨propiedad propiedad-id¨ 
                                             tipo=¨{enumTipoControl.Editor.Render()}¨ 
                                             type=¨text¨ 
                                             readonly
                                             value=¨¨>
                                           </input >";
            return htmdDescriptorControl;
        }

        protected virtual string htmlRenderObjetoVacio(DescriptorDeTabla tabla)
        {
            /*
             * */

            var htmlObjeto = @$"<table id=¨{tabla.IdHtml}¨ name=¨table_propiedad¨  class=¨{Css.Render(enumCssEdicion.TablaDeEdicion)}¨>
                                  <tbody class=¨{Css.Render(enumCssEdicion.CuerpoDeTablaDeEdicion)}¨>
                                     htmlFilas
                                  </tbody>
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