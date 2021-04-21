using System.Collections.Generic;
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
using System;
using System.Reflection;
using GestoresDeNegocio.Archivos;
using System.IO;

namespace GestoresDeNegocio.TrabajosSometidos
{

    public class GestorDeTrabajosSometido : GestorDeElementos<ContextoSe, TrabajoSometidoDtm, TrabajoSometidoDto>
    {

        public class MapeadorTrabajosSometidos : Profile
        {

            public MapeadorTrabajosSometidos()
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

        internal static TrabajoSometidoDtm Obtener(ContextoSe contexto, string nombreTs ,string dll, string clase, string metodo)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var filtroDll = new ClausulaDeFiltrado() { Clausula = nameof(TrabajoSometidoDtm.Dll), Criterio = ModeloDeDto.CriteriosDeFiltrado.igual, Valor = dll };
            var filtroClase = new ClausulaDeFiltrado() { Clausula = nameof(TrabajoSometidoDtm.Clase), Criterio = ModeloDeDto.CriteriosDeFiltrado.igual, Valor = clase };
            var filtroMetodo = new ClausulaDeFiltrado() { Clausula = nameof(TrabajoSometidoDtm.Metodo), Criterio = ModeloDeDto.CriteriosDeFiltrado.igual, Valor = metodo };

            var filtros = new List<ClausulaDeFiltrado>() { filtroDll, filtroClase, filtroMetodo };

            var ts = gestor.LeerRegistroCacheado(filtros, false, true);
            if (ts == null)
              ts =  gestor.CrearTs(nombreTs, filtros);

            return ts;
        }

        private TrabajoSometidoDtm CrearTs(string nombreTs, List<ClausulaDeFiltrado> filtros)
        {
            var ts = new TrabajoSometidoDtm();
            ts.Nombre = nombreTs;
            ts.EsDll = true;
            ts.Dll = filtros[0].Valor;
            ts.Clase = filtros[1].Valor;
            ts.Metodo = filtros[2].Valor;
            ts.ComunicarError = true;
            ts.ComunicarFin = false;

            return PersistirRegistro(ts, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
        }

        public List<TrabajoSometidoDto> LeerTrabajos(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            var registros = LeerRegistrosPorNombre(posicion, cantidad, filtros);
            return MapearElementos(registros).ToList();
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

            if (parametros.Operacion == enumTipoOperacion.Eliminar)
                return;

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
                ValidarExisteTrabajoSometido(registro);
            }
            else
                GestorDePa.ValidarExistePa(registro.Pa, registro.Esquema);
        }

        public static MethodInfo ValidarExisteTrabajoSometido(TrabajoSometidoDtm registro)
        {
            var rutaBinarios = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            return Ensamblados.ValidarMetodoEstatico($"{rutaBinarios}\\{registro.Dll}.dll", registro.Clase, registro.Metodo);
        }
    }
}



