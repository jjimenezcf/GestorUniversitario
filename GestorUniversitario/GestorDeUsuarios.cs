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
    static class UsuarioRegFlt
    {
        public static IQueryable<T> AplicarFiltroNombre<T>(this IQueryable<T> regristros, List<ClausulaDeFiltrado> filtros) where T : UsuarioReg
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == UsuariosPor.NombreCompleto)
                    return regristros.Where(x => x.Apellido.Contains(filtro.Valor) || x.Nombre.Contains(filtro.Valor));

            return regristros;
        }

        public static IQueryable<T> AplicarFiltroCurso<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : UsuarioReg
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



    static class UsuarioRegOrd
    {
        public const string OrdenPorApellido = "PorApellido";

        public static IQueryable<UsuarioReg> Orden(this IQueryable<UsuarioReg> set, Dictionary<string, Ordenacion> orden)
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


    public class GestorDeUsuarios : GestorDeElementos<ContextoUniversitario, UsuarioReg, UsuarioDto>
    {

        public class MapeoRegistroUsuario : Profile
        {
            public MapeoRegistroUsuario()
            {
                CreateMap<UsuarioReg, UsuarioDto>();
                CreateMap<UsuarioDto, UsuarioReg>();
            }
        }

        public GestorDeUsuarios(ContextoUniversitario contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override IQueryable<UsuarioReg> AplicarOrden(IQueryable<UsuarioReg> registros, Dictionary<string, Ordenacion> orden)
        {
            registros = base.AplicarOrden(registros, orden);
            return registros.Orden(orden);
        }

               

        protected override IQueryable<UsuarioReg> AplicarFiltros(IQueryable<UsuarioReg> registros, List<ClausulaDeFiltrado> filtros) 
        {
            foreach (var f in filtros)
                if (f.Propiedad == FiltroPor.Id)
                    return base.AplicarFiltros(registros, filtros);

            return registros
                   .AplicarFiltroNombre(filtros)
                   .AplicarFiltroCurso(filtros);
        }
                

        protected override UsuarioReg LeerConDetalle(int Id)
        {
            return Contexto.Set<UsuarioReg>()
                            .Include(i => i.Inscripciones)
                            .ThenInclude(e => e.Curso)
                            .AsNoTracking()
                            .FirstOrDefault(m => m.Id == Id);
        }

    }


}
