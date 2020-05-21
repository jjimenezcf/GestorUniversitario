using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Gestor.Elementos.Entorno;
using System.IO;
using ServicioDeDatos.Archivos;
using ServicioDeDatos;
using ServicioDeDatos.Utilidades;

namespace Gestor.Elementos.Archivos
{
    public class GestorDocumental : GestorDeElementos<ContextoDeElementos, ArchivosDtm, ArchivosDto>
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

            var contexto = ContextoDeElementos.CrearContexto();

            var gestorDocumental = new GestorDocumental(contexto, mapeador);

            gestorDocumental.SubirArchivoInterno(rutaConFichero);
        }

        public GestorDocumental(ContextoDeElementos contexto, IMapper mapeador) 
        : base(contexto, mapeador)
        {
        }

        private void SubirArchivoInterno(string rutaConFichero)
        {

            var contexto = ContextoDeElementos.CrearContexto();

            var gestorDeVariables = (GestorDeVariables) Generador<ContextoDeElementos, IMapper>.GenerarObjeto("GestorDeEntorno"
                                                             , nameof(GestorDeVariables)
                                                             , new object[] { contexto, Mapeador });

            var rutaDocumental = gestorDeVariables.LeerRegistroCacheado(nameof(VariableDto.Nombre), Variable.Servidor_Archivos);


            var ruta = new CacheDeVariable(Contexto).ServidorDeArchivos;
            var fichero = Path.GetFileName(rutaConFichero);
            File.Move(rutaConFichero, $@"{ruta}\{fichero}", true);
        }

    }
}
