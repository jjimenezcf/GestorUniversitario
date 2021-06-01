using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using ServicioDeDatos.Callejero;
using ModeloDeDto.Callejero;
using Utilidades;
using GestoresDeNegocio.TrabajosSometidos;
using System;
using GestoresDeNegocio.Archivos;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto;

namespace GestoresDeNegocio.Callejero
{
    public class GestorDeMunicipios : GestorDeElementos<ContextoSe, MunicipioDtm, MunicipioDto>
    {

        public const string ParametroMunicipio = "csvMunicipio"; 

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<MunicipioDtm, MunicipioDto>()
                    .ForMember(dto => dto.Provincia, dtm => dtm.MapFrom(dtm => $"({dtm.Provincia.Codigo}) {dtm.Provincia.Nombre}"));

                CreateMap<MunicipioDto, MunicipioDtm>()
                .ForMember(dtm => dtm.Provincia, dto => dto.Ignore())
                .ForMember(dtm => dtm.FechaCreacion, dto => dto.Ignore())
                .ForMember(dtm => dtm.FechaModificacion, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdUsuaCrea, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdUsuaModi, dto => dto.Ignore());

            }

        }

        public GestorDeMunicipios(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDeMunicipios Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeMunicipios(contexto, mapeador); ;
        }

        protected override IQueryable<MunicipioDtm> AplicarJoins(IQueryable<MunicipioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Provincia);
            registros = registros.Include(p => p.Provincia.Pais);
            return registros;
        }

        internal static void ImportarFicheroDeMunicipios(EntornoDeTrabajo entorno, int idArchivo)
        {
            var gestorProceso = GestorDeMunicipios.Gestor(entorno.contextoDelProceso, entorno.contextoDelProceso.Mapeador);
            var rutaFichero = GestorDocumental.DescargarArchivo(entorno.contextoDelProceso, idArchivo);
            var fichero = new FicheroCsv(rutaFichero);
            var linea = 0;
            entorno.AnotarTraza($"Inicio del proceso");
            var idTraza = entorno.AnotarTraza($"Procesando la fila {linea}");
            var idTrazaInformativa = entorno.AnotarTraza($"Traza informativa del proceso");
            foreach (var fila in fichero)
            {
                var tran = gestorProceso.IniciarTransaccion();
                try
                {
                    linea++;
                    if (fila.EnBlanco)
                        continue;

                    if (fila.Columnas != 5)
                        throw new Exception($"la fila {linea} solo debe tener 5 columnas");

                    if (fila["A"].IsNullOrEmpty() || fila["B"].IsNullOrEmpty() ||
                        fila["C"].IsNullOrEmpty() || fila["D"].IsNullOrEmpty() || 
                        fila["E"].IsNullOrEmpty())
                        throw new Exception($"El contenido de la fila {linea} debe ser: código de provincia, código municipio, DC, nombre del municipio");

                    ProcesarMunicipioLeido(entorno, gestorProceso,
                        codigoPais: fila["A"],
                        codigoProvincia: fila["B"],
                        codigoMunicipio: fila["C"],
                        DC: fila["D"],
                        nombreMunicipio: fila["E"],
                        idTrazaInformativa);
                    gestorProceso.Commit(tran);
                }
                catch (Exception e)
                {
                    gestorProceso.Rollback(tran);
                    entorno.AnotarError(e);
                }
                finally
                {
                    entorno.AnotarTraza(idTraza, $"Procesando la fila {linea}");
                }
            }

            entorno.AnotarTraza($"Procesadas un total de {linea} filas");

        }

        private static void ProcesarMunicipioLeido(EntornoDeTrabajo entorno, GestorDeMunicipios gestorProceso, string codigoPais, string codigoProvincia, string codigoMunicipio, string DC, string nombreMunicipio, int idTrazaInformativa)
        {


        }

        private static List<MunicipioDtm> BuscarMunicipio(GestorDeMunicipios gestor, string codigoProvincia, string  codigoMunicipio)
        {
            var filtros = new List<ClausulaDeFiltrado>();
            var filtro1 = new ClausulaDeFiltrado(nameof(MunicipioDtm.Provincia.Codigo), CriteriosDeFiltrado.igual, codigoProvincia);
            var filtro2 = new ClausulaDeFiltrado(nameof(MunicipioDtm.Codigo), CriteriosDeFiltrado.igual, codigoMunicipio);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            List<MunicipioDtm> municipioDtm = gestor.LeerRegistros(0, -1, filtros);
            return municipioDtm;
        }
    }
}
