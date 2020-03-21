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

            var f = new FuncionalidadDto()  { Nombre = "Configuración",  Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

                 f = new FuncionalidadDto() { Nombre = "Funcionalidad", Opciones = new List<FuncionalidadDto>()};
                 menu[0].Opciones.Add(f);
                 f = new FuncionalidadDto() { Nombre = "Accesos", Opciones = new List<FuncionalidadDto>() };
                 menu[0].Opciones.Add(f);

                     f = new FuncionalidadDto() { Nombre = "Usuarios", Accion = new AccionDto {Nombre = "Usuarios", Controlador="Usuarios", Accion="Index", Parametros = "" } };
                     menu[0].Opciones[1].Opciones.Add(f);
                     f = new FuncionalidadDto() { Nombre = "Permisos", Accion = new AccionDto {Nombre = "Permisos", Controlador = "Permisos", Accion = "Index", Parametros = "" } };
                     menu[0].Opciones[1].Opciones.Add(f);
           
            f = new FuncionalidadDto() { Nombre = "Maestros", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

            f = new FuncionalidadDto() { Nombre = "Gestión documental", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

            f = new FuncionalidadDto() { Nombre = "Gestión administrativa", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

            f = new FuncionalidadDto() { Nombre = "Gestión jurídica", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

            f = new FuncionalidadDto() { Nombre = "Gestión logística", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f); 

            f = new FuncionalidadDto() { Nombre = "Gestión técnica", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

            f = new FuncionalidadDto() { Nombre = "Gestión financiera", Opciones = new List<FuncionalidadDto>() };
            menu.Add(f);

            return menu;
        }


    }


    }
