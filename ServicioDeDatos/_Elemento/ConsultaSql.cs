using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ServicioDeDatos.Elemento
{
   public class RegistrosAfectados: Registro
    {
        public int cantidad { get; set; }
    }

    public class ConsultaSql<T> where T : Registro
    {
        public string Conexion;

        public string Sentencia { get; private set; }

        public static string CadenaDeConexion => ContextoSe.ObtenerDatosDeConexion().CadenaConexion;

        public ConsultaSql(string sentencia)
        {
            Sentencia = sentencia;
        }

        public List<T> Ejecutar()
        {
            List<T> resultado = null;            


            using (IDbConnection db = new SqlConnection(CadenaDeConexion))
            {
                db.Open();
                resultado = db.Query<T>(Sentencia).ToList();
            }
            return resultado;
        }
    }
}

//Antigua forma, antes de usar Dapper
//public class ConsultaSql
//{
//    public string Select { get; private set; }
//    public int Leidos { get; set; }
//    public List<string> Columnas { get; private set; }
//    public Dictionary<int, List<object>> Registros { get; private set; } = new Dictionary<int, List<object>>();

//    private int _RegistrosPorLeer;
//    private ContextoSe _Contexto;

//    public ConsultaSql(ContextoSe contexto, string consulta)
//    {
//        Inicializar(consulta);
//        _Contexto = contexto;
//    }

//    public ConsultaSql(ContextoSe contexto, string consulta, int registrosPorLeer)
//        : this(contexto, consulta)
//    {
//        _RegistrosPorLeer = registrosPorLeer;
//        Select.Replace("Select", $"Select Top({_RegistrosPorLeer})");
//    }

//    private void Inicializar(string consulta)
//    {
//        Select = consulta;
//        Columnas = new List<string>();
//        Registros = new Dictionary<int, List<object>>();
//    }

//    public bool Ejecutar()
//    {
//        GestorDeConsultas.Seleccionar(_Contexto, this, null);
//        return true;
//    }
//}