using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Elemento
{
    public class Registro
    {
        [Key]
        [Column("ID", Order = 1, TypeName = "INT")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }

    public class ConsultaSql
    {
        public string Select { get; private set; }
        public int Leidos { get; set; }
        public List<string> Columnas { get; private set; }
        public Dictionary<int, List<object>> Registros { get; private set; } = new Dictionary<int, List<object>>();

        private int _RegistrosPorLeer;
        private ContextoDeElementos _Contexto;

        public ConsultaSql(ContextoDeElementos contexto,string consulta)
        {
            Inicializar(consulta);
            _Contexto = contexto;
        }

        public ConsultaSql(ContextoDeElementos contexto, string consulta, int registrosPorLeer)
            : this(contexto, consulta)
        {
            _RegistrosPorLeer = registrosPorLeer;
            Select.Replace("Select", $"Select Top({_RegistrosPorLeer})");
        }

        private void Inicializar(string consulta)
        {
            Select = consulta;
            Columnas = new List<string>();
            Registros = new Dictionary<int, List<object>>();
        }

        public bool Ejecutar()
        {
            GestorDeConsultas.Seleccionar(_Contexto, this, null);
            return true;
        }
    }
}