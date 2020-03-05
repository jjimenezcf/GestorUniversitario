using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario;
using Gestor.Elementos.Universitario.ModeloIu;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Universitario
{
    static class RegistroDeEstudianteFiltros
    {
        public static IQueryable<T> AplicarFiltroNombre<T>(this IQueryable<T> regristros, List<ClausulaDeFiltrado> filtros) where T : Usuario
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == UsuariosPor.NombreCompleto)
                    return regristros.Where(x => x.Apellido.Contains(filtro.Valor) || x.Nombre.Contains(filtro.Valor));

            return regristros;
        }

        public static IQueryable<T> AplicarFiltroCurso<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : Usuario
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == UsuariosPor.CursosInscrito)
                {
                    var listaIds = filtro.Valor.ListaEnteros(); 
                    foreach(int id in listaIds)
                    {
                        registros = registros.Where(x => x.Inscripciones.Any(i => i.CursoId == id));
                    }
                }

            return registros;

        }
    }



    static class OrdenacionEstudiante
    {
        public const string OrdenPorApellido = "PorApellido";

        public static IQueryable<Usuario> Orden(this IQueryable<Usuario> set, Dictionary<string, Ordenacion> orden)
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


    public class GestorDeEstudiantes : GestorDeElementos<ContextoUniversitario, Usuario, UsuarioDto>
    {

        public class MapeoRegistroEstudiante : Profile
        {
            public MapeoRegistroEstudiante()
            {
                CreateMap<Usuario, UsuarioDto>();
                CreateMap<UsuarioDto, Usuario>();
            }
        }

        public GestorDeEstudiantes(ContextoUniversitario contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override IQueryable<Usuario> AplicarOrden(IQueryable<Usuario> registros, Dictionary<string, Ordenacion> orden)
        {
            registros = base.AplicarOrden(registros, orden);
            return registros.Orden(orden);
        }

               

        protected override IQueryable<Usuario> AplicarFiltros(IQueryable<Usuario> registros, List<ClausulaDeFiltrado> filtros) 
        {
            foreach (var f in filtros)
                if (f.Propiedad == FiltroPor.Id)
                    return base.AplicarFiltros(registros, filtros);

            return registros
                   .AplicarFiltroNombre(filtros)
                   .AplicarFiltroCurso(filtros);
        }
                

        protected override Usuario LeerConDetalle(int Id)
        {
            return Contexto.Set<Usuario>()
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
