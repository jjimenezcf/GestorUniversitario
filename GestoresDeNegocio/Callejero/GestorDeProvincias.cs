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

namespace GestoresDeNegocio.Callejero
{
    public class GestorDeProvincias : GestorDeElementos<ContextoSe, ProvinciaDtm, ProvinciaDto>
    {
        class archivoParaImportar
        {
            public string parametro { get; set; }
            public int valor { get; set; }
        }

        public const string ParametroProvincia = "csvProvincia";

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<ProvinciaDtm, ProvinciaDto>()
                    .ForMember(dto => dto.Pais, dtm => dtm.MapFrom(dtm => $"({dtm.Pais.Codigo}) {dtm.Pais.Nombre}"));

                CreateMap<ProvinciaDto, ProvinciaDtm>()
                .ForMember(dtm => dtm.Pais, dto => dto.Ignore())
                .ForMember(dtm => dtm.FechaCreacion, dto => dto.Ignore())
                .ForMember(dtm => dtm.FechaModificacion, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdUsuaCrea, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdUsuaModi, dto => dto.Ignore());

            }
        }

        public GestorDeProvincias(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDeProvincias Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeProvincias(contexto, mapeador); ;
        }


        public static void ImportarFicheroDeProvincias(EntornoDeTrabajo entorno, int idArchivo)
        {
            var gestor = GestorDeProvincias.Gestor(entorno.contextoPr, entorno.contextoPr.Mapeador);
            var rutaFichero = GestorDocumental.DescargarArchivo(entorno.contextoPr, idArchivo);
            var fichero = new FicheroCsv(rutaFichero);
            var linea = 0;
            entorno.AnotarTraza($"Inicio del proceso");
            var idTraza = entorno.AnotarTraza($"Procesando la fila {linea}");
            foreach (var fila in fichero)
            {
                var tran = gestor.IniciarTransaccion();
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
                        throw new Exception($"El contenido de la fila {linea} debe ser: nombre de la provincia, nombre en ingles, iso de 2 iso de 3 y prefijo telefónico");

                    ProcesarProvinciaLeida(gestor,
                        codigoPais:fila["E"],
                        nombreProvincia: fila["C"],
                        sigla: fila["A"], 
                        codigo: fila["B"],
                        prefijoTelefono: fila["D"]);
                    gestor.Commit(tran);
                }
                catch (Exception e)
                {
                    gestor.Rollback(tran);
                    entorno.AnotarError(e);
                }
                finally
                {
                    entorno.AnotarTraza(idTraza, $"Procesando la fila {linea}");
                }
            }

            entorno.AnotarTraza($"Procesadas un total de {linea} filas");
        }

        protected override IQueryable<ProvinciaDtm> AplicarJoins(IQueryable<ProvinciaDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Pais);
            return registros;
        }

        private static ProvinciaDtm ProcesarProvinciaLeida(GestorDeProvincias gestor, string codigoPais, string nombreProvincia, string sigla, string codigo, string prefijoTelefono)
        {
            ParametrosDeNegocio operacion;
            var p = gestor.LeerRegistro(nameof(ProvinciaDtm.Codigo), codigo, false, true, true, true);
            if (p == null)
            {
                var pais = GestorDePaises.LeerPaisPorCodigo(gestor.Contexto, codigoPais);
                p = new ProvinciaDtm();
                p.Codigo = codigo;
                p.Nombre = nombreProvincia;
                p.Sigla = sigla;
                p.IdPais = pais.Id;
                p.Prefijo = prefijoTelefono;
                operacion = new ParametrosDeNegocio(enumTipoOperacion.Insertar);
            }
            else
            {
                if (p.Nombre != nombreProvincia || p.Codigo != codigo || p.Sigla != sigla || p.Prefijo != prefijoTelefono)
                {
                    p.Nombre = nombreProvincia;
                    p.Sigla = sigla;
                    p.Codigo = codigo;
                    p.Prefijo = prefijoTelefono;
                    operacion = new ParametrosDeNegocio(enumTipoOperacion.Modificar);
                }
                else
                    return p;
            }

            return gestor.PersistirRegistro(p, operacion);
        }

    }
}
