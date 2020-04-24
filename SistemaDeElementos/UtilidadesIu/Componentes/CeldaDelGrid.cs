using System;

namespace UtilidadesParaIu
{
    public class CeldaDelGrid<TElemento>
    {
        private ColumnaDelGrid<TElemento> _columna;

        public FilaDelGrid<TElemento> Fila { get; set;}

        public string Id => $"{Fila.IdHtml}.{Propiedad}";

        public string idTdHtml => $"{Fila.IdHtml}.{NumeroCelda}".ToLower();
        
        public string idHtml => Id.ToLower();

        public int NumeroCelda { get; set; }

        public string Propiedad => _columna.Propiedad;
        public object Valor { get; set; }
        public Type Tipo => _columna.Tipo;
        public bool Visible => _columna.Visible;
        public bool Editable => _columna.Editable;

        public string AlEntrar { get; set; }
        public string AlSalir { get; set; }
        public string AlCambiar { get; set; }

        public CeldaDelGrid(ColumnaDelGrid<TElemento> columna)
        {
            _columna = columna;
        }

        internal string AlineacionCss()
        {
            return _columna.AlineacionCss;
        }
    }
}