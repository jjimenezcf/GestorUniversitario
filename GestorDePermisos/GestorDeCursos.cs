using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Seguridad
{

    static class RegistroDeCursosFiltros
    {
        public static IQueryable<T> AplicarFiltroNombre<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoReg
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == PermisoPor.Nombre)
                    return registros.Where(x => x.Nombre.Contains(filtro.Valor));

            return registros;
        }

        public static IQueryable<T> AplicarFiltroPermisos<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoReg
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


    public class GestorDeCursos : GestorDeElementos<CtoPermisos, PermisoReg, PermisoDto>
    {
        public class MapeoRegistroCurso : Profile
        {
            public MapeoRegistroCurso()
            {
                CreateMap<PermisoReg, PermisoDto>();
                CreateMap<PermisoDto,PermisoReg>();
            }
        }

        public GestorDeCursos(CtoPermisos contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
            
        }
               
        protected override PermisoReg LeerConDetalle(int Id)
        {
            return null;
        }

        protected override IQueryable<PermisoReg> AplicarFiltros(IQueryable<PermisoReg> registros, List<ClausulaDeFiltrado> filtros)
        {
            foreach (var f in filtros)
                if (f.Propiedad == FiltroPor.Id)
                  return base.AplicarFiltros(registros, filtros);

            return registros
                .AplicarFiltroNombre(filtros)
                .AplicarFiltroPermisos(filtros);
        }

    }
}
