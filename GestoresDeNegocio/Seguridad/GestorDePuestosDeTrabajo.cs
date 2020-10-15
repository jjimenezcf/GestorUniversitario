﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GestorDeElementos;
using ModeloDeDto.Seguridad;
using Utilidades;

namespace ServicioDeDatos.Seguridad
{
    public class GestorDePuestosDeTrabajo : GestorDeElementos<ContextoSe, PuestoDtm, PuestoDto>
    {
        public class MapearPuestoDeTrabajo : Profile
        {
            public MapearPuestoDeTrabajo()
            {
                CreateMap<PuestoDtm, PuestoDto>();
                CreateMap<PuestoDto, PuestoDtm>();
            }
        }

        public GestorDePuestosDeTrabajo(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
        }


        internal static GestorDePuestosDeTrabajo Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePuestosDeTrabajo(contexto, mapeador);
        }

        internal static List<PuestoDto> Leer(GestorDePuestosDeTrabajo gestor, int posicion, int cantidad, string filtro)
        {
            var filtros = new List<ClausulaDeFiltrado>();
            if (!filtro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(PuestoDto.Nombre), Valor = filtro });

            var puestosDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(puestosDtm).ToList();
        }

        protected override IQueryable<PuestoDtm> AplicarFiltros(IQueryable<PuestoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (!hayFiltroPorId)
                registros = FiltrarPuestosDeTrabajo(registros,filtros);

            return registros;
        }

        private IQueryable<PuestoDtm> FiltrarPuestosDeTrabajo(IQueryable<PuestoDtm> registros, List<ClausulaDeFiltrado> filtros)
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(PuestosDeUnUsuarioDtm.IdUsuario).ToLower() &&
                    filtro.Criterio == CriteriosDeFiltrado.diferente)
                    registros = registros.Where(i => !i.Usuarios.Any(r => r.IdUsuario == filtro.Valor.Entero()));

                if (filtro.Clausula.ToLower() == nameof(RolesDeUnPuestoDtm.IdRol).ToLower() &&
                    filtro.Criterio == CriteriosDeFiltrado.diferente)
                    registros = registros.Where(i => !i.Roles.Any(r => r.IdRol == filtro.Valor.Entero()));
            }

            return registros;
        }
    }
}