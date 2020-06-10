using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Gestor.Elementos;
using Gestor.Elementos.Seguridad;

namespace ServicioDeDatos.Seguridad
{
    class GestorDeTipoPermiso : GestorDeElementos<ContextoSe, TipoPermisoDtm, TipoPermisoDto>
    {
        public class MapearTipoPermiso : Profile
        {
            public MapearTipoPermiso()
            {
                CreateMap<TipoPermisoDtm, TipoPermisoDto>();
            }
        }

        public GestorDeTipoPermiso(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
        }

        public GestorDeTipoPermiso(Func<ContextoSe> generadorDeContexto, IMapper mapeador) 
        : base(generadorDeContexto, mapeador)
        {
        }

        internal static GestorDeTipoPermiso Gestor(IMapper mapeador)
        {
            return (GestorDeTipoPermiso)CrearGestor<GestorDeTipoPermiso>(() => new GestorDeTipoPermiso(() => ContextoSe.ObtenerContexto(), mapeador));
        }
    }
}
