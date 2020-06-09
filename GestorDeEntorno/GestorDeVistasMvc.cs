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


        public class GestorDeVistaMvc : GestorDeElementos<ContextoSe, VistaMvcDtm, VistaMvcDto>
    {

        public class MapearVistaMvc : Profile
        {
            public MapearVistaMvc()
            {
                CreateMap<VistaMvcDtm, VistaMvcDto>()
                .ForMember("Menus", x => x.MapFrom(x => x.Menus));

                CreateMap<VistaMvcDto, VistaMvcDtm>();
            }
        }

        public GestorDeVistaMvc(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }
        
        public GestorDeVistaMvc(Func<ContextoSe> generadorDeContexto, IMapper mapeador)
        : base(generadorDeContexto, mapeador)
        {

        }

        internal static GestorDeVistaMvc Gestor(IMapper mapeador)
        {
            return (GestorDeVistaMvc) CrearGestor<GestorDeVistaMvc>(() => new GestorDeVistaMvc(() => ContextoSe.ObtenerContexto(), mapeador));
        }

        protected override IQueryable<VistaMvcDtm> AplicarFiltros(IQueryable<VistaMvcDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId(registros))
                return registros;

            return registros.FiltraPorControlador(filtros,parametros).FiltraPorAccion(filtros, parametros);
        }


        public static List<VistaMvcDto> VistaMvc()
        {
            var vistaMvc = new List<VistaMvcDto>();

            vistaMvc.Add(new VistaMvcDto { Id = 0, Nombre = "Usuarios", Controlador = "Usuarios", Accion = "Index", Parametros = "" });
            vistaMvc.Add(new VistaMvcDto { Id = 0, Nombre = "Menus", Controlador = "Menus", Accion = "Index", Parametros = "" });

            return vistaMvc;
        }

        public void InicializarVistaMvc()
        {
            var e_vistaMvc = VistaMvc();
            var parametros = new ParametrosDeNegocio(TipoOperacion.Insertar);
            var r_vistaMvc = MapearRegistros(e_vistaMvc, parametros);
            PersistirRegistros(r_vistaMvc, parametros);
        }

    }

}

