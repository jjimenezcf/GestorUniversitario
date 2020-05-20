using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using System;
using Gestor.Elementos.ModeloIu;
using Gestor.Elementos.Entorno;
using GestorDeElementos;
using Gestor.Elementos.Archivos;
using System.IO;

namespace Gestor.Elementos.Archivos
{
    public class GestorDocumental : GestorDeElementos<CtoDocumental, ArchivosDtm, ArchivosDto>
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

            var contexto = CtoDocumental.CrearContexto();

            var gestorDocumental = (GestorDocumental)Generador<CtoDocumental, IMapper>.GenerarObjeto(contexto.GetType().Assembly.GetName().Name
                                                             , nameof(GestorDocumental)
                                                             , new object[] { contexto, mapeador });

            gestorDocumental.SubirArchivoInterno(rutaConFichero);
        }

        public GestorDocumental(CtoDocumental contexto, IMapper mapeador) : base(contexto, mapeador)
        {
        }

        private void SubirArchivoInterno(string rutaConFichero)
        {

            var contexto = CtoEntorno.CrearContexto();

            var gestorDeVariables = (GestorDeVariables) Generador<CtoEntorno, IMapper>.GenerarObjeto(contexto.GetType().Assembly.GetName().Name
                                                             , nameof(GestorDeVariables)
                                                             , new object[] { contexto, Mapeador });

            var rutaDocumental = gestorDeVariables.LeerRegistroCacheado(nameof(VariableDto.Nombre), Variable.Servidor_Archivos);


            var ruta = new CacheDeVariable(Contexto).ServidorDeArchivos;
            var fichero = Path.GetFileName(rutaConFichero);
            File.Move(rutaConFichero, $@"{ruta}\{fichero}", true);
        }

    }
}
