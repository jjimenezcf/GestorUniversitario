using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using AutoMapper;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto;
using ModeloDeDto.Entorno;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;

namespace GestoresDeNegocio.Entorno
{

    public class GestorDePermisosDeUnUsuario : GestorDeElementos<ContextoSe, PermisosDeUnUsuarioDtm, PermisosDeUnUsuarioDto>
    {

        public class MapearPermisosDeUnUsuario : Profile
        {
            public MapearPermisosDeUnUsuario()
            {
                CreateMap<PermisosDeUnUsuarioDtm, PermisosDeUnUsuarioDto>()
                    .ForMember(dto => dto.Usuario, dtm => dtm.MapFrom(dtm => dtm.Usuario.Nombre))
                    .ForMember(dto => dto.Permiso, dtm => dtm.MapFrom(dtm => dtm.Permiso.Nombre));

                CreateMap<PermisosDeUnUsuarioDto, PermisosDeUnUsuarioDtm>();
            }
        }

        public GestorDePermisosDeUnUsuario(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }


        internal static GestorDePermisosDeUnUsuario Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePermisosDeUnUsuario(contexto, mapeador);
        }

        protected override IQueryable<PermisosDeUnUsuarioDtm> AplicarJoins(IQueryable<PermisosDeUnUsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(rp => rp.Permiso);
            registros = registros.Include(rp => rp.Usuario);

            return registros;
        }

        protected override IQueryable<PermisosDeUnUsuarioDtm> AplicarFiltros(IQueryable<PermisosDeUnUsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnUsuarioDtm.Permiso).ToLower() && filtro.Criterio == CriteriosDeFiltrado.contiene)
                    registros = registros.Where(x => x.Permiso.Nombre.Contains(filtro.Valor));
           
            return registros;

        }
    }
}
