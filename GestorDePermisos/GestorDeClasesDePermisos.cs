using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Gestor.Elementos;
using Gestor.Elementos.Seguridad;

namespace Gestor.Elementos.Seguridad
{
    public class GestorDeClaseDePermisos : GestorDeElementos<CtoSeguridad, ClasePermisoDtm, ClasePermisoDto>
    {
        public class MapearPermiso : Profile
        {
            public MapearPermiso()
            {
                CreateMap<ClasePermisoDtm, ClasePermisoDto>();
            }
        }

        public GestorDeClaseDePermisos(CtoSeguridad contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {


        }
    }
}
