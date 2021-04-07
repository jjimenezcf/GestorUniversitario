using System;
using UtilidadesParaIu;
using ModeloDeDto;
using Enumerados;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeCreacion<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;

        public DescriptorDeMantenimiento<TElemento> Mnt => Crud.Mnt;

        public ZonaDeMenu<TElemento> MenuCreacion { get; private set; }
        public string htmlDeCreacionEspecifico { get; set; }

        public bool AbrirEnModal { set; get; }

        public DescriptorDeCreacion(DescriptorDeCrud<TElemento> crud, string etiqueta)
        : base(
          padre: crud,
          id: $"{crud.Id}_{enumTipoControl.pnlCreador.Render()}",
          etiqueta: etiqueta,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = enumTipoControl.pnlCreador;
            MenuCreacion = new ZonaDeMenu<TElemento>(creador: this);
            MenuCreacion.AnadirOpcionDeNuevoElemento();
            MenuCreacion.AnadirOpcionDeCerrarCreacion();
        }



        public override string RenderControl()
        {
            if (!Crud.NegocioActivo || !(bool)ElementoDto.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.OpcionDeCrear)))
            {
                MenuCreacion.QuitarOpcionDeMenu(TipoDeAccionDeCreacion.NuevoElemento);
            }

            string htmContenedorCreacion;
            if ( AbrirEnModal)
            {
                htmContenedorCreacion = RendelModalDeCreacion();
            }
            else
            {
               htmContenedorCreacion = 
                $@"
                   <div id=¨{IdHtml}¨ 
                         class=¨{Css.Render(enumCssDiv.DivOculto)} {Css.Render(enumCssCreacion.CuerpoDeCrearcion)}¨
                         controlador=¨{Crud.Controlador}¨>
                           {RenderContenedorDeCreacionCabecera()}
                           {RenderContenedorDeCreacionCuerpo()}
                           {RenderContenedorDeCreacionPie()}
                   </div>
                ";
            }

            return htmContenedorCreacion.Render();
        }


        private string RendelModalDeCreacion()
        {
            var htmlModal = RenderizarModal(
                idHtml: IdHtml
                , controlador: Crud.Controlador
                , tituloH2: "Creación"
                , cuerpo: RenderContenedorDeCreacionCuerpo() + RenderContenedorDeCreacionPie()
                , idOpcion: $"{IdHtml}-crear"
                , opcion: Crud.NegocioActivo ? "Crear": ""
                , accion: Crud.NegocioActivo ? "Crud.EventosModalDeCreacion('crear-elemento')": ""
                , cerrar: "Crud.EventosModalDeCreacion('cerrar-modal')"
                , navegador: htmlRenderOpciones()
                , claseBoton: enumCssOpcionMenu.DeElemento
                , permisosNecesarios: ServicioDeDatos.Seguridad.enumModoDeAccesoDeDatos.Gestor);

            return htmlModal;
        }

        private string RenderContenedorDeCreacionCabecera()
        {
            var htmlModal = $@"<div id=¨contenedor_creacion_cabecera_{IdHtml}¨ class=¨{Css.Render(enumCssEdicion.ContenedorDeEdicionCabecera)}¨>
                                 <h2>Creación</h2> 
                                 {MenuCreacion.RenderControl()}
                              </div>";
            return htmlModal;
        }

        private string RenderContenedorDeCreacionCuerpo()
        {
            var htmlModal = $@"<div id=¨contenedor_creacion_cuerpo_{IdHtml}¨ class=¨{Css.Render(enumCssEdicion.ContenedorDeEdicionCuerpo)}¨>
                                 {htmlRenderObjetoVacio()}
                               </div>";
            return htmlModal;
        }

        private string RenderContenedorDeCreacionPie()
        {
            var htmlModal = $@"<div id=¨contenedor_creacion_pie_{IdHtml}¨ class=¨{Css.Render(enumCssEdicion.ContenedorDeEdicionPie)}¨>
                                  {htmlDeCreacionEspecifico}
                                  {(AbrirEnModal ? "" : htmlRenderOpciones())}
                               </div>";
            return htmlModal;
        }


        private string htmlRenderOpciones()
        {

            var htmdDescriptorControl = $@"<input id=¨{IdHtml}-crear-mas¨ type=¨checkbox¨ checked/>
                                           <label for=¨{IdHtml}-crear-mas¨>Cerrar tras crear</label>";


            var htmContenedorPie =
                   $@"
                   <Div id=¨opciones-{IdHtml}¨ class=¨{(!AbrirEnModal ? Css.Render(enumCssCreacion.ContenedorPieOpciones) : Css.Render(enumCssCreacion.ContenedorPieModalOpciones))}¨>
                    {htmdDescriptorControl}
                  </Div>
                ";
            return htmContenedorPie;
        }

        protected virtual string htmlRenderObjetoVacio()
        {
            var tabla = new DescriptorDeTabla(typeof(TElemento), ModoDeTrabajo.Nuevo, Crud.Controlador);

            var htmlObjeto = @$"<table id=¨{tabla.IdHtml}¨ 
                                  name=¨table_propiedad¨  
                                  class=¨{Css.Render(enumCssCreacion.TablaDeCreacion)}¨>
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