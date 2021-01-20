using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto.Seguridad;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace GestoresDeNegocio.Seguridad
{

    public class GestorDePermisosDeUnPuesto : GestorDeElementos<ContextoSe, PermisosDeUnPuestoDtm, PermisosDeUnPuestoDto>
    {

        public class MapearPermisosDeUnPuesto : Profile
        {
            public MapearPermisosDeUnPuesto()
            {
                CreateMap<PermisosDeUnPuestoDtm, PermisosDeUnPuestoDto>()
                    .ForMember(dto => dto.Puesto, dtm => dtm.MapFrom(dtm => dtm.Puesto.Nombre))
                    .ForMember(dto => dto.Permiso, dtm => dtm.MapFrom(dtm => dtm.Permiso.Nombre));

                CreateMap<PermisosDeUnPuestoDto, PermisosDeUnPuestoDtm>();
            }
        }

        public GestorDePermisosDeUnPuesto(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }


        internal static GestorDePermisosDeUnPuesto Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePermisosDeUnPuesto(contexto, mapeador);
        }

        protected override IQueryable<PermisosDeUnPuestoDtm> AplicarJoins(IQueryable<PermisosDeUnPuestoDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(rp => rp.Permiso);
            registros = registros.Include(rp => rp.Puesto);
            return registros;
        }

        protected override IQueryable<PermisosDeUnPuestoDtm> AplicarFiltros(IQueryable<PermisosDeUnPuestoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnPuestoDtm.IdPuesto).ToLower())
                    registros = registros.Where(x => x.IdPuesto == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnPuestoDtm.IdPermiso).ToLower())
                    registros = registros.Where(x => x.IdPermiso == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnPuestoDtm.Permiso).ToLower())
                    registros = registros.Where(x => x.Permiso.Nombre.Contains(filtro.Valor));
            }
            return registros;

        }

        protected override IQueryable<PermisosDeUnPuestoDtm> AplicarOrden(IQueryable<PermisosDeUnPuestoDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);

            if (ordenacion.Count == 0)
                return registros.OrderBy(x => x.Permiso.Nombre);

            return registros;
        }


    }
}
