using System.Collections.Generic;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using ServicioDeDatos.Callejero;
using ModeloDeDto.Callejero;
using Gestor.Errores;
using Utilidades;
using GestoresDeNegocio.TrabajosSometidos;
using System;
using GestoresDeNegocio.Archivos;
using System.Linq;
using ServicioDeDatos.TrabajosSometidos;

namespace GestoresDeNegocio.Callejero
{

    public class GestorDeCodigosPostales : GestorDeElementos<ContextoSe, CodigoPostalDtm, CodigoPostalDto>
    {

        class archivoParaImportar
        {
            public string parametro { get; set; }
            public int valor { get; set; }
        }

        public class ltrCps
        {
            internal static readonly string NombreProvincia = nameof(NombreProvincia);
            internal static readonly string NombreMunicipio = nameof(NombreMunicipio);
            internal const string csvCp = nameof(csvCp);
        }


        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<CodigoPostalDtm, CodigoPostalDto>();
                CreateMap<CodigoPostalDto, CodigoPostalDtm>();
            }
        }

        public GestorDeCodigosPostales(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDeCodigosPostales Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeCodigosPostales(contexto, mapeador); ;
        }


        public List<CodigoPostalDto> LeerCodigosPostales(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            var registros = LeerRegistrosPorNombre(posicion, cantidad, filtros);
            return MapearElementos(registros).ToList();
        }

        internal static CodigoPostalDtm LeerTipoDeViaPorCp(ContextoSe contexto, string cp, bool paraActualizar, bool errorSiNoHay = true, bool errorSiMasDeUno = true)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.LeerRegistro(nameof(CodigoPostalDtm.cp), cp, errorSiNoHay, errorSiMasDeUno, paraActualizar ? true : false, paraActualizar ? true : false);
        }


        public static void ImportarFicheroDeCodigosPostales(EntornoDeTrabajo entorno, int idArchivo)
        {
            var gestor = Gestor(entorno.contextoDelProceso, entorno.contextoDelProceso.Mapeador);
            var rutaFichero = GestorDocumental.DescargarArchivo(entorno.contextoDelProceso, idArchivo, entorno.ProcesoIniciadoPorLaCola);
            var fichero = new FicheroCsv(rutaFichero);
            var linea = 0;
            entorno.CrearTraza($"Inicio del proceso");
            var trazaPrcDtm = entorno.CrearTraza($"Procesando la fila {linea}");
            var trazaInfDtm = entorno.CrearTraza($"Traza informativa del proceso");
            foreach (var fila in fichero)
            {
                var tran = gestor.IniciarTransaccion();
                try
                {
                    linea++;
                    if (fila.EnBlanco)
                        continue;

                    if (fila.Columnas != 3)
                        throw new Exception($"la fila {linea} solo debe tener 3 columnas");

                    if (fila["A"].IsNullOrEmpty())
                        GestorDeErrores.Emitir($"El contenido de la fila {linea} donde se indica la provincia, celda A, no puede ser nulo");
                    if (fila["B"].IsNullOrEmpty())
                        GestorDeErrores.Emitir($"El contenido de la fila {linea} donde se indica el municipio, celda B, no puede ser nulo");
                    if (fila["C"].IsNullOrEmpty())
                        GestorDeErrores.Emitir($"El contenido de la fila {linea} donde se indica el CP, celda C, no puede ser nulo");

                    ProcesarCodigosPostales(entorno, gestor, fila["A"], fila["B"], fila["C"], trazaInfDtm);
                    gestor.Commit(tran);
                }
                catch (Exception e)
                {
                    gestor.Rollback(tran);
                    entorno.AnotarError($"Error al procesar la línea {linea}", e);
                }
                finally
                {
                    entorno.ActualizarTraza(trazaPrcDtm, $"Procesando la fila {linea}");
                }
            }

            entorno.CrearTraza($"Procesadas un total de {linea} filas");
        }

        private static CodigoPostalDtm ProcesarCodigosPostales(EntornoDeTrabajo entorno, GestorDeCodigosPostales gestor, string provincia, string municipio, string cp, TrazaDeUnTrabajoDtm trazaInfDtm)
        {
            ParametrosDeNegocio operacion;
            var codigoPostalDtm = gestor.LeerRegistro(nameof(CodigoPostalDtm.cp), cp, errorSiNoHay: false, errorSiHayMasDeUno: true, traqueado: false, conBloqueo: false);
            if (codigoPostalDtm == null)
            {
                codigoPostalDtm = new CodigoPostalDtm();
                codigoPostalDtm.cp = cp;
                operacion = new ParametrosDeNegocio(enumTipoOperacion.Insertar);
                entorno.ActualizarTraza(trazaInfDtm, $"Creando el codigo postal {cp}");
            }
            else
            {
                entorno.ActualizarTraza(trazaInfDtm, $"El codigo postal {cp} ya existe");
                GestorDeCpsDeUnMunicipio.CrearRelacionConMunicipioSiNoExiste(entorno.contextoDelProceso, codigoPostalDtm, "ES", provincia, municipio);
                return codigoPostalDtm;
            }
            operacion.Parametros[ltrCps.NombreProvincia] = provincia;
            operacion.Parametros[ltrCps.NombreMunicipio] = municipio;
            return gestor.PersistirRegistro(codigoPostalDtm, operacion);
        }

        protected override void AntesDePersistir(CodigoPostalDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);
            if (parametros.Operacion == enumTipoOperacion.Insertar)
            {
                if (parametros.Parametros.ContainsKey(ltrCps.NombreProvincia) && parametros.Parametros.ContainsKey(ltrCps.NombreMunicipio))
                {
                    var np = parametros.Parametros[ltrCps.NombreProvincia].ToString();
                    var nm = parametros.Parametros[ltrCps.NombreMunicipio].ToString();
                    var municipioDtm = GestorDeMunicipios.LeerMunicipioPorNombre(Contexto, "ES", np, nm, paraActualizar: false, errorSiNoHay: false);
                    if (municipioDtm != null)
                        parametros.Parametros[nameof(MunicipioDtm)] = municipioDtm;
                }

            }
            if (parametros.Operacion == enumTipoOperacion.Eliminar)
            {
                //TODO:
                //validar que el cp no está usado en ninguna dirección
            }
        }

        protected override void DespuesDePersistir(CodigoPostalDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            if (parametros.Operacion == enumTipoOperacion.Insertar)
            {
                //relacionar con la provincia usando los dos primeros caractéres
                var gestorProvincias = GestorDeProvincias.Gestor(Contexto, Contexto.Mapeador);
                var provinciaDtm = gestorProvincias.LeerRegistro(nameof(ProvinciaDtm.Codigo), registro.cp.PadLeft(5, '0').Substring(0, 2), errorSiNoHay: true, errorSiHayMasDeUno: true, traqueado: false, conBloqueo: false);
                GestorDeCpsDeUnaProvincia.CrearRelacion(Contexto, registro, provinciaDtm);

                //relacionar con el municipio usando lo indicado en los parámetros
                if (parametros.Parametros.ContainsKey(nameof(MunicipioDtm)))
                {
                    var municipioDtm = (MunicipioDtm)parametros.Parametros[nameof(MunicipioDtm)];
                    GestorDeCpsDeUnMunicipio.CrearRelacion(Contexto, registro, municipioDtm);
                }
            }
            if (parametros.Operacion == enumTipoOperacion.Eliminar)
            {
                //TODO:
                //eliminar relación con la provincia
                //eliminar relación con el municipio
            }

        }

    }
}
