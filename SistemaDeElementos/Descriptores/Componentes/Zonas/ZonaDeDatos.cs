using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ZonaDeDatos<TElemento> : ControlHtml
    {
        public DescriptorMantenimiento<TElemento> Mnt => (DescriptorMantenimiento<TElemento>)Padre;

        public Grid<TElemento> Grid { get; private set; }

        public List<ColumnaDelGrid<TElemento>> Columnas => Grid.columnas;

        public List<FilaDelGrid<TElemento>> Filas => Grid.filas;

        public int CantidadPorLeer { get; set; } = 5;
        public int PosicionInicial { get; set; } = 0;

        public int TotalEnBd { get; set; }
        public ZonaDeDatos(DescriptorMantenimiento<TElemento> mnt)
        : base(
          padre: mnt,
          id: $"{mnt.Id}_Grid",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.ZonaDeDatos;
            Grid = new Grid<TElemento>(this);
        }

        private string RenderZonaDeDatos()
        {

            var idHtmlZonaFiltro = ((DescriptorMantenimiento<TElemento>)Padre).Filtro.IdHtml;
            const string htmlDiv = @"<div id = ¨{idZonaDeDatos}¨
                                      seleccionables =2
                                      seleccionados =¨¨
                                      zonaDeFiltro = ¨{idFiltro}¨
                                     >     
                                       tabla_Navegador 
                                     </div>";
            var htmlContenedor = htmlDiv.Replace("{idZonaDeDatos}", $"{IdHtml}")
                                        .Replace("{idFiltro}", idHtmlZonaFiltro)
                                        .Replace("tabla_Navegador", Grid.ToHtml());
            return htmlContenedor;
        }

        internal void AnadirColumna(ColumnaDelGrid<TElemento> columnaDelGrid)
        {
            Mnt.Datos.Columnas.Add(columnaDelGrid);
            columnaDelGrid.ZonaDeDatos = this;
        }

        public string RenderDelGrid(ModoDescriptor modo)
        {
            var mnt = (DescriptorMantenimiento<TElemento>)Padre;
            var crud = (DescriptorDeCrud<TElemento>)mnt.Padre;
            crud.CambiarModo(modo);
            return Grid.ToHtml();
        }

        public override string RenderControl()
        {
            return RenderZonaDeDatos();
        }
    }

}
