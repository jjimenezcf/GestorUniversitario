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

namespace GestoresDeNegocio.Seguridad
{
    public static partial class Joins
    {
        public static IQueryable<T> JoinDePuestosDeUnUsuario<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        where T : PuestosDeUnUsuarioDtm
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

        public static IQueryable<T> PuestosDeUnUsuario<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros)
        where T : PuestosDeUnUsuarioDtm
        {
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
    }

    static class OrdenacionDePuestosDeUnUsuario
    {
        public static IQueryable<PuestosDeUnUsuarioDtm> OrdenarPuestosDeUnUsuario(this IQueryable<PuestosDeUnUsuarioDtm> set, List<ClausulaDeOrdenacion> ordenacion)
        {
            if (ordenacion.Count == 0)
                return set.OrderBy(x => x.Puesto.Nombre);
            return set;
        }
    }


    public class GestorDePuestosDeUnUsuario : GestorDeElementos<ContextoSe, PuestosDeUnUsuarioDtm, PuestosDeUnUsuarioDto>
    {

        public class MapearClasePermiso : Profile
        {
            public MapearClasePermiso()
            {
                CreateMap<PuestosDeUnUsuarioDtm, PuestosDeUnUsuarioDto>()
                    .ForMember(dto => dto.Puesto, dtm => dtm.MapFrom(dtm => dtm.Puesto.Nombre))
                    .ForMember(dto => dto.Usuario, dtm => dtm.MapFrom(dtm => dtm.Usuario.Login));

                CreateMap<PuestosDeUnUsuarioDto, PuestosDeUnUsuarioDtm>();
            }
        }

        public GestorDePuestosDeUnUsuario(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {


        }

        internal static GestorDePuestosDeUnUsuario Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePuestosDeUnUsuario(contexto, mapeador);
        }

        protected override void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            base.DefinirJoins(filtros, joins, parametros);
            joins.Add(new ClausulaDeJoin { Dtm = typeof(UsuarioDtm) });
            joins.Add(new ClausulaDeJoin { Dtm = typeof(PuestoDtm) });
        }

        protected override IQueryable<PuestosDeUnUsuarioDtm> AplicarJoins(IQueryable<PuestosDeUnUsuarioDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, joins, parametros);

            return registros.JoinDePuestosDeUnUsuario(joins, parametros);
        }

        protected override IQueryable<PuestosDeUnUsuarioDtm> AplicarFiltros(IQueryable<PuestosDeUnUsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId(registros))
                return registros;

            return registros.PuestosDeUnUsuario(filtros);
        }

        protected override IQueryable<PuestosDeUnUsuarioDtm> AplicarOrden(IQueryable<PuestosDeUnUsuarioDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);
            return registros.OrdenarPuestosDeUnUsuario(ordenacion);
        }


        public dynamic LeerUsuarios(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDeUsuarios.Gestor(Contexto, Mapeador);            
            var filtros = new List<ClausulaDeFiltrado>();
            if (!filtro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(UsuarioDto.NombreCompleto), Valor = filtro });

            var clasesDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(clasesDtm).ToList();
        }

        public dynamic LeerPuestos(int posicion, int cantidad, string filtro)
        {
            var gestor = GestorDePuestosDeTrabajo.Gestor(Contexto, Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            if (!filtro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(PuestoDto.Nombre), Valor = filtro });

            var clasesDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(clasesDtm).ToList();
        }
    }
}

