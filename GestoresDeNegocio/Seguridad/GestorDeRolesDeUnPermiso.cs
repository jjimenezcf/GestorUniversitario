using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GestorDeElementos;
using GestoresDeNegocio.Entorno;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto.Seguridad;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace GestoresDeNegocio.Seguridad
{

    public class GestorDeRolesDeUnPermiso : GestorDeElementos<ContextoSe, PermisosDeUnRolDtm, RolesDeUnPermisoDto>
    {

        public class MapearRolesDeUnPermiso : Profile
        {
            public MapearRolesDeUnPermiso()
            {
                CreateMap<PermisosDeUnRolDtm, RolesDeUnPermisoDto>()
                    .ForMember(dto => dto.Rol, dtm => dtm.MapFrom(dtm => dtm.Rol.Nombre))
                    .ForMember(dto => dto.Permiso, dtm => dtm.MapFrom(dtm => dtm.Permiso.Nombre));

                CreateMap<RolesDeUnPermisoDto, PermisosDeUnRolDtm>()
                    .ForMember(dtm => dtm.Rol, dto => dto.Ignore())
                    .ForMember(dtm => dtm.Permiso, dto => dto.Ignore()); 
            }
        }

        public GestorDeRolesDeUnPermiso(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
            InvertirMapeoDeRelacion = true;
        }


        internal static GestorDeRolesDeUnPermiso Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeRolesDeUnPermiso(contexto, mapeador);
        }

        protected override IQueryable<PermisosDeUnRolDtm> AplicarJoins(IQueryable<PermisosDeUnRolDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(rp => rp.Permiso);
            registros = registros.Include(rp => rp.Rol);
            return registros;
        }

        protected override IQueryable<PermisosDeUnRolDtm> AplicarFiltros(IQueryable<PermisosDeUnRolDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnRolDtm.IdRol).ToLower())
                    registros = registros.Where(x => x.IdRol == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnRolDtm.IdPermiso).ToLower())
                    registros = registros.Where(x => x.IdPermiso == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(RolesDeUnPermisoDto.Rol).ToLower())
                    registros = registros.Where(x => x.Rol.Nombre.Contains(filtro.Valor));
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

        protected override void DespuesDePersistir(PermisosDeUnRolDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros); 
            if (parametros.Operacion == TipoOperacion.Modificar || parametros.Operacion == TipoOperacion.Eliminar)
            {
                var parteDeLaClave = $"Permiso:{registro.IdPermiso}";
                ServicioDeCaches.EliminarElementos($"{nameof(GestorDeVistaMvc)}.{nameof(GestorDeVistaMvc.TienePermisos)}", parteDeLaClave);
                ServicioDeCaches.EliminarElementos($"{nameof(GestorDeElementos)}.{nameof(ValidarPermisosDePersistencia)}", parteDeLaClave);
            }
        }
    }
}
