using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Gestor.Elementos.Entorno;
using GestorDeSeguridad.ModeloIu;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace Gestor.Elementos.Seguridad
{
    public static partial class Joins
    {
        public static IQueryable<T> JoinDePuestosDeUnUsuario<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        where T : PuestoDeUnUsuarioDtm
        {
            foreach (ClausulaDeJoin join in joins)
            {
                if (join.Dtm == typeof(UsuarioDtm))
                    registros = registros.Include(p => p.Usuario);
                if (join.Dtm == typeof(PuestoDtm))
                    registros = registros.Include(p => p.Puesto);
            }

            return registros;
        }
    }

    static class FiltrosDePuestosDeUnUsuario    {

        public static IQueryable<T> AnadirFiltros<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros)
        where T : PuestoDeUnUsuarioDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(PuestoDeUnUsuarioDtm.IdUsuario).ToLower())
                {
                    registros = registros.Where(p => p.IdUsuario == filtro.Valor.Entero());
                }
            }

            return registros;
        }
    }

    public class GestorDePuestoDeUnUsuario : GestorDeElementos<ContextoSe, PuestoDeUnUsuarioDtm, PuestoDeUnUsuarioDto>
    {

        public class MapearClasePermiso : Profile
        {
            public MapearClasePermiso()
            {
                CreateMap<PuestoDeUnUsuarioDtm, PuestoDeUnUsuarioDto>()
                    .ForMember(dto => dto.Puesto, dtm => dtm.MapFrom(dtm => dtm.Puesto.Nombre))
                    .ForMember(dto => dto.Usuario, dtm => dtm.MapFrom(dtm => dtm.Usuario.Login));

                CreateMap<PuestoDeUnUsuarioDto, PuestoDeUnUsuarioDtm>();
            }
        }

        public GestorDePuestoDeUnUsuario(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {


        }

        internal static GestorDePuestoDeUnUsuario Gestor(IMapper mapeador)
        {
            var contexto = ContextoSe.ObtenerContexto();
            return (GestorDePuestoDeUnUsuario)CrearGestor<GestorDePuestoDeUnUsuario>(() => new GestorDePuestoDeUnUsuario(contexto, mapeador));
        }

        protected override void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            base.DefinirJoins(filtros, joins, parametros);
            joins.Add(new ClausulaDeJoin { Dtm = typeof(UsuarioDtm) });
            joins.Add(new ClausulaDeJoin { Dtm = typeof(PuestoDtm) });
        }

        protected override IQueryable<PuestoDeUnUsuarioDtm> AplicarJoins(IQueryable<PuestoDeUnUsuarioDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, joins, parametros);

            return registros.JoinDePuestosDeUnUsuario(joins, parametros);
        }

        protected override IQueryable<PuestoDeUnUsuarioDtm> AplicarFiltros(IQueryable<PuestoDeUnUsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId(registros))
                return registros;

            return registros.AnadirFiltros(filtros);
        }

        public dynamic LeerUsuarios(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDeUsuarios.Gestor(Mapeador); var filtros = new List<ClausulaDeFiltrado>();
            if (!filtro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(UsuarioDto.NombreCompleto), Valor = filtro });

            var clasesDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(clasesDtm).ToList();
        }

        public dynamic LeerPuestos(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDePuestosDeTrabajo.Gestor(Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            if (!filtro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(PuestoDto.Nombre), Valor = filtro });

            var clasesDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(clasesDtm).ToList();
        }
    }
}

