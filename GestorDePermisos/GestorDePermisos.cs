using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Seguridad
{

    static class FiltrosDePermisos
    {
        public static IQueryable<T> FiltroPorNombre<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : rPermiso
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == PermisoPor.Nombre)
                    return registros.Where(x => x.Nombre.Contains(filtro.Valor));

            return registros;
        }

        public static IQueryable<T> FiltroPorRol<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : rPermiso
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == PermisoPor.PermisoDeUnRol)
                {
                    var listaIds = filtro.Valor.ListaEnteros();
                    foreach (int id in listaIds)
                    {
                        registros = registros.Where(x => x.Roles.Any(i => i.IdPermiso == id));
                    }
                }

            return registros;

        }
    }


    public class GestorDePermisos : GestorDeElementos<CtoPermisos, rPermiso, PermisoDto>
    {
        public class MapearPermiso : Profile
        {
            public MapearPermiso()
            {
                CreateMap<rPermiso, PermisoDto>();
                CreateMap<PermisoDto,rPermiso>();
            }
        }

        public GestorDePermisos(CtoPermisos contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
            
        }
               
        protected override rPermiso LeerConDetalle(int Id)
        {
            return null;
        }

        protected override IQueryable<rPermiso> AplicarFiltros(IQueryable<rPermiso> registros, List<ClausulaDeFiltrado> filtros)
        {
            foreach (var f in filtros)
                if (f.Propiedad == FiltroPor.Id)
                  return base.AplicarFiltros(registros, filtros);

            return registros
                .FiltroPorNombre(filtros)
                .FiltroPorRol(filtros);
        }

    }
}
