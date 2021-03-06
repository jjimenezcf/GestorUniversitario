﻿using System;
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

    public class ClausulasDeConsultas
    {
       public const string ListaDeValores = nameof(ListaDeValores);
    }

    public class ConsultaSql<T> where T : IRegistro
    {
        public string Conexion;

        public string Sentencia { get; private set; }
        public bool HayQueDebugar { get; }
        public string Fichero { get; }
        public TrazaSql Traza { get; set; }

        public static string CadenaDeConexion => ContextoSe.ObtenerDatosDeConexion().CadenaConexion;

        public ConsultaSql(string sentencia) : 
            this(sentencia, false, "")
        {
        }

        public ConsultaSql(string sentencia, bool hayQueDebugar, string fichero)
        {
            Sentencia = sentencia;
            HayQueDebugar = hayQueDebugar;
            Fichero = fichero;
        }

        public ConsultaSql(TrazaSql traza, string sentencia)
        {
            Sentencia = sentencia;
            Traza = traza;
        }

        public List<T> LanzarConsulta(DynamicParameters parametros = null)
        {
            List<T> resultado = null;
            if (HayQueDebugar) Traza = TrazaSql.CrearTraza(Fichero);
            try
            {
                resultado = Lanzar(parametros);
            }
            finally
            {
              if (HayQueDebugar)  Traza.Cerrar();
            }
            return resultado;
        }

        private List<T> Lanzar(DynamicParameters parametros = null)
        {
            List<T> resultado = null;
            if (parametros == null)
                parametros = new DynamicParameters();
            SqlParameterCollection spc = new SqlCommand().Parameters;
            foreach (var nombre in parametros.ParameterNames)
            {
                var valor = parametros.Get<object>(nombre);
                spc.AddWithValue(nombre, valor);
            }

            using (IDbConnection db = new SqlConnection(CadenaDeConexion))
            {
                db.Open();
                var cronometro = new Stopwatch();
                try
                {
                    if (Traza != null)
                        cronometro.Start();

                    resultado = db.Query<T>(Sentencia, parametros).ToList();

                    if (Traza != null)
                    {
                        cronometro.Stop();
                        Traza.AnotarTrazaSql(Sentencia, spc, cronometro.ElapsedMilliseconds);
                    }
                }
                catch (Exception e)
                {
                    if (Traza != null)
                    {
                        cronometro.Stop();
                        Traza.AnotarParametros(spc);
                        Traza.AnotarExcepcion(e);
                    }
                    throw;
                }
            }
            return resultado;
        }

        public int EjecutarSentencia(DynamicParameters parametros = null)
        {
            if (HayQueDebugar) Traza = TrazaSql.CrearTraza(Fichero);
            try
            {
                return Ejecutar(parametros);
            }
            finally
            {
                if (HayQueDebugar) Traza.Cerrar();
            }
        }

        private int Ejecutar(DynamicParameters parametros = null)
        {
            int resultado = 0;

            if (parametros == null)
                parametros = new DynamicParameters();

            SqlParameterCollection spc = new SqlCommand().Parameters;
            foreach (var nombre in parametros.ParameterNames)
            {
                var valor = parametros.Get<object>(nombre);
                spc.AddWithValue(nombre, valor);
            }

            using (IDbConnection db = new SqlConnection(CadenaDeConexion))
            {
                var cronometro = new Stopwatch();
                try
                {
                    if (Traza != null)
                        cronometro.Start();
                    db.Open();
                    resultado = db.Execute(Sentencia, parametros);

                    if (Traza != null)
                    {
                        cronometro.Stop();
                        Traza.AnotarTrazaSql(Sentencia, spc, cronometro.ElapsedMilliseconds);
                    }
                }
                catch (Exception e)
                {
                    if (Traza != null)
                    {
                        cronometro.Stop();
                        Traza.AnotarMensaje("Sentencia que se ha intentado ejecutar", Sentencia);
                        Traza.AnotarParametros(spc);
                        Traza.AnotarExcepcion(e);
                    }
                    throw;
                }
            }
            return resultado;
        }


        public void EliminarCriterio(string filtro)
        {
          Sentencia = Sentencia.Replace($"[{filtro}]", "");
        }

        public void AplicarFiltro(string filtro, string clausula )
        {
            Sentencia = Sentencia.Replace($"[{filtro}]", clausula);
        }

        public void AplicarClausulaIn(string filtro, string clausula, List<int> valores)
        {
            if (valores.Count > 0)
            {
                var lista = "";
                foreach (int valor in valores)
                    lista = lista + "," + valor.ToString();

                lista = lista.Substring(1);
                clausula = clausula.Replace($"[{ClausulasDeConsultas.ListaDeValores}]", lista);

                Sentencia = Sentencia.Replace($"[{filtro}]", clausula);
            }
            else
                EliminarCriterio(filtro);
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