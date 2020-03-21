using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Entorno
{

    public class GestorDeFuncionalidad : GestorDeElementos<CtoEntorno, Fun_Elemento, FuncionalidadDto>
    {

        public class MapearFuncionalidad : Profile
        {
            public MapearFuncionalidad()
            {
                CreateMap<Fun_Elemento, FuncionalidadDto>();
                CreateMap<FuncionalidadDto, Fun_Elemento>();
            }
        }

        public GestorDeFuncionalidad(CtoEntorno contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override Fun_Elemento LeerConDetalle(int Id)
        {
            throw new System.NotImplementedException();
        }


        public static List<FuncionalidadDto> MenuPrincipal()
        {
            var menu = new List<FuncionalidadDto>();

            var f = new FuncionalidadDto()  { Id = 1, Nombre = "Configuración", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>(), Activo = true };
            menu.Add(f);

                 f = new FuncionalidadDto() { Id = 2, Nombre = "Funcionalidad", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>(), Activo = true};
                 menu[0].Opciones.Add(f);
                 f = new FuncionalidadDto() { Id = 3, Nombre = "Accesos", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>(), Activo = true };
                 menu[0].Opciones.Add(f);

                     f = new FuncionalidadDto() { Id = 4, Nombre = "Usuarios", Descripcion = "", Icono = "cog-solid.svg", Accion = new AccionDto {Id=1, Nombre = "Usuarios", Controlador="Usuarios", Accion="Index", Parametros = "" }, Activo = true };
                     menu[0].Opciones[1].Opciones.Add(f);
                     f = new FuncionalidadDto() { Id = 5, Nombre = "Permisos", Descripcion = "", Icono = "cog-solid.svg", Accion = new AccionDto {Id=2, Nombre = "Permisos", Controlador = "Permisos", Accion = "Index", Parametros = "" }, Activo = true };
                     menu[0].Opciones[1].Opciones.Add(f);
           
            f = new FuncionalidadDto() { Id = 6, Nombre = "Maestros", Descripcion = "", Icono = "home-solid.svg", Opciones = new List<FuncionalidadDto>(), Activo = true };
            menu.Add(f);

            f = new FuncionalidadDto() {Id = 7, Nombre = "Gestión documental", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>(), Activo = true };
            menu.Add(f);

            f = new FuncionalidadDto() { Id = 8, Nombre = "Gestión administrativa", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>() , Activo = true};
            menu.Add(f);

            f = new FuncionalidadDto() {Id = 9, Nombre = "Gestión jurídica", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>(), Activo = true };
            menu.Add(f);

            f = new FuncionalidadDto() {Id = 10, Nombre = "Gestión logística", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>() , Activo = true};
            menu.Add(f); 

            f = new FuncionalidadDto() {Id = 11, Nombre = "Gestión técnica", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>(), Activo = true };
            menu.Add(f);

            f = new FuncionalidadDto() {Id = 12, Nombre = "Gestión financiera", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>() , Activo = true};
            menu.Add(f);

            return menu;
        }


    }


    }
