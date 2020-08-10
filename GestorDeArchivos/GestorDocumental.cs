﻿using AutoMapper;
using Gestor.Elementos.Entorno;
using System.IO;
using ServicioDeDatos.Archivos;
using ServicioDeDatos;
using System;
using ModeloDeDto.Archivos;
using ModeloDeDto.Entorno;

namespace Gestor.Elementos.Archivos
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


        public static int SubirArchivo(string rutaConFichero, IMapper mapeador)
        {
            var gestor = Gestor(mapeador);
            return gestor.SubirArchivoInterno(rutaConFichero);
        }


        public GestorDocumental(ContextoSe contexto, IMapper mapeador) 
        : base(contexto, mapeador)
        {
        }


        private static GestorDocumental Gestor(IMapper mapeador)
        {
            var contexto = ContextoSe.ObtenerContexto();
            return (GestorDocumental)CrearGestor<GestorDocumental>(() => new GestorDocumental(contexto, mapeador));
        }

        private int SubirArchivoInterno(string rutaConFichero)
        {
            var gestor = (GestorDeVariables) Generador<ContextoSe, IMapper>.ObtenerGestor("GestorDeEntorno"
                                                             , nameof(GestorDeVariables)
                                                             , new object[] { Contexto, Mapeador });

            var rutaDocumental = gestor.LeerRegistroCacheado(nameof(VariableDto.Nombre), Variable.Servidor_Archivos);

            if (!Directory.Exists(rutaDocumental.Valor))
                throw new Exception($"La ruta del servidor documental {rutaDocumental.Valor} asignada a la variable {Variable.Servidor_Archivos} no está definida");

            var fecha = DateTime.Now;
            var almacenarEn = $@"{rutaDocumental.Valor}\{fecha.Year}\{fecha.Month}\{fecha.Day}\{fecha.Hour}\{gestor.Contexto.DatosDeConexion.IdUsuario}";
            Directory.CreateDirectory(almacenarEn);
            var fichero = Path.GetFileName(rutaConFichero);

            var archivo = new ArchivoDtm();
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
