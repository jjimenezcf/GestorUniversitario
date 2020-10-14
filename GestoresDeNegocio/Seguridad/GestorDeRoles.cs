using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GestorDeElementos;
using Microsoft.AspNetCore.Identity;
using ModeloDeDto.Seguridad;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace GestoresDeNegocio.Seguridad
{
   
    public class GestorDeRoles : GestorDeElementos<ContextoSe, RolDtm, RolDto>, IRoleStore<RolDtm>
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

            if (!hayFiltroPorId)
                registros = FiltrarRoles(registros, filtros);

            return registros;
        }

        private IQueryable<RolDtm> FiltrarRoles(IQueryable<RolDtm> registros, List<ClausulaDeFiltrado> filtros)
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

        public Task<IdentityResult> CreateAsync(RolDtm role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(RolDtm role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(RolDtm role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(RolDtm role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(RolDtm role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(RolDtm role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(RolDtm role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(RolDtm role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<RolDtm> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<RolDtm> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
