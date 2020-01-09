using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario.ModeloIu;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using AutoMapper;
using System.Linq.Expressions;
using System;
using Extensiones.String;

namespace Gestor.Elementos.Universitario
{
    static class FiltroEstudiante
    {
        public const string FiltroPorApellido= "PorApellido";

        public static IQueryable<RegistroDeEstudiante> Filtro(this IQueryable<RegistroDeEstudiante> set, Dictionary<string, string> filtros)
        {
            if (filtros.ContainsKey(FiltroPorApellido))
                return set.Where(x => x.Apellido == filtros[FiltroPorApellido]);

            return set;
        }
    }


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

        protected override IQueryable<RegistroDeEstudiante> IncluirFiltros(IQueryable<RegistroDeEstudiante> registros, Dictionary<string, string> filtros)
        {
            registros = base.IncluirFiltros(registros, filtros);
            return registros.Filtro(filtros);
        }

        protected override RegistroDeEstudiante LeerConDetalle(int Id)
        {
            return Contexto.Set<RegistroDeEstudiante>()
                            .Include(i => i.Inscripciones)
                            .ThenInclude(e => e.Curso)
                            .AsNoTracking()
                            .FirstOrDefault(m => m.Id == Id);
        }

        protected override Expression<Func<RegistroDeEstudiante, object>> EstablecerOrden(string orden)
        {
            switch (orden)
            {
                case nameof(RegistroDeEstudiante.Apellido):
                    return x => x.Apellido;

                case nameof(RegistroDeEstudiante.InscritoEl):
                    return x => x.InscritoEl;
            }

            return base.EstablecerOrden(orden);
        }

    }


}
