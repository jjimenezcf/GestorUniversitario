using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GestorDeElementos;
using ModeloDeDto.Seguridad;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace GestoresDeNegocio.Seguridad
{

    public class GestorDeRoles : GestorDeElementos<ContextoSe, RolDtm, RolDto>
    {
        public class MapearPuestoDeTrabajo : Profile
        {
            public MapearPuestoDeTrabajo()
            {
                CreateMap<RolDtm, RolDto>();
                CreateMap<RolDto, RolDtm>();
            }
        }

        public GestorDeRoles(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
        }


        internal static GestorDeRoles Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeRoles(contexto, mapeador);
        }

        internal static List<RolDto> Leer(GestorDeRoles gestor, int posicion, int cantidad, string filtro)
        {
            var filtros = new List<ClausulaDeFiltrado>();
            if (!filtro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(RolDto.Nombre), Valor = filtro });

            var rolesDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(rolesDtm).ToList();
        }

    }
}
