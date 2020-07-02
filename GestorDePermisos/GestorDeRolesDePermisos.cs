using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Gestor.Elementos.ModeloIu;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace Gestor.Elementos.Seguridad
{
    public static partial class Joins
    {
        public static IQueryable<T> AplicarJoinsDeRolPermiso<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros) where T : RolesDeUnPermisoDtm
        {
            foreach (ClausulaDeJoin join in joins)
            {
                if (join.Dtm == typeof(PermisoDtm))
                    registros = registros.Include(rp => rp.Permiso);

                if (join.Dtm == typeof(RolDtm))
                    registros = registros.Include(rp => rp.Rol);
            }

            return registros;
        }
    }


    static class FiltrosDeRolesDePermisos
    {
        public static IQueryable<T> FiltroPorPermiso<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : RolesDeUnPermisoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Clausula.ToLower() == nameof(RolesDeUnPermisoDtm.IdPermiso).ToLower() && filtro.Valor.Entero() > 0)
                    return registros.Where(x => x.IdPermiso == filtro.Valor.Entero());

            return registros;
        }
    }


    public class GestorDeRolesDePermisos : GestorDeElementos<ContextoSe, RolesDeUnPermisoDtm, RolPermisoDto>
    {

        public class MapearRolPermiso : Profile
        {
            public MapearRolPermiso()
            {
                CreateMap<RolesDeUnPermisoDtm, RolPermisoDto>();
            }
        }

        public GestorDeRolesDePermisos(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }


        internal static GestorDeRolesDePermisos Gestor(IMapper mapeador)
        {
            var contexto = ContextoSe.ObtenerContexto();
            return (GestorDeRolesDePermisos)CrearGestor<GestorDeRolesDePermisos>(() => new GestorDeRolesDePermisos(contexto, mapeador));
        }

        protected override void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            base.DefinirJoins(filtros, joins, parametros);
            joins.Add(new ClausulaDeJoin { Dtm = typeof(PermisoDtm) });
            joins.Add(new ClausulaDeJoin { Dtm = typeof(RolDtm) });
        }

        protected override IQueryable<RolesDeUnPermisoDtm> AplicarJoins(IQueryable<RolesDeUnPermisoDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, joins, parametros);
            registros = registros.AplicarJoinsDeRolPermiso(joins, parametros);
            return registros;
        }

        protected override IQueryable<RolesDeUnPermisoDtm> AplicarFiltros(IQueryable<RolesDeUnPermisoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId(registros))
                return registros;

            return registros.FiltroPorPermiso(filtros);
        }

    }
}
