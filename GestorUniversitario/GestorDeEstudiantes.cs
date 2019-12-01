﻿using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario.ModeloIu;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Gestor.Elementos.Universitario
{
    public class GestorDeEstudiantes : GestorDeElementos<ContextoUniversitario, RegistroDeEstudiante, ElementoEstudiante>
    {
        
        public class MapeoRegistroEstudiante : Profile
        {
            public MapeoRegistroEstudiante()
            {
                CreateMap<RegistroDeEstudiante, ElementoEstudiante>();
                CreateMap<ElementoEstudiante, RegistroDeEstudiante>();
            }
        }
        
        public GestorDeEstudiantes(ContextoUniversitario contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override RegistroDeEstudiante LeerConDetalle(int Id)
        {
            return Contexto.Set<RegistroDeEstudiante>()
                            .Include(i => i.Inscripciones)
                            .ThenInclude(e => e.Curso)
                            .AsNoTracking()
                            .FirstOrDefault(m => m.Id == Id);
        }

    }


}
