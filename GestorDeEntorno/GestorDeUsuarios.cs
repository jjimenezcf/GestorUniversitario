using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Entorno
{
    static class FiltrosDeUsuario
    {
        public static IQueryable<T> FiltrarPorNombre<T>(this IQueryable<T> regristros, List<ClausulaDeFiltrado> filtros) where T : UsuarioDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == UsuariosPor.NombreCompleto)
                    return regristros.Where(x => x.Apellido.Contains(filtro.Valor) || x.Nombre.Contains(filtro.Valor));

            return regristros;
        }

        public static IQueryable<T> FiltrarPorRelacion<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : UsuarioDtm
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == UsuariosPor.CursosInscrito)
                {
                    var listaIds = filtro.Valor.ListaEnteros(); 
                    foreach(int id in listaIds)
                    {
                        //registros = registros.Where(x => x.Inscripciones.Any(i => i.CursoId == id));
                    }
                }

            return registros;
        }
    }

    static class UsuarioRegOrd
    {
        public const string OrdenPorApellido = "PorApellido";

        public static IQueryable<UsuarioDtm> Orden(this IQueryable<UsuarioDtm> set, List<ClausulaOrdenacion> ordenacion)
        {
            if (ordenacion.Count == 0)
                return set.OrderBy(x => x.Apellido);

            foreach (var orden in ordenacion)
            {
                if (orden.Propiedad == nameof(UsuarioDtm.Apellido))
                    return orden.modo == ModoDeOrdenancion.ascendente
                        ? set.OrderBy(x => x.Apellido)
                        : set.OrderByDescending(x => x.Apellido);
            }

            return set;
        }
    }

    public class GestorDeUsuarios : GestorDeElementos<CtoEntorno,UsuarioDtm, UsuarioDto>
    {

        public class MapearUsuario : Profile
        {
            public MapearUsuario()
            {
                CreateMap<UsuarioDtm, UsuarioDto>();
                CreateMap<UsuarioDto, UsuarioDtm>();
            }
        }

        public GestorDeUsuarios(CtoEntorno contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override IQueryable<UsuarioDtm> AplicarOrden(IQueryable<UsuarioDtm> registros, List<ClausulaOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);
            return registros.Orden(ordenacion);
        }

               

        protected override IQueryable<UsuarioDtm> AplicarFiltros(IQueryable<UsuarioDtm> registros, List<ClausulaDeFiltrado> filtros) 
        {
            foreach (var f in filtros)
                if (f.Propiedad == FiltroPor.Id)
                    return base.AplicarFiltros(registros, filtros);

            return registros
                   .FiltrarPorNombre(filtros)
                   .FiltrarPorRelacion(filtros);
        }
                

        protected override UsuarioDtm LeerConDetalle(int Id)
        {
            return Contexto.Set<UsuarioDtm>()
                            //.Include(i => i.Inscripciones)
                            //.ThenInclude(e => e.Curso)
                            .AsNoTracking()
                            .FirstOrDefault(m => m.Id == Id);
        }

    }


}
