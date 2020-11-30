using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GestorDeElementos;
using GestoresDeNegocio.Entorno;
using GestoresDeNegocio.Seguridad;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto.Entorno;
using ModeloDeDto.Seguridad;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace GestoresDeNegocio.Entorno
{

    public class GestorDePermisosDeUnUsuario : GestorDeElementos<ContextoSe, PermisosDeUnUsuarioDtm, PermisosDeUnUsuarioDto>
    {

        public class MapearPermisosDeUnUsuario : Profile
        {
            public MapearPermisosDeUnUsuario()
            {
                CreateMap<PermisosDeUnUsuarioDtm, PermisosDeUnUsuarioDto>()
                    .ForMember(dto => dto.Usuario, dtm => dtm.MapFrom(dtm => dtm.Usuario.Nombre))
                    .ForMember(dto => dto.Permiso, dtm => dtm.MapFrom(dtm => dtm.Permiso.Nombre));

                CreateMap<PermisosDeUnUsuarioDto, PermisosDeUnUsuarioDtm>();
            }
        }

        public GestorDePermisosDeUnUsuario(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }


        internal static GestorDePermisosDeUnUsuario Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePermisosDeUnUsuario(contexto, mapeador);
        }

        protected override IQueryable<PermisosDeUnUsuarioDtm> AplicarJoins(IQueryable<PermisosDeUnUsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(rp => rp.Permiso);
            registros = registros.Include(rp => rp.Usuario);

            return registros;
        }

        protected override IQueryable<PermisosDeUnUsuarioDtm> AplicarFiltros(IQueryable<PermisosDeUnUsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (string.Equals(filtro.Clausula, nameof(PermisosDeUnUsuarioDtm.IdUsuario), StringComparison.CurrentCultureIgnoreCase) && filtro.Criterio == CriteriosDeFiltrado.igual)
                    registros = registros.Where(x => x.IdUsuario == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnUsuarioDtm.IdPermiso).ToLower() && filtro.Criterio == CriteriosDeFiltrado.igual)
                    registros = registros.Where(x => x.IdPermiso == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnUsuarioDtm.Permiso).ToLower() && filtro.Criterio == CriteriosDeFiltrado.contiene)
                    registros = registros.Where(x => x.Permiso.Nombre.Contains(filtro.Valor));

                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnUsuarioDtm.IdPermiso).ToLower() && filtro.Criterio == CriteriosDeFiltrado.esAlgunoDe)
                {
                    var lista = filtro.Valor.Split(',').Select(s => s.Entero()).ToArray();
                    return registros.Where(x => lista.Contains(x.Id));
                }
            }
            return registros;

        }

        protected override IQueryable<PermisosDeUnUsuarioDtm> AplicarOrden(IQueryable<PermisosDeUnUsuarioDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);

            if (ordenacion.Count == 0)
                return registros.OrderBy(x => x.Permiso.Nombre);

            return registros;
        }

        public List<UsuarioDto> LeerUsuarios(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDeUsuarios.Gestor(Contexto, Mapeador);
            return GestorDeUsuarios.Leer(gestor, posicion, cantidad, filtro);
        }

        public List<PermisoDto> LeerPermisos(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDePermisos.Gestor(Contexto, Mapeador);
            return GestorDePermisos.Leer(gestor, posicion, cantidad, filtro);
        }

    }
}
