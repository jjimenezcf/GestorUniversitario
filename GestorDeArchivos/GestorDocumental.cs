using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Gestor.Elementos.Entorno;
using System.IO;
using ServicioDeDatos.Archivos;
using ServicioDeDatos;
using ServicioDeDatos.Utilidades;
using System;

namespace Gestor.Elementos.Archivos
{
    public class GestorDocumental : GestorDeElementos<ContextoSe, ArchivosDtm, ArchivosDto>
    {
        public class MapearArchivos : Profile
        {
            public MapearArchivos()
            {
                CreateMap<ArchivosDtm, ArchivosDto>();
                CreateMap<ArchivosDto, ArchivosDtm>();
            }
        }


        public static void SubirArchivo(string rutaConFichero, IMapper mapeador)
        {

            var contexto = ContextoSe.ObtenerContexto();
             
            var gestorDocumental = (GestorDocumental) Generador<ContextoSe, IMapper>.CachearGestor("GestorDocumental"
                                                             , nameof(GestorDeVariables)
                                                             , () => new GestorDocumental(contexto, mapeador));

            gestorDocumental.SubirArchivoInterno(rutaConFichero);
        }

        public GestorDocumental(ContextoSe contexto, IMapper mapeador) 
        : base(contexto, mapeador)
        {
        }

        private int SubirArchivoInterno(string rutaConFichero)
        {
            var gestor = (GestorDeVariables) Generador<ContextoSe, IMapper>.ObtenerGestor("GestorDeEntorno"
                                                             , nameof(GestorDeVariables)
                                                             , new object[] { Contexto, Mapeador });

            var rutaDocumental = gestor.LeerRegistroCacheado(nameof(VariableDto.Nombre), Variable.Servidor_Archivos);
            var fecha = DateTime.Now;
            var almacenarEn = $@"{rutaDocumental.Valor}\{fecha.Year}\{fecha.Month}\{fecha.Day}\{fecha.Hour}\{gestor.Contexto.DatosDeConexion.IdUsuario}";
            Directory.CreateDirectory(almacenarEn);
            var fichero = Path.GetFileName(rutaConFichero);

            var archivo = new ArchivosDtm();
            archivo.Nombre = fichero;
            archivo.AlmacenadoEn = almacenarEn;
            var parametros = new ParametrosDeNegocio(TipoOperacion.Insertar);
            var tran = Contexto.IniciarTransaccion();
            try
            {
                PersistirElementoDtm(archivo, parametros);
                File.Move(rutaConFichero, $@"{archivo.AlmacenadoEn}\{archivo.Id}.se", true);
                Contexto.Commit(tran);
            }
            catch(Exception exc)
            {
                Contexto.Rollback(tran);
                throw exc;
            }

            return archivo.Id;
        }

    }
}
