using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using ServicioDeDatos.Utilidades;

namespace ServicioDeDatos.Elemento
{
    public class RegistrosAfectados : Registro
    {
        public int cantidad { get; set; }
    }

    public class ConsultaSql<T> where T : Registro
    {
        public string Conexion;

        public string Sentencia { get; private set; }
        public TrazaSql Traza { get; set; }

        public static string CadenaDeConexion => ContextoSe.ObtenerDatosDeConexion().CadenaConexion;

        public ConsultaSql(string sentencia)
        {
            Sentencia = sentencia;
        }

        public ConsultaSql(TrazaSql traza, string sentencia)
        {
            Sentencia = sentencia;
            Traza = traza;
        }
        public List<T> LanzarConsulta()
        {
            List<T> resultado = null;


            using (IDbConnection db = new SqlConnection(CadenaDeConexion))
            {
                db.Open();
                var cronometro = new Stopwatch();
                try
                {
                    if (Traza != null)
                        cronometro.Start();

                    resultado = db.Query<T>(Sentencia).ToList();

                    if (Traza != null)
                    {
                        cronometro.Stop();
                        Traza.AnotarTrazaSql(Sentencia, null, cronometro.ElapsedMilliseconds);
                    }
                }
                catch (Exception e)
                {
                    if (Traza != null)
                    {
                        cronometro.Stop();
                        Traza.AnotarExcepcion(e);
                    }
                    throw;
                }
            }
            return resultado;
        }

        public int EjecutarConsulta()
        {
            int resultado = 0;

            using (IDbConnection db = new SqlConnection(CadenaDeConexion))
            {
                var cronometro = new Stopwatch(); 
                try
                {
                    if (Traza != null)
                        cronometro.Start();
                    db.Open();
                    resultado = db.Execute(Sentencia);

                    if (Traza != null)
                    {
                        cronometro.Stop();
                        Traza.AnotarTrazaSql(Sentencia, null, cronometro.ElapsedMilliseconds);
                    }
                }
                catch (Exception e)
                {
                    if (Traza != null)
                    {
                        cronometro.Stop();
                        Traza.AnotarMensaje("Sentencia que se ha intentado ejecutar", Sentencia);
                        Traza.AnotarExcepcion(e);
                    }
                    throw;
                }
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