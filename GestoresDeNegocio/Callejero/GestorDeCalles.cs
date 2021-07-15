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
using System.Reflection;

namespace GestoresDeNegocio.Callejero
{
    public class GestorDeCalles : GestorDeElementos<ContextoSe, CalleDtm, CalleDto>
    {

        public const string ParametroCalle = "csvCalle"; 

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<CalleDtm, CalleDto>()
                    .ForMember(dto => dto.Municipio, dtm => dtm.MapFrom(dtm => $"({dtm.Municipio.Codigo}) {dtm.Municipio.Nombre}"))
                    .ForMember(dto => dto.Pais, dtm => dtm.MapFrom(dtm => $"({dtm.Municipio.Provincia.Pais.Codigo}) {dtm.Municipio.Provincia.Pais.Nombre}"))
                    .ForMember(dto => dto.IdPais, dtm => dtm.MapFrom(dtm => dtm.Municipio.Provincia.Pais.Id));

                CreateMap<CalleDto, CalleDtm>()
                .ForMember(dtm => dtm.FechaCreacion, dto => dto.Ignore())
                .ForMember(dtm => dtm.FechaModificacion, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdUsuaCrea, dto => dto.Ignore())
                .ForMember(dtm => dtm.IdUsuaModi, dto => dto.Ignore());

            }

        }

        public GestorDeCalles(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDeCalles Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeCalles(contexto, mapeador); ;
        }

        public static CalleDtm LeerCallePorClave(ContextoSe contexto, int idMunicipio, string codigoCalle, bool paraActualizar, bool errorSiNoHay = true, bool errorSiMasDeUno = true)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            var filtro1 = new ClausulaDeFiltrado(nameof(CalleDtm.IdMunicipio), CriteriosDeFiltrado.igual, idMunicipio.ToString());
            var filtro3 = new ClausulaDeFiltrado(nameof(codigoCalle), CriteriosDeFiltrado.igual, codigoCalle);
            filtros.Add(filtro1);
            filtros.Add(filtro3);
            var p = new ParametrosDeNegocio(paraActualizar ? enumTipoOperacion.LeerConBloqueo : enumTipoOperacion.LeerSinBloqueo);
            p.Parametros.Add(ltrJoinAudt.IncluirUsuarioDtm, false);
            return gestor.LeerRegistro(filtros, p, errorSiNoHay, errorSiMasDeUno);
        }

        private static CalleDtm LeerCallePorCodigo(ContextoSe contexto, string iso2Pais, string codigoProvincia, string codigoMunicipio, string codigoCalle,bool paraActualizar,  bool errorSiNoHay = true, bool errorSiMasDeUno = true)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            var filtro1 = new ClausulaDeFiltrado(nameof(iso2Pais), CriteriosDeFiltrado.igual, iso2Pais);
            var filtro2 = new ClausulaDeFiltrado(nameof(codigoProvincia), CriteriosDeFiltrado.igual, codigoProvincia);
            var filtro3 = new ClausulaDeFiltrado(nameof(codigoMunicipio), CriteriosDeFiltrado.igual, codigoMunicipio);
            var filtro4 = new ClausulaDeFiltrado(nameof(codigoCalle), CriteriosDeFiltrado.igual, codigoCalle);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro4);
            filtros.Add(filtro3);
            var p = new ParametrosDeNegocio(paraActualizar ? enumTipoOperacion.LeerConBloqueo : enumTipoOperacion.LeerSinBloqueo);
            p.Parametros.Add(ltrJoinAudt.IncluirUsuarioDtm, false);
            List<CalleDtm> calles = gestor.LeerRegistros(0, -1, filtros, null, null, p);

            if (calles.Count == 0 && errorSiNoHay)
                GestorDeErrores.Emitir($"No se ha localizado la calle con Iso2 del pais {iso2Pais}, codigo de provincia {codigoProvincia} y código municipio {codigoMunicipio}");
            if (calles.Count > 1 && errorSiMasDeUno)
                GestorDeErrores.Emitir($"Se han localizado más de un registro con Iso2 del pais {iso2Pais}, codigo de provincia {codigoProvincia} y código municipio {codigoMunicipio}");

            return calles.Count == 1 ? calles[0] : null;
        }

        public static CalleDtm LeerCallePorNombre(ContextoSe contexto, string iso2Pais, string nombreProvincia, string nombreMunicipio, string nombreCalle, bool paraActualizar, bool errorSiNoHay = true, bool errorSiMasDeUno = true)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            var filtro1 = new ClausulaDeFiltrado(nameof(iso2Pais), CriteriosDeFiltrado.igual, iso2Pais);
            var filtro2 = new ClausulaDeFiltrado(nameof(nombreProvincia), CriteriosDeFiltrado.igual, nombreProvincia);
            var filtro3 = new ClausulaDeFiltrado(nameof(nombreProvincia), CriteriosDeFiltrado.igual, nombreMunicipio);
            var filtro4 = new ClausulaDeFiltrado(nameof(nombreCalle), CriteriosDeFiltrado.igual, nombreCalle);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);
            filtros.Add(filtro4);
            var p = new ParametrosDeNegocio(paraActualizar ? enumTipoOperacion.LeerConBloqueo : enumTipoOperacion.LeerSinBloqueo);
            p.Parametros.Add(ltrJoinAudt.IncluirUsuarioDtm, false);
            List<CalleDtm> calles = gestor.LeerRegistros(0, -1, filtros, null, null, p);

            if (calles.Count == 0 && errorSiNoHay)
                GestorDeErrores.Emitir($"No se ha localizado la calle con Iso2 del pais {iso2Pais}, provincia {nombreProvincia} y municipio {nombreMunicipio}");
            if (calles.Count > 1 && errorSiMasDeUno)
                GestorDeErrores.Emitir($"Se han localizado más de un registro con Iso2 del pais {iso2Pais}, provincia {nombreProvincia} y municipio {nombreMunicipio}");

            return calles.Count == 1 ? calles[0] : null;
        }

        protected override IQueryable<CalleDtm> AplicarJoins(IQueryable<CalleDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Municipio);
            registros = registros.Include(p => p.Municipio.Provincia);
            registros = registros.Include(p => p.Municipio.Provincia.Pais);
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                //if (filtro.Clausula == nameof(CpsDeUnCalleDtm.IdCp).ToLower())
                //{
                //    registros = registros.Include(p => p.Cps);
                //}
            }
            return registros;
        }

        protected override IQueryable<CalleDtm> AplicarFiltros(IQueryable<CalleDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros =  base.AplicarFiltros(registros, filtros, parametros);

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Aplicado)
                    continue;

                if (filtro.Clausula.Equals(nameof(CalleDto.IdPais), StringComparison.CurrentCultureIgnoreCase))
                    registros = registros.Where(x => x.Municipio.Provincia.Pais.Id == filtro.AplicarFiltro().Entero());

                if (filtro.Clausula.Equals(nameof(CalleDto.IdProvincia), StringComparison.CurrentCultureIgnoreCase))
                    registros = registros.Where(x => x.Municipio.Provincia.Id == filtro.AplicarFiltro().Entero());

                if (filtro.Clausula.Equals(nameof(CalleDto.IdMunicipio), StringComparison.CurrentCultureIgnoreCase))
                    registros = registros.Where(x => x.IdMunicipio == filtro.AplicarFiltro().Entero());

                if (filtro.Clausula.Equals(ltrCallejero.iso2Pais, StringComparison.CurrentCultureIgnoreCase))
                    registros = registros.Where(x => x.Municipio.Provincia.Pais.ISO2 == filtro.AplicarFiltro());

                if (filtro.Clausula.Equals(ltrCallejero.codigoProvincia,StringComparison.CurrentCultureIgnoreCase))
                    registros = registros.Where(x => x.Municipio.Provincia.Codigo == filtro.AplicarFiltro());

                if (filtro.Clausula.Equals(ltrCallejero.codigoMunicipio, StringComparison.CurrentCultureIgnoreCase))
                    registros = registros.Where(x => x.Municipio.Codigo == filtro.AplicarFiltro());

                if (filtro.Clausula.Equals(ltrCallejero.codigoCalle, StringComparison.CurrentCultureIgnoreCase))
                    registros = registros.Where(x => x.Codigo == filtro.AplicarFiltro());

                if (filtro.Clausula.Equals(ltrCallejero.nombreProvincia, StringComparison.CurrentCultureIgnoreCase) && filtro.Criterio == CriteriosDeFiltrado.comienza)
                    registros = registros.Where(x => x.Municipio.Provincia.Nombre.StartsWith(filtro.AplicarFiltro()));

                if (filtro.Clausula.Equals(ltrCallejero.nombreMunicipio, StringComparison.CurrentCultureIgnoreCase) && filtro.Criterio == CriteriosDeFiltrado.comienza)
                    registros = registros.Where(x => x.Municipio.Nombre.StartsWith(filtro.AplicarFiltro()));

                if (filtro.Clausula.Equals(nameof(CalleDtm.Nombre), StringComparison.CurrentCultureIgnoreCase) && filtro.Criterio == CriteriosDeFiltrado.contiene)
                    registros = registros.Where(x => x.Nombre.Contains(filtro.AplicarFiltro()));

                //if (filtro.Clausula.ToLower() == nameof(CpsDeUnCalleDtm.CodigoPostal).ToLower())
                //{
                //    registros = filtro.Valor.Length == 5 
                //    ? registros.Where(x => x.Cps.Any(y => y.CodigoPostal.Codigo == filtro.Valor)) 
                //    : registros.Where(x => x.Cps.Any(y => y.CodigoPostal.Codigo.StartsWith(filtro.Valor)));
                //}
            }

            return registros;


        }

        //Todo: --> Reglas de negocio
        protected override void AntesDePersistir(CalleDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);

            if (parametros.Operacion == enumTipoOperacion.Modificar || parametros.Operacion == enumTipoOperacion.Insertar)
            {
                //Si la calle esta relacionada con CPs validar que esos Cps corresponden al municipio
            }

            if (parametros.Operacion == enumTipoOperacion.Eliminar)
            {
                //validar que no se referencia
            }

        }

        //Todo: --> Reglas de negocio
        protected override void DespuesDePersistir(CalleDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            if (parametros.Operacion == enumTipoOperacion.Modificar || parametros.Operacion == enumTipoOperacion.Insertar)
            {
                
            }
        }

        internal static void ImportarFicheroDeCalles(EntornoDeTrabajo entorno, int idArchivo)
        {
            var gestorProceso = Gestor(entorno.contextoDelProceso, entorno.contextoDelProceso.Mapeador);
            var rutaFichero = GestorDocumental.DescargarArchivo(entorno.contextoDelProceso, idArchivo, entorno.ProcesoIniciadoPorLaCola);
            var fichero = new FicheroCsv(rutaFichero);
            var linea = 1;
            entorno.CrearTraza($"Inicio del proceso");
            var trazaPrcDtm = entorno.CrearTraza($"Procesando la fila {linea}");
            var trazaInfDtm = entorno.CrearTraza($"Traza informativa del proceso");
            foreach (var fila in fichero)
            {
                var tran = gestorProceso.IniciarTransaccion();
                try
                {
                    if (fila.EnBlanco)
                        continue;

                    if (fila.Columnas != 6)
                        throw new Exception($"la fila {linea} solo debe tener 6 columnas");

                    if (fila["A"].IsNullOrEmpty() || fila["B"].IsNullOrEmpty() ||
                        fila["C"].IsNullOrEmpty() || fila["D"].IsNullOrEmpty() || 
                        fila["E"].IsNullOrEmpty() || fila["F"].IsNullOrEmpty())
                        throw new Exception($"El contenido de la fila {linea} debe ser:Iso2Pais, código de provincia, código municipio, codigo de calle, nombre de la calle, sigla del tipo de vía");

                    ProcesarCalleLeido(entorno, gestorProceso,
                        iso2Pais: fila["A"],
                        codigoProvincia: fila["B"],
                        codigoMunicipio: fila["C"],
                        codigoCalle: fila["D"],
                        nombreCalle: fila["E"],
                        siglaTipoVia: fila["F"],
                        trazaInfDtm);
                    gestorProceso.Commit(tran);
                }
                catch (Exception e)
                {
                    gestorProceso.Rollback(tran);
                    entorno.AnotarError($"Error al procesar la fila {linea}", e);
                }
                finally
                {
                    entorno.ActualizarTraza(trazaPrcDtm, $"Procesando la fila {linea}");
                    linea++;
                }
            }

            entorno.CrearTraza($"Procesadas un total de {linea} filas");

        }

        private static CalleDtm ProcesarCalleLeido(EntornoDeTrabajo entorno, GestorDeCalles gestorProceso, string iso2Pais, string codigoProvincia, string codigoMunicipio, string codigoCalle, string nombreCalle, string siglaTipoVia, TrazaDeUnTrabajoDtm trazaInfDtm)
        {
            ParametrosDeNegocio operacion;
            var calleDtm = LeerCallePorCodigo(gestorProceso.Contexto, iso2Pais, codigoProvincia, codigoMunicipio, codigoCalle, paraActualizar: false, errorSiNoHay: false);
            var tipoViaDtm = gestorProceso.Contexto.Set<TipoDeViaDtm>().LeerCacheadoPorPropiedad(nameof(TipoDeViaDtm.Sigla), siglaTipoVia);
            if (calleDtm == null) 
            {
                var municipioDtm = GestorDeMunicipios.LeerMunicipioPorCodigo(gestorProceso.Contexto, iso2Pais, codigoProvincia, codigoMunicipio, paraActualizar: false);

                calleDtm = new CalleDtm();
                calleDtm.IdMunicipio = municipioDtm.Id;
                calleDtm.Codigo = codigoCalle;
                calleDtm.Nombre = nombreCalle;
                calleDtm.IdTipoVia = tipoViaDtm.Id;
                operacion = new ParametrosDeNegocio(enumTipoOperacion.Insertar);
                entorno.ActualizarTraza(trazaInfDtm, $"Creando la calle {nombreCalle}");
            }
            else
            {
                if (calleDtm.Nombre != nombreCalle || calleDtm.IdTipoVia != tipoViaDtm.Id)
                {
                    calleDtm.Nombre = nombreCalle;
                    calleDtm.IdTipoVia = tipoViaDtm.Id;
                    operacion = new ParametrosDeNegocio(enumTipoOperacion.Modificar);
                    calleDtm.UsuarioModificador = null;
                    entorno.ActualizarTraza(trazaInfDtm, $"Modificando la calle {nombreCalle}");
                }
                else
                {
                    entorno.ActualizarTraza(trazaInfDtm, $"la calle {nombreCalle} ya exite");
                    return calleDtm;
                }
            }
            calleDtm.Municipio = null;
            return gestorProceso.PersistirRegistro(calleDtm, operacion);

        }

    }
}
