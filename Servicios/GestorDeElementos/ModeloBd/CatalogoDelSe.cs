using System.Collections.Generic;

namespace Gestor.Elementos.ModeloBd
{
    public class CatalogoDelSe : RegistroBase
    {
        public string Catalogo { get; set; }
        public string Esquema { get; set; }
        public string Tabla { get; set; }
    }

    public class ConsultaSql
    {
        public string Select { get; private set; }
        public int Leidos { get; set; }
        public List<string> Columnas { get; private set; }
        public Dictionary<int, List<object>> Registros {get; private set; }
        private int _RegistrosPorLeer;
        
        public ConsultaSql(string consulta)
        {
            Inicializar(consulta);
        }

        public ConsultaSql(string consulta, int registrosPorLeer)
            : this(consulta)
        {
            _RegistrosPorLeer = registrosPorLeer;
        }

        private void Inicializar(string consulta)
        {
            Select = consulta;
            Columnas = new List<string>();
            Registros = new Dictionary<int, List<object>>();
        }
    }
}
