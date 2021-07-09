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
    public class GestorDeMunicipios : GestorDeElementos<ContextoSe, MunicipioDtm, MunicipioDto>
    {

        public const string ParametroMunicipio = "csvMunicipio"; 

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<MunicipioDtm, MunicipioDto>()
                    .ForMember(dto => dto.Provincia, dtm => dtm.MapFrom(dtm => $"({dtm.Provincia.Codigo}) {dtm.Provincia.Nombre}"))
                    .ForMember(dto => dto.Pais, dtm => dtm.MapFrom(dtm => $"({dtm.Provincia.Pais.Codigo}) {dtm.Provincia.Pais.Nombre}"))
                    .ForMember(dto => dto.IdPais, dtm => dtm.MapFrom(dtm => dtm.Provincia.Pais.Id));

                CreateMap<MunicipioDto, MunicipioDtm>()
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

        public List<MunicipioDto> LeerMunicipios(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            var registros = LeerRegistrosPorNombre(posicion, cantidad, filtros);
            return MapearElementos(registros).ToList();
        }

        public static MunicipioDtm LeerMunicipioPorClave(ContextoSe contexto, int idProvincia, string codigoMunicipio, bool paraActualizar, bool errorSiNoHay = true, bool errorSiMasDeUno = true)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            var filtro1 = new ClausulaDeFiltrado(nameof(MunicipioDtm.IdProvincia), CriteriosDeFiltrado.igual, idProvincia.ToString());
            var filtro3 = new ClausulaDeFiltrado(nameof(codigoMunicipio), CriteriosDeFiltrado.igual, codigoMunicipio);
            filtros.Add(filtro1);
            filtros.Add(filtro3);
            var p = new ParametrosDeNegocio(paraActualizar ? enumTipoOperacion.LeerConBloqueo : enumTipoOperacion.LeerSinBloqueo);
            p.Parametros.Add(ltrJoinAudt.IncluirUsuarioDtm, false);
            return gestor.LeerRegistro(filtros, p, errorSiNoHay, errorSiMasDeUno);
        }

        public static MunicipioDtm LeerMunicipioPorCodigo(ContextoSe contexto, string iso2Pais, string codigoProvincia, string codigoMunicipio,bool paraActualizar,  bool errorSiNoHay = true, bool errorSiMasDeUno = true)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            var filtro1 = new ClausulaDeFiltrado(nameof(iso2Pais), CriteriosDeFiltrado.igual, iso2Pais);
            var filtro2 = new ClausulaDeFiltrado(nameof(codigoProvincia), CriteriosDeFiltrado.igual, codigoProvincia);
            var filtro3 = new ClausulaDeFiltrado(nameof(codigoMunicipio), CriteriosDeFiltrado.igual, codigoMunicipio);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);
            var p = new ParametrosDeNegocio(paraActualizar ? enumTipoOperacion.LeerConBloqueo : enumTipoOperacion.LeerSinBloqueo);
            p.Parametros.Add(ltrJoinAudt.IncluirUsuarioDtm, false);
            List<MunicipioDtm> municipios = gestor.LeerRegistros(0, -1, filtros, null, null, p);

            if (municipios.Count == 0 && errorSiNoHay)
                GestorDeErrores.Emitir($"No se ha localizado el municipio con Iso2 del pais {iso2Pais}, codigo de provincia {codigoProvincia} y código municipio {codigoMunicipio}");
            if (municipios.Count > 1 && errorSiMasDeUno)
                GestorDeErrores.Emitir($"Se han localizado más de un registro con Iso2 del pais {iso2Pais}, codigo de provincia {codigoProvincia} y código municipio {codigoMunicipio}");

            return municipios.Count == 1 ? municipios[0] : null;
        }

        public static MunicipioDtm LeerMunicipioPorNombre(ContextoSe contexto, string iso2Pais, string nombreProvincia, string nombreMunicipio, bool paraActualizar, bool errorSiNoHay = true, bool errorSiMasDeUno = true)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            var filtro1 = new ClausulaDeFiltrado(nameof(iso2Pais), CriteriosDeFiltrado.igual, iso2Pais);
            var filtro2 = new ClausulaDeFiltrado(nameof(nombreProvincia), CriteriosDeFiltrado.igual, nombreProvincia);
            var filtro3 = new ClausulaDeFiltrado(nameof(nombreMunicipio), CriteriosDeFiltrado.igual, nombreMunicipio);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);
            var p = new ParametrosDeNegocio(paraActualizar ? enumTipoOperacion.LeerConBloqueo : enumTipoOperacion.LeerSinBloqueo);
            p.Parametros.Add(ltrJoinAudt.IncluirUsuarioDtm, false);
            List<MunicipioDtm> municipios = gestor.LeerRegistros(0, -1, filtros, null, null, p);

            if (municipios.Count == 0 && errorSiNoHay)
                GestorDeErrores.Emitir($"No se ha localizado el municipio con Iso2 del pais {iso2Pais}, provincia {nombreProvincia} y municipio {nombreMunicipio}");
            if (municipios.Count > 1 && errorSiMasDeUno)
                GestorDeErrores.Emitir($"Se han localizado más de un registro con Iso2 del pais {iso2Pais}, provincia {nombreProvincia} y municipio {nombreMunicipio}");

            return municipios.Count == 1 ? municipios[0] : null;
        }

        protected override IQueryable<MunicipioDtm> AplicarJoins(IQueryable<MunicipioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Provincia);
            registros = registros.Include(p => p.Provincia.Pais);
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula == nameof(CpsDeUnMunicipioDtm.IdCp).ToLower())
                {
                    registros = registros.Include(p => p.Cps);
                }
            }
            return registros;
        }

        protected override IQueryable<MunicipioDtm> AplicarFiltros(IQueryable<MunicipioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros =  base.AplicarFiltros(registros, filtros, parametros);

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.Equals(nameof(MunicipioDto.IdProvincia), StringComparison.CurrentCultureIgnoreCase))
                    registros = registros.Where(x => x.Provincia.Id == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(MunicipioDto.IdPais).ToLower())
                    registros = registros.Where(x => x.Provincia.Pais.Id == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == "iso2Pais".ToLower())
                    registros = Filtrar.AplicarFiltroDeCadena(registros, filtro, "Provincia.Pais.ISO2");

                if (filtro.Clausula.ToLower() == "codigoProvincia".ToLower())
                    registros = Filtrar.AplicarFiltroDeCadena(registros, filtro, "Provincia.Codigo");

                if (filtro.Clausula.ToLower() == "codigoMunicipio".ToLower())
                    registros = Filtrar.AplicarFiltroDeCadena(registros, filtro, nameof(MunicipioDtm.Codigo));

                if (filtro.Clausula.ToLower() == "nombreProvincia".ToLower())
                    registros = Filtrar.AplicarFiltroDeCadena(registros, filtro, "Provincia.Nombre");

                if (filtro.Clausula.ToLower() == "nombreMunicipio".ToLower())
                    registros = Filtrar.AplicarFiltroDeCadena(registros, filtro, nameof(MunicipioDtm.Nombre));

                if (filtro.Clausula.ToLower() == nameof(CpsDeUnMunicipioDtm.CodigoPostal).ToLower())
                {
                    registros = filtro.Valor.Length == 5 
                    ? registros.Where(x => x.Cps.Any(y => y.CodigoPostal.Codigo == filtro.Valor)) 
                    : registros.Where(x => x.Cps.Any(y => y.CodigoPostal.Codigo.StartsWith(filtro.Valor)));
                }
            }

            return registros;


        }

        //Todo: --> Reglas de negocio
        protected override void AntesDePersistir(MunicipioDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);

            if (parametros.Operacion == enumTipoOperacion.Modificar || parametros.Operacion == enumTipoOperacion.Insertar)
            {

                //Obtener el código de la provincia del municipio

                //ver si el municipio está relacionado con códigos postales

                //si lo está, validar que los dos primeros dígitos del código postal corresponden con el código de la provincia
            }

            if (parametros.Operacion == enumTipoOperacion.Eliminar)
            {
                //Validar que no hay calles relacionadas con el municipio

                //Eliminar los CPS relacionados con el municipio

            }

        }

        //Todo: --> Reglas de negocio
        protected override void DespuesDePersistir(MunicipioDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            if (parametros.Operacion == enumTipoOperacion.Modificar || parametros.Operacion == enumTipoOperacion.Insertar)
            {
                //Si la provincia del municipio no está relacionada con el cp, relacionarla
            }
        }

        internal static void ImportarFicheroDeMunicipios(EntornoDeTrabajo entorno, int idArchivo)
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

                    if (fila.Columnas != 5)
                        throw new Exception($"la fila {linea} solo debe tener 5 columnas");

                    if (fila["A"].IsNullOrEmpty() || fila["B"].IsNullOrEmpty() ||
                        fila["C"].IsNullOrEmpty() || fila["D"].IsNullOrEmpty() || 
                        fila["E"].IsNullOrEmpty())
                        throw new Exception($"El contenido de la fila {linea} debe ser: código de provincia, código municipio, DC, nombre del municipio");

                    ProcesarMunicipioLeido(entorno, gestorProceso,
                        iso2Pais: fila["A"],
                        codigoProvincia: fila["B"],
                        codigoMunicipio: fila["C"],
                        DC: fila["D"],
                        nombreMunicipio: fila["E"],
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

        private static MunicipioDtm ProcesarMunicipioLeido(EntornoDeTrabajo entorno, GestorDeMunicipios gestorProceso, string iso2Pais, string codigoProvincia, string codigoMunicipio, string DC, string nombreMunicipio, TrazaDeUnTrabajoDtm trazaInfDtm)
        {
            ParametrosDeNegocio operacion;
            var municipioDtm = LeerMunicipioPorCodigo(gestorProceso.Contexto, iso2Pais, codigoProvincia, codigoMunicipio, paraActualizar: false, errorSiNoHay: false);
            if (municipioDtm == null) 
            {
                var provinciaDtm = GestorDeProvincias.LeerProvinciaPorCodigo(gestorProceso.Contexto, iso2Pais, codigoProvincia, paraActualizar: false);
                
                municipioDtm = new MunicipioDtm();
                municipioDtm.IdProvincia = provinciaDtm.Id;
                municipioDtm.Codigo = codigoMunicipio;
                municipioDtm.Nombre = nombreMunicipio;
                municipioDtm.DC = DC;
                operacion = new ParametrosDeNegocio(enumTipoOperacion.Insertar);
                entorno.ActualizarTraza(trazaInfDtm, $"Creando el municipio {nombreMunicipio}");
            }
            else
            {
                if (municipioDtm.Nombre != nombreMunicipio || municipioDtm.Codigo != codigoMunicipio || municipioDtm.DC != DC )
                {
                    municipioDtm.Nombre = nombreMunicipio;
                    municipioDtm.DC = DC;
                    municipioDtm.Codigo = codigoMunicipio;
                    operacion = new ParametrosDeNegocio(enumTipoOperacion.Modificar);
                    municipioDtm.UsuarioModificador = null;
                    entorno.ActualizarTraza(trazaInfDtm, $"Modificando el municipio {nombreMunicipio}");
                }
                else
                {
                    entorno.ActualizarTraza(trazaInfDtm, $"el municipio {nombreMunicipio} ya exite");
                    return municipioDtm;
                }
            }
            municipioDtm.Provincia = null;
            return gestorProceso.PersistirRegistro(municipioDtm, operacion);

        }

    }
}
