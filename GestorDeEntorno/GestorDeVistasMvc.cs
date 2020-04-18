using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Entorno
{

    public class GestorDeVistasMvc : GestorDeElementos<CtoEntorno, VistaMvcDtm, VistaMvcDto>
    {

        public class MapearVistasMvc : Profile
        {
            public MapearVistasMvc()
            {
                CreateMap<VistaMvcDtm, VistaMvcDto>()
                .ForMember("Menus", x => x.MapFrom(x => x.Menus));

                CreateMap<VistaMvcDto, VistaMvcDtm>();
            }
        }

        public GestorDeVistasMvc(CtoEntorno contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override VistaMvcDtm LeerConDetalle(int Id)
        {
            throw new System.NotImplementedException();
        }


        public static List<VistaMvcDto> VistasMvc()
        {
            var vistasMvc = new List<VistaMvcDto>();

            vistasMvc.Add(new VistaMvcDto { Id = 0, Nombre = "Usuarios", Controlador = "Usuarios", Accion = "Index", Parametros = "" });
            vistasMvc.Add(new VistaMvcDto { Id = 0, Nombre = "Menus", Controlador = "Menus", Accion = "Index", Parametros = "" });

            return vistasMvc;
        }

        public void InicializarVistasMvc()
        {
            var e_vistasMvc = VistasMvc();
            var parametros = new ParametrosDeNegocio(TipoOperacion.Insertar);
            var r_vistasMvc = MapearRegistros(e_vistasMvc, parametros);
            PersistirRegistros(r_vistasMvc, parametros);
        }


    }

}

