using System;

namespace UtilidadesParaIu
{
    public class CeldaDelGrid
    {
        private ColumnaDelGrid _columna;
        private string _id; 

        public string IdCabecera => _columna.Id;
        public string Id { get { return _id.ToLower(); } set { _id = value; } }
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
            return _columna.AlineacionCss;
        }
    }
}