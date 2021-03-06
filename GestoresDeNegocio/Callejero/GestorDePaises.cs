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

namespace GestoresDeNegocio.Callejero
{

    public class GestorDePaises : GestorDeElementos<ContextoSe, PaisDtm, PaisDto>
    {

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

        public static GestorDeTrabajosDeUsuario Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeTrabajosDeUsuario(contexto, mapeador); ;
        }

        public static void SometerImportarCallejero(ContextoSe contexto, string parametros)
        {
            if (parametros.IsNullOrEmpty())
                GestorDeErrores.Emitir("No se han proporcionado los parámetros para someter el trabajo de importación");

            var dll = Assembly.GetExecutingAssembly().GetName().Name;
            var clase = typeof(GestorDePaises).FullName;
            var ts = GestorDeTrabajosSometido.Obtener(contexto, "Importar callejero", dll, clase, nameof(SometerImportarCallejero).Replace("Someter",""));
            // crear trabajo de usuario

            var tu = GestorDeTrabajosDeUsuario.Crear(contexto, ts, parametros);
            //liberarlo
        }

        public static void ImportarCallejero(ContextoSe contextoTu, ContextoSe contextoPr, int idTrabajoDeUsuario)
        {
            var gestorTu = GestorDeTrabajosDeUsuario.Gestor(contextoTu, contextoTu.Mapeador);
            var tu = gestorTu.LeerRegistroPorId(idTrabajoDeUsuario);
            var gestorPr = GestorDePaises.Gestor(contextoPr, contextoPr.Mapeador);
            var tran = gestorPr.IniciarTransaccion();
            try
            {
                GestorDeErrores.Emitir($"Falta implementar el proceso para el trabajo {tu.Trabajo.Nombre}");
                contextoPr.Commit(tran);
            }
            catch(Exception e)
            {
                contextoPr.Rollback(tran);
                GestorDeErroresDeUnTrabajo.AnotarError(gestorTu.Contexto, tu, e);
                throw;
            }
        }


    }
}
