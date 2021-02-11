﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using Utilidades;
using Gestor.Errores;
using GestoresDeNegocio.Entorno;
using ServicioDeDatos.Entorno;

namespace GestoresDeNegocio.TrabajosSometidos
{

    public class GestorDeTrabajosSometido : GestorDeElementos<ContextoSe, TrabajoSometidoDtm, TrabajoSometidoDto>
    {

        public class MapearNegocio : Profile
        {
            public MapearNegocio()
            {
                CreateMap<TrabajoSometidoDtm, TrabajoSometidoDto>()
                .ForMember(dto => dto.Ejecutor, dtm => dtm.MapFrom(x => $"({x.Ejecutor.Login})- {x.Ejecutor.Nombre} {x.Ejecutor.Apellido}"))
                .ForMember(dto => dto.InformarA, dtm => dtm.MapFrom(x => x.InformarA.Nombre))
                .ForMember(dto => dto.Programa, dtm => dtm.MapFrom(x => x.EsDll ? $"{x.Clase}.{x.Metodo}" : $"{x.Esquema}.{x.Pa}"));


                CreateMap<TrabajoSometidoDto, TrabajoSometidoDtm>()
                .ForMember(dtm => dtm.Ejecutor, dto => dto.Ignore())
                .ForMember(dtm => dtm.InformarA, dto => dto.Ignore());
            }
        }

        public GestorDeTrabajosSometido(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        public static GestorDeTrabajosSometido Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeTrabajosSometido(contexto, mapeador);
        }

        internal static TrabajoSometidoDtm Obtener(ContextoSe contexto, IMapper mapeador,string dll, string clase, string metodo)
        {
            var gestor = Gestor(contexto, mapeador);
            var filtroDll = new ClausulaDeFiltrado() { Clausula = nameof(TrabajoSometidoDtm.Dll), Criterio = ModeloDeDto.CriteriosDeFiltrado.igual, Valor = dll };
            var filtroClase = new ClausulaDeFiltrado() { Clausula = nameof(TrabajoSometidoDtm.Clase), Criterio = ModeloDeDto.CriteriosDeFiltrado.igual, Valor = clase };
            var filtroMetodo = new ClausulaDeFiltrado() { Clausula = nameof(TrabajoSometidoDtm.Metodo), Criterio = ModeloDeDto.CriteriosDeFiltrado.igual, Valor = metodo };

            var filtros = new List<ClausulaDeFiltrado>() { filtroDll, filtroClase, filtroMetodo };

            var ts = gestor.LeerRegistroCacheado(filtros);
            if (ts == null)
              ts =  gestor.CrearTs(dll, clase, metodo);

            return ts;
        }

        private TrabajoSometidoDtm CrearTs(string dll, string clase, string metodo)
        {
            throw new System.NotImplementedException();
        }

        protected override IQueryable<TrabajoSometidoDtm> AplicarJoins(IQueryable<TrabajoSometidoDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.InformarA);
            registros = registros.Include(p => p.Ejecutor);
            return registros;
        }

        protected override void AntesDePersistirValidarRegistro(TrabajoSometidoDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistirValidarRegistro(registro, parametros);

            if (registro.Pa.IsNullOrEmpty() && registro.Esquema.IsNullOrEmpty() && registro.Dll.IsNullOrEmpty() && registro.Clase.IsNullOrEmpty() && registro.Metodo.IsNullOrEmpty())
                GestorDeErrores.Emitir("Debe indicar una Dll o un PA");

            if (!registro.EsDll && (registro.Pa.IsNullOrEmpty() || registro.Esquema.IsNullOrEmpty()))
                GestorDeErrores.Emitir("Debe indicar el PA y su Esquema");

            if (!registro.EsDll && (!registro.Dll.IsNullOrEmpty() || !registro.Clase.IsNullOrEmpty() || !registro.Metodo.IsNullOrEmpty()))
                GestorDeErrores.Emitir("Si ha indicado que no es una dll los campos de ddl, clase y métodos han de ser nulos");

            if (registro.EsDll && (!registro.Pa.IsNullOrEmpty() || !registro.Esquema.IsNullOrEmpty()))
                GestorDeErrores.Emitir("Si ha indicado que es una dll los campos de PA y esquema han de ser nulos");

            if (registro.EsDll && (registro.Dll.IsNullOrEmpty() || registro.Clase.IsNullOrEmpty() || registro.Metodo.IsNullOrEmpty()))
                GestorDeErrores.Emitir("Si ha indicado que es una dll los campos de ddl, clase y métodos han de ser indicados");


            if (registro.EsDll)
            {
                var ruta = GestorDeVariables.Gestor(Contexto, Mapeador).LeerVariable(Variable.Binarios);
                Ensamblados.ValidarMetodo($"{ruta}\\{registro.Dll}.dll", registro.Clase, registro.Metodo);
            }
            else
                GestorDePa.ValidarExistePa(registro.Pa, registro.Esquema);

        }

    }
}

//Antigua forma, antes de usar Dapper
//using (var c = ContextoSe.ObtenerContexto())
//{
//    if (!new ExistePa(c, registro.Pa, registro.Esquema).Existe)
//        GestorDeErrores.Emitir($"El {registro.Esquema}.{registro.Pa} indicado no existe en la BD");
//}
