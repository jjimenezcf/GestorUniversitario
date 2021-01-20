using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using ModeloDeDto.Entorno;
using GestorDeElementos;
using ServicioDeDatos.Seguridad;
using ModeloDeDto;

namespace GestoresDeNegocio.Entorno
{
    public class GestorDeMenus : GestorDeElementos<ContextoSe, MenuDtm, MenuDto>
    {
        public class MapearMenus : Profile
        {
            public MapearMenus()
            {
                CreateMap<MenuDtm, MenuDto>()
                .ForMember(dto => dto.Padre, dtm => dtm.MapFrom(dtm => dtm.Padre.Nombre))
                .ForMember(dto => dto.VistaMvc, dtm => dtm.MapFrom(dtm => dtm.VistaMvc.Nombre))
                //.ForMember(dto => dto.Permiso, dtm => dtm.MapFrom(x => x.Permiso.Nombre))
                ;

                CreateMap<MenuDto, MenuDtm>()
                //.ForMember(dtm => dtm.Permiso, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdVistaMvc, dto => dto.MapFrom(dto => dto.idVistaMvc == 0 ? null : dto.idVistaMvc))
                .ForMember(dtm => dtm.IdPadre, dto => dto.MapFrom(dto => dto.idPadre == 0 ? null : dto.idPadre));
            }
        }

        public GestorDeMenus(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        public static GestorDeMenus Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeMenus(contexto, mapeador);
        }

        protected override IQueryable<MenuDtm> AplicarFiltros(IQueryable<MenuDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(MenuDto.Padre).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.esNulo)
                        registros = registros.Where(x => x.IdPadre == null);

                    if (filtro.Criterio == CriteriosDeFiltrado.noEsNulo)
                        registros = registros.Where(x => x.IdPadre != null);

                    if (filtro.Criterio == CriteriosDeFiltrado.igual && filtro.Valor.Entero() > 0)
                        registros = registros.Where(x => x.IdPadre == filtro.Valor.Entero());
                }

                if (filtro.Clausula.ToLower() == nameof(MenuDtm.Activo).ToLower())
                {
                    registros = registros.Where(x => x.Activo == bool.Parse(filtro.Valor));
                }

                if (filtro.Clausula.ToLower() == CamposDeFiltrado.PorDefecto)
                {
                    registros = registros.Where(x => x.Nombre.Contains(filtro.Valor));
                }
            }

            return registros;
        }

        protected override IQueryable<MenuDtm> AplicarOrden(IQueryable<MenuDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);

            foreach (ClausulaDeOrdenacion orden in ordenacion)
            {
                if (orden.Criterio == ClausulaDeOrdenacion.PorDefecto)
                {
                    registros = registros.OrderBy(x => x.IdPadre).ThenBy(x => x.Orden).ThenBy(x => x.Nombre);
                    break;
                }

                if (orden.Criterio.ToLower() == nameof(MenuDtm.Padre).ToLower())
                {
                    registros = orden.Modo == ModoDeOrdenancion.ascendente
                    ? registros.OrderBy(x => x.Padre.Orden)
                    : registros.OrderByDescending(x => x.Padre.Orden);

                    break;
                }

                if (orden.Criterio.ToLower() == nameof(MenuDtm.Nombre).ToLower())
                {
                    registros = orden.Modo == ModoDeOrdenancion.ascendente
                    ? registros.OrderBy(x => x.Padre).ThenBy(x => x.Nombre)
                    : registros.OrderBy(x => x.Padre).ThenByDescending(x => x.Nombre);
                    break;
                }

                if (orden.Criterio.ToLower() == nameof(MenuDtm.Orden).ToLower())
                {
                    registros = orden.Modo == ModoDeOrdenancion.ascendente
                    ? registros.OrderBy(x => x.Padre).ThenBy(x => x.Orden).ThenBy(x => x.Nombre)
                    : registros.OrderBy(x => x.Padre).ThenByDescending(x => x.Orden).ThenBy(x => x.Nombre);
                    break;
                }
            }

            return registros;
        }

        protected override IQueryable<MenuDtm> AplicarJoins(IQueryable<MenuDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);

            foreach (var filtro in filtros)
                if (filtro.Clausula == nameof(MenuDtm.IdPadre) && filtro.Criterio == CriteriosDeFiltrado.esNulo)
                    return registros;

            registros = registros.Include(p => p.Padre);
            registros = registros.Include(p => p.VistaMvc);

            return registros;
        }

        public List<MenuDto> LeerPadres()
        {
            var registros = Contexto
                            .Menus
                            .FromSqlInterpolated($@"select 
                                                      t1.ID
                                                    , case
                                                         WHEN t2.Nombre is null THEN t1.nombre
                                                         ELSE t2.nombre+'.'+t1.nombre
                                                      END as NOMBRE
                                                    , t1.DESCRIPCION
                                                    , t1.icono
                                                    , t1.ACTIVO
                                                    , t1.IDPADRE
                                                    , t1.IDVISTA_MVC
                                                    , T1.ORDEN
                                                    --, T1.IDPERMISO
                                                    from entorno.MENU_SE t1
                                                    left join entorno.menu t2 on t2.id = t1.IDPADRE
                                                    where vista is null
                                                    order by t1.IDPADRE, T1.ORDEN, T1.NOMBRE")
                            .ToList();

            var elementos = MapearElementos(registros).ToList();
            return elementos;
        }


        public List<MenuDto> LeerMenus(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            var registros = LeerRegistros(posicion, cantidad, filtros);
            return MapearElementos(registros).ToList();
        }


        protected override void DespuesDePersistir(MenuDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            ServicioDeCaches.EliminarCache(nameof(GestorDeArbolDeMenu.LeerArbolDeMenu));
        }

    }

}
