﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Entorno
{
    static class UsuarioRegFlt
    {
        public static IQueryable<T> AplicarFiltroNombre<T>(this IQueryable<T> regristros, List<ClausulaDeFiltrado> filtros) where T : RegUsuario
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == UsuariosPor.NombreCompleto)
                    return regristros.Where(x => x.Apellido.Contains(filtro.Valor) || x.Nombre.Contains(filtro.Valor));

            return regristros;
        }

        public static IQueryable<T> AplicarFiltroDeRelacion<T>(this IQueryable<T> registros, List<ClausulaDeFiltrado> filtros) where T : RegUsuario
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

        public static IQueryable<RegUsuario> Orden(this IQueryable<RegUsuario> set, Dictionary<string, Ordenacion> orden)
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


    public class GestorDeUsuarios : GestorDeElementos<CtoEntorno, RegUsuario, EleUsuario>
    {

        public class MapearUsuario : Profile
        {
            public MapearUsuario()
            {
                CreateMap<RegUsuario, EleUsuario>();
                CreateMap<EleUsuario, RegUsuario>();
            }
        }

        public GestorDeUsuarios(CtoEntorno contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        protected override IQueryable<RegUsuario> AplicarOrden(IQueryable<RegUsuario> registros, Dictionary<string, Ordenacion> orden)
        {
            registros = base.AplicarOrden(registros, orden);
            return registros.Orden(orden);
        }

               

        protected override IQueryable<RegUsuario> AplicarFiltros(IQueryable<RegUsuario> registros, List<ClausulaDeFiltrado> filtros) 
        {
            foreach (var f in filtros)
                if (f.Propiedad == FiltroPor.Id)
                    return base.AplicarFiltros(registros, filtros);

            return registros
                   .AplicarFiltroNombre(filtros)
                   .AplicarFiltroDeRelacion(filtros);
        }
                

        protected override RegUsuario LeerConDetalle(int Id)
        {
            return Contexto.Set<RegUsuario>()
                            //.Include(i => i.Inscripciones)
                            //.ThenInclude(e => e.Curso)
                            .AsNoTracking()
                            .FirstOrDefault(m => m.Id == Id);
        }

    }


}
