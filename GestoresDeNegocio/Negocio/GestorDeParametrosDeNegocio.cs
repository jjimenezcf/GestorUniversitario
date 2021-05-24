using System.Collections.Generic;
using AutoMapper;
using ServicioDeDatos;
using ModeloDeDto.Entorno;
using GestorDeElementos;
using ModeloDeDto;
using Utilidades;
using ServicioDeDatos.Negocio;
using Gestor.Errores;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GestoresDeNegocio.Negocio
{

    public class GestorDeParametrosDeNegocio : GestorDeElementos<ContextoSe, ParametroDeNegocioDtm, ParametroDeNegocioDto>
    {
        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<ParametroDeNegocioDtm, ParametroDeNegocioDto>()
                .ForMember(dto => dto.Negocio, dtm => dtm.MapFrom(dtm => dtm.Negocio.Nombre);

                CreateMap<ParametroDeNegocioDto, ParametroDeNegocioDtm>()
                .ForMember(dtm => dtm.Negocio, dto => dto.Ignore());
            }
        }

        public GestorDeParametrosDeNegocio(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }
        internal static GestorDeParametrosDeNegocio Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeParametrosDeNegocio(contexto, mapeador);
        }

        protected override void DespuesDePersistir(ParametroDeNegocioDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            if (parametros.Operacion == enumTipoOperacion.Modificar || parametros.Operacion == enumTipoOperacion.Eliminar)
                ServicioDeCaches.EliminarElemento(typeof(ParametroDeNegocioDtm).FullName, $"{registro.IdNegocio}-{nameof(ParametroDeNegocioDtm.Nombre)}-{registro.Nombre}");
        }

        protected override IQueryable<ParametroDeNegocioDtm> AplicarJoins(IQueryable<ParametroDeNegocioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Negocio);
            return registros;
        }

        internal static ParametroDeNegocioDtm LeerParametro(ContextoSe contextoSe, int idNegocio, string parametro, bool emitirErrorSiNoExiste)
        {
            var indice = $"{idNegocio}-{nameof(ParametroDeNegocioDtm.Nombre)}-{parametro}";
            var cache = ServicioDeCaches.Obtener(typeof(ParametroDeNegocioDtm).FullName);
            if (cache.ContainsKey(indice))
                return (ParametroDeNegocioDtm)cache[indice];

            var gestor = Gestor(contextoSe, contextoSe.Mapeador);
            var filtroPorNegocio = new ClausulaDeFiltrado(nameof(ParametroDeNegocioDtm.IdNegocio), CriteriosDeFiltrado.igual, idNegocio.ToString());
            var filtroPorParametro = new ClausulaDeFiltrado(nameof(ParametroDeNegocioDtm.Nombre), CriteriosDeFiltrado.igual, parametro.ToString());

            var registros = gestor.LeerRegistros(0, 2, new List<ClausulaDeFiltrado> { filtroPorNegocio, filtroPorParametro });
            if (registros.Count != 1)
                GestorDeErrores.Emitir($"La informaciónal leer el parametro {parametro} del negocio con id {idNegocio} no es correcta");

            cache[indice] = registros[0];

            return (ParametroDeNegocioDtm)cache[indice];
        }

        private static ParametroDeNegocioDtm CrearParametro(ContextoSe contexto, int idNegocio, string parametro, string valor)
        {
            var v = new ParametroDeNegocioDtm();
            v.Nombre = parametro;
            v.Valor = valor;
            v.IdNegocio = idNegocio;
            var gestor = Gestor(contexto, contexto.Mapeador);
            v = gestor.PersistirRegistro(v, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
            return v;
        }



    }
}
