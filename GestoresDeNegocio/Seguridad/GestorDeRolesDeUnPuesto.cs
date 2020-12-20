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

    public class GestorDeRolesDeUnPuesto : GestorDeElementos<ContextoSe, RolesDeUnPuestoDtm, RolesDeUnPuestoDto>
    {

        public class MapearRolesDeUnPuesto : Profile
        {
            public MapearRolesDeUnPuesto()
            {
                CreateMap<RolesDeUnPuestoDtm, RolesDeUnPuestoDto>()
                    .ForMember(dto => dto.Puesto, dtm => dtm.MapFrom(dtm => dtm.Puesto.Nombre))
                    .ForMember(dto => dto.Rol, dtm => dtm.MapFrom(dtm => dtm.Rol.Nombre));

                CreateMap<RolesDeUnPuestoDto, RolesDeUnPuestoDtm>()
                    .ForMember(dtm => dtm.Rol, dto => dto.Ignore())
                    .ForMember(dtm => dtm.Puesto, dto => dto.Ignore()); ;
            }
        }

        public GestorDeRolesDeUnPuesto(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {


        }

        internal static GestorDePuestosDeUnUsuario Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePuestosDeUnUsuario(contexto, mapeador);
        }

        protected override IQueryable<RolesDeUnPuestoDtm> AplicarJoins(IQueryable<RolesDeUnPuestoDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Rol);
            registros = registros.Include(p => p.Puesto);
            return registros;
        }

        protected override IQueryable<RolesDeUnPuestoDtm> AplicarFiltros(IQueryable<RolesDeUnPuestoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(RolesDeUnPuestoDtm.IdPuesto).ToLower())
                    registros = registros.Where(x => x.IdPuesto == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(RolesDeUnPuestoDtm.IdRol).ToLower())
                    registros = registros.Where(x => x.IdRol == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(RolesDeUnPuestoDto.Rol).ToLower())
                    registros = registros.Where(x => x.Rol.Nombre.Contains(filtro.Valor));
            }

            return registros;
        }

        protected override IQueryable<RolesDeUnPuestoDtm> AplicarOrden(IQueryable<RolesDeUnPuestoDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);

            if (ordenacion.Count == 0)
                return registros.OrderBy(x => x.Puesto.Nombre);

            return registros;
        }

        public List<RolDto> LeerRoles(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDeRoles.Gestor(Contexto, Mapeador);
            return GestorDeRoles.Leer(gestor, posicion, cantidad, filtro);
        }

        public List<PuestoDto> LeerPuestos(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDePuestosDeTrabajo.Gestor(Contexto, Mapeador);
            return GestorDePuestosDeTrabajo.Leer(gestor, posicion, cantidad, filtro);
        }

        protected override void DespuesDePersistir(RolesDeUnPuestoDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            if (parametros.Operacion == TipoOperacion.Modificar || parametros.Operacion == TipoOperacion.Eliminar)
            {
                ServicioDeCaches.EliminarCache($"{nameof(GestorDeVistaMvc)}.{nameof(GestorDeVistaMvc.TienePermisos)}");
                ServicioDeCaches.EliminarCache($"{nameof(GestorDeElementos)}.{nameof(ValidarPermisosDePersistencia)}");
            }
        }
    }
}

