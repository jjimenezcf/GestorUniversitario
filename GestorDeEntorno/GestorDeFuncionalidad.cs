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

        /*
        Maestros  
        Gestión documental  
        Gestión administrativa  
        Gestión jurídica  
        Gestión logística  
        Gestión técnica  
        Gestión financiera 
        */

        public static List<FuncionalidadDto> MenuPrincipal()
        {
            var menu = new List<FuncionalidadDto>();

            var f = new FuncionalidadDto()  { Nombre = "Configuración", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

                 f = new FuncionalidadDto() { Nombre = "Funcionalidad", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>()};
                 menu[0].Opciones.Add(f);
                 f = new FuncionalidadDto() { Nombre = "Accesos", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>() };
                 menu[0].Opciones.Add(f);

                     f = new FuncionalidadDto() { Nombre = "Usuarios", Descripcion = "", Icono = "cog-solid.svg", Accion = new AccionDto {Nombre = "Usuarios", Controlador="Usuarios", Accion="Index", Parametros = "" } };
                     menu[0].Opciones[1].Opciones.Add(f);
                     f = new FuncionalidadDto() { Nombre = "Permisos", Descripcion = "", Icono = "cog-solid.svg", Accion = new AccionDto {Nombre = "Permisos", Controlador = "Permisos", Accion = "Index", Parametros = "" } };
                     menu[0].Opciones[1].Opciones.Add(f);
           
            f = new FuncionalidadDto() { Nombre = "Maestros", Descripcion = "", Icono = "home-solid.svg", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

            f = new FuncionalidadDto() { Nombre = "Gestión documental", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

            f = new FuncionalidadDto() { Nombre = "Gestión administrativa", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

            f = new FuncionalidadDto() { Nombre = "Gestión jurídica", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

            f = new FuncionalidadDto() { Nombre = "Gestión logística", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f); 

            f = new FuncionalidadDto() { Nombre = "Gestión técnica", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

            f = new FuncionalidadDto() { Nombre = "Gestión financiera", Descripcion = "", Icono = "cog-solid.svg", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

            return menu;
        }


    }


    }
