using System;

namespace UtilidadesParaIu
{
    public class CeldaDelGrid
    {
        private ColumnaDelGrid _columna;
        public string IdCabecera => _columna.Id;
        public string Id { get; set; }
        public object Valor { get; set; }
        public Type Tipo => _columna.Tipo;
        public bool Visible => _columna.Visible;
        public bool Editable => _columna.Editable;

        public string AlEntrar { get; set; }
        public string AlSalir { get; set; }
        public string AlCambiar { get; set; }

        public CeldaDelGrid(ColumnaDelGrid columna)
        {
            _columna = columna;
        }

        internal string AlineacionCss()
        {
            return _columna.AlineacionCss();
        }
    }
}