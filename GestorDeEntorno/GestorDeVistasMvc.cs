using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Entorno
{

    public class GestorDeVistasMvc : GestorDeElementos<CtoEntorno, R_VistaMvc, E_VistaMvc>
    {

        public class MapearVistasMvc : Profile
        {
            public MapearVistasMvc()
            {
                CreateMap<R_VistaMvc, E_VistaMvc>();
                CreateMap<E_VistaMvc, R_VistaMvc>();
            }
        }

        public GestorDeVistasMvc(CtoEntorno contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override R_VistaMvc LeerConDetalle(int Id)
        {
            throw new System.NotImplementedException();
        }


        public static List<E_VistaMvc> VistasMvc()
        {
            var vistasMvc = new List<E_VistaMvc>();

            vistasMvc.Add(new E_VistaMvc { Id = 0, Nombre = "Usuarios", Controlador = "Usuarios", Accion = "Index", Parametros = "" });
            vistasMvc.Add(new E_VistaMvc { Id = 0, Nombre = "Menus", Controlador = "Menus", Accion = "Index", Parametros = "" });

            return vistasMvc;
        }

        public void InicializarVistasMvc()
        {
            var e_vistasMvc = VistasMvc();
            var r_vistasMvc = MapearRegistros(e_vistasMvc, TipoOperacion.Insertar);
            InsertarRegistros(r_vistasMvc);
        }


    }

}

