using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Gestor.Elementos;
using Gestor.Elementos.Seguridad;

namespace ServicioDeDatos.Seguridad
{

    public class GestorDePuestosDeTrabajo : GestorDeElementos<ContextoSe, PuestoDtm, PuestoDto>
    {
        public class MapearPuestoDeTrabajo : Profile
        {
            public MapearPuestoDeTrabajo()
            {
                CreateMap<PuestoDtm, PuestoDto>();
                CreateMap<PuestoDto, PuestoDtm>();
            }
        }

        public GestorDePuestosDeTrabajo(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
        }


        internal static GestorDePuestosDeTrabajo Gestor(IMapper mapeador)
        {
            var contexto = ContextoSe.ObtenerContexto();
            return (GestorDePuestosDeTrabajo)CrearGestor<GestorDePuestosDeTrabajo>(() => new GestorDePuestosDeTrabajo(contexto, mapeador));
        }

    }
}
