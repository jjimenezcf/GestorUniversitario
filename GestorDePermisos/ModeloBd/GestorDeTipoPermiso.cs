using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Gestor.Elementos;
using Gestor.Elementos.Seguridad;

namespace Gestor.Elementos.Seguridad
{
    class GestorDeTipoPermiso : GestorDeElementos<CtoSeguridad, TipoPermisoDtm, TipoPermisoDto>
    {
        public class MapearTipoPermiso : Profile
        {
            public MapearTipoPermiso()
            {
                CreateMap<TipoPermisoDtm, TipoPermisoDto>();
            }
        }

        public GestorDeTipoPermiso(CtoSeguridad contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {


        }
    }
}
