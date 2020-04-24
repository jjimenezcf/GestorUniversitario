using System.Collections.Generic;
using MVCSistemaDeElementos.Descriptores;

namespace UtilidadesParaIu
{
    public class FilaDelGrid<TElemento>
    {
        private List<CeldaDelGrid<TElemento>> Celdas = new List<CeldaDelGrid<TElemento>>();

        public string Id => $"{Datos.Id}_d_tr_{NumeroDeFila}";
        public string IdHtml => Id.ToLower();

        public int NumeroDeCeldas => Celdas.Count;

        public ZonaDeDatos<TElemento> Datos { get; set; }

        public int NumeroDeFila { get; set; }

        public void AnadirCelda(CeldaDelGrid<TElemento> celda)
        {
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