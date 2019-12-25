using Gestor.Elementos.ModeloIu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extensiones
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
                   <div class=¨modal-body¨>
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
        private string _titulo;
        private string _idSelector;

        Func<string> _renderElementos;

        public string IdModal { get; }
        public string IdTabla => $"T_{IdModal}";

        public string ColumnaId { get; internal set; }
        public string ColumnaMostrar { get; internal set; }
        public string jsDeSeleccion { get; set; }

        public SelectorModal(string claseElemento,  Func<string> RenderElementos)
        {
            IdModal = $"SelectorDe{claseElemento}";

            _titulo = $"Seleccionar {claseElemento}";
            _idSelector = $"id{claseElemento}Seleccionado";
            _renderElementos = RenderElementos;
        }

        public string RenderSelector()
        {
            return _htmlSelector
                    .Replace("idModal", IdModal)
                    .Replace("titulo", _titulo)
                    .Replace("idSelector", _idSelector)
                    .Render();
        }

        public string RenderModal()
        {
            return _htmlModalSelector
                    .Replace("idModal", IdModal)
                    .Replace("titulo", _titulo)
                    .Replace("{idSelector}", _idSelector)
                    .Replace("{referenciaChecks}", $"chx_{IdTabla}")
                    .Replace("{columnaId}",ColumnaId)
                    .Replace("{columnaMostrar}", ColumnaMostrar)
                    .Replace("listaDeElementos", RenderizarElementos())
                    .Replace("AlAbrirLaModal",_alAbrirLaModal
                                              .Replace("{idModal}", IdModal)
                                              .Replace("{idTabla}", IdTabla)
                                              .Replace("{columnaId}", ColumnaId)
                                              .Replace("{idSelector}", _idSelector))
                    .Replace("AlCerrarLaModal", _alCerrarLaModal
                                              .Replace("{idModal}", IdModal)
                                              .Replace("referenciaChecks", $"chx_{IdTabla}"))
                    .Render();
        }
        
        private string RenderizarElementos()
        {
            return _renderElementos();
        }

    }
}
