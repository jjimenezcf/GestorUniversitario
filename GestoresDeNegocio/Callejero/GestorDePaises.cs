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
            var gestor = GestorDeTrabajosDeUsuario.Gestor(contextoPr, contextoPr.Mapeador);
            var tu = gestor.LeerRegistroPorId(idTrabajoDeUsuario);
            throw new System.Exception($"Falta implementar el proceso para el trabajo {tu.Trabajo.Nombre}");
        }


    }
}
