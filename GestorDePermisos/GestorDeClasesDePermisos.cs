using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Gestor.Elementos;
using Gestor.Elementos.Seguridad;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;

namespace Gestor.Elementos.Seguridad
{

    //static class FiltrosDeClaseDePermiso
    //{
    //    public static IQueryable<T> FiltroPorNombre<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : ClasePermisoDtm
    //    {
    //        foreach (ClausulaDeFiltrado filtro in filtros)
    //            if (filtro.Propiedad.ToLower() == nameof(ClasePermisoDtm.Nombre).ToLower())
    //                return registros.Where(x => x.Nombre.Contains(filtro.Valor));

    //        return registros;
    //    }
    //}

    public class GestorDeClaseDePermisos : GestorDeElementos<ContextoSe, ClasePermisoDtm, ClasePermisoDto>
    {
        public class MapearClasePermiso : Profile
        {
            public MapearClasePermiso()
            {
                CreateMap<ContextoSe, ClasePermisoDto>();
            }
        }

        public GestorDeClaseDePermisos(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {


        }


        internal static GestorDeClaseDePermisos Gestor(IMapper mapeador)
        {
            var contexto = ContextoSe.ObtenerContexto();
            return (GestorDeClaseDePermisos)CrearGestor<GestorDeClaseDePermisos>(() => new GestorDeClaseDePermisos(contexto, mapeador));
        }

    }
}
