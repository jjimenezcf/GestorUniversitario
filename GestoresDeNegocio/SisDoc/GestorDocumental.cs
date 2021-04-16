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
            var gestor = GestorDeVariables.Gestor(Contexto, Mapeador);
            var rutaDocumental = gestor.LeerRegistroCacheado(nameof(VariableDto.Nombre), Variable.Servidor_Archivos);

            if (!Directory.Exists(rutaDocumental.Valor))
                throw new Exception($"La ruta del servidor documental {rutaDocumental.Valor} asignada a la variable {Variable.Servidor_Archivos} no está definida");

            var fecha = DateTime.Now;
            var almacenarEn = $@"{rutaDocumental.Valor}\{fecha.Year}\{fecha.Month}\{fecha.Day}\{fecha.Hour}\{gestor.Contexto.DatosDeConexion.IdUsuario}";
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

        public static void GenerarExcel<T>(ContextoSe contexto, List<T> elementos)
        {
            VariableDtm ruta = GestorDeVariables.VariableDeRutaDeExportaciones(contexto);
            var fichero = $"{elementos[0].GetType()}.xls";
            elementos.ToExcel(ruta.Valor, fichero);
        }

    }
}
