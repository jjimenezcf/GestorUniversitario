using AutoMapper;
using System.IO;
using ServicioDeDatos.Archivos;
using ServicioDeDatos;
using System;
using ModeloDeDto.Archivos;
using ModeloDeDto.Entorno;
using GestorDeElementos;
using GestoresDeNegocio.Entorno;
using System.Collections.Generic;
using ServicioDeDatos.Elemento;
using ModeloDeDto;
using Utilidades;
using ServicioDeDatos.Entorno;
using Gestor.Errores;
using System.Reflection;
using GestoresDeNegocio.TrabajosSometidos;
using Newtonsoft.Json;
using Enumerados;
using System.Linq;

namespace GestoresDeNegocio.Archivos
{
    public class GestorDocumental : GestorDeElementos<ContextoSe, ArchivoDtm, ArchivosDto>
    {
        public class MapearArchivos : Profile
        {
            public MapearArchivos()
            {
                CreateMap<ArchivoDtm, ArchivosDto>();
                CreateMap<ArchivosDto, ArchivoDtm>();
            }
        }


        public static int SubirArchivo(ContextoSe contexto, string rutaConFichero, IMapper mapeador)
        {
            var gestor = Gestor(contexto, mapeador);
            return gestor.SubirArchivoInterno(rutaConFichero);
        }

        public static string DescargarArchivo(ContextoSe contexto, int idArchivo)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.DescargarArchivoInterno(idArchivo);
        }


        public GestorDocumental(ContextoSe contexto, IMapper mapeador) 
        : base(contexto, mapeador)
        {
        }


        private static GestorDocumental Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDocumental(contexto, mapeador);
        }


        private string DescargarArchivoInterno(int idArchivo)
        {
            var archivo = LeerRegistroPorId(idArchivo);
            var rutaConFichero =  GestorDeElementos.Utilidades.DescargarArchivo(archivo.Id, archivo.Nombre, archivo.AlmacenadoEn);
            return rutaConFichero;
        }

        private int SubirArchivoInterno(string rutaConFichero)
        {
            var servidorDeArchivos = GestorDeVariables.LeerVariable(Contexto, Variable.Servidor_Archivos, true);

            if (!Directory.Exists(servidorDeArchivos.Valor))
                throw new Exception($"La ruta del servidor documental {servidorDeArchivos.Valor} asignada a la variable {Variable.Servidor_Archivos} no está definida");

            var fecha = DateTime.Now;
            var almacenarEn = $@"{servidorDeArchivos.Valor}\{fecha.Year}\{fecha.Month}\{fecha.Day}\{fecha.Hour}\{Contexto.DatosDeConexion.IdUsuario}";
            Directory.CreateDirectory(almacenarEn);
            var fichero = Path.GetFileName(rutaConFichero);

            var archivo = new ArchivoDtm { Nombre = fichero, AlmacenadoEn = almacenarEn };
            var parametros = new ParametrosDeNegocio(enumTipoOperacion.Insertar);
            var tran = Contexto.IniciarTransaccion();
            try
            {
                PersistirRegistro(archivo, parametros);
                File.Move(rutaConFichero, $@"{archivo.AlmacenadoEn}\{archivo.Id}.se", true);
                Contexto.Commit(tran);
            }
            catch (Exception)
            {
                Contexto.Rollback(tran);
                throw;
            }

            return archivo.Id;
        }

        public static void SometerExportacion(ContextoSe contexto, string parametros)
        {
            if (parametros.IsNullOrEmpty())
                GestorDeErrores.Emitir("No se han proporcionado los parámetros para someter el trabajo de exportacion");

            var dll = Assembly.GetExecutingAssembly().GetName().Name;
            var clase = typeof(GestorDocumental).FullName;
            var ts = GestorDeTrabajosSometido.Obtener(contexto, "Exportar a excel", dll, clase, nameof(SometerExportacion).Replace("Someter", ""));
            // crear trabajo de usuario

            var tu = GestorDeTrabajosDeUsuario.Crear(contexto, ts, parametros);
            //liberarlo
        }

        public static Dictionary<string, object> ParametrosDeExportacion(string parametrosJson)
        {
            var parametros = new Dictionary<string, object>();
            if (!parametrosJson.IsNullOrEmpty())
            {
                var listaJson = parametrosJson.ToListaDeParametros();
                foreach (var p in listaJson)
                    parametros.Add(p.parametro, p.valor);
            }
            return parametros;
        }

        public static void Exportacion(EntornoDeTrabajo entorno)
        {
            Dictionary<string, object> parametros = entorno.Trabajo.Parametros.ToDiccionarioDeParametros();
            if (!parametros.ContainsKey(nameof(ElementoDto)))
                GestorDeErrores.Emitir("No se ha indicado el ElementoDto de exportación");

            if (!parametros.ContainsKey(nameof(Registro)))
                GestorDeErrores.Emitir("No se ha indicado el Registro de exportación");

           var gestor = NegociosDeSe.CrearGestor(entorno.contextoPr, parametros[nameof(Registro)].ToString(), parametros[nameof(ElementoDto)].ToString());

            var cantidad = !parametros.ContainsKey("cantidad") ? -1 : parametros["cantidad"].ToString().Entero();
            var posicion = !parametros.ContainsKey("posicion") ? 0 : parametros["posicion"].ToString().Entero();
            List<ClausulaDeFiltrado> filtros = !parametros.ContainsKey("filtro") || parametros["filtro"].ToString().IsNullOrEmpty() ? new List<ClausulaDeFiltrado>() : JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(parametros["filtro"].ToString());
            List<ClausulaDeOrdenacion> orden = !parametros.ContainsKey("orden") || parametros["orden"].ToString().IsNullOrEmpty() ? new List<ClausulaDeOrdenacion>() : JsonConvert.DeserializeObject<List<ClausulaDeOrdenacion>>(parametros["orden"].ToString());

            var opcionesDeMapeo = new Dictionary<string, object>();
            opcionesDeMapeo.Add(ElementoDto.DescargarGestionDocumental, false);

            var elementos = gestor.LeerElementos<ElementoDto>(posicion,cantidad, filtros, orden, opcionesDeMapeo);
            GestorDocumental.GenerarExcel(entorno.contextoPr, elementos.ToList());

            /* enviar por correo */
        }

        public static string GenerarExcel<T>(ContextoSe contexto, List<T> elementos)
        {
            VariableDtm ruta = GestorDeVariables.VariableDeRutaDeExportaciones(contexto);
            var fichero = $"{elementos[0].GetType()}.xls";
            var fecha = DateTime.Now;
            var rutaDeExportacion = $@"{ruta.Valor}\{fecha.Year}-{fecha.Month}-{fecha.Day}\{contexto.DatosDeConexion.Login}";
            if (!Directory.Exists(rutaDeExportacion))
                Directory.CreateDirectory(rutaDeExportacion);
            var ficheroConRuta = elementos.ToExcel(rutaDeExportacion, fichero);
            return GestorDeElementos.Utilidades.UrlDeArchivo(GestorDeElementos.Utilidades.DescargarArchivo(ficheroConRuta)); 
        }

    }
}
