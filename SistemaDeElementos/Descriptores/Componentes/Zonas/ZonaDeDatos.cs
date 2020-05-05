using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ZonaDeDatos<TElemento> : ControlHtml where TElemento : Elemento
    {
        public DescriptorMantenimiento<TElemento> Mnt => (DescriptorMantenimiento<TElemento>)Padre;

        public Grid<TElemento> Grid { get; private set; }

        public List<ColumnaDelGrid<TElemento>> Columnas => Grid.columnas;

        private List<FilaDelGrid<TElemento>> Filas => Grid.filas;

        public string ExpresionElemento { get; set; } 

        public int CantidadPorLeer { get; set; } = 5;
        public int PosicionInicial { get; set; } = 0;

        public string IdHtmlModal { get; set; }

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

        public void AnadirFila(FilaDelGrid<TElemento> fila)
        {
            fila.Datos = this;
            fila.NumeroDeFila = Filas.Count;
            Filas.Add(fila);
        }

        internal void AnadirColumna(ColumnaDelGrid<TElemento> columnaDelGrid)
        {
            Mnt.Datos.Columnas.Add(columnaDelGrid);
            columnaDelGrid.ZonaDeDatos = this;
        }


        internal void InsertarColumna(ColumnaDelGrid<TElemento> columnaDelGrid, int posicion)
        {
            if (posicion>= Mnt.Datos.Columnas.Count || posicion == -1)
                Mnt.Datos.Columnas.Add(columnaDelGrid);
            else
                Mnt.Datos.Columnas.Insert(posicion, columnaDelGrid);
            
            columnaDelGrid.ZonaDeDatos = this;
        }
        

        internal ColumnaDelGrid<TElemento> ObtenerColumna(string nombreColumna)
        {
            for (var i = 0; i < Columnas.Count; i++)
                if (Columnas[i].Propiedad == nombreColumna)
                    return Columnas[i];

            return null;
        }


        public void CalcularAnchosColumnas()
        {
            var totalPorcentaje = 0;
            var colDefinidas = 0;
            var colSinDefinir = 0;
            foreach (var col in Columnas)
            {
                if (!col.Visible) continue;

                if (col.PorAnchoMnt == 0) colSinDefinir++;
                else
                {
                    totalPorcentaje += col.PorAnchoMnt;
                    colDefinidas++;
                }
            }

            if (totalPorcentaje > 100)
                Gestor.Errores.GestorDeErrores.Emitir($"Las columnas definidas para el tipo {typeof(TElemento)} sobrepasan el 100%");

            var porcDeReparto = 100 - totalPorcentaje;
            
            if (colSinDefinir == 0)
                return;

            var porcPorColNoDefinida = porcDeReparto / colSinDefinir;
            foreach (var col in Columnas)
            {
                if (col.PorAnchoMnt > 0 || !col.Visible) continue;

                    col.PorAnchoMnt = porcPorColNoDefinida;
                    porcDeReparto = porcDeReparto - porcPorColNoDefinida;
                
                if (porcPorColNoDefinida > porcDeReparto)
                    porcPorColNoDefinida = porcDeReparto;
            }
        }
        private string RenderZonaDeDatos()
        {

            var idHtmlZonaFiltro = ((DescriptorMantenimiento<TElemento>)Padre).Filtro.IdHtml;
            const string htmlDiv = @"<div id = ¨{idZonaDeDatos}¨ class=¨ZonaDeDatos¨ seleccionables = 2 seleccionados =¨¨ zonaDeFiltro = ¨{idFiltro}¨ expresion-elemento = ¨{expresion}¨>     
                                       tabla_Navegador 
                                     </div>";
            var htmlContenedor = htmlDiv.Replace("{idZonaDeDatos}", $"{IdHtml}")
                                        .Replace("{idFiltro}", idHtmlZonaFiltro)
                                        .Replace("{expresion}", ExpresionElemento)
                                        .Replace("tabla_Navegador", Grid.ToHtml());
            return htmlContenedor;
        }
        public string RenderDelGrid(ModoDescriptor modo)
        {
            var mnt = (DescriptorMantenimiento<TElemento>)Padre;
            var crud = (DescriptorDeCrud<TElemento>)mnt.Padre;
            crud.CambiarModo(modo);
            return Grid.ToHtml();
        }

        public string RenderDelGridModal (string idModal)
        {
            var mnt = (DescriptorMantenimiento<TElemento>)Padre;
            var crud = (DescriptorDeCrud<TElemento>)mnt.Padre;
            crud.CambiarModo(ModoDescriptor.Seleccion);
            crud.Mnt.Datos.IdHtmlModal = idModal;
            return Grid.ToHtml();
        }

        public override string RenderControl()
        {
            return RenderZonaDeDatos();
        }
    }

}
