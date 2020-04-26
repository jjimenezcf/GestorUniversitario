using System.Collections.Generic;
using MVCSistemaDeElementos.Descriptores;

namespace UtilidadesParaIu
{
    public class FilaDelGrid<TElemento>
    {
        private List<CeldaDelGrid<TElemento>> Celdas = new List<CeldaDelGrid<TElemento>>();

        public string Id => $"{Datos.Id}_d_tr_{NumeroDeFila}";
        public string IdHtml => Id.ToLower();

        public string idHtmlCheckDeSeleccion => $"{IdHtml}.chksel";
        public int NumeroDeCeldas => Celdas.Count;

        public ZonaDeDatos<TElemento> Datos { get; set; }

        public int NumeroDeFila { get; set; }

        public FilaDelGrid(ZonaDeDatos<TElemento> datos, Gestor.Elementos.ModeloIu.Elemento elemento)
        {
            Datos = datos;
            var columna = datos.ObtenerColumna("chksel");
            if (columna != null)
                AnadirCelda(new CeldaDelGrid<TElemento>(columna) { Valor = false });

            columna = datos.ObtenerColumna(nameof(elemento.Id));
            if (columna != null)
                AnadirCelda(new CeldaDelGrid<TElemento>(columna) { Valor = elemento.Id });

        }

        public void AnadirCelda(CeldaDelGrid<TElemento> celda)
        {
            foreach(var c in Celdas)
            {
                if (c.Propiedad == celda.Propiedad)
                    return;
            }

            celda.Fila = this;
            celda.NumeroCelda = Celdas.Count;
            Celdas.Add(celda);
        }

        public CeldaDelGrid<TElemento> ObtenerCelda(int i)
        {
            return Celdas[i];
        }
    }
}