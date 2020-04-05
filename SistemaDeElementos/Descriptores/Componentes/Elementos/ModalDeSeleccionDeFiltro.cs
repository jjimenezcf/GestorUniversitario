using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ModalDeSeleccionDeFiltro<TElemento, TSeleccionado> : ControlFiltroHtml
    {
        public SelectorDeFiltro<TElemento, TSeleccionado> Selector { get; set; }
        public DescriptorDeCrud<TSeleccionado> Descriptor { get; set; }

        public string Titulo => Ayuda;

        public ModalDeSeleccionDeFiltro(ControlHtml padre, SelectorDeFiltro<TElemento, TSeleccionado> selectorAsociado, DescriptorDeCrud<TSeleccionado> descriptor)
        : base(
          padre: padre,
          id: $"Modal_{(selectorAsociado.Id.Replace("_" + TipoControl.Selector.ToString(), ""))}",
          etiqueta: $"Seleccionar {selectorAsociado.propiedadParaMostrar}",
          propiedad: selectorAsociado.propiedadParaMostrar,
          ayuda: selectorAsociado.Ayuda,
          posicion: null
        )
        {
            Tipo = TipoControl.GridModal;
            Selector = selectorAsociado;
            Selector.Modal = this;
            Descriptor = descriptor;
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


            var nombreCheckDeSeleccion = $"chksel.{s.IdHtml}";

            var jsAbrirModal = _alAbrirLaModal
                .Replace("idModal", IdHtml)
                .Replace("{IdGrid}", Descriptor.DescriptorDeMantenimiento.Grid.IdHtml)
                .Replace("{idSelector}", s.IdHtml)
                .Replace("{columnaId}", s.propiedadParaFiltrar)
                .Replace("{columnaMostrar}", s.propiedadParaMostrar);

            var jsCerrarModal = _alCerrarLaModal
                .Replace("idModal", IdHtml)
                .Replace("{IdGrid}", Descriptor.DescriptorDeMantenimiento.Grid.IdHtml)
                .Replace("{nameSelCheck}", nombreCheckDeSeleccion);


            return _htmlModalSelector
                    .Replace("idModal", IdHtml)
                    .Replace("titulo", Titulo)
                    .Replace("{idSelector}", s.IdHtml)
                    .Replace("{idGrid}", Descriptor.DescriptorDeMantenimiento.Grid.IdHtml)
                    .Replace("{nameSelCheck}", $"{nombreCheckDeSeleccion}")
                    .Replace("{columnaId}", s.propiedadParaFiltrar)
                    .Replace("{columnaMostrar}", s.propiedadParaMostrar)
                    .Replace("{idContenedor}", $"{s.Modal.IdHtml}_contenedor")
                    .Replace("{gridDeElementos}", Descriptor.RenderControl())
                    .Replace("AlAbrirLaModal", jsAbrirModal)
                    .Replace("AlCerrarLaModal", jsCerrarModal)
                    .Render();
        }

        public override string RenderControl()
        {
            return RenderGridModal();
        }
    }
}
