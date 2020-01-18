﻿using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario.ModeloIu;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

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

    static class OrdenacionEstudiante
    {
        public const string OrdenPorApellido = "PorApellido";

        public static IQueryable<RegistroDeEstudiante> Orden(this IQueryable<RegistroDeEstudiante> set, Dictionary<string, Ordenacion> orden)
        {
            if (orden.Count == 0)
                return set.OrderBy(x => x.Apellido);

            if (orden.ContainsKey(OrdenPorApellido))
            {
                if (orden[OrdenPorApellido] == Ordenacion.Ascendente)
                    return set.OrderBy(x => x.Apellido);

                if (orden[OrdenPorApellido] == Ordenacion.Descendente)
                    return set.OrderByDescending(x => x.Apellido);
            }

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

        protected override IQueryable<RegistroDeEstudiante> AplicarOrden(IQueryable<RegistroDeEstudiante> registros, Dictionary<string, Ordenacion> orden)
        {
            registros = base.AplicarOrden(registros, orden);
            return registros.Orden(orden);
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

        //protected override Expression<Func<RegistroDeEstudiante, object>> EstablecerOrden(string orden)
        //{
        //    switch (orden)
        //    {
        //        case nameof(RegistroDeEstudiante.Apellido):
        //            return x => x.Apellido;

        //        case nameof(RegistroDeEstudiante.InscritoEl):
        //            return x => x.InscritoEl;
        //    }

        //    return base.EstablecerOrden(orden);
        //}

    }


}
