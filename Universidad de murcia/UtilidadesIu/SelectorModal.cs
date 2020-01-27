using System;
using System.Collections.Generic;

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
                     {gridDeElementos}
                   </div>
                   <div class=¨modal-footer¨>
                     <button type = ¨button¨ class=¨btn btn-secondary¨ data-dismiss=¨modal¨>Cerrar</button>
                     <button type = ¨button¨ class=¨btn btn-primary¨ data-dismiss=¨modal¨ onclick=¨AlSeleccionar('{idSelector}', '{idGrid}', '{referenciaChecks}')¨>Seleccionar</button>
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
                                            AlAbrir('{IdGrid}', '{columnaId}', '{columnaMostrar}', ElementosMarcados('{idSelector}'))
                                          })
                                      ";
        const string _alCerrarLaModal = @"
                                         $('#{idModal}').on('hidden.bs.modal', function (event) {
                                            AlCerrar('{idModal}', '{idGrid}', 'referenciaChecks')
                                          })
                                      ";


        private string NombreDelObjeto { get; set; }
        
        private string _titulo;

        private string _idModal;
        private string _columnaId;
        private string _columnaMostrar;

        private string _ClaseDeElemento => $"{NombreDelObjeto.Replace("Elemento", "")}";
        private string _Ruta => $"{_ClaseDeElemento}s";

        private string IdModal => _idModal.ToLower();
        private string IdGrid => $"{IdModal}.Grid".ToLower();
        private string IdSelector => $"{IdModal}.Selector".ToLower();

        public string ColumnaId { get { return _columnaId.ToLower(); }  set { _columnaId = value; } }
        public string ColumnaMostrar { get { return _columnaMostrar.ToLower(); } set { _columnaMostrar = value; } }


        public int TotalEnBd { get; set; }
        public int PosicionInicial { get; private set; }
        public int CantidadPorLeer { get; private set; }
        
        private Func<List<ColumnaDelGrid>, (List<FilaDelGrid> filas, int totalBd)> LeerFilasParaElGrid { get; set; }
        private List<ColumnaDelGrid> DescriptorDeColumnas { get; set; }

        public SelectorModal(string nombreDelObjeto, List<ColumnaDelGrid> descriptorDeColumnas, Func<List<ColumnaDelGrid>, (List<FilaDelGrid> filas, int totalBd)> leerFilasParaElGrid, int posicionInicial, int cantidadPorLeer)
        {
            NombreDelObjeto = nombreDelObjeto;
            _idModal = $"Selector_{_ClaseDeElemento}";

            _titulo = $"Seleccionar {NombreDelObjeto}";
            DescriptorDeColumnas = descriptorDeColumnas;
            LeerFilasParaElGrid = leerFilasParaElGrid;

            PosicionInicial = posicionInicial;
            CantidadPorLeer = cantidadPorLeer;
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
            var resultado = LeerFilasParaElGrid(DescriptorDeColumnas);

            Grid grid = new Grid(IdGrid, DescriptorDeColumnas, resultado.filas, PosicionInicial, CantidadPorLeer)
            {
                Ruta = _Ruta,
                TotalEnBd = resultado.totalBd,
                ConNavegador = true,
                ConSeleccion = true
            };

            var nombreCheckDeSeleccion = $"chksel.{IdGrid}";

            return _htmlModalSelector
                    .Replace("idModal", IdModal)
                    .Replace("titulo", _titulo)
                    .Replace("{idSelector}", IdSelector)
                    .Replace("{idGrid}", IdGrid)
                    .Replace("{referenciaChecks}", $"{nombreCheckDeSeleccion}")
                    .Replace("{columnaId}",ColumnaId)
                    .Replace("{columnaMostrar}", ColumnaMostrar)
                    .Replace("{idContenedor}", $"contenedor.{IdGrid}")
                    .Replace("{gridDeElementos}",grid.ToHtml())
                    .Replace("AlAbrirLaModal",_alAbrirLaModal
                                              .Replace("{idModal}", IdModal)
                                              .Replace("{IdGrid}", IdGrid)
                                              .Replace("{columnaId}", ColumnaId)
                                              .Replace("{columnaMostrar}", ColumnaMostrar)
                                              .Replace("{idSelector}", IdSelector))
                    .Replace("AlCerrarLaModal", _alCerrarLaModal
                                              .Replace("{idModal}", IdModal)
                                              .Replace("{idGrid}", IdGrid)
                                              .Replace("referenciaChecks", $"{nombreCheckDeSeleccion}"))
                    .Render();
        }
    }
}
