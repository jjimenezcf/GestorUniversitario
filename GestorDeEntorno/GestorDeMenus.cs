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

    static class FiltrosDeMenu
    {
        public static IQueryable<T> FiltrarPorPadre<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : MenuDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == nameof(MenuDtm.IdPadre).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.esNulo)
                       registros = registros.Where(x => x.IdPadre == null);

                    if (filtro.Criterio == CriteriosDeFiltrado.noEsNulo)
                        registros = registros.Where(x => x.IdPadre != null);

                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        registros = registros.Where(x => x.IdPadre != filtro.Valor.Entero());
                }
                     

            return registros;
        }
    }

    public class GestorDeMenus : GestorDeElementos<CtoEntorno, MenuDtm, MenuDto>
    {
        const string sqlMenu = @"
                  WITH menuPadre 
                  AS
                  (
                      SELECT ID, NOMBRE, DESCRIPCION, ICONO, ACTIVO, IDPADRE, IDVISTA_MVC
                      FROM entorno.MENU
                      UNION ALL
                      --RECURSIVIDAD
                      SELECT submenu.ID, submenu.NOMBRE, submenu.DESCRIPCION, submenu.ICONO, submenu.ACTIVO, submenu.IDPADRE, submenu.IDVISTA_MVC
                      FROM entorno.MENU AS submenu 
                  	 JOIN menuPadre AS menu ON submenu.IDPADRE = menu.id
                  )
                  SELECT DISTINCT ID, NOMBRE, DESCRIPCION, ICONO, ACTIVO, IDPADRE, IDVISTA_MVC
                  FROM menuPadre
                  order by id
                 ";

        public class MapearMenus : Profile
        {
            public MapearMenus()
            {
                CreateMap<MenuDtm, MenuDto>();
                CreateMap<MenuDto, MenuDtm>()
                .ForMember(rm => rm.IdVistaMvc, em => em.MapFrom(s => s.VistaMvc != null ? s.VistaMvc.Id : int.Parse(null)))
                .ForMember(rm => rm.IdPadre, em => em.MapFrom(m => m.Padre != null ? m.Padre.Id : int.Parse(null)))
                ;
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

        protected override bool ValidarAntesDeMapearElElemento(MenuDtm registro)
        {
            base.ValidarAntesDeMapearElElemento(registro);

            return registro.IdPadre == null;
        }

        public List<MenuDto> LeerMenuSe()
        {
            //var filtros = new List<ClausulaDeFiltrado>() { new ClausulaDeFiltrado { Propiedad = nameof(MenuDtm.IdPadre), Criterio = CriteriosDeFiltrado.esNulo } };
            //var ordenacion = new List<ClausulaOrdenacion>() { new ClausulaOrdenacion { Propiedad = nameof(MenuDtm.Id), modo = ModoDeOrdenancion.ascendente } };
            //var menusDto = new List<MenuDto>();
            //List<MenuDtm> menusDtm = LeerRegistros(0, -1, filtros, ordenacion).ToList();

            var menusDtm = Contexto.Menus.FromSqlRaw<MenuDtm>(sqlMenu).ToList();
            return MapearElementos(menusDtm).ToList();


            //foreach (var menuDtm in menusDtm)
            //{
            //    var menuLeido = Contexto.Menus
            //               .Include(x => x.Submenus)
            //               .SingleOrDefault(x => x.Id == menuDtm.Id);

            //    LeerSubMenus(menuLeido);
            //    var resultado = MapearElemento(menuLeido);
            //    menusDto.Add(resultado);
            //}

            //return menusDto;
        }

        private void LeerSubMenus(MenuDtm menuDtm)
        {
            foreach (var submenu in menuDtm.Submenus)
            {
                var menu = Contexto.Menus
                           .Include(x => x.Submenus)
                           .SingleOrDefault(x => x.Id == submenu.Id);
                if (menu.Submenus.Count > 0)
                    LeerSubMenus(submenu);
            }
        }

        protected override IQueryable<MenuDtm> AplicarFiltros(IQueryable<MenuDtm> registros, List<ClausulaDeFiltrado> filtros)
        {
            foreach (var f in filtros)
                if (f.Propiedad == FiltroPor.Id)
                    return base.AplicarFiltros(registros, filtros);

            return registros.FiltrarPorPadre(filtros);
        }

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
            m = new MenuDtm() { Id = 0, Padre = p, Nombre = "Funcionalidad", Descripcion = "", Icono = "cog-solid.svg",  Activo = true };
            InsertarRegistro(m);

            m = new MenuDtm() { Id = 0, Padre = p, Nombre = "Accesos", Descripcion = "", Icono = "cog-solid.svg",  Activo = true };
            
            p = m;
            m = new MenuDtm() { Id = 0, Padre = p, Nombre = "Usuarios", Descripcion = "", Icono = "cog-solid.svg", VistaMvc = null, Activo = true };
            InsertarRegistro(m);

            m = new MenuDtm() { Id = 0, Padre = p, Nombre = "Permisos", Descripcion = "", Icono = "cog-solid.svg", VistaMvc = null, Activo = true };
            InsertarRegistro(m);

            m = new MenuDtm() { Id = 0, Padre = null, Nombre = "Maestros", Descripcion = "", Icono = "home-solid.svg",  Activo = true };
            InsertarRegistro(m);


        }

    }

}
