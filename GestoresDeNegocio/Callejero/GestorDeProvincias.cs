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
using Gestor.Errores;
using ServicioDeDatos.TrabajosSometidos;

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

        internal static ProvinciaDtm LeerProvinciaPorCodigo(ContextoSe contexto, string iso2Pais, string codigoProvincia, bool paraActualizar, bool errorSiNoHay = true, bool errorSiMasDeUno = true)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            var filtro1 = new ClausulaDeFiltrado(nameof(ProvinciaDtm.Pais.ISO2), CriteriosDeFiltrado.igual, iso2Pais);
            var filtro2 = new ClausulaDeFiltrado(nameof(ProvinciaDtm.Codigo), CriteriosDeFiltrado.igual, codigoProvincia);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            var p = new ParametrosDeNegocio(paraActualizar ? enumTipoOperacion.LeerConBloqueo : enumTipoOperacion.LeerSinBloqueo);
            p.Parametros.Add(ltrJoinAudt.IncluirUsuarioDtm, false);
            List<ProvinciaDtm> provincias = gestor.LeerRegistros(0, -1, filtros, null, null, p);

            if (provincias.Count == 0 && errorSiNoHay)
                GestorDeErrores.Emitir($"No se ha localizado la provincia para el código del pais {iso2Pais} y codigo de provincia {codigoProvincia}");
            if (provincias.Count > 1 && errorSiMasDeUno)
                GestorDeErrores.Emitir($"Se han localizado más de un registro de provincia con el código del pais {iso2Pais} y codigo de provincia {codigoProvincia}");

            return provincias.Count == 1 ? provincias[0] : null;
        }

        public static void ImportarFicheroDeProvincias(EntornoDeTrabajo entorno, int idArchivo)
        {
            var gestorProceso = GestorDeProvincias.Gestor(entorno.contextoDelProceso, entorno.contextoDelProceso.Mapeador);
            var rutaFichero = GestorDocumental.DescargarArchivo(entorno.contextoDelProceso, idArchivo, entorno.ProcesoIniciadoPorLaCola);
            var fichero = new FicheroCsv(rutaFichero);
            var linea = 0;
            entorno.CrearTraza($"Inicio del proceso");
            var trazaPrcDtm = entorno.CrearTraza($"Procesando la fila {linea}");
            var trazaInfDtm = entorno.CrearTraza($"Traza informativa del proceso");
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
                        throw new Exception($"El contenido de la fila {linea} debe ser: nombre de la provincia, nombre en ingles, iso de 2 iso de 3 y prefijo telefónico");

                    ProcesarProvinciaLeida(entorno, gestorProceso,
                        iso2Pais:fila["E"],
                        nombreProvincia: fila["C"],
                        sigla: fila["A"], 
                        codigo: fila["B"],
                        prefijoTelefono: fila["D"],
                        trazaInfDtm);
                    gestorProceso.Commit(tran);
                }
                catch (Exception e)
                {
                    gestorProceso.Rollback(tran);
                    entorno.AnotarError(e);
                }
                finally
                {
                    entorno.ActualizarTraza(trazaPrcDtm, $"Procesando la fila {linea}");
                }
            }

            entorno.CrearTraza($"Procesadas un total de {linea} filas");
        }

        private static ProvinciaDtm ProcesarProvinciaLeida(EntornoDeTrabajo entorno, GestorDeProvincias gestor, string iso2Pais, string nombreProvincia, string sigla, string codigo, string prefijoTelefono, TrazaDeUnTrabajoDtm trazaInfDtm)
        {
            ParametrosDeNegocio operacion;
            var provinciaDtm = LeerProvinciaPorCodigo(gestor.Contexto, iso2Pais, codigo, paraActualizar: false, errorSiNoHay: false);
            if (provinciaDtm == null)
            {
                var pais = GestorDePaises.LeerPaisPorCodigo(gestor.Contexto, iso2Pais, errorSiNoHay: false);
                provinciaDtm = new ProvinciaDtm();
                provinciaDtm.Codigo = codigo;
                provinciaDtm.Nombre = nombreProvincia;
                provinciaDtm.Sigla = sigla;
                provinciaDtm.IdPais = pais.Id;
                provinciaDtm.Prefijo = prefijoTelefono;
                operacion = new ParametrosDeNegocio(enumTipoOperacion.Insertar);
                entorno.ActualizarTraza(trazaInfDtm, $"Creando la provincia {nombreProvincia}");
            }
            else
            {
                if (provinciaDtm.Nombre != nombreProvincia || provinciaDtm.Codigo != codigo || provinciaDtm.Sigla != sigla || provinciaDtm.Prefijo != prefijoTelefono)
                {
                    provinciaDtm.Nombre = nombreProvincia;
                    provinciaDtm.Sigla = sigla;
                    provinciaDtm.Codigo = codigo;
                    provinciaDtm.Prefijo = prefijoTelefono;
                    operacion = new ParametrosDeNegocio(enumTipoOperacion.Modificar);
                    entorno.ActualizarTraza(trazaInfDtm, $"Modificando la provincia {nombreProvincia}");
                }
                else
                {
                    entorno.ActualizarTraza(trazaInfDtm, $"La provincia {nombreProvincia} ya exite");
                    return provinciaDtm;
                }
            }

            provinciaDtm.Pais = null;
            return gestor.PersistirRegistro(provinciaDtm, operacion);
        }

        protected override IQueryable<ProvinciaDtm> AplicarJoins(IQueryable<ProvinciaDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Pais);
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula == nameof(CpsDeUnaProvinciaDtm.IdCp).ToLower())
                {
                    registros = registros.Include(p => p.Cps);
                }
            }
            return registros;
        }

        protected override IQueryable<ProvinciaDtm> AplicarFiltros(IQueryable<ProvinciaDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            foreach (ClausulaDeFiltrado filtro in filtros.Where(filtro => filtro.Clausula.Equals(nameof(CpsDeUnaProvinciaDtm.CodigoPostal), StringComparison.CurrentCultureIgnoreCase)))
            {
                registros = filtro.Valor.Length == 5
                ? registros.Where(x => x.Cps.Any(y => y.CodigoPostal.Codigo == filtro.Valor))
                : registros.Where(x => x.Cps.Any(y => y.CodigoPostal.Codigo.StartsWith(filtro.Valor)));
            }

            return registros;
        }

        //Todo: --> Reglas de negocio
        protected override void AntesDePersistir(ProvinciaDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);

            if (parametros.Operacion == enumTipoOperacion.Modificar)
            {
                //validar que si la provincia está relacionada con códigos postales, los dos primeros dígitos del código son igual que el código de la provincia
            }

            if (parametros.Operacion == enumTipoOperacion.Eliminar)
            {
                //Validar que no hay municipios con la provincia

                //Eliminar los CPS relacionados con la provincia

            }
        }

        //Todo: --> Reglas de negocio
        protected override void DespuesDePersistir(ProvinciaDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);

            if (parametros.Operacion == enumTipoOperacion.Modificar || parametros.Operacion == enumTipoOperacion.Insertar)
            {
                //Si hay códigos postales que sus dos primeros dígitos corresponden con el de la provincia entonces relacionarlos
            }
        }

        public List<ProvinciaDto> LeerProvincias(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            var registros = LeerRegistrosPorNombre(posicion, cantidad, filtros);
            return MapearElementos(registros).ToList();
        }

    }
}
