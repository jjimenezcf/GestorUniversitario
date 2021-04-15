using System.Collections.Generic;
using System.Reflection;
using Enumerados;
using Gestor.Errores;
using ModeloDeDto;
using ServicioDeDatos.Elemento;
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

        public int CantidadPorLeer { get; set; } = 10;
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
            Tipo = enumTipoControl.ZonaDeDatos;
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
            var mostrarExpresion = $"{(string)ElementoDto.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.MostrarExpresion))}"; 

            var expresionElemento = typeof(TElemento).GetField("ExpresionElemento");
            if (expresionElemento != null)
                mostrarExpresion = $"[{expresionElemento.GetValue(typeof(TElemento))}]";
            else
            if (typeof(TElemento).BaseType.Name != nameof(ElementoDto) && typeof(TElemento).BaseType.Name != nameof(AuditoriaDto))
                GestorDeErrores.Emitir($"Debe definir los campos que componen la 'exprexión del elemento' para el objeto {typeof(TElemento).Name}");


            var idHtmlZonaFiltro = ((DescriptorDeMantenimiento<TElemento>)Padre).Filtro.IdHtml;
            var htmlDiv = @$" <!-- ********************  grid de datos ******************** -->
                              <div id = ¨{IdHtml}¨ 
                                  class=¨{Css.Render(enumCssCuerpo.CuerpoDatosGrid)}¨ 
                                  seleccionables = ¨-1¨ 
                                  zona-de-filtro = ¨{idHtmlZonaFiltro}¨ 
                                  expresion-elemento = ¨{mostrarExpresion.ToLower()}¨ 
                                  tabla-de-datos = ¨{Grid.IdHtmlTabla}¨ 
                                  zona-de-navegador = ¨{Mnt.IdHtmlZonaNavegador}¨
                                  cabecera-de-tabla = ¨{Grid.IdHtmlCabeceraDeTabla}¨
                                  {(IdHtmlModal.IsNullOrEmpty() ? "" :$"id-modal=¨{IdHtmlModal}¨")}>     
                                  {Grid.ToHtml()}
                             </div>";
            return htmlDiv;
        }

        public override string RenderControl()
        {
            return RenderZonaDeDatos();
        }
    }

}
