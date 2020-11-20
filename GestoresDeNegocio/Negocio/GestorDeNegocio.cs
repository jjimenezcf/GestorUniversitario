using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos.Negocio;
using ServicioDeDatos;
using ModeloDeDto.Negocio;
using GestorDeElementos;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Entorno;
using GestoresDeNegocio.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace GestoresDeNegocio.Negocio
{

    public class GestorDeNegocio : GestorDeElementos<ContextoSe, NegocioDtm, NegocioDto>
    {

        public class MapearNegocio : Profile
        {
            public MapearNegocio()
            {
                CreateMap<NegocioDtm, NegocioDto>()
                .ForMember(dto => dto.PermisoDeGestor, dtm => dtm.MapFrom(x => x.PermisoDeGestor.Nombre))
                .ForMember(dto => dto.PermisoDeAdministrador, dtm => dtm.MapFrom(x => x.PermisoDeAdministrador.Nombre))
                .ForMember(dto => dto.PermisoDeConsultor, dtm => dtm.MapFrom(x => x.PermisoDeConsultor.Nombre));

                CreateMap<NegocioDto, NegocioDtm>()
                .ForMember(dtm => dtm.PermisoDeGestor, dto => dto.Ignore())
                .ForMember(dtm => dtm.PermisoDeAdministrador, dto => dto.Ignore())
                .ForMember(dtm => dtm.PermisoDeConsultor, dto => dto.Ignore());
            }
        }

        public GestorDeNegocio(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }
        internal static GestorDeNegocio Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeNegocio(contexto, mapeador);
        }


        protected override IQueryable<NegocioDtm> AplicarFiltros(IQueryable<NegocioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(NegocioDtm.Elemento).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        return registros.Where(x => x.Elemento == filtro.Valor);

                    if (filtro.Criterio == CriteriosDeFiltrado.contiene)
                        return registros.Where(x => x.Elemento.Contains(filtro.Valor));
                }
            }

            return registros;
        }

        protected override IQueryable<NegocioDtm> AplicarJoins(IQueryable<NegocioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.PermisoDeAdministrador);
            registros = registros.Include(p => p.PermisoDeConsultor);
            registros = registros.Include(p => p.PermisoDeGestor);
            return registros;
        }


        public bool TienePermisos(UsuarioDtm usuarioConectado, enumTipoDePermiso permisosNecesarios, string negocio)
        {
            var negocioDtm = LeerRegistroCacheado(nameof(NegocioDtm.Nombre), negocio);
            var cache = ServicioDeCaches.Obtener($"{nameof(GestorDeNegocio)}.{nameof(TienePermisos)}");
            var indice = $"{negocioDtm.Id}.{permisosNecesarios}";

            if (!cache.ContainsKey(indice))
            {
                var gestor = GestorDePermisosDeUnUsuario.Gestor(Contexto, Mapeador);

                var filtros = new List<ClausulaDeFiltrado>
                {
                    new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdUsuario), Criterio = CriteriosDeFiltrado.igual, Valor = usuarioConectado.Id.ToString()}
                };

                if (permisosNecesarios == enumTipoDePermiso.Administrador)
                    filtros.Add(new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdPermiso), Criterio = CriteriosDeFiltrado.igual, Valor = negocioDtm.IdPermisoDeAdministrador.ToString() });

                if (permisosNecesarios == enumTipoDePermiso.Gestor)
                    filtros.Add(new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdPermiso), Criterio = CriteriosDeFiltrado.contiene, Valor = $"{negocioDtm.IdPermisoDeGestor},{negocioDtm.IdPermisoDeAdministrador}" });

                if (permisosNecesarios == enumTipoDePermiso.Consultor)
                    filtros.Add(new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdPermiso), Criterio = CriteriosDeFiltrado.igual, Valor = $"{negocioDtm.IdPermisoDeConsultor},{negocioDtm.IdPermisoDeGestor},{negocioDtm.IdPermisoDeAdministrador}" });


                cache[indice] = gestor.Contar(filtros) > 0;
            }
            return (bool)cache[indice];
        }

        protected override void AntesDePersistir(NegocioDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);
            if (parametros.Tipo == TipoOperacion.Insertar)
            {
                registro.IdPermisoDeAdministrador = GestorDePermisos.CrearObtener(Contexto, Mapeador, registro.Nombre, enumClaseDePermiso.Negocio, enumTipoDePermiso.Administrador).Id;
                registro.IdPermisoDeGestor = GestorDePermisos.CrearObtener(Contexto, Mapeador, registro.Nombre, enumClaseDePermiso.Negocio, enumTipoDePermiso.Gestor).Id;
                registro.IdPermisoDeConsultor = GestorDePermisos.CrearObtener(Contexto, Mapeador, registro.Nombre, enumClaseDePermiso.Negocio, enumTipoDePermiso.Consultor).Id;
            }
        }

    }
}
