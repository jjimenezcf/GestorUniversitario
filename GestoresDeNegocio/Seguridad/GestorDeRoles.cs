using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GestorDeElementos;
using ModeloDeDto.Seguridad;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace GestoresDeNegocio.Seguridad
{
    static class FiltrosRoles
    {
        public static IQueryable<T> Roles<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros)
        where T : RolDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(RolesDeUnPuestoDtm.idPuesto).ToLower() &&
                    filtro.Criterio == CriteriosDeFiltrado.diferente)
                    registros = registros.Where(i => !i.Puestos.Any(r => r.idPuesto == filtro.Valor.Entero()));


                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnRolDtm.IdPermiso).ToLower() &&
                    filtro.Criterio == CriteriosDeFiltrado.diferente)
                    registros = registros.Where(i => !i.Permisos.Any(r => r.IdPermiso == filtro.Valor.Entero()));
            }
            
            return registros;
        }
    }

    public class GestorDeRoles : GestorDeElementos<ContextoSe, RolDtm, RolDto>
    {
        public class MapearPuestoDeTrabajo : Profile
        {
            public MapearPuestoDeTrabajo()
            {
                CreateMap<RolDtm, RolDto>();
                CreateMap<RolDto, RolDtm>();
            }
        }

        public GestorDeRoles(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
        }


        internal static GestorDeRoles Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeRoles(contexto, mapeador);
        }

        internal static List<RolDto> Leer(GestorDeRoles gestor, int posicion, int cantidad, string filtro)
        {
            var filtros = new List<ClausulaDeFiltrado>();
            if (!filtro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(RolDto.Nombre), Valor = filtro });

            var rolesDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(rolesDtm).ToList();
        }

        protected override IQueryable<RolDtm> AplicarFiltros(IQueryable<RolDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId(registros))
                return registros;

            return registros.Roles(filtros);
        }

    }
}
