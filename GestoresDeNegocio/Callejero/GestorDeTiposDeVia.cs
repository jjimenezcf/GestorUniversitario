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

    public class GestorDeTiposDeVia : GestorDeElementos<ContextoSe, TipoDeViaDtm, TipoDeViaDto>
    {

        class archivoParaImportar
        {
            public string parametro { get; set; }
            public int valor { get; set; }
        }

        public const string ParametroTipoDeVia = "csvTipoDeVias";

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<TipoDeViaDtm, TipoDeViaDto>();
                CreateMap<TipoDeViaDto, TipoDeViaDtm>();
            }
        }

        public GestorDeTiposDeVia(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDeTiposDeVia Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeTiposDeVia(contexto, mapeador); ;
        }


        public List<TipoDeViaDto> LeerTiposDeVia(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            var registros = LeerRegistrosPorNombre(posicion, cantidad, filtros);
            return MapearElementos(registros).ToList();
        }

        internal static TipoDeViaDtm LeerTipoDeViaPorSigla(ContextoSe contexto, string sigla, bool paraActualizar, bool errorSiNoHay = true, bool errorSiMasDeUno = true)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.LeerRegistro(nameof(TipoDeViaDtm.Sigla), sigla, errorSiNoHay, errorSiMasDeUno, paraActualizar ? true : false, paraActualizar ? true : false);
        }


        public static void ImportarFicheroDeTiposDeVia(EntornoDeTrabajo entorno, int idArchivo)
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

                    if (fila.Columnas != 2)
                        throw new Exception($"la fila {linea} solo debe tener 2 columnas");

                    if (fila["A"].IsNullOrEmpty())
                        GestorDeErrores.Emitir($"El contenido de la fila {linea} donde se indica la sigla, celda A, no puede ser nulo");
                    if (fila["B"].IsNullOrEmpty())
                        GestorDeErrores.Emitir($"El contenido de la fila {linea} donde se indica el nombre, celda B, no puede ser nulo");

                    ProcesarTipoDeViaLeido(entorno, gestor, fila["A"], fila["B"], trazaInfDtm);
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

        private static TipoDeViaDtm ProcesarTipoDeViaLeido(EntornoDeTrabajo entorno, GestorDeTiposDeVia gestor, string sigla, string nombre, TrazaDeUnTrabajoDtm trazaInfDtm)
        {
            ParametrosDeNegocio operacion;
            var p = gestor.LeerRegistro(nameof(TipoDeViaDtm.Sigla), sigla, errorSiNoHay: false, errorSiHayMasDeUno: true, traqueado: true, conBloqueo: false);
            //var p = LeerTipoDeViaPorSigla(entorno.contextoDelProceso, sigla, paraActualizar: true); 
            if (p == null)
            {
                p = new TipoDeViaDtm();
                p.Sigla = sigla;
                p.Nombre = nombre;
                operacion = new ParametrosDeNegocio(enumTipoOperacion.Insertar);
                entorno.ActualizarTraza(trazaInfDtm, $"Creando el tipo de vía {sigla}");
            }
            else
            {
                if (p.Nombre != nombre)
                {
                    p.Nombre = nombre;
                    operacion = new ParametrosDeNegocio(enumTipoOperacion.Modificar);
                    entorno.ActualizarTraza(trazaInfDtm, $"Modificando el tipo de vía {sigla}");
                    entorno.CrearTraza($"Existe un tipo de vía con la sigla {p.Sigla}, el nombre es {p.Nombre}, vaya al mantenimiento si quiere cambiar el nombre por {nombre}");
                }
                else
                {
                    entorno.ActualizarTraza(trazaInfDtm, $"El tipo de vía {sigla} ya existe");
                    return p;
                }
            }

            return gestor.PersistirRegistro(p, operacion);
        }

    }
}
