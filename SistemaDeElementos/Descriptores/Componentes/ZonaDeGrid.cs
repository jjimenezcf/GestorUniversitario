using System.Collections.Generic;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ZonaDeGrid<TElemento> : ControlHtml
    {
        public List<ColumnaDelGrid> Columnas { get; private set; } = new List<ColumnaDelGrid>();

        public List<FilaDelGrid> Filas { get; private set; } = new List<FilaDelGrid>();

        public int CantidadPorLeer { get; set; } = 5;
        public int PosicionInicial { get; set; } = 0;

        public int TotalEnBd { get; set; }
        public ZonaDeGrid(DescriptorDeCrud<TElemento> padre)
        : base(
          padre: padre,
          id: $"{padre.Id}_Grid",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.ZonaDeGrid;
        }

        private string RenderGrid()
        {

            var idHtmlZonaFiltro = ((DescriptorDeCrud<TElemento>)Padre).Filtro.IdHtml;
            const string htmlDiv = @"<div id = ¨{idGrid}¨
                                      seleccionables =2
                                      seleccionados =¨¨
                                      zonaDeFiltro = ¨{idFiltro}¨
                                     >     
                                       tabla_Navegador 
                                     </div>";
            var htmlContenedor = htmlDiv.Replace("{idGrid}", $"{IdHtml}")
                                        .Replace("{idFiltro}", idHtmlZonaFiltro)
                                        .Replace("tabla_Navegador", RenderDelGrid());
            return htmlContenedor;
        }

        public string RenderDelGrid()
        {
            var grid = new Grid(IdHtml, Columnas, Filas, PosicionInicial, CantidadPorLeer)
            {
                Controlador = ((DescriptorDeCrud<TElemento>)Padre).Controlador,
                TotalEnBd = TotalEnBd
            };
            var htmlGrid = grid.ToHtml();
            return htmlGrid.Render();
        }

        public override string RenderControl()
        {
            return RenderGrid();
        }
    }

}
