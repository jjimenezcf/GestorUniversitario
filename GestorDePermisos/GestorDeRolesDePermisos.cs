using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Gestor.Elementos.ModeloIu;
using Microsoft.EntityFrameworkCore;
using Utilidades;

namespace Gestor.Elementos.Seguridad
{
    public static partial class Joins
    {
        public static IQueryable<T> AplicarJoinsDeRolPermiso<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros) where T : RolPermisoDtm
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
        public static IQueryable<T> FiltroPorPermiso<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : RolPermisoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == nameof(RolPermisoDtm.IdPermiso).ToLower())
                    return registros.Where(x => x.IdPermiso == filtro.Valor.Entero());

            return registros;
        }
    }


        public class GestorDeRolesDePermisos : GestorDeElementos<CtoSeguridad, RolPermisoDtm, RolPermisoDto>
    {

        public class MapearRolPermiso : Profile
        {
            public MapearRolPermiso()
            {
                CreateMap<RolPermisoDtm, RolPermisoDto>();
            }
        }

        public GestorDeRolesDePermisos(CtoSeguridad contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }

        protected override void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            base.DefinirJoins(filtros, joins, parametros);
            joins.Add(new ClausulaDeJoin { Dtm = typeof(PermisoDtm) });
            joins.Add(new ClausulaDeJoin { Dtm = typeof(RolDtm) });
        }

        protected override IQueryable<RolPermisoDtm> AplicarJoins(IQueryable<RolPermisoDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, joins, parametros);
            registros = registros.AplicarJoinsDeRolPermiso(joins,parametros);
            return registros;
        }

        protected override IQueryable<RolPermisoDtm> AplicarFiltros(IQueryable<RolPermisoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            foreach (var f in filtros)
                if (f.Propiedad == FiltroPor.Id)
                    return base.AplicarFiltros(registros, filtros, parametros);

            return registros.FiltroPorPermiso(filtros);
        }


    }
}
