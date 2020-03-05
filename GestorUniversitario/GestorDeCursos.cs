using Gestor.Elementos.Usuario.ModeloBd;
using Gestor.Elementos.Usuario;
using Gestor.Elementos.Usuario.ModeloIu;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Usuario
{

    static class RegistroDeCursosFiltros
    {
        public static IQueryable<T> AplicarFiltroNombre<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : RegistroDeCurso
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == CursoPor.Nombre)
                    return registros.Where(x => x.Titulo.Contains(filtro.Valor));

            return registros;
        }

        public static IQueryable<T> AplicarFiltroUsuarios<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : RegistroDeCurso
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == CursoPor.EstudianteInscrito)
                {
                    var listaIds = filtro.Valor.ListaEnteros();
                    foreach (int id in listaIds)
                    {
                        registros = registros.Where(x => x.Inscripciones.Any(i => i.EstudianteId == id));
                    }
                }

            return registros;

        }
    }


    public class GestorDeCursos : GestorDeElementos<ContextoUniversitario, RegistroDeCurso, ElementoCurso>
    {
        public class MapeoRegistroCurso : Profile
        {
            public MapeoRegistroCurso()
            {
                CreateMap<RegistroDeCurso, ElementoCurso>();
                CreateMap<ElementoCurso,RegistroDeCurso>();
            }
        }

        public GestorDeCursos(ContextoUniversitario contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
            
        }
               
        protected override RegistroDeCurso LeerConDetalle(int Id)
        {
            return null;
        }

        protected override IQueryable<RegistroDeCurso> AplicarFiltros(IQueryable<RegistroDeCurso> registros, List<ClausulaDeFiltrado> filtros)
        {
            foreach (var f in filtros)
                if (f.Propiedad == FiltroPor.Id)
                  return base.AplicarFiltros(registros, filtros);

            return registros
                .AplicarFiltroNombre(filtros)
                .AplicarFiltroUsuarios(filtros);
        }

    }
}
