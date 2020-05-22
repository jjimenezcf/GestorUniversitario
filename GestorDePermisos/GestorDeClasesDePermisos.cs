using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Gestor.Elementos;
using Gestor.Elementos.Seguridad;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;

namespace Gestor.Elementos.Seguridad
{
    public class GestorDeClaseDePermisos : GestorDeElementos<ContextoSe, ClasePermisoDtm, ClasePermisoDto>
    {
        public class MapearClasePermiso : Profile
        {
            public MapearClasePermiso()
            {
                CreateMap<ContextoSe, ClasePermisoDto>();
            }
        }

        public GestorDeClaseDePermisos(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {


        }
    }
}
