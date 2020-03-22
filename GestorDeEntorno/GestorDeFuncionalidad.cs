using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Entorno
{

    public class GestorDeFuncionalidad : GestorDeElementos<CtoEntorno, rMenu, Funcion>
    {

        public class MapearFuncionalidad : Profile
        {
            public MapearFuncionalidad()
            {
                CreateMap<rMenu, Funcion>();
                CreateMap<Funcion, rMenu>();
            }
        }

        public GestorDeFuncionalidad(CtoEntorno contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override rMenu LeerConDetalle(int Id)
        {
            throw new System.NotImplementedException();
        }


        public static List<Funcion> MenuPrincipal()
        {
            var menu = new List<Funcion>();

            var f = new Funcion()  { Id = 1, Nombre = "Configuración", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<Funcion>(), Activo = true };
            menu.Add(f);

                 f = new Funcion() { Id = 2, Nombre = "Funcionalidad", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<Funcion>(), Activo = true};
                 menu[0].Opciones.Add(f);
                 f = new Funcion() { Id = 3, Nombre = "Accesos", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<Funcion>(), Activo = true };
                 menu[0].Opciones.Add(f);

                     f = new Funcion() { Id = 4, Nombre = "Usuarios", Descripcion = "", Icono = "cog-solid.svg", Accion = new EleAccion {Id=1, Nombre = "Usuarios", Controlador="Usuarios", Accion="Index", Parametros = "" }, Activo = true };
                     menu[0].Opciones[1].Opciones.Add(f);
                     f = new Funcion() { Id = 5, Nombre = "Permisos", Descripcion = "", Icono = "cog-solid.svg", Accion = new EleAccion {Id=2, Nombre = "Permisos", Controlador = "Permisos", Accion = "Index", Parametros = "" }, Activo = true };
                     menu[0].Opciones[1].Opciones.Add(f);
           
            f = new Funcion() { Id = 6, Nombre = "Maestros", Descripcion = "", Icono = "home-solid.svg", Opciones = new List<Funcion>(), Activo = true };
            menu.Add(f);

            f = new Funcion() {Id = 7, Nombre = "Gestión documental", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<Funcion>(), Activo = true };
            menu.Add(f);

            f = new Funcion() { Id = 8, Nombre = "Gestión administrativa", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<Funcion>() , Activo = true};
            menu.Add(f);

            f = new Funcion() {Id = 9, Nombre = "Gestión jurídica", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<Funcion>(), Activo = true };
            menu.Add(f);

            f = new Funcion() {Id = 10, Nombre = "Gestión logística", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<Funcion>() , Activo = true};
            menu.Add(f); 

            f = new Funcion() {Id = 11, Nombre = "Gestión técnica", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<Funcion>(), Activo = true };
            menu.Add(f);

            f = new Funcion() {Id = 12, Nombre = "Gestión financiera", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<Funcion>() , Activo = true};
            menu.Add(f);

            return menu;
        }


    }


    }
