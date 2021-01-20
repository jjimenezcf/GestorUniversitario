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
using System;

namespace GestoresDeNegocio.Seguridad
{

    public class GestorDeUsuariosDeUnPuesto : GestorDeElementos<ContextoSe, PuestosDeUnUsuarioDtm, UsuariosDeUnPuestoDto>
    {
        public class MapearClasePermiso : Profile
        {
            public MapearClasePermiso()
            {
                CreateMap<PuestosDeUnUsuarioDtm, UsuariosDeUnPuestoDto>()
                    .ForMember(dto => dto.Puesto, dtm => dtm.MapFrom(dtm => dtm.Puesto.Nombre))
                    .ForMember(dto => dto.Usuario, dtm => dtm.MapFrom(dtm => $"({dtm.Usuario.Login}) {dtm.Usuario.Apellido}, {dtm.Usuario.Nombre}"));

                CreateMap<UsuariosDeUnPuestoDto, PuestosDeUnUsuarioDtm>();
            }
        }

        public GestorDeUsuariosDeUnPuesto(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
            InvertirMapeoDeRelacion = true;
        }

        internal static GestorDeUsuariosDeUnPuesto Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeUsuariosDeUnPuesto(contexto, mapeador);
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
                    registros = registros.Where(x => x.IdUsuario == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(PuestosDeUnUsuarioDtm.IdPuesto).ToLower())
                    registros = registros.Where(x => x.IdPuesto == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(UsuariosDeUnPuestoDto.Usuario).ToLower())
                    registros = registros.Where(x => x.Usuario.Apellido.Contains(filtro.Valor)
                                                  || x.Usuario.Nombre.Contains(filtro.Valor)
                                                  || x.Usuario.Login.Contains(filtro.Valor));
            }

            return registros;
        }

        protected override IQueryable<PuestosDeUnUsuarioDtm> AplicarOrden(IQueryable<PuestosDeUnUsuarioDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);

            if (ordenacion.Count == 0)
                return registros.OrderBy(x => x.Puesto.Nombre);
            return registros;
        }

        protected override void DespuesDePersistir(PuestosDeUnUsuarioDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            if (parametros.Operacion == TipoOperacion.Modificar || parametros.Operacion == TipoOperacion.Eliminar)
            {
                var parteDeLaClave = $"Usuario:{registro.IdUsuario}";
                ServicioDeCaches.EliminarElementos($"{nameof(GestorDeVistaMvc)}.{nameof(GestorDeVistaMvc.TienePermisos)}", parteDeLaClave);
                ServicioDeCaches.EliminarElementos($"{nameof(GestorDeElementos)}.{nameof(ValidarPermisosDePersistencia)}", parteDeLaClave);
            }
        }

    }
}

