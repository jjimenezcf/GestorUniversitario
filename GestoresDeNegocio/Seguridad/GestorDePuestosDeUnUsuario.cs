using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto.Entorno;
using ModeloDeDto.Seguridad;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;
using Utilidades;
using GestoresDeNegocio.Entorno;
using ServicioDeDatos.Elemento;
using System;
using ModeloDeDto;

namespace GestoresDeNegocio.Seguridad
{

    public class GestorDePuestosDeUnUsuario : GestorDeRelaciones<ContextoSe, PuestosDeUnUsuarioDtm, PuestosDeUnUsuarioDto>
    {

        public class MapearClasePermiso : Profile
        {
            public MapearClasePermiso()
            {
                CreateMap<PuestosDeUnUsuarioDtm, PuestosDeUnUsuarioDto>()
                    .ForMember(dto => dto.Puesto, dtm => dtm.MapFrom(dtm => dtm.Puesto.Nombre))
                    .ForMember(dto => dto.Usuario, dtm => dtm.MapFrom(dtm => dtm.Usuario.Login));

                CreateMap<PuestosDeUnUsuarioDto, PuestosDeUnUsuarioDtm>()
                    .ForMember(dtm => dtm.Usuario, dto => dto.Ignore())
                    .ForMember(dtm => dtm.Puesto, dto => dto.Ignore());
            }
        }

        public GestorDePuestosDeUnUsuario(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador, false)
        {


        }

        internal static GestorDePuestosDeUnUsuario Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePuestosDeUnUsuario(contexto, mapeador);
        }

        protected override IQueryable<PuestosDeUnUsuarioDtm> AplicarJoins(IQueryable<PuestosDeUnUsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Usuario);
            registros = registros.Include(p => p.Puesto);
            return registros;
        }

        protected override IQueryable<PuestosDeUnUsuarioDtm> AplicarFiltros(IQueryable<PuestosDeUnUsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(PuestosDeUnUsuarioDtm.IdUsuario).ToLower())
                {
                    registros = registros.Where(x => x.IdUsuario == filtro.Valor.Entero());
                }

                if (filtro.Clausula.ToLower() == nameof(PuestosDeUnUsuarioDto.Puesto).ToLower() && !filtro.Valor.IsNullOrEmpty())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.contiene)
                        registros = registros.Where(x => x.Puesto.Nombre.Contains(filtro.Valor));

                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        registros = registros.Where(x => x.Puesto.Nombre == filtro.Valor);
                }

                if (filtro.Clausula.ToLower() == nameof(PuestosDeUnUsuarioDtm.IdPuesto).ToLower())
                    registros = registros.Where(x => x.IdPuesto == filtro.Valor.Entero());
            }

            return registros;
        }

        protected override void DespuesDePersistir(PuestosDeUnUsuarioDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            if (parametros.Operacion == enumTipoOperacion.Modificar || parametros.Operacion == enumTipoOperacion.Eliminar)
            {
                var parteDeLaClave = $"Usuario:{registro.IdUsuario}";
                ServicioDeCaches.EliminarElementos($"{nameof(GestorDeVistaMvc)}.{nameof(GestorDeVistaMvc.TienePermisos)}", parteDeLaClave);
                ServicioDeCaches.EliminarElementos($"{nameof(GestorDeElementos)}.{nameof(ValidarPermisosDePersistencia)}", parteDeLaClave);
                ServicioDeCaches.EliminarElementos($"{nameof(GestorDeElementos)}.{nameof(LeerModoDeAccesoAlNegocio)}", parteDeLaClave);
            }
        }
    }
}

