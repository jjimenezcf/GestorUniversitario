using System.Collections.Generic;
using AutoMapper;
using ServicioDeDatos;
using GestorDeElementos;
using ServicioDeDatos.Callejero;
using ModeloDeDto.Callejero;
using Gestor.Errores;
using Utilidades;
using GestoresDeNegocio.TrabajosSometidos;
using System.Reflection;
using System;
using Newtonsoft.Json;
using GestoresDeNegocio.Archivos;
using System.Linq;
using GestoresDeNegocio.Entorno;
using GestoresDeNegocio.Negocio;
using ServicioDeDatos.TrabajosSometidos;

namespace GestoresDeNegocio.Callejero
{

    public class GestorDePaises : GestorDeElementos<ContextoSe, PaisDtm, PaisDto>
    {

        class archivoParaImportar
        {
            public string parametro { get; set; }
            public int valor { get; set; }
        }

        public const string ParametroPais = "csvPais";

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<PaisDtm, PaisDto>();

                CreateMap<PaisDto, PaisDtm>()
                .ForMember(dtm => dtm.FechaCreacion, dto => dto.Ignore())
                .ForMember(dtm => dtm.FechaModificacion, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdUsuaCrea, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdUsuaModi, dto => dto.Ignore());
            }
        }

        public GestorDePaises(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDePaises Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePaises(contexto, mapeador); ;
        }


        public List<PaisDto> LeerPaises(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            var registros = LeerRegistrosPorNombre(posicion, cantidad, filtros);
            return MapearElementos(registros).ToList();
        }

        internal static PaisDtm LeerPaisPorCodigo(ContextoSe contexto, string iso2Pais, bool paraActualizar, bool errorSiNoHay = true, bool errorSiMasDeUno = true)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.LeerRegistro(nameof(PaisDtm.ISO2), iso2Pais, errorSiNoHay, errorSiMasDeUno, paraActualizar ? true : false, paraActualizar ? true : false);
        }

        public static void SometerImportarCallejero(ContextoSe contexto, string parametros)
        {
            if (parametros.IsNullOrEmpty())
                GestorDeErrores.Emitir("No se han proporcionado los parámetros para someter el trabajo de importación");

            var dll = Assembly.GetExecutingAssembly().GetName().Name;
            var clase = typeof(GestorDePaises).FullName;
            var ts = GestorDeTrabajosSometido.Obtener(contexto, "Importar callejero", dll, clase, nameof(SometerImportarCallejero).Replace("Someter", ""));
            // crear trabajo de usuario

            var tu = GestorDeTrabajosDeUsuario.Crear(contexto, ts, parametros);
            //liberarlo
        }

        public static void ImportarCallejero(EntornoDeTrabajo entorno)
        {
            var archivos = JsonConvert.DeserializeObject<List<archivoParaImportar>>(entorno.Trabajo.Parametros);

            if (archivos.Count == 0)
                GestorDeErrores.Emitir("No se ha sometido ningún fichero a cargar");

            foreach (archivoParaImportar archivo in archivos)
            {
                switch (archivo.parametro)
                {
                    case GestorDePaises.ParametroPais:
                        GestorDePaises.ImportarFicheroDePaises(entorno, archivo.valor);
                        break;
                    case GestorDeProvincias.ParametroProvincia:
                        GestorDeProvincias.ImportarFicheroDeProvincias(entorno, archivo.valor);
                        break;
                    case GestorDeMunicipios.ParametroMunicipio:
                        GestorDeMunicipios.ImportarFicheroDeMunicipios(entorno, archivo.valor);
                        break;
                    default:
                        GestorDeErrores.Emitir($"No es valido el parámetro {archivo.parametro} en el proceso {nameof(ImportarCallejero)}");
                        break;
                }
            }
        }

        private static void ImportarFicheroDePaises(EntornoDeTrabajo entorno, int idArchivo)
        {
            var gestor = GestorDePaises.Gestor(entorno.contextoDelProceso, entorno.contextoDelProceso.Mapeador);
            var rutaFichero = GestorDocumental.DescargarArchivo(entorno.contextoDelProceso, idArchivo);
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

                    if (fila.Columnas != 5)
                        throw new Exception($"la fila {linea} solo debe tener 5 columnas");

                    if (fila["A"].IsNullOrEmpty())
                        GestorDeErrores.Emitir($"El contenido de la fila {linea} donde se indica el nombre del país, celda A, no puede ser nulo");
                    if (fila["B"].IsNullOrEmpty())
                        GestorDeErrores.Emitir($"El contenido de la fila {linea} donde se indica el nombre en Inglés, celda B, no puede ser nulo");
                    if (fila["C"].IsNullOrEmpty())
                        GestorDeErrores.Emitir($"El contenido de la fila {linea} donde se indica el iso2, celda C, no puede ser nulo");
                    if (fila["D"].IsNullOrEmpty())
                        GestorDeErrores.Emitir($"El contenido de la fila {linea} donde se indica el iso3, celda D, no puede ser nulo");
                    if (fila["E"].IsNullOrEmpty())
                        GestorDeErrores.Emitir($"El contenido de la fila {linea} donde se indica el prefijo telefónico, celda E, no puede ser nulo");

                    ProcesarPaisLeido(entorno, gestor, fila["A"], fila["B"], fila["C"], fila["D"], fila["E"], trazaInfDtm);
                    gestor.Commit(tran);
                }
                catch (Exception e)
                {
                    gestor.Rollback(tran);
                    entorno.AnotarError(e);
                }
                finally
                {
                    entorno.ActualizarTraza(trazaPrcDtm, $"Procesando la fila {linea}");
                }
            }

            entorno.CrearTraza($"Procesadas un total de {linea} filas");
        }

        private static PaisDtm ProcesarPaisLeido(EntornoDeTrabajo entorno, GestorDePaises gestor, string nombrePais, string nombreEnIngles, string Iso2, string codigoPais, string prefijoTelefono, TrazaDeUnTrabajoDtm trazaInfDtm)
        {
            ParametrosDeNegocio operacion;
            var p = gestor.LeerRegistro(nameof(PaisDtm.Codigo), codigoPais, false, true, false, false);
            if (p == null)
            {
                p = new PaisDtm();
                p.Codigo = codigoPais;
                p.Nombre = nombrePais;
                p.NombreIngles = nombreEnIngles;
                p.ISO2 = Iso2;
                p.Prefijo = prefijoTelefono;
                operacion = new ParametrosDeNegocio(enumTipoOperacion.Insertar);
                entorno.ActualizarTraza(trazaInfDtm, $"Creando el pais {nombrePais}");
            }
            else
            {
                if (p.Nombre != nombrePais || p.ISO2 != Iso2 || p.NombreIngles != nombreEnIngles || p.Prefijo != prefijoTelefono)
                {
                    p.Nombre = nombrePais;
                    p.NombreIngles = nombreEnIngles;
                    p.ISO2 = Iso2;
                    p.Prefijo = prefijoTelefono;
                    operacion = new ParametrosDeNegocio(enumTipoOperacion.Modificar);
                    entorno.ActualizarTraza(trazaInfDtm, $"Modificando el pais {nombrePais}");
                }
                else
                {
                    entorno.ActualizarTraza(trazaInfDtm, $"El pais {nombrePais} ya existe");
                    return p;
                }
            }

            return gestor.PersistirRegistro(p, operacion);
        }

    }
}
