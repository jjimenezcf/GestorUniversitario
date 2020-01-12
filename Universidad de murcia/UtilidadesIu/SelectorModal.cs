using System;

namespace UtilidadesParaIu
{
    public class SelectorModal
    {
        const string _htmlModalSelector =
            @"
             <div class=¨modal fade¨ id=¨idModal¨ tabindex=¨-1¨ role=¨dialog¨ aria-labelledby=¨exampleModalLabel¨ aria-hidden=¨true¨>
               <div class=¨modal-dialog¨ role=¨document¨>
                 <div class=¨modal-content¨>
                   <div class=¨modal-header¨>
                     <h5 class=¨modal-title¨ id=¨exampleModalLabel¨>titulo</h5>
                   </div>
                   <div id=¨{idContenedor}¨ class=¨modal-body¨>
                     listaDeElementos
                   </div>
                   <div class=¨modal-footer¨>
                     <button type = ¨button¨ class=¨btn btn-secondary¨ data-dismiss=¨modal¨>Cerrar</button>
                     <button type = ¨button¨ class=¨btn btn-primary¨ data-dismiss=¨modal¨ onclick=¨AlSeleccionar('{idSelector}', '{referenciaChecks}', '{columnaId}', '{columnaMostrar}')¨>Seleccionar</button>
                   </div>
                 </div>
               </div>
             </div>
             <script>
               AlAbrirLaModal
               AlCerrarLaModal
             </script>
             ";

        const string _htmlSelector =
              @"<div class=¨input-group mb-3¨>
                   <input id=¨idSelector¨ type = ¨text¨ class=¨form-control¨ placeholder=¨titulo¨ aria-label=¨titulo¨ aria-describedby=¨basic-addon2¨>
                   <div class=¨input-group-append¨>
                        <button class=¨btn btn-outline-secondary¨ type=¨button¨ data-toggle=¨modal¨ data-target=¨#idModal¨ >Seleccionar</button>
                   </div>
                </div>
              ";

        const string _alAbrirLaModal = @"
                                         $('#{idModal}').on('show.bs.modal', function (event) {
                                            AlAbrir('{idTabla}', '{columnaId}', ElementosMarcados('{idSelector}'))
                                          })
                                      ";
        const string _alCerrarLaModal = @"
                                         $('#{idModal}').on('hidden.bs.modal', function (event) {
                                            AlCerrar('{idModal}', 'referenciaChecks')
                                          })
                                      ";


        public string ClaseDeElemento { get; private set; }

        private string _titulo;
        private string _idSelector;

        Func<string> _funcionParaRenderizarGrid;
        private string _idModal;
        private string _columnaId;
        private string _columnaMostrar;

        public string IdModal => _idModal.ToLower();
        public string IdGrig => $"GridSel_{ClaseDeElemento}".ToLower();
        public string IdSelector => _idSelector.ToLower();

        public string ColumnaId { get { return _columnaId.ToLower(); }  set { _columnaId = value; } }
        public string ColumnaMostrar { get { return _columnaMostrar.ToLower(); } set { _columnaMostrar = value; } }

        public SelectorModal(string claseDeElemento,  Func<string> funcionParaRenderizarTabla)
        {
            ClaseDeElemento = claseDeElemento;
            _idModal = $"Selector_{ClaseDeElemento}";

            _titulo = $"Seleccionar {ClaseDeElemento}";
            _idSelector = $"id_{ClaseDeElemento}_Seleccionado";
            _funcionParaRenderizarGrid = funcionParaRenderizarTabla;
        }

        public string RenderSelector()
        {
            return _htmlSelector
                    .Replace("idModal", IdModal)
                    .Replace("titulo", _titulo)
                    .Replace("idSelector", IdSelector)
                    .Render();
        }

        public string RenderModal()
        {
            return _htmlModalSelector
                    .Replace("idModal", IdModal)
                    .Replace("titulo", _titulo)
                    .Replace("{idSelector}", IdSelector)
                    .Replace("{referenciaChecks}", $"chk_{IdGrig}")
                    .Replace("{columnaId}",ColumnaId)
                    .Replace("{columnaMostrar}", ColumnaMostrar)
                    .Replace("{idContenedor}", $"contenedor_{IdGrig}")
                    .Replace("listaDeElementos", RenderTablaDeSeleccion())
                    .Replace("AlAbrirLaModal",_alAbrirLaModal
                                              .Replace("{idModal}", IdModal)
                                              .Replace("{idTabla}", IdGrig)
                                              .Replace("{columnaId}", ColumnaId)
                                              .Replace("{idSelector}", IdSelector))
                    .Replace("AlCerrarLaModal", _alCerrarLaModal
                                              .Replace("{idModal}", IdModal)
                                              .Replace("referenciaChecks", $"chk_{IdGrig}"))
                    .Render();
        }
        
        private string RenderTablaDeSeleccion()
        {
            return _funcionParaRenderizarGrid();
        }

    }
}
