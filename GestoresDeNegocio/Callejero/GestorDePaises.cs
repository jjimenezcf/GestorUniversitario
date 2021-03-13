using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos.Entorno;
using ServicioDeDatos;
using ModeloDeDto.Entorno;
using GestorDeElementos;
using ServicioDeDatos.Callejero;
using ModeloDeDto.Callejero;
using Gestor.Errores;
using Utilidades;
using GestoresDeNegocio.TrabajosSometidos;
using System.Reflection;
using System;
using Newtonsoft.Json;
using GestoresDeNegocio.Archivos;
using ServicioDeDatos.TrabajosSometidos;

namespace GestoresDeNegocio.Callejero
{

    public class GestorDePaises : GestorDeElementos<ContextoSe, PaisDtm, PaisDto>
    {

        class Archivo
        {
            public string parametro { get; set; }
            public int valor { get; set; }
        }

        public const string ParametroPais = "csvPais";

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<PaisDtm, PaisDto>();
                CreateMap<PaisDto, PaisDtm>();
            }
        }

        public GestorDePaises(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDePaises Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePaises(contexto, mapeador); ;
        }

        public static void SometerImportarCallejero(ContextoSe contexto, string parametros)
        {
            if (parametros.IsNullOrEmpty())
                GestorDeErrores.Emitir("No se han proporcionado los parámetros para someter el trabajo de importación");

            var dll = Assembly.GetExecutingAssembly().GetName().Name;
            var clase = typeof(GestorDePaises).FullName;
            var ts = GestorDeTrabajosSometido.Obtener(contexto, "Importar callejero", dll, clase, nameof(SometerImportarCallejero).Replace("Someter", ""));
            // crear trabajo de usuario

            var tu = GestorDeTrabajosDeUsuario.Crear(contexto, ts, parametros);
            //liberarlo
        }

        public static void ImportarCallejero(EntornoDeTrabajo entorno)
        {
            var archivos = JsonConvert.DeserializeObject<List<Archivo>>(entorno.Trabajo.Parametros);
            foreach (Archivo archivo in archivos)
            {
                if (archivo.parametro.Equals(ParametroPais))
                    ImportarFicheroDePaises(entorno, archivo.valor);

            }
        }


        private static void ImportarFicheroDePaises(EntornoDeTrabajo entorno, int idArchivo)
        {
            var gestor = GestorDePaises.Gestor(entorno.contextoPr, entorno.contextoPr.Mapeador);
            var rutaFichero = GestorDocumental.DescargarArchivo(entorno.contextoPr, idArchivo);
            var fichero = new FicheroCsv(rutaFichero);
            var linea = 0;
            entorno.AnotarTraza($"Inicio del proceso");
            var idTraza = entorno.AnotarTraza($"Procesando la fila {linea}");
            foreach (var fila in fichero)
            {
                var tran = gestor.IniciarTransaccion();
                try
                {
                    linea++;
                    if (fila.EnBlanco)
                        continue;

                    if (fila.Columnas != 2)
                        throw new Exception($"la fila {linea} solo debe tener 2 columnas");

                    if (fila["A"].IsNullOrEmpty() || fila["B"].IsNullOrEmpty())
                        throw new Exception($"El contenido de la fila {linea} debe ser código y nombre del pais");

                    CrearPais(gestor, fila["A"], fila["B"]);
                    gestor.Commit(tran);
                }
                catch (Exception e)
                {
                    gestor.Rollback(tran);
                    entorno.AnotarError(e);
                }
                finally
                {
                    entorno.AnotarTraza(idTraza, $"Procesando la fila {linea}");
                }
            }

            entorno.AnotarTraza($"Procesadas un total de {linea} filas");
        }

        private static PaisDtm CrearPais(GestorDePaises gestor, string codigoPais, string NombrePais)
        {
            var pais = new PaisDtm();
            pais.Codigo = codigoPais;
            pais.Nombre = NombrePais;
            return gestor.PersistirRegistro(pais, new ParametrosDeNegocio(TipoOperacion.Insertar));
        }

    }
}
