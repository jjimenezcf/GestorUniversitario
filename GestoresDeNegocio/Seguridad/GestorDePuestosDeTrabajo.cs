using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GestorDeElementos;
using ModeloDeDto.Seguridad;
using Utilidades;

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


        internal static GestorDePuestosDeTrabajo Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePuestosDeTrabajo(contexto, mapeador);
        }

        internal static List<PuestoDto> Leer(GestorDePuestosDeTrabajo gestor, int posicion, int cantidad, string filtro)
        {
            var filtros = new List<ClausulaDeFiltrado>();
            if (!filtro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(PuestoDto.Nombre), Valor = filtro });

            var puestosDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(puestosDtm).ToList();
        }

    }
}
