using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Utilidades;
using Gestor.Elementos.ModeloIu;
using Microsoft.EntityFrameworkCore;
using System;
using Gestor.Errores;

namespace Gestor.Elementos.Seguridad
{
    public static partial class Joins
    {
        public static IQueryable<T> AplicarJoinsDePermisos<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros) where T : PermisoDtm
        {
            foreach (ClausulaDeJoin join in joins)
            {
                if (join.Dtm == typeof(ClasePermisoDtm))
                    registros = registros.Include(p => p.Clase);

                if (join.Dtm == typeof(TipoPermisoDtm))
                    registros = registros.Include(p => p.Tipo);
            }

            return registros;
        }
    }

    static class FiltrosDePermisos
    {
        public static IQueryable<T> FiltroPorNombre<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == PermisoPor.Nombre)
                    return registros.Where(x => x.Nombre.Contains(filtro.Valor));

            return registros;
        }

        public static IQueryable<T> FiltrarPorUsuario<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == PermisoPor.PermisosDeUnUsuario)
                {
                    var listaIds = filtro.Valor.ListaEnteros();
                    foreach (int id in listaIds)
                    {
                        registros = registros.Where(p => p.Usuarios.Any(up => up.IdUsua == id && up.IdPermiso == p.Id));
                    }
                }

            return registros;
        }

        public static IQueryable<T> FiltroPorRol<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoDtm
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
        public static IQueryable<T> FiltroPorClase<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == nameof(PermisoDtm.Clase).ToLower())
                {
                    registros = registros.Where(x => x.IdClase == filtro.Valor.Entero());
                }

            return registros;
        }
        public static IQueryable<T> FiltroPorTipo<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : PermisoDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == nameof(PermisoDtm.Tipo).ToLower())
                {
                    registros = registros.Where(x => x.IdTipo == filtro.Valor.Entero());
                }

            return registros;
        }


    }
    static class PermisosRegOrd
    {
        public static IQueryable<PermisoDtm> Orden(this IQueryable<PermisoDtm> set, List<ClausulaDeOrdenacion> ordenacion)
        {
            if (ordenacion.Count == 0)
                return set.OrderBy(x => x.Nombre);

            foreach (var orden in ordenacion)
            {
                if (orden.Propiedad == nameof(PermisoDtm.Nombre).ToLower())
                    set = orden.Modo == ModoDeOrdenancion.ascendente
                        ? set.OrderBy(x => x.Nombre)
                        : set.OrderByDescending(x => x.Nombre);
                
                if (orden.Propiedad == nameof(PermisoDtm.Clase).ToLower())
                    set = orden.Modo == ModoDeOrdenancion.ascendente
                        ? set.OrderBy(x => x.Clase)
                        : set.OrderByDescending(x => x.Clase);

                if (orden.Propiedad == nameof(PermisoDtm.Tipo).ToLower())
                    set = orden.Modo == ModoDeOrdenancion.ascendente
                        ? set.OrderBy(x => x.Tipo)
                        : set.OrderByDescending(x => x.Tipo);
            }

            return set;
        }
    }

    public class GestorDePermisos : GestorDeElementos<CtoSeguridad, PermisoDtm, PermisoDto>
    {
        public class MapearPermiso : Profile
        {
            public MapearPermiso()
            {
                CreateMap<PermisoDtm, PermisoDto>()
                .ForMember(dto => dto.Clase, dtm => dtm.MapFrom(dtm => dtm.Clase.Nombre))
                .ForMember(dto => dto.Tipo, dtm => dtm.MapFrom(dtm => dtm.Tipo.Nombre));

                CreateMap<PermisoDto, PermisoDtm>();

                CreateMap<ClasePermisoDtm, ClasePermisoDto>();

            }
        }

        public GestorDePermisos(CtoSeguridad contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override IQueryable<PermisoDtm> AplicarFiltros(IQueryable<PermisoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            foreach (var f in filtros)
                if (f.Propiedad == FiltroPor.Id)
                    return base.AplicarFiltros(registros, filtros, parametros);

            return registros
                .FiltroPorNombre(filtros)
                .FiltrarPorUsuario(filtros)
                .FiltroPorRol(filtros)
                .FiltroPorTipo(filtros)
                .FiltroPorClase(filtros);
        }
        protected override IQueryable<PermisoDtm> AplicarOrden(IQueryable<PermisoDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);
            return registros.Orden(ordenacion);
        }
        protected override void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            base.DefinirJoins(filtros, joins, parametros);

            joins.Add(new ClausulaDeJoin { Dtm = typeof(ClasePermisoDtm) });
            joins.Add(new ClausulaDeJoin { Dtm = typeof(TipoPermisoDtm) });
        }
        protected override IQueryable<PermisoDtm> AplicarJoins(IQueryable<PermisoDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            return registros.AplicarJoinsDePermisos(joins, parametros);
        }
        public List<ClasePermisoDto> LeerClases()
        {
            var clases = Contexto.ClasesDePermisos.AsNoTracking().ToList();
            var gestor = new GestorDeClaseDePermisos(Contexto, Mapeador);
            return gestor.MapearElementos(clases).ToList();
        }
        public List<TipoPermisoDto> LeerTipos()
        {
            var tipos = Contexto.TiposDePermisos.AsNoTracking().ToList();
            var gestor = new GestorDeTipoPermiso(Contexto, Mapeador);
            return gestor.MapearElementos(tipos).ToList();
        }

        protected override void AntesEliminarFila(PermisoDto elemento, ParametrosDeNegocio opciones)
        {
            base.AntesEliminarFila(elemento, opciones);

            var gestor = new GestorDeRolesDePermisos(Contexto, Mapeador);
            var filtro = new ClausulaDeFiltrado { Propiedad = nameof(RolPermisoDtm.IdPermiso), Criterio = CriteriosDeFiltrado.igual, Valor = elemento.Id.ToString() };
            var filtros = new List<ClausulaDeFiltrado>();
            filtros.Add(filtro);
            var r = gestor.LeerRegistros(0, 1, filtros);
            if (r.Count > 0)
            {
                var roles = "";
                foreach (var r1 in r)
                    roles = $"{(roles == "" ? "" : $"{roles},")} {r1.Rol.Nombre}";

                Exception exc = GestorDeErrores.MostrarExcepcion(excepcioMostrar: $"El permiso está incluido en {(r.Count == 1 ? "el rol" : "los roles") }: '{roles}'");
                throw exc;
            }
        }
    }

}
