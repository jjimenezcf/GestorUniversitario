using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using Gestor.Elementos.ModeloIu;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using System;

namespace Gestor.Elementos.Entorno
{

    public static partial class Filtros
    {
        public static IQueryable<T> FiltraPorControlador<T>(this IQueryable<T> registros , List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros) where T : VistaMvcDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == nameof(VistaMvcDtm.Controlador).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        registros = registros.Where(x => x.Controlador == filtro.Valor);
                }

            return registros;
        }
        public static IQueryable<T> FiltraPorAccion<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros) where T : VistaMvcDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == nameof(VistaMvcDtm.Accion).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        registros = registros.Where(x => x.Accion == filtro.Valor);
                }

            return registros;
        }
    }


        public class GestorDeVistasMvc : GestorDeElementos<ContextoSe, VistaMvcDtm, VistaMvcDto>
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

        public GestorDeVistasMvc(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }
        
        public GestorDeVistasMvc(Func<ContextoSe> generadorDeContexto, IMapper mapeador)
        : base(generadorDeContexto, mapeador)
        {

        }

        internal static GestorDeVistasMvc Gestor(IMapper mapeador)
        {
            return (GestorDeVistasMvc) CrearGestor<GestorDeVistasMvc>(() => new GestorDeVistasMvc(() => ContextoSe.ObtenerContexto(), mapeador));
        }

        protected override IQueryable<VistaMvcDtm> AplicarFiltros(IQueryable<VistaMvcDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId(registros))
                return registros;

            return registros.FiltraPorControlador(filtros,parametros).FiltraPorAccion(filtros, parametros);
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

