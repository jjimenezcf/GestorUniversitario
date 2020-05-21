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
    public class GestorDeClaseDePermisos : GestorDeElementos<ContextoDeElementos, ClasePermisoDtm, ClasePermisoDto>
    {
        public class MapearClasePermiso : Profile
        {
            public MapearClasePermiso()
            {
                CreateMap<ContextoDeElementos, ClasePermisoDto>();
            }
        }

        public GestorDeClaseDePermisos(ContextoDeElementos contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {


        }
    }
}
