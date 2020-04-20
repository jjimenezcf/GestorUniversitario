﻿using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ZonaDeGrid<TElemento> : ControlHtml
    {
        public DescriptorMantenimiento<TElemento> Mnt => (DescriptorMantenimiento<TElemento>)Padre;

        public List<ColumnaDelGrid> Columnas { get; private set; } = new List<ColumnaDelGrid>();

        public List<FilaDelGrid> Filas { get; private set; } = new List<FilaDelGrid>();

        public int CantidadPorLeer { get; set; } = 5;
        public int PosicionInicial { get; set; } = 0;

        public int TotalEnBd { get; set; }
        public ZonaDeGrid(DescriptorMantenimiento<TElemento> mnt)
        : base(
          padre: mnt,
          id: $"{mnt.Id}_Grid",
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

            var idHtmlZonaFiltro = ((DescriptorMantenimiento<TElemento>)Padre).Filtro.IdHtml;
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

        public string RenderDelGrid(ModoDescriptor modo)
        {
            var mnt = (DescriptorMantenimiento<TElemento>)Padre;
            var crud = (DescriptorDeCrud<TElemento>)mnt.Padre;
            crud.CambiarModo(modo);
            return RenderDelGrid();

        }

        private string RenderDelGrid()
        {
            var mnt = (DescriptorMantenimiento<TElemento>)Padre;
            var crud = (DescriptorDeCrud<TElemento>)mnt.Padre;

            var grid = new Grid<TElemento>(crud, IdHtml, Columnas, Filas, PosicionInicial, CantidadPorLeer)
            {
                Controlador = crud.Controlador,
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
