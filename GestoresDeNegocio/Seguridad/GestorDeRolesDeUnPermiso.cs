using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto.Seguridad;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace GestoresDeNegocio.Seguridad
{
    public static partial class Joins
    {
        public static IQueryable<T> JoinDeRolesDeUnPermiso<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros) 
        where T : PermisosDeUnRolDtm
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


    static class OrdenacionDeRolesDeUnPermiso
    {
        public static IQueryable<PermisosDeUnRolDtm> Orden(this IQueryable<PermisosDeUnRolDtm> set, List<ClausulaDeOrdenacion> ordenacion)
        {
            if (ordenacion.Count == 0)
                return set.OrderBy(x => x.Permiso.Nombre);
            return set;
        }
    }


    public class GestorDeRolesDeUnPermiso : GestorDeElementos<ContextoSe, PermisosDeUnRolDtm, RolesDeUnPermisoDto>
    {

        public class MapearRolesDeUnPermiso : Profile
        {
            public MapearRolesDeUnPermiso()
            {
                CreateMap<PermisosDeUnRolDtm, RolesDeUnPermisoDto>()
                    .ForMember(dto => dto.Rol, dtm => dtm.MapFrom(dtm => dtm.Rol.Nombre))
                    .ForMember(dto => dto.Permiso, dtm => dtm.MapFrom(dtm => dtm.Permiso.Nombre));
                
                CreateMap<RolesDeUnPermisoDto, PermisosDeUnRolDtm>();
            }
        }

        public GestorDeRolesDeUnPermiso(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
            invertirMapeoDeRelacion = true;
        }


        internal static GestorDeRolesDeUnPermiso Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeRolesDeUnPermiso(contexto, mapeador);
        }

        protected override void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            base.DefinirJoins(filtros, joins, parametros);
            joins.Add(new ClausulaDeJoin { Dtm = typeof(PermisoDtm) });
            joins.Add(new ClausulaDeJoin { Dtm = typeof(RolDtm) });
        }

        protected override IQueryable<PermisosDeUnRolDtm> AplicarJoins(IQueryable<PermisosDeUnRolDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, joins, parametros);
            registros = registros.JoinDeRolesDeUnPermiso(joins, parametros);
            return registros;
        }

        protected override IQueryable<PermisosDeUnRolDtm> AplicarFiltros(IQueryable<PermisosDeUnRolDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (!hayFiltroPorId)
                registros = FiltrarRolesDeUnPermiso(registros, filtros);

            return registros;
        }

        private IQueryable<PermisosDeUnRolDtm> FiltrarRolesDeUnPermiso(IQueryable<PermisosDeUnRolDtm> registros, List<ClausulaDeFiltrado> filtros)
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnRolDtm.IdRol).ToLower() ||
                    filtro.Clausula.ToLower() == "idElemento1".ToLower())
                    registros = registros.Where(x => x.IdRol == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnRolDtm.IdPermiso).ToLower() ||
                    filtro.Clausula.ToLower() == "idElemento2".ToLower())
                    registros = registros.Where(x => x.IdPermiso == filtro.Valor.Entero());
            }
            return registros;

        }

        public List<RolDto> LeerRoles(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDeRoles.Gestor(Contexto, Mapeador);
            return GestorDeRoles.Leer(gestor, posicion, cantidad, filtro);
        }

        public List<PermisoDto> LeerPermisos(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDePermisos.Gestor(Contexto, Mapeador);
            return GestorDePermisos.Leer(gestor, posicion, cantidad, filtro);
        }

        //protected override void MapearDatosDeRelacion(RolesDeUnPermisoDtm registro, int idElemento1, int idElemento2)
        //{
        //    registro.IdRol = idElemento1;
        //    registro.IdPermiso = idElemento2;
        //}
    }
}
