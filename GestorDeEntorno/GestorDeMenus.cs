using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using AutoMapper.Configuration.Annotations;
using System;
using Gestor.Errores;
using AutoMapper.Configuration.Conventions;

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
        public static IQueryable<T> FiltrarMenus<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : MenuDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == nameof(MenuDtm.IdPadre).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.esNulo)
                        registros = registros.Where(x => x.IdPadre == null);

                    if (filtro.Criterio == CriteriosDeFiltrado.noEsNulo)
                        registros = registros.Where(x => x.IdPadre != null);

                    if (filtro.Criterio == CriteriosDeFiltrado.igual && filtro.Valor.Entero() > 0)
                        registros = registros.Where(x => x.IdPadre == filtro.Valor.Entero());
                }

            return registros;
        }

        public static IQueryable<T> FiltrarPorMenuPadre<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : MenuDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == nameof(MenuDtm.Padre).ToLower())
                {
                    registros = registros.Where(x => x.IdPadre == filtro.Valor.Entero());
                }

            return registros;
        }
        public static IQueryable<T> FiltrarPorActivo<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : MenuDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == nameof(MenuDtm.Activo).ToLower())
                {
                    registros = registros.Where(x => x.Activo == bool.Parse(filtro.Valor));
                }

            return registros;
        }
    }

    public static partial class Ordenaciones
    {
        public static IQueryable<T> OrdenarMenus<T>(this IQueryable<T> registros, List<ClausulaDeOrdenacion> ordenacion) where T : MenuDtm
        {
            foreach (ClausulaDeOrdenacion orden in ordenacion)
                if (orden.Propiedad.ToLower() == nameof(MenuDtm.Padre).ToLower())
                    registros = orden.Modo == ModoDeOrdenancion.ascendente
                    ? registros.OrderBy(x => x.Padre.Orden)
                    : registros.OrderByDescending(x => x.Padre.Orden);
                else
                if (orden.Propiedad.ToLower() == nameof(MenuDtm.Nombre).ToLower())
                    registros = orden.Modo == ModoDeOrdenancion.ascendente
                    ? registros.OrderBy(x => x.Orden)
                    : registros.OrderByDescending(x => x.Orden);
                else
                    registros = registros.OrdenPorId(orden);


            return registros;
        }
    }

    public class GestorDeMenus : GestorDeElementos<ContextoSe, MenuDtm, MenuDto>
    {
        public class MapearMenus : Profile
        {
            public MapearMenus()
            {
                CreateMap<MenuDtm, MenuDto>()
                .ForMember(dto => dto.Padre, dtm => dtm.MapFrom(dtm => dtm.Padre.Nombre))
                .ForMember(dto => dto.VistaMvc, dtm => dtm.MapFrom(dtm => $"{dtm.VistaMvc.Controlador}.{dtm.VistaMvc.Accion}"));

                CreateMap<MenuDto, MenuDtm>()
                    .ForMember(dtm => dtm.IdVistaMvc, dto => dto.Ignore())
                    .ForMember(dtm => dtm.VistaMvc, dto => dto.Ignore());
            }
        }

        public GestorDeMenus(ContextoSe contexto, IMapper mapeador)
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

        protected override IQueryable<MenuDtm> AplicarFiltros(IQueryable<MenuDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);
           
            if (HayFiltroPorId(registros))
                return registros;

            registros = registros
                .FiltrarMenus(filtros)
                .FiltrarPorMenuPadre(filtros)
                .FiltrarPorActivo(filtros);

            return registros;
        }

        protected override IQueryable<MenuDtm> AplicarOrden(IQueryable<MenuDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            return registros.OrdenarMenus(ordenacion);
        }

        protected override IQueryable<MenuDtm> AplicarJoins(IQueryable<MenuDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            return registros.JoinConMenus(joins, parametros);
        }


        protected override void DespuesDeMapearElemento(MenuDtm registro, MenuDto elemento, ParametrosDeMapeo parametros)
        {
            base.DespuesDeMapearElemento(registro, elemento, parametros);
            if (registro.Icono != null)
            {
                elemento.Icono = $@"/images/menu/{elemento.Icono}";
            }
        }

        protected override void DespuesDeMapearRegistro(MenuDto elemento, MenuDtm registro, ParametrosDeNegocio opciones)
        {
            base.DespuesDeMapearRegistro(elemento, registro, opciones);

            registro.IdVistaMvc = LeerVistaMvc(vistaMvc: elemento.VistaMvc);

            registro.Activo = true;

            if (TipoOperacion.Insertar == opciones.Tipo)
            {
                registro.Padre = null;
                registro.VistaMvc = null;
            }
        }

        private int? LeerVistaMvc(string vistaMvc)
        {
            if (vistaMvc.IsNullOrEmpty())
                return null;

            var partes = vistaMvc.Split(".");
            
            if (partes.Length != 2)
                GestorDeErrores.Emitir($"El valor proporcionado {vistaMvc} no es válido, ha de seguir el patrón Controlador.Vista");
            
            var gestor = GestorDeVistasMvc.Gestor(Mapeador);
            var filtros = new List<ClausulaDeFiltrado>
                {
                    new ClausulaDeFiltrado { Propiedad = nameof(VistaMvcDtm.Controlador), Criterio = CriteriosDeFiltrado.igual, Valor = partes[0] },
                    new ClausulaDeFiltrado { Propiedad = nameof(VistaMvcDtm.Accion), Criterio = CriteriosDeFiltrado.igual, Valor = partes[1] }
                };

            var vistas = gestor.LeerRegistros(0, -1, filtros);
            if (vistas.Count != 1)
            {
                if (vistas.Count == 0)
                    GestorDeErrores.Emitir($"No se ha localizado la vistaMvc {partes[0]}.{partes[1]}");
                else
                    GestorDeErrores.Emitir($"Se han localizado {vistas.Count} vistasMvc para {partes[0]}.{partes[1]}");
            }

            return vistas[0].Id;


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

        public static List<ArbolDeMenuDto> LeerArbolDeMenu(IMapper mapeador)
        {

            object gestor = CrearGestor<GestorDeArbolDeMenu>(() => new GestorDeArbolDeMenu(() => ContextoSe.ObtenerContexto(), mapeador));

            return ((GestorDeArbolDeMenu)gestor).LeerArbolDeMenu();
        }

    }

}
