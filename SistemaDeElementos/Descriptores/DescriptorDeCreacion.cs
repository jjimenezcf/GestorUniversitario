using System;
using UtilidadesParaIu;
using ModeloDeDto;

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
          id: $"{crud.Id}_{TipoControl.pnlCreador}",
          etiqueta: etiqueta,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.pnlCreador;
            MenuCreacion = new ZonaDeMenu<TElemento>(creador: this);
            MenuCreacion.AnadirOpcionDeNuevoElemento();
            MenuCreacion.AnadirOpcionDeCerrarCreacion();
        }



        public override string RenderControl()
        {
            string htmContenedorCreacion;
            if ( AbrirEnModal)
            {
                htmContenedorCreacion = RendelModal();
            }
            else
            {
                if (!Crud.NegocioActivo)
                    MenuCreacion.QuitarOpcionDeMenu(TipoDeAccionDeCreacion.NuevoElemento);

                htmContenedorCreacion = 
                $@"
                   <div id=¨{IdHtml}¨ 
                         class=¨{Css.Render(enumCssDiv.DivOculto)} {Css.Render(enumCssCreacion.CuerpoDeCrearcion)}¨
                         controlador=¨{Crud.Controlador}¨>
                         <h2>Creación</h2> 
                         {MenuCreacion.RenderControl()}
                         {RendelDivDeCreacion()}
                   </div>
                ";
            }

            return htmContenedorCreacion.Render();
        }


        private string RendelModal()
        {
            var htmlModal = RenderizarModal(
                idHtml: IdHtml
                , controlador: Crud.Controlador
                , tituloH2: "Creación"
                , cuerpo: RendelDivDeCreacion()
                , idOpcion: $"{IdHtml}-crear"
                , opcion: Crud.NegocioActivo ? "Crear": ""
                , accion: Crud.NegocioActivo ? "Crud.EventosModalDeCreacion('crear-elemento')": ""
                , cerrar: "Crud.EventosModalDeCreacion('cerrar-modal')"
                , navegador: htmlRenderOpciones()
                , claseBoton: enumCssOpcionMenu.DeElemento
                , permisosNecesarios: ServicioDeDatos.Seguridad.enumModoDeAccesoDeDatos.Gestor);

            return htmlModal;
        }


        private string RendelDivDeCreacion()
        {
            var htmlModal = $@"
                               {htmlRenderObjetoVacio()}
                               {htmlDeCreacionEspecifico}
                               {(AbrirEnModal ? "":htmlRenderOpciones())}
            ";
            return htmlModal;
        }

        private string htmlRenderOpciones()
        {

            var htmdDescriptorControl = $@"<input id=¨{IdHtml}-crear-mas¨ type=¨checkbox¨ checked/>
                                           <label for=¨{IdHtml}-crear-mas¨>Cerrar tras crear</label>";


            var htmContenedorPie =
                   $@"
                   <Div id=¨opciones-{IdHtml}¨ class=¨contenedor-opciones-creacion¨>
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