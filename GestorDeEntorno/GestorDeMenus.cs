using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using Gestor.Elementos.ModeloIu;
using System;
using System.Threading.Tasks;

namespace Gestor.Elementos.Entorno
{
    public static partial class Joins
    {
        public static IQueryable<T> JoinConMenus<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins) where T : MenuDtm
        {
            foreach (ClausulaDeJoin join in joins)
            {
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

                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        registros = registros.Where(x => x.IdPadre == filtro.Valor.Entero());
                }
                else
                    registros = registros.FiltrarPorId(filtro);


            return registros;
        }
    }

    public static partial class Ordenaciones
    {
        public static IQueryable<T> OrdenarMenus<T>(this IQueryable<T> registros, List<ClausulaOrdenacion> ordenacion) where T : MenuDtm
        {
            foreach (ClausulaOrdenacion orden in ordenacion)
                if (orden.Propiedad.ToLower() == nameof(MenuDtm.Orden).ToLower())
                    registros = orden.modo == ModoDeOrdenancion.ascendente
                    ? registros.OrderBy(x => x.Orden)
                    : registros.OrderByDescending(x => x.Orden);
                else
                    registros = registros.OrdenPorId(orden);


            return registros;
        }
    }

    public class GestorDeMenus : GestorDeElementos<CtoEntorno, MenuDtm, MenuDto>
    {
        const string sqlMenu = @"
                  WITH menuPadre 
                  AS
                  (
                      SELECT ID, NOMBRE, DESCRIPCION, ICONO, ACTIVO, IDPADRE, IDVISTA_MVC, ORDEN
                      FROM entorno.MENU
                      UNION ALL
                      --RECURSIVIDAD
                      SELECT submenu.ID, submenu.NOMBRE, submenu.DESCRIPCION, submenu.ICONO, submenu.ACTIVO, submenu.IDPADRE, submenu.IDVISTA_MVC, submenu.ORDEN
                      FROM entorno.MENU AS submenu 
                  	 JOIN menuPadre AS menu ON submenu.IDPADRE = menu.id
                  )
                  SELECT DISTINCT ID, NOMBRE, DESCRIPCION, ICONO, ACTIVO, IDPADRE, IDVISTA_MVC, ORDEN
                  FROM menuPadre
                  order by id
                 ";

        public class MapearMenus : Profile
        {
            public MapearMenus()
            {
                CreateMap<MenuDtm, MenuDto>()
                .ForMember(dtm => dtm.Submenus, dto => dto.MapFrom(dtm => dtm.Submenus))
                .ForMember(dtm => dtm.VistaMvc, dto => dto.MapFrom(dtm => dtm.VistaMvc)); // (VistaMvcDto)null));
            }
        }

        public GestorDeMenus(CtoEntorno contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override void DespuesDeMapearRegistro(MenuDto elemento, MenuDtm registro, TipoOperacion tipo)
        {
            base.DespuesDeMapearRegistro(elemento, registro, tipo);
            if (TipoOperacion.Insertar == tipo)
            {
                registro.Padre = null;
                registro.VistaMvc = null;
            }
        }

        protected override MenuDtm LeerConDetalle(int Id)
        {
            throw new System.NotImplementedException();
        }

        protected override bool AntesDeMapearElemento(MenuDtm registro, Dictionary<string, object> parametros)
        {
            if (base.AntesDeMapearElemento(registro, parametros))
                return registro.IdPadre == null;

            return false;
        }

        public List<MenuDto> LeerMenuSe()
        {
            var filtros = new List<ClausulaDeFiltrado>() { new ClausulaDeFiltrado { Propiedad = nameof(MenuDtm.IdPadre), Criterio = CriteriosDeFiltrado.esNulo } };
            var ordenacion = new List<ClausulaOrdenacion>() { new ClausulaOrdenacion { Propiedad = nameof(MenuDtm.Orden), modo = ModoDeOrdenancion.ascendente } };
            var menusDto = new List<MenuDto>();
            List<MenuDtm> menusDtm = LeerRegistros(0, -1, filtros, ordenacion).ToList();
            
            foreach (var menuDtm in menusDtm)
            {
                LeerSubMenus(menuDtm);
                var resultado = MapearElemento(menuDtm, new Dictionary<string, object>());
                menusDto.Add(resultado);
            }

            return menusDto;

            //var menusDtm = Contexto.Menus.FromSqlRaw<MenuDtm>(sqlMenu).ToList();
            //return MapearElementos(menusDtm).ToList();
        }

        private void LeerSubMenus(MenuDtm menuDtm)
        {
            var filtros = new List<ClausulaDeFiltrado>() { new ClausulaDeFiltrado { Propiedad = nameof(MenuDtm.IdPadre), Criterio = CriteriosDeFiltrado.igual, Valor = menuDtm.Id.ToString() } };
            var ordenacion = new List<ClausulaOrdenacion>() { new ClausulaOrdenacion { Propiedad = nameof(MenuDtm.Orden), modo = ModoDeOrdenancion.ascendente } };
            var joins = new List<ClausulaDeJoin>() { new ClausulaDeJoin { Dtm = typeof(VistaMvcDtm)} };
            menuDtm.Submenus = LeerRegistros(0, -1, filtros, ordenacion, joins).ToList();

            foreach (var submenu in menuDtm.Submenus)
            {
                LeerSubMenus(submenu);
                //if (submenu.IdVistaMvc != null)
                //    submenu.VistaMvc = CrearGestorDeVista(Contexto).LeerRegistroPorId(submenu.IdVistaMvc);
            }
        }

        private GestorDeVistasMvc CrearGestorDeVista(CtoEntorno contexto)
        {
            var configuradorDeMapeos = new MapperConfiguration(cfg => { cfg.CreateMap<VistaMvcDtm, VistaMvcDto>(); });
            IMapper mapeador = configuradorDeMapeos.CreateMapper();

            return new GestorDeVistasMvc(contexto, mapeador);
        }

        protected override IQueryable<MenuDtm> AplicarFiltros(IQueryable<MenuDtm> registros, List<ClausulaDeFiltrado> filtros)
        {
            return registros.FiltrarMenus(filtros);
        }

        protected override IQueryable<MenuDtm> AplicarOrden(IQueryable<MenuDtm> registros, List<ClausulaOrdenacion> ordenacion)
        {
            return registros.OrdenarMenus(ordenacion);
        }

        protected override IQueryable<MenuDtm> AplicarJoins(IQueryable<MenuDtm> registros, List<ClausulaDeJoin> joins)
        {
            return registros.JoinConMenus(joins);
        }

        #region Codigo a borrar
        public void InicializarMenu()
        {
            var m = new MenuDto() { Id = 1, Padre = null, Nombre = "Configuración", Descripcion = "", Icono = "cog-solid.svg", Submenus = new List<MenuDto>(), Activo = true };
            InsertarElemento(m);

            CrearMenuDeConfiguracion(m);

            m = new MenuDto() { Id = 6, Padre = null, Nombre = "Maestros", Descripcion = "", Icono = "home-solid.svg", Submenus = new List<MenuDto>(), Activo = true };
            InsertarElemento(m);

            m = new MenuDto() { Id = 7, Padre = null, Nombre = "Gestión documental", Descripcion = "", Icono = "cog-solid.svg", Submenus = new List<MenuDto>(), Activo = true };
            InsertarElemento(m);

            m = new MenuDto() { Id = 8, Padre = null, Nombre = "Gestión administrativa", Descripcion = "", Icono = "cog-solid.svg", Submenus = new List<MenuDto>(), Activo = true };
            InsertarElemento(m);

            m = new MenuDto() { Id = 9, Padre = null, Nombre = "Gestión jurídica", Descripcion = "", Icono = "cog-solid.svg", Submenus = new List<MenuDto>(), Activo = true };
            InsertarElemento(m);

            m = new MenuDto() { Id = 10, Padre = null, Nombre = "Gestión logística", Descripcion = "", Icono = "cog-solid.svg", Submenus = new List<MenuDto>(), Activo = true };
            InsertarElemento(m);

            m = new MenuDto() { Id = 11, Padre = null, Nombre = "Gestión técnica", Descripcion = "", Icono = "cog-solid.svg", Submenus = new List<MenuDto>(), Activo = true };
            InsertarElemento(m);

            m = new MenuDto() { Id = 12, Padre = null, Nombre = "Gestión financiera", Descripcion = "", Icono = "cog-solid.svg", Submenus = new List<MenuDto>(), Activo = true };
            InsertarElemento(m);
        }

        private void CrearMenuDeConfiguracion(MenuDto padre)
        {
            MenuDto m = new MenuDto() { Id = 2, Padre = padre, Nombre = "Funcionalidad", Descripcion = "", Icono = "cog-solid.svg", Submenus = new List<MenuDto>(), Activo = true };
            InsertarElemento(m);

            m = new MenuDto() { Id = 3, Padre = padre, Nombre = "Accesos", Descripcion = "", Icono = "cog-solid.svg", Submenus = new List<MenuDto>(), Activo = true };
            InsertarElemento(m);
            MenuDeAccesos(m);
        }

        private void MenuDeAccesos(MenuDto padre)
        {
            MenuDto m = new MenuDto() { Id = 4, Padre = padre, Nombre = "Usuarios", Descripcion = "", Icono = "cog-solid.svg", VistaMvc = null, Activo = true };
            //var gestorDeVistasMvc = new GestorDeVistasMvc(Contexto, Mapeador);
            //gestorDeVistasMvc.Leer(0,1,)

            InsertarElemento(m);

            m = new MenuDto() { Id = 5, Padre = padre, Nombre = "Permisos", Descripcion = "", Icono = "cog-solid.svg", VistaMvc = null, Activo = true };
            InsertarElemento(m);
        }

        public void InicializarMenuDeGolpe()
        {
            var menus = new List<MenuDtm>();

            var m = new MenuDtm() { Id = 0, Padre = null, Nombre = "Configuración", Descripcion = "", Icono = "cog-solid.svg", Activo = true };

            var p = m;
            m = new MenuDtm() { Id = 0, Padre = p, Nombre = "Funcionalidad", Descripcion = "", Icono = "cog-solid.svg", Activo = true };
            InsertarRegistro(m);

            m = new MenuDtm() { Id = 0, Padre = p, Nombre = "Accesos", Descripcion = "", Icono = "cog-solid.svg", Activo = true };

            p = m;
            m = new MenuDtm() { Id = 0, Padre = p, Nombre = "Usuarios", Descripcion = "", Icono = "cog-solid.svg", VistaMvc = null, Activo = true };
            InsertarRegistro(m);

            m = new MenuDtm() { Id = 0, Padre = p, Nombre = "Permisos", Descripcion = "", Icono = "cog-solid.svg", VistaMvc = null, Activo = true };
            InsertarRegistro(m);

            m = new MenuDtm() { Id = 0, Padre = null, Nombre = "Maestros", Descripcion = "", Icono = "home-solid.svg", Activo = true };
            InsertarRegistro(m);


        }
        #endregion

    }

}
