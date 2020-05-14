using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using System;

namespace Gestor.Elementos.Entorno
{
    public static partial class Joins
    {
        public static IQueryable<T> JoinConMenus<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros) where T : MenuDtm
        {
            foreach (ClausulaDeJoin join in joins)
            {
                if (join.Dtm == typeof(MenuDtm))
                    registros = registros.Include(p => p.Padre);
                if (join.Dtm == typeof(VistaMvcDtm))
                    registros = registros.Include(p => p.VistaMvc);
            }

            return registros;
        }
    }

    public static partial class Filtros
    {
        public static IQueryable<T> FiltrarMenus<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros) where T : MenuDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == nameof(MenuDtm.IdPadre).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.esNulo)
                        registros = registros.Where(x => x.IdPadre == null);

                    if (filtro.Criterio == CriteriosDeFiltrado.noEsNulo)
                        registros = registros.Where(x => x.IdPadre != null);

                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        registros = registros.Where(x => x.IdPadre == filtro.Valor.Entero());
                }
                else
                    registros = registros.FiltrarPorId(filtro);

            return registros;
        }

        public static IQueryable<T> FiltrarPorMenuPadre<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros) where T : MenuDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == nameof(MenuDtm.Padre).ToLower())
                {
                    registros = registros.Where(x => x.IdPadre == filtro.Valor.Entero());
                }

            return registros;
        }
    }

    public static partial class Ordenaciones
    {
        public static IQueryable<T> OrdenarMenus<T>(this IQueryable<T> registros, List<ClausulaDeOrdenacion> ordenacion) where T : MenuDtm
        {
            foreach (ClausulaDeOrdenacion orden in ordenacion)
                if (orden.Propiedad.ToLower() == nameof(MenuDtm.Orden).ToLower())
                    registros = orden.Modo == ModoDeOrdenancion.ascendente
                    ? registros.OrderBy(x => x.Orden)
                    : registros.OrderByDescending(x => x.Orden);
                else
                    registros = registros.OrdenPorId(orden);


            return registros;
        }
    }

    public class GestorDeMenus : GestorDeElementos<CtoEntorno, MenuDtm, MenuDto>
    {
        public class MapearMenus : Profile
        {
            public MapearMenus()
            {
                CreateMap<MenuDtm, MenuDto>()
                .ForMember(dto => dto.Padre, dtm => dtm.MapFrom(dtm => dtm.Padre.Nombre))
                .ForMember(dto => dto.VistaMvc, dtm => dtm.MapFrom(dtm => dtm.VistaMvc));
            }
        }

        public GestorDeMenus(CtoEntorno contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            base.DefinirJoins(filtros, joins, parametros);

            foreach (var filtro in filtros)
                if (filtro.Propiedad == nameof(MenuDtm.IdPadre) && filtro.Criterio == CriteriosDeFiltrado.esNulo)
                    return;

            joins.Add(new ClausulaDeJoin { Dtm = typeof(MenuDtm) });
            joins.Add(new ClausulaDeJoin { Dtm = typeof(VistaMvcDtm) });
        }

        protected override void DespuesDeMapearRegistro(MenuDto elemento, MenuDtm registro, ParametrosDeNegocio opciones)
        {
            base.DespuesDeMapearRegistro(elemento, registro, opciones);
            if (TipoOperacion.Insertar == opciones.Tipo)
            {
                registro.Padre = null;
                registro.VistaMvc = null;
            }
        }

        protected override IQueryable<MenuDtm> AplicarFiltros(IQueryable<MenuDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros) => registros
                .FiltrarMenus(filtros, parametros)
                .FiltrarPorMenuPadre(filtros, parametros);

        protected override IQueryable<MenuDtm> AplicarOrden(IQueryable<MenuDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            return registros.OrdenarMenus(ordenacion);
        }

        protected override IQueryable<MenuDtm> AplicarJoins(IQueryable<MenuDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            return registros.JoinConMenus(joins, parametros);
        }

        public List<MenuDto> LeerPadres()
        {
            var registros = Contexto
                            .Menus
                            .FromSqlInterpolated($@"select distinct t2.*
                                                    from entorno.MENU t1
                                                    inner join entorno.MENU t2 on t2.id =t1.IDPADRE
                                                    order by t2.IDPADRE, t2.id")
                            .ToList();

            var elementos = MapearElementos(registros).ToList();
            return elementos;
        }

        public List<ArbolDeMenuDto> LeerArbolDeMenu()
        {
            var resultadoDto = new List<ArbolDeMenuDto>();
            var gestor = new GestorDeArbolDeMenu(Contexto, Mapeador);
            List<ArbolDeMenuDtm> arbolDeMenu = gestor.LeerRegistros(0, -1);
            procesarSubMenus(gestor, resultadoDto, arbolDeMenu, padre: null);
            return resultadoDto;
        }

        private void procesarSubMenus(GestorDeArbolDeMenu gestor, List<ArbolDeMenuDto> resultadoDto, List<ArbolDeMenuDtm> arbolDeMenu, ArbolDeMenuDto padre)
        {
            List<ArbolDeMenuDtm> procesarMenus = MenusParaProcesar(arbolDeMenu, padre);
            if (procesarMenus.Count == 0)
                return;

            foreach (var menuDtm in procesarMenus)
            {
                var menuDto = gestor.MapearElemento(menuDtm);
                if (padre != null)
                    padre.Submenus.Add(menuDto);

                resultadoDto.Add(menuDto);
                if (menuDtm.IdVistaMvc == null)
                {
                    procesarSubMenus(gestor, resultadoDto, arbolDeMenu, padre: menuDto);
                }
            }
        }

        private List<ArbolDeMenuDtm> MenusParaProcesar(List<ArbolDeMenuDtm> arbolDeMenu, ArbolDeMenuDto padre)
        {
            var resultado = new List<ArbolDeMenuDtm>();
            var procesar = new List<ArbolDeMenuDtm>();

            foreach (var nodo in arbolDeMenu)
                if ((nodo.IdPadre == null && padre == null) || (padre != null && nodo.IdPadre == padre.Id))
                    procesar.Add(nodo);

            if (procesar.Count == 0)
                return resultado;

            while (procesar.Count > 0)
            {
                var orden = procesar[0].Orden;
                var quitar = 0;
                for (var i = 0; i < procesar.Count; i++)
                {
                    if (procesar[i].Orden <= orden)
                    {
                        orden = procesar[i].Orden;
                        quitar = i;
                    }
                }
                resultado.Add(procesar[quitar]);
                procesar.RemoveAt(quitar);
            }

            return resultado;
        }


    }

}
