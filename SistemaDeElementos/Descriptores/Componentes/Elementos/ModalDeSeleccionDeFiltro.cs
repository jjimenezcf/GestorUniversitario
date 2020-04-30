using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ModalDeSeleccionDeFiltro<TElemento, TSeleccionado> : ControlFiltroHtml
    {
        public SelectorDeFiltro<TElemento, TSeleccionado> Selector { get; set; }
        public DescriptorDeCrud<TSeleccionado> CrudModal { get; set; }

        public string Titulo => Ayuda;

        public ModalDeSeleccionDeFiltro(SelectorDeFiltro<TElemento, TSeleccionado> selector, DescriptorDeCrud<TSeleccionado> crudModal)
        : base(
          padre: selector.Bloque,
          id: $"Modal_{selector.IdHtml}",    //{(selectorAsociado.Id.Replace("_" + TipoControl.Selector.ToString(), ""))}",
          etiqueta: $"Seleccionar {selector.propiedadParaMostrar}",
          propiedad: selector.propiedadParaMostrar,
          ayuda: selector.Ayuda,
          posicion: null
        )
        {
            Tipo = TipoControl.GridModal;
            Selector = selector;
            Selector.Modal = this;
            CrudModal = crudModal;
        }


        private string RenderGridModal()
        {
            var s = Selector;

            const string _alAbrirLaModal = @"
                                         $('#idModal').on('show.bs.modal', function (event) {
                                            AlAbrir('{IdGrid}', '{idSelector}', '{columnaId}', '{columnaMostrar}')
                                          })
                                      ";
            const string _alCerrarLaModal = @"
                                         $('#idModal').on('hidden.bs.modal', function (event) {
                                            AlCerrar('{idModal}', '{idGrid}', '{nameSelCheck}')
                                          })
                                      ";

            //
            const string _htmlModalSelector =
            @"
             <div class=¨modal fade¨ id=¨idModal¨ tabindex=¨-1¨ role=¨dialog¨ aria-labelledby=¨exampleModalLabel¨ aria-hidden=¨true¨>
               <div class=¨modal-dialog¨ role=¨document¨>
                 <div class=¨modal-content¨>
                   <div class=¨modal-header¨>
                     <h5 class=¨modal-title¨ id=¨exampleModalLabel¨>titulo</h5>
                   </div>
                   <div id=¨{idContenedor}¨ class=¨modal-body¨>
                     {gridDeElementos}
                   </div>
                   <div class=¨modal-footer¨>
                     <button type = ¨button¨ class=¨btn btn-secondary¨ data-dismiss=¨modal¨>Cerrar</button>
                     <button type = ¨button¨ class=¨btn btn-primary¨ data-dismiss=¨modal¨ onclick=¨AlSeleccionar('{idSelector}', '{idGrid}', '{nameSelCheck}')¨>Seleccionar</button>
                   </div>
                 </div>
               </div>
             </div>
             <script>
               AlAbrirLaModal
               AlCerrarLaModal
             </script>
             ";

            string _htmlMiModal = $@"<div id=¨{IdHtml}¨ class=¨contenedor-modal¨>
                              		<div id=¨{IdHtml}_contenido¨ class=¨cotenido-modal modal-seleccion¨ >
                              		    <div id=¨{IdHtml}_cabecera¨ class=¨cotenido-cabecera¨>
                              		    	titulo
                                        </div>
                              		    <div id=¨{IdHtml}_cuerpo¨ class=¨cotenido-cuerpo¨>
                              			    crudDeSeleccion
                                        </div>
                                        <div id=¨{IdHtml}_pie¨ class=¨cotenido-pie¨>
                                           <input type=¨text¨ id=¨{IdHtml}_Aceptar¨ class=¨boton-modal¨ value=¨Aceptar¨ onclick=¨Crud.EjecutarMenuMnt('borrarelemento')¨       />
                                           <input type=¨text¨ id=¨{IdHtml}_Cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨  onclick=¨Crud.EjecutarMenuMnt('cerrarmodaldeborrado')¨ />
                                        </div>
                                      </div>
                              </div>";


            var nombreCheckDeSeleccion = $"chksel.{s.IdHtml}";

            var jsAbrirModal = _alAbrirLaModal
                .Replace("idModal", IdHtml)
                .Replace("{IdGrid}", CrudModal.Mnt.Datos.IdHtml)
                .Replace("{idSelector}", s.IdHtml)
                .Replace("{columnaId}", s.propiedadParaFiltrar)
                .Replace("{columnaMostrar}", s.propiedadParaMostrar);

            var jsCerrarModal = _alCerrarLaModal
                .Replace("idModal", IdHtml)
                .Replace("{IdGrid}", CrudModal.Mnt.Datos.IdHtml)
                .Replace("{nameSelCheck}", nombreCheckDeSeleccion);


            //return _htmlModalSelector
            //        .Replace("idModal", IdHtml)
            //        .Replace("titulo", Titulo)
            //        .Replace("{idSelector}", s.IdHtml)
            //        .Replace("{idGrid}", CrudModal.Mnt.Datos.IdHtml)
            //        .Replace("{nameSelCheck}", $"{nombreCheckDeSeleccion}")
            //        .Replace("{columnaId}", s.propiedadParaFiltrar)
            //        .Replace("{columnaMostrar}", s.propiedadParaMostrar)
            //        .Replace("{idContenedor}", $"{s.Modal.IdHtml}_contenedor")
            //        .Replace("{gridDeElementos}", CrudModal.RenderControl())
            //        .Replace("AlAbrirLaModal", jsAbrirModal)
            //        .Replace("AlCerrarLaModal", jsCerrarModal)
            //        .Render();

            return _htmlMiModal
                .Replace("titulo", Titulo)
                .Replace("crudDeSeleccion", CrudModal.RenderControl());

        }

        public override string RenderControl()
        {
            return RenderGridModal();
        }
    }
}
