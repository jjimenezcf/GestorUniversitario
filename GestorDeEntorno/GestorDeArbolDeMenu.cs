using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;

namespace Gestor.Elementos.Entorno
{
    public class GestorDeArbolDeMenu : GestorDeElementos<CtoEntorno, ArbolDeMenuDtm, ArbolDeMenuDto>
    {
        public class MapearMenus : Profile
        {
            public MapearMenus()
            {
                CreateMap<ArbolDeMenuDtm, ArbolDeMenuDto>();
            }
        }

        public GestorDeArbolDeMenu(CtoEntorno contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

    }
}
