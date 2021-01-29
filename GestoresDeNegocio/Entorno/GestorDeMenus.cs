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
                ;

                CreateMap<MenuDto, MenuDtm>()
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

        protected override IQueryable<MenuDtm> AplicarOrden(IQueryable<MenuDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            if (ordenacion.Count == 0)
            {
                registros = registros.OrderBy(x => x.IdPadre).ThenBy(x => x.Orden).ThenBy(x => x.Nombre);
                return registros;
            }

            foreach (ClausulaDeOrdenacion orden in ordenacion)
            {
                if (orden.OrdenarPor.ToLower() == nameof(MenuDtm.Id).ToLower())
                    return registros = OrdenPorId(registros, orden);

                if (orden.OrdenarPor == nameof(MenuDtm.IdPadre))
                {
                    registros = registros.OrderBy(x => x.IdPadre).ThenBy(x => x.Orden).ThenBy(x => x.Nombre);
                    break;
                }

                if (orden.OrdenarPor.ToLower() == nameof(MenuDtm.Padre).ToLower())
                {
                    registros = orden.Modo == ModoDeOrdenancion.ascendente
                    ? registros.OrderBy(x => x.Padre.Orden)
                    : registros.OrderByDescending(x => x.Padre.Orden);

                    break;
                }

                if (orden.OrdenarPor.ToLower() == nameof(MenuDtm.Nombre).ToLower())
                {
                    registros = orden.Modo == ModoDeOrdenancion.ascendente
                    ? registros.OrderBy(x => x.Padre).ThenBy(x => x.Nombre)
                    : registros.OrderBy(x => x.Padre).ThenByDescending(x => x.Nombre);
                    break;
                }

                if (orden.OrdenarPor.ToLower() == nameof(MenuDtm.Orden).ToLower())
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
            List<ClausulaDeOrdenacion> orden = new List<ClausulaDeOrdenacion>();
            orden.Add(new ClausulaDeOrdenacion() {OrdenarPor = nameof(MenuDto.idPadre), Modo = ModoDeOrdenancion.ascendente });

            var registros = LeerRegistros(posicion, cantidad, filtros, orden);
            return MapearElementos(registros).ToList();
        }


        protected override void DespuesDePersistir(MenuDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            ServicioDeCaches.EliminarCache(nameof(GestorDeArbolDeMenu.LeerArbolDeMenu));
        }

    }

}
