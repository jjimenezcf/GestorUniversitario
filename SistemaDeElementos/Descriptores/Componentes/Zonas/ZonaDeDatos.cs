using System.Collections.Generic;
using Gestor.Errores;
using ModeloDeDto;
using Utilidades;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ZonaDeDatos<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public DescriptorDeMantenimiento<TElemento> Mnt => (DescriptorDeMantenimiento<TElemento>)Padre;

        public Grid<TElemento> Grid { get; private set; }

        public List<ColumnaDelGrid<TElemento>> Columnas => Grid.columnas;

        private List<FilaDelGrid<TElemento>> Filas => Grid.filas;

        public string ExpresionElemento { get; private set; } = (string) ElementoDto.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.ExpresionNombre)); 

        public int CantidadPorLeer { get; set; } = 5;
        public int PosicionInicial { get; set; } = 0;

        public string IdHtmlModal { get; set; }

        public int TotalEnBd { get; set; }
        public ZonaDeDatos(DescriptorDeMantenimiento<TElemento> mnt)
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
                GestorDeErrores.Emitir($"Las columnas definidas para el tipo {typeof(TElemento)} sobrepasan el 100%");

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
            if (ExpresionElemento.IsNullOrEmpty())
                GestorDeErrores.Emitir($"Debe definir los campos que componen la 'exprexión del elemento' para el objeto {typeof(TElemento).Name}");

            var idHtmlZonaFiltro = ((DescriptorDeMantenimiento<TElemento>)Padre).Filtro.IdHtml;
            var htmlDiv = @$"<div id = ¨{IdHtml}¨ 
                                  class=¨ZonaDeDatos¨ 
                                  seleccionables = ¨-1¨ 
                                  seleccionados =¨¨ 
                                  zona-de-filtro = ¨{idHtmlZonaFiltro}¨ 
                                  expresion-elemento = ¨{ExpresionElemento}¨ 
                                  tabla-de-datos = ¨{Grid.IdHtmlTabla}¨ 
                                  id-modal=¨{IdHtmlModal}¨>     
                                  {Grid.ToHtml()}
                             </div>";
            return htmlDiv;
        }
        public string RenderDelGrid(ModoDescriptor modo)
        {
            var mnt = (DescriptorDeMantenimiento<TElemento>)Padre;
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
