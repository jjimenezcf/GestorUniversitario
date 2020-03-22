using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Entorno
{

    public class GestorDeMenus : GestorDeElementos<CtoEntorno, R_Menu, E_Menu>
    {

        public class MapearMenus : Profile
        {
            public MapearMenus()
            {
                CreateMap<R_Menu, E_Menu>();
                CreateMap<E_Menu, R_Menu>()
                .ForMember(rm => rm.IdVistaMvc, em => em.MapFrom(s => s.VistaMvc != null ? s.VistaMvc.Id : int.Parse(null)))
                .ForMember(rm => rm.IdPadre, em => em.MapFrom(m => m.Padre != null ? m.Padre.Id : int.Parse(null)))
                ;
            }
        }

        public GestorDeMenus(CtoEntorno contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override void DespuesDeMapearRegistro(E_Menu elemento, R_Menu registro, TipoOperacion tipo)
        {
            base.DespuesDeMapearRegistro(elemento, registro, tipo);
            if (TipoOperacion.Insertar == tipo)
            {
                registro.Padre = null;
                registro.VistaMvc = null;
            }
        }

        protected override R_Menu LeerConDetalle(int Id)
        {
            throw new System.NotImplementedException();
        }

        public static List<E_Menu> MenuPrincipal()
        {
            var menus = new List<E_Menu>();

            var m = new E_Menu() { Id = 1, Padre = null, Nombre = "Configuración", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            menus.Add(m);

            m = new E_Menu() { Id = 6, Padre = null, Nombre = "Maestros", Descripcion = "", Icono = "home-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            menus.Add(m);

            m = new E_Menu() { Id = 7, Padre = null, Nombre = "Gestión documental", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            menus.Add(m);

            m = new E_Menu() { Id = 8, Padre = null, Nombre = "Gestión administrativa", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            menus.Add(m);

            m = new E_Menu() { Id = 9, Padre = null, Nombre = "Gestión jurídica", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            menus.Add(m);

            m = new E_Menu() { Id = 10, Padre = null, Nombre = "Gestión logística", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            menus.Add(m);

            m = new E_Menu() { Id = 11, Padre = null, Nombre = "Gestión técnica", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            menus.Add(m);

            m = new E_Menu() { Id = 12, Padre = null, Nombre = "Gestión financiera", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            menus.Add(m);

            return menus;

        }

        public void InicializarMenu()
        {
            var m = new E_Menu() { Id = 1, Padre = null, Nombre = "Configuración", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            InsertarElemento(m);

            CrearMenuDeConfiguracion(m);

            m = new E_Menu() { Id = 6, Padre = null, Nombre = "Maestros", Descripcion = "", Icono = "home-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            InsertarElemento(m);

            m = new E_Menu() { Id = 7, Padre = null, Nombre = "Gestión documental", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            InsertarElemento(m);

            m = new E_Menu() { Id = 8, Padre = null, Nombre = "Gestión administrativa", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            InsertarElemento(m);

            m = new E_Menu() { Id = 9, Padre = null, Nombre = "Gestión jurídica", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            InsertarElemento(m);

            m = new E_Menu() { Id = 10, Padre = null, Nombre = "Gestión logística", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            InsertarElemento(m);

            m = new E_Menu() { Id = 11, Padre = null, Nombre = "Gestión técnica", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            InsertarElemento(m);

            m = new E_Menu() { Id = 12, Padre = null, Nombre = "Gestión financiera", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            InsertarElemento(m);
        }

        private void CrearMenuDeConfiguracion(E_Menu padre)
        {
            E_Menu m = new E_Menu() { Id = 2, Padre = padre, Nombre = "Funcionalidad", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            InsertarElemento(m);

            m = new E_Menu() { Id = 3, Padre = padre, Nombre = "Accesos", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<E_Menu>(), Activo = true };
            InsertarElemento(m);
            MenuDeAccesos(m);
        }

        private void MenuDeAccesos(E_Menu padre)
        {
            E_Menu m = new E_Menu() { Id = 4, Padre = padre, Nombre = "Usuarios", Descripcion = "", Icono = "cog-solid.svg", VistaMvc = null, Activo = true };
            //var gestorDeVistasMvc = new GestorDeVistasMvc(Contexto, Mapeador);
            //gestorDeVistasMvc.Leer(0,1,)

            InsertarElemento(m);

            m = new E_Menu() { Id = 5, Padre = padre, Nombre = "Permisos", Descripcion = "", Icono = "cog-solid.svg", VistaMvc = null, Activo = true };
            InsertarElemento(m);
        }


    }

}
