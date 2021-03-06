﻿using AutoMapper;
using Gestor.Errores;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto;
using ModeloDeDto.Entorno;
using ModeloDeDto.Negocio;
using Newtonsoft.Json;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Negocio;
using ServicioDeDatos.Seguridad;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;
using Utilidades;

namespace GestorDeElementos
{
    public enum enumTipoOperacion { Insertar, Modificar, LeerConBloqueo, LeerSinBloqueo, NoDefinida, Eliminar, Contar };

    public static class TipoOperacion
    {
        public static enumTipoOperacion ToTipoOperacion(this object tipo)
        {
            switch (tipo.ToString())
            {
                case nameof(enumTipoOperacion.Insertar): return enumTipoOperacion.Insertar;
                case nameof(enumTipoOperacion.Modificar): return enumTipoOperacion.Modificar;
                case nameof(enumTipoOperacion.LeerConBloqueo): return enumTipoOperacion.LeerConBloqueo;
                case nameof(enumTipoOperacion.LeerSinBloqueo): return enumTipoOperacion.LeerSinBloqueo;
                case nameof(enumTipoOperacion.NoDefinida): return enumTipoOperacion.NoDefinida;
                case nameof(enumTipoOperacion.Eliminar): return enumTipoOperacion.Eliminar;
                case nameof(enumTipoOperacion.Contar): return enumTipoOperacion.Contar;
            }

            throw new Exception($"No se ha definido el tipo de operación {tipo}");
        }
        public static string ToBd(this enumTipoOperacion tipo)
        {
            switch (tipo)
            {
                case enumTipoOperacion.Insertar: return "I";
                case enumTipoOperacion.Modificar: return "M";
                case enumTipoOperacion.LeerConBloqueo: return "L";
                case enumTipoOperacion.LeerSinBloqueo: return "X";
                case enumTipoOperacion.NoDefinida: return "N";
                case enumTipoOperacion.Eliminar: return "E";
                case enumTipoOperacion.Contar: return "C";
            }

            throw new Exception($"No se ha definidocomo registrar en la BD la operación {tipo}");
        }
    }

    #region Extensiones para filtrar, hacer joins y ordenar
    public class ClausulaDeJoin
    {
        public Type Dtm { get; set; }
    }

    #endregion

    #region Extensiones a pasar a las operaciones a realizar


    public class EnumParametro
    {
        public static string accion = nameof(accion);
    }


    public class ParametrosDeNegocio
    {
        public enumTipoOperacion Operacion { get; private set; }
        public bool LeerParaActualizar { get; set; } = false;

        public bool AplicarJoin { get; }

        public Dictionary<string, object> Parametros = new Dictionary<string, object>();
        public IRegistro registroEnBd = null;

        public ParametrosDeNegocio(enumTipoOperacion tipo, bool aplicarJoin = true)
        {
            Operacion = tipo;

            if (tipo == enumTipoOperacion.LeerConBloqueo)
                LeerParaActualizar = true;

            if (tipo == enumTipoOperacion.LeerSinBloqueo)
                LeerParaActualizar = false;

            AplicarJoin = aplicarJoin;
        }
    }

    public class ParametrosDeMapeo
    {
        public bool AnularMapeo = false;
        public Dictionary<string, object> Opciones = new Dictionary<string, object>();
    }

    #endregion

    public interface IGestor
    {
        //public IEnumerable<TElemento> LeerElementos<TElemento>(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden, Dictionary<string, object> opcionesDeMapeo);
    }


    public class GestorDeElementos<TContexto, TRegistro, TElemento> : IGestor, IDisposable
        where TRegistro : Registro
        where TElemento : ElementoDto
        where TContexto : ContextoSe
    {
        public TContexto Contexto;
        public IMapper Mapeador => Contexto.Mapeador;

        private static readonly ConcurrentDictionary<string, bool> _CacheDeRecuentos = new ConcurrentDictionary<string, bool>();

        //private TRegistro RegistroEnBD { get; set; }

        public bool HayFiltroPorId { get; private set; } = false;

        public string ClaseDto => typeof(TElemento).Name;
        public string ClaseDtm => typeof(TRegistro).Name;

        public GestorDeElementos(TContexto contexto, IMapper mapeador)
        : this(contexto)
        {
            if (mapeador == null)
                throw new Exception("Falta definir el mapeador");

            Contexto.Mapeador = mapeador;
        }

        public GestorDeElementos(TContexto contexto)
        {
            Contexto = contexto;
        }


        public GestorDeElementos(Func<TContexto> generadorDeContexto, IMapper mapeador)
        : this(generadorDeContexto(), mapeador)
        {
        }

        #region ASYNC

        #region Métodos de inserción ASYN

        public async Task InsertarElementoAsync(TElemento elemento, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(enumTipoOperacion.Insertar);

            TRegistro elementoBD = MapearRegistro(elemento, parametros);
            Contexto.Add(elementoBD);
            await Contexto.SaveChangesAsync();
        }

        #endregion

        #region Métodos de modificación

        public async Task ModificarElementoAsync(TElemento elemento, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(enumTipoOperacion.Modificar);

            TRegistro registro = MapearRegistro(elemento, parametros);
            await ModificarRegistroAsync(registro);
        }

        protected async Task ModificarRegistroAsync(TRegistro registro, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(enumTipoOperacion.Modificar);

            Contexto.Update(registro);
            await Contexto.SaveChangesAsync();
        }

        #endregion

        #endregion

        #region Métodos de persistencia

        public bool IniciarTransaccion()
        {
            return Contexto.IniciarTransaccion();
        }

        public void Rollback(bool transaccion)
        {
            Contexto.Rollback(transaccion);
        }
        public void Commit(bool transaccion)
        {
            Contexto.Commit(transaccion);
        }

        public void PersistirElementosDto(List<TElemento> elementosDto, ParametrosDeNegocio parametros)
        {
            foreach (var elementoDto in elementosDto)
                PersistirElementoDto(elementoDto, parametros);
        }

        public void PersistirElementoDto(TElemento elementoDto, ParametrosDeNegocio parametros)
        {
            TRegistro registro = MapearRegistro(elementoDto, parametros);
            PersistirRegistro(registro, parametros);
            elementoDto.Id = registro.Id;
        }


        protected void PersistirRegistros(List<TRegistro> registros, ParametrosDeNegocio parametros)
        {
            var transaccion = Contexto.IniciarTransaccion();
            try
            {
                foreach (var registro in registros)
                {
                    PersistirRegistro(registro, parametros);
                }

                Contexto.Commit(transaccion);
            }
            catch (Exception)
            {
                Contexto.Rollback(transaccion);
                throw;
            }
        }

        public TRegistro PersistirRegistro(TRegistro registro, ParametrosDeNegocio parametros)
        {
            if (parametros.Operacion != enumTipoOperacion.Insertar)
                parametros.registroEnBd = LeerRegistroPorId(registro.Id, false, false, false, aplicarJoin: false);

            var transaccion = Contexto.IniciarTransaccion();
            try
            {
                AntesDePersistir(registro, parametros);

                if (parametros.Operacion == enumTipoOperacion.Insertar)
                    Contexto.Add(registro);
                else
                if (parametros.Operacion == enumTipoOperacion.Modificar)
                    Contexto.Update(registro);
                else
                if (parametros.Operacion == enumTipoOperacion.Eliminar)
                    Contexto.Remove(registro);
                else
                    throw new Exception($"Solo se pueden persistir operaciones del tipo {enumTipoOperacion.Insertar} o  {enumTipoOperacion.Modificar} o {enumTipoOperacion.Eliminar}");

                Contexto.SaveChanges();

                if (Auditoria.ImplementaAuditoria(typeof(TRegistro)))
                {
                    var negocio = NegociosDeSe.NegocioDeUnDtm(typeof(TRegistro).FullName);
                    var auditar = parametros.Operacion == enumTipoOperacion.Modificar ? parametros.registroEnBd : registro;
                    AuditoriaDeElementos.RegistrarAuditoria(Contexto, negocio, parametros.Operacion, (IElementoDtm)auditar);
                }

                DespuesDePersistir(registro, parametros);
                Contexto.Commit(transaccion);
            }
            catch (Exception)
            {
                Contexto.Rollback(transaccion);
                throw;
            }
            return registro;
        }

        public virtual void BorrarRegistros(IQueryable<TRegistro> registros)
        {
            var transaccion = Contexto.IniciarTransaccion();
            try
            {
                Contexto.RemoveRange(registros);
                Contexto.SaveChanges();
                Contexto.Commit(transaccion);
            }
            catch (Exception)
            {
                Contexto.Rollback(transaccion);
                throw;
            }
            finally
            {
                ServicioDeCaches.EliminarCache(typeof(TRegistro).FullName);
                ServicioDeCaches.EliminarCache($"{typeof(TRegistro).FullName}-ak");
            }
        }


        protected virtual void DespuesDePersistir(TRegistro registro, ParametrosDeNegocio parametros)
        {
            var indice = typeof(TRegistro).FullName;
            _CacheDeRecuentos[indice] = true;

            var propiedades = registro.PropiedadesDelObjeto();
            foreach (var propiedad in propiedades)
            {
                if (typeof(TRegistro).ImplementaNombre() && propiedad.Name == nameof(INombre.Nombre))
                {
                    ServicioDeCaches.EliminarElemento(typeof(TRegistro).FullName, $"{nameof(INombre.Nombre)}-{registro.ValorPropiedad(nameof(INombre.Nombre))}-1");
                    ServicioDeCaches.EliminarElemento(typeof(TRegistro).FullName, $"{nameof(INombre.Nombre)}-{registro.ValorPropiedad(nameof(INombre.Nombre))}-0");
                }

                if (propiedad.Name == nameof(registro.Id))
                {
                    ServicioDeCaches.EliminarElemento(typeof(TRegistro).FullName, $"{nameof(registro.Id)}-{registro.Id}-1");
                    ServicioDeCaches.EliminarElemento(typeof(TRegistro).FullName, $"{nameof(registro.Id)}-{registro.Id}-0");
                }
            }

            ServicioDeCaches.EliminarCache($"{typeof(TRegistro).FullName}-ak");
        }

        protected virtual void AntesDePersistir(TRegistro registro, ParametrosDeNegocio parametros)
        {
            AntesDePersistirValidarRegistro(registro, parametros);

            if (registro.ImplementaUnElemento())
            {
                var elemento = (IElementoDtm)registro;
                if (parametros.Operacion == enumTipoOperacion.Insertar)
                {
                    elemento.IdUsuaCrea = Contexto.DatosDeConexion.IdUsuario;
                    elemento.FechaCreacion = DateTime.Now;
                }
                else
                if (parametros.Operacion == enumTipoOperacion.Modificar)
                {
                    elemento.IdUsuaCrea = ((IElementoDtm)parametros.registroEnBd).IdUsuaCrea;
                    elemento.FechaCreacion = ((IElementoDtm)parametros.registroEnBd).FechaCreacion;
                    elemento.IdUsuaModi = Contexto.DatosDeConexion.IdUsuario;
                    elemento.FechaModificacion = DateTime.Now;
                }
            }
        }

        protected virtual void AntesDePersistirValidarRegistro(TRegistro registro, ParametrosDeNegocio parametros)
        {
            var negocio = NegociosDeSe.NegocioDeUnDtm(registro.GetType().FullName);

            if (!Contexto.DatosDeConexion.CreandoModelo && (!parametros.Parametros.ContainsKey(NegociosDeSe.ValidarSeguridad) || (bool)parametros.Parametros[NegociosDeSe.ValidarSeguridad]))
                ValidarPermisosDePersistencia(parametros.Operacion, negocio, registro);

            if ((parametros.Operacion == enumTipoOperacion.Insertar || parametros.Operacion == enumTipoOperacion.Modificar) && registro.ImplementaNombre())
            {
                var propiedades = registro.PropiedadesDelObjeto();
                foreach (var propiedad in propiedades)
                {
                    if (propiedad.Name == nameof(INombre.Nombre))
                    {
                        if (((string)propiedad.GetValue(registro)).IsNullOrEmpty())
                            GestorDeErrores.Emitir($"El nombre del objeto {typeof(TRegistro).Name} es obligatorio");
                        break;
                    }
                }
            }

            if (parametros.Operacion == enumTipoOperacion.Modificar || parametros.Operacion == enumTipoOperacion.Eliminar)
            {
            }
        }

        #endregion

        #region Métodos de lectura

        public TElemento LeerElementoPorId(int id, Dictionary<string, object> opcionesDelMapeo = null)
        {
            TRegistro elementoDtm;
            var parametros = opcionesDelMapeo == null ? new ParametrosDeMapeo() : new ParametrosDeMapeo() { Opciones = opcionesDelMapeo };

            elementoDtm = LeerRegistroPorId(id, true, false, false, aplicarJoin: true);
            return MapearElemento(elementoDtm, parametros);
        }

        public IEnumerable<TElemento> LeerElementos(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden, Dictionary<string, object> opcionesDeMapeo)
        {
            if (!opcionesDeMapeo.ContainsKey(nameof(ParametrosDeNegocio.Operacion)))
                opcionesDeMapeo.Add(nameof(ParametrosDeNegocio.Operacion), enumTipoOperacion.LeerSinBloqueo.ToString());

            var to = opcionesDeMapeo[nameof(ParametrosDeNegocio.Operacion)].ToTipoOperacion();
            var aplicarJoin = opcionesDeMapeo.ContainsKey(nameof(ParametrosDeNegocio.AplicarJoin)) ? (bool)opcionesDeMapeo[nameof(ParametrosDeNegocio.AplicarJoin)] : true; 
            var p = new ParametrosDeNegocio(to,  aplicarJoin);

            List<TRegistro> elementosDeBd = LeerRegistros(posicion, cantidad, filtros, orden, null, p);

            ParametrosDeMapeo parametrosDelMapeo = opcionesDeMapeo.Count > 0 ? new ParametrosDeMapeo() { Opciones = opcionesDeMapeo } : null;

            return MapearElementos(elementosDeBd, parametrosDelMapeo);
        }

        public List<TElemento> ProyectarElementos(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden, ParametrosDeNegocio parametros = null)
        {
            IQueryable<TRegistro> registros = DefinirConsulta(posicion, cantidad, filtros, orden, null, parametros);

            return Mapeador.ProjectTo<TElemento>(registros).AsNoTracking().ToList();
        }

        public TRegistro LeerRegistroPorId(int? id, bool usarLaCache, bool traqueado, bool conBloqueo, bool aplicarJoin)
        {
            if (!usarLaCache)
                return LeerRegistro(nameof(IRegistro.Id), id.ToString(), errorSiNoHay: true, errorSiHayMasDeUno: true, traqueado, conBloqueo, aplicarJoin);

            return LeerRegistroCacheado(nameof(IRegistro.Id), id.ToString(), errorSiNoHay: true, errorSiHayMasDeUno: true, aplicarJoin);
        }


        public TRegistro LeerRegistroCacheado(List<ClausulaDeFiltrado> filtros, bool apliacarJoin, bool errorSiNoHay = true, bool errorSiHayMasDeUno = true)
        {
            string indice = "";
            foreach (var filtro in filtros)
                indice = indice.IsNullOrEmpty() ? filtro.Clausula : $"{indice}-{filtro.Clausula}";
            var cache = ServicioDeCaches.Obtener($"{typeof(TRegistro).FullName}-ak");
            if (!cache.ContainsKey(indice))
            {
                var registros = LeerRegistros(0, -1, filtros, null, null, new ParametrosDeNegocio(enumTipoOperacion.LeerSinBloqueo, apliacarJoin));

                if (errorSiNoHay && registros.Count == 0)
                    GestorDeErrores.Emitir($"No se ha localizado el registro solicitada para el filtro proporcionado");

                if (errorSiHayMasDeUno && registros.Count > 1)
                    GestorDeErrores.Emitir($"Hay más de un registro para el filtro proporcionado");

                if (registros.Count == 0)
                    return null;

                if (registros.Count > 1)
                    return registros[0];

                cache[indice] = registros[0];
            }
            return (TRegistro)cache[indice];
        }
        public TRegistro LeerRegistroCacheado(string propiedad, string valor, bool errorSiNoHay, bool errorSiHayMasDeUno, bool aplicarJoin)
        {
            var indice = $"{propiedad}-{valor}-{(!aplicarJoin ? "0" : "1")}";
            var cache = ServicioDeCaches.Obtener(typeof(TRegistro).FullName);
            if (!cache.ContainsKey(indice))
            {
                var a = LeerRegistro(propiedad, valor, errorSiNoHay, errorSiHayMasDeUno, traqueado: false, conBloqueo: false, aplicarJoin);
                if (a == null)
                    return null;

                cache[indice] = a;
            }
            return (TRegistro)cache[indice];
        }

        public TRegistro LeerRegistro(string propiedad, string valor, bool errorSiNoHay, bool errorSiHayMasDeUno, bool traqueado, bool conBloqueo, bool aplicarJoin)
        {
            List<TRegistro> registros = LeerRegistroInterno(propiedad, valor, traqueado, conBloqueo, aplicarJoin);

            if (errorSiNoHay && registros.Count == 0)
                GestorDeErrores.Emitir($"No se ha localizado el registro solicitada para el valor {valor} en la clase {typeof(TRegistro).Name}");

            if (errorSiHayMasDeUno && registros.Count > 1)
                GestorDeErrores.Emitir($"Hay más de un registro para el valor {valor} en la clase {typeof(TRegistro).Name}");

            if (registros.Count == 0)
                return null;

            if (registros.Count > 1)
                return registros[0];

            return registros[0];
        }

        public TRegistro LeerRegistro(List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros, bool errorSiNoHay, bool errorSiHayMasDeUno)
        {
            List<TRegistro> registros = LeerRegistros(0, -1, filtros, null, null, parametros);

            if (errorSiNoHay && registros.Count == 0)
                GestorDeErrores.Emitir($"No se ha localizado el registro solicitada para los filtros indicados en la clase {typeof(TRegistro).Name}");

            if (errorSiHayMasDeUno && registros.Count > 1)
                GestorDeErrores.Emitir($"Hay más de un registro para los filtros indicados en la clase {typeof(TRegistro).Name}");

            if (registros.Count == 0)
                return null;

            if (registros.Count > 1)
                return registros[0];

            return registros[0];
        }

        private List<TRegistro> LeerRegistroInterno(string propiedad, string valor, bool traqueado, bool ConBloqueo, bool aplicarJoin)
        {
            var filtro = new ClausulaDeFiltrado()
            {
                Criterio = CriteriosDeFiltrado.igual,
                Clausula = propiedad,
                Valor = valor
            };
            var filtros = new List<ClausulaDeFiltrado>() { filtro };

            var parametros = new ParametrosDeNegocio(ConBloqueo ? enumTipoOperacion.LeerConBloqueo : enumTipoOperacion.LeerSinBloqueo, aplicarJoin);
            IQueryable<TRegistro> registros = DefinirConsulta(0, -1, filtros, null, null, parametros);

            if (!traqueado)
                return registros.AsNoTracking().ToList();

            return registros.ToList();
        }


        public List<TRegistro> LeerRegistrosPorNombre(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros = null, ParametrosDeNegocio parametros = null)
        {
            if (!typeof(TRegistro).ImplementaNombre())
                throw new Exception($"se ha solicitado leer registros por nombre, el tipo {typeof(TRegistro).Name} no tiene dicho campo");

            List<ClausulaDeOrdenacion> orden = new List<ClausulaDeOrdenacion>();
            orden.Add(new ClausulaDeOrdenacion() { OrdenarPor = nameof(INombre.Nombre), Modo = ModoDeOrdenancion.ascendente });

            if (parametros ==null) parametros = new ParametrosDeNegocio(enumTipoOperacion.LeerSinBloqueo,aplicarJoin: false);
               
            return LeerRegistros(posicion, cantidad, filtros, orden,null, parametros);
        }

        public List<TRegistro> LeerRegistros(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros = null, List<ClausulaDeOrdenacion> orden = null, List<ClausulaDeJoin> joins = null, ParametrosDeNegocio parametros = null)
        {
            List<TRegistro> elementosDeBd;
            if (parametros == null)
                parametros = new ParametrosDeNegocio(enumTipoOperacion.LeerSinBloqueo);

            IQueryable<TRegistro> registros = DefinirConsulta(posicion, cantidad, filtros, orden, joins, parametros);
            if (!Contexto.HayTransaccion && parametros.Operacion == enumTipoOperacion.LeerSinBloqueo)
            {
                var transactionOptions = new System.Transactions.TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;
                using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, transactionOptions))
                {
                    elementosDeBd = parametros.LeerParaActualizar ? registros.ToList() : registros.AsNoTracking().ToList();
                    transactionScope.Complete();
                }
            }
            else
                elementosDeBd = parametros.LeerParaActualizar ? registros.ToList() : registros.AsNoTracking().ToList();

            return elementosDeBd;
        }

        public TRegistro LeerUltimoRegistro(List<ClausulaDeFiltrado> filtros = null, List<ClausulaDeJoin> joins = null, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(enumTipoOperacion.LeerSinBloqueo);

            var orden = new ClausulaDeOrdenacion() { OrdenarPor = nameof(IRegistro.Id), Modo = ModoDeOrdenancion.descendente };

            var registros = DefinirConsulta(0, -1, filtros, new List<ClausulaDeOrdenacion> { orden }, joins, parametros);

            var registro = parametros.LeerParaActualizar ? registros.FirstOrDefault() : registros.AsNoTracking().FirstOrDefault();

            return registro;
        }

        private IQueryable<TRegistro> DefinirConsulta(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            if (joins == null)
                joins = new List<ClausulaDeJoin>();

            if (filtros == null)
                filtros = new List<ClausulaDeFiltrado>();

            IQueryable<TRegistro> registros = Contexto.Set<TRegistro>();

            if (parametros.AplicarJoin)
                registros = AplicarJoins(registros, filtros, joins, parametros);

            if (filtros.Count > 0)
            {
                registros = AplicarFiltros(registros, filtros, parametros);
                registros = registros.AplicarFiltroPorPropiedades(filtros);
            }

            if (parametros.Operacion == enumTipoOperacion.LeerSinBloqueo)
            {
                if (orden == null) orden = new List<ClausulaDeOrdenacion>();
                registros = AplicarOrden(registros, orden);
            }

            registros = registros.Skip(posicion);

            if (cantidad > 0)
            {
                registros = registros.Take(cantidad);
            }

            return registros;
        }

        /// <summary>
        /// se indican que joins se han de montar cuando se defina la consulta en función de los filtros y los parámetros de negocio
        /// </summary>
        /// <param name="filtros">filtros que se van a aplicar</param>
        /// <param name="joins">join a incluir</param>
        /// <param name="parametros">parámetros de negocio que modifican el comportamiento</param>
        protected virtual IQueryable<TRegistro> AplicarOrden(IQueryable<TRegistro> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            return registros.AplicarOrdenesBasicos(ordenacion);
        }

        protected static IQueryable<TRegistro> OrdenPorId(IQueryable<TRegistro> registros, ClausulaDeOrdenacion orden)
        {
            return orden.Modo == ModoDeOrdenancion.ascendente
                ? registros.OrderBy(x => x.Id)
                : registros.OrderByDescending(x => x.Id);
        }


        protected virtual IQueryable<TRegistro> AplicarFiltros(IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(IRegistro.Id).ToLower() && filtro.Criterio == CriteriosDeFiltrado.igual)
                {
                    HayFiltroPorId = filtro.Criterio == CriteriosDeFiltrado.igual;
                    registros = registros.Where(x => x.Id == filtro.Valor.Entero());
                    filtro.Aplicado = true;
                    return registros;
                    //return registros.AplicarFiltroPorIdentificador(filtro, nameof(IRegistro.Id));
                }
            }

            return registros;
        }

        protected virtual IQueryable<TRegistro> AplicarJoins(IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            if (ApiDeRegistro.ImplementaUnElemento(typeof(TRegistro)) && HacerJoinCon(parametros, ltrJoinAudt.IncluirUsuarioDtm))
            {
                registros = registros.Include(e => ((IElementoDtm)e).UsuarioCreador);
                registros = registros.Include(e => ((IElementoDtm)e).UsuarioModificador);
            }
            return registros;
        }

        protected bool HacerJoinCon(ParametrosDeNegocio parametros, string join)
        {
            if (!parametros.Parametros.ContainsKey(join))
                return true;
            return (bool)parametros.Parametros[join];
        }

        #endregion

        #region Métodos de acceso a BD
        public bool ExisteObjetoEnBd(int id)
        {
            return Contexto.Set<TRegistro>().Any(e => e.Id == id);
        }

        public int Contar(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins = null, ParametrosDeNegocio parametros = null)
        {

            if (parametros == null)
                parametros = new ParametrosDeNegocio(enumTipoOperacion.Contar);

            var registros = DefinirConsulta(0, -1, filtros, null, joins, parametros);
            var total = registros.Count();

            _CacheDeRecuentos[typeof(TRegistro).FullName] = false;

            return total;
        }

        public int Recontar(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins = null, ParametrosDeNegocio parametros = null)
        {
            if (!_CacheDeRecuentos.ContainsKey(typeof(TRegistro).FullName) || _CacheDeRecuentos[typeof(TRegistro).FullName])
            {
                return Contar(filtros, joins, parametros);
            }

            return 0;
        }

        #endregion

        #region Métodos de mapeo

        public List<TRegistro> MapearRegistros(List<TElemento> elementos, ParametrosDeNegocio opciones)
        {
            var registros = new List<TRegistro>();
            foreach (var elemento in elementos)
            {
                var registro = MapearRegistro(elemento, opciones);
                registros.Add(registro);
            }
            return registros;
        }

        public TRegistro MapearRegistro(TElemento elemento, ParametrosDeNegocio opciones)
        {
            var registro = Mapeador.Map<TElemento, TRegistro>(elemento,
                   opt =>
                   {
                       opt.BeforeMap((src, dest) => AntesMapearRegistro(elemento, opciones));
                       opt.AfterMap((src, dest) => DespuesDeMapearRegistro(elemento, dest, opciones));
                   }
                );

            return registro;
        }

        protected virtual void DespuesDeMapearRegistro(TElemento elemento, TRegistro registro, ParametrosDeNegocio opciones)
        {
            if (enumTipoOperacion.Insertar == opciones.Operacion)
                registro.Id = 0;
        }

        private void AntesMapearRegistro(TElemento elemento, ParametrosDeNegocio opciones)
        {

            if (opciones.Operacion == enumTipoOperacion.Insertar)
                AntesMapearRegistroParaInsertar(elemento, opciones);
            else
            if (opciones.Operacion == enumTipoOperacion.Modificar)
                AntesMapearRegistroParaModificar(elemento, opciones);
            else
            if (opciones.Operacion == enumTipoOperacion.Eliminar)
                AntesMapearRegistroParaEliminar(elemento, opciones);
        }

        protected virtual void AntesMapearRegistroParaEliminar(TElemento elemento, ParametrosDeNegocio opciones)
        {
            if (elemento.Id == 0)
                GestorDeErrores.Emitir($"No puede eliminar un elemento {typeof(TElemento).Name} con id 0");
        }

        protected virtual void AntesMapearRegistroParaModificar(TElemento elemento, ParametrosDeNegocio opciones)
        {
            if (elemento.Id == 0)
                GestorDeErrores.Emitir($"No puede modificar un elemento {typeof(TElemento).Name} con id 0");
        }

        protected virtual void AntesMapearRegistroParaInsertar(TElemento elemento, ParametrosDeNegocio opciones)
        {
            if (elemento.Id > 0)
                GestorDeErrores.Emitir($"No puede crear un elemento {typeof(TElemento).Name} con id {elemento.Id}");
        }

        public IEnumerable<TElemento> MapearElementos(List<TRegistro> registros, ParametrosDeMapeo parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeMapeo();

            var lista = new List<TElemento>();
            foreach (var registro in registros)
            {
                var elemento = MapearElemento(registro, parametros);
                if (elemento != null)
                    lista.Add(elemento);
            }
            return lista.AsEnumerable();
        }

        protected virtual void AntesDeMapearElemento(TRegistro registro, ParametrosDeMapeo parametros)
        {

        }

        public TElemento MapearElemento(TRegistro registro, ParametrosDeMapeo parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeMapeo();

            if (parametros.AnularMapeo)
                return null;

            TElemento elemento = null;
            elemento = Mapeador.Map<TRegistro, TElemento>(registro,
                opt =>
                {
                    opt.BeforeMap((registro, elemento) => AntesDeMapearElemento(registro, parametros));
                    opt.AfterMap((registro, elemento) => DespuesDeMapearElemento(registro, elemento, parametros));
                }
                );

            return elemento;
        }

        protected virtual void DespuesDeMapearElemento(TRegistro registro, TElemento elemento, ParametrosDeMapeo parametros)
        {
            if (registro.ImplementaUnElemento())
            {
                ((IAuditadoDto)elemento).CreadoEl = ((IElementoDtm)registro).FechaCreacion;
                ((IAuditadoDto)elemento).ModificadoEl = ((IElementoDtm)registro).FechaModificacion;

                ((IAuditadoDto)elemento).Creador = ((IElementoDtm)registro).UsuarioCreador == null ? "" : UsuarioDtm.NombreCompleto(((IElementoDtm)registro).UsuarioCreador);
                ((IAuditadoDto)elemento).Modificador = ((IElementoDtm)registro).UsuarioModificador == null ? "" : UsuarioDtm.NombreCompleto(((IElementoDtm)registro).UsuarioModificador);
            }
        }


        #endregion

        #region  Métodos de seguridad

        public bool ValidarPermisosDePersistencia(enumTipoOperacion operacion, enumNegocio negocio, TRegistro registro)
        {
            if (Contexto.DatosDeConexion.EsAdministrador || negocio == enumNegocio.No_Definido || !NegociosDeSe.UsaSeguridad(negocio))
                return true;

            if (!Contexto.DatosDeConexion.EsAdministrador && NegociosDeSe.EsDeParametrizacion(negocio))
                GestorDeErrores.Emitir($"El usuario {Contexto.DatosDeConexion.Login} no tiene permisos de parametrización sobre el negocio {negocio.ToNombre()}");

            var modoAcceso = LeerModoDeAccesoAlNegocio(Contexto.DatosDeConexion.IdUsuario, negocio);
            var hayPermisos = modoAcceso == enumModoDeAccesoDeDatos.Administrador;
            if (!hayPermisos)
            {
                if (operacion == enumTipoOperacion.Insertar)
                    hayPermisos = modoAcceso == enumModoDeAccesoDeDatos.Gestor;
                else
                {
                    var modoAccesoElemento = LeerModoDeAccesoAlElemento(Contexto.DatosDeConexion.IdUsuario, negocio, registro.Id);
                    hayPermisos = modoAccesoElemento == enumModoDeAccesoDeDatos.Gestor || modoAccesoElemento == enumModoDeAccesoDeDatos.Administrador;
                }
            }

            //var gestorDeNegocio = Gestores<TContexto, NegocioDtm, NegocioDto>.Obtener(Contexto, Mapeador, "Negocio.GestorDeNegocio");
            //var negocioDtm = gestorDeNegocio.LeerRegistroCacheado(nameof(NegocioDtm.Nombre), NegociosDeSe.ToString(negocio));
            //var cache = ServicioDeCaches.Obtener($"{nameof(GestorDeElementos)}.{nameof(ValidarPermisosDePersistencia)}");
            //var indice = $"Usuario:{idUsuario} Permiso:{negocioDtm.IdPermisoDeGestor}";

            //if (!cache.ContainsKey(indice))
            //{
            //    var gestorDePermisosDeUnUsuario = Gestores<TContexto, PermisosDeUnUsuarioDtm, PermisosDeUnUsuarioDto>.Obtener(Contexto, Mapeador, "Entorno.GestorDePermisosDeUnUsuario");
            //    var filtros = new List<ClausulaDeFiltrado>();
            //    filtros.Add(new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdUsuario), Criterio = CriteriosDeFiltrado.igual, Valor = idUsuario.ToString() });
            //    filtros.Add(new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdPermiso), Criterio = CriteriosDeFiltrado.esAlgunoDe, Valor = $"{negocioDtm.IdPermisoDeGestor},{negocioDtm.IdPermisoDeAdministrador}" });

            //    if (gestorDePermisosDeUnUsuario.Contar(filtros) == 0)
            //        GestorDeErrores.Emitir($"El usuario {Contexto.DatosDeConexion.Login} no tiene permisos para {operacion.ToString().ToLower()} los datos de {NegociosDeSe.ToString(negocio)}");

            //    cache[indice] = true;
            //}
            //return (bool)cache[indice];

            return hayPermisos;
        }

        public enumModoDeAccesoDeDatos LeerModoDeAccesoAlElemento(int idUsuario, enumNegocio negocio, int id)
        {
            var m = LeerModoDeAccesoAlNegocio(idUsuario, negocio);

            //analizar modo de acceso al elemento (por ahora sólo negocio)

            return m;
        }

        public enumModoDeAccesoDeDatos LeerModoDeAccesoAlNegocio(int idUsuario, enumNegocio negocio)
        {
            if (Contexto.DatosDeConexion.EsAdministrador)
                return enumModoDeAccesoDeDatos.Administrador;

            if (NegociosDeSe.EsDeParametrizacion(negocio) && !Contexto.DatosDeConexion.EsAdministrador)
                return enumModoDeAccesoDeDatos.Consultor;

            if (!NegociosDeSe.UsaSeguridad(negocio) || negocio == enumNegocio.No_Definido)
                return enumModoDeAccesoDeDatos.Administrador;

            enumModoDeAccesoDeDatos modoDelUsuario = enumModoDeAccesoDeDatos.SinPermiso;

            var cache = ServicioDeCaches.Obtener($"{nameof(GestorDeElementos)}.{nameof(LeerModoDeAccesoAlNegocio)}");
            var indice = $"Usuario:{idUsuario} Negocio:{negocio.ToNombre()}";
            if (!cache.ContainsKey(indice))
            {
                var modosLeidos = ModosDeAccesoAlNegocio(idUsuario, negocio);
                foreach (var modoLeido in modosLeidos)
                {
                    if (modoLeido.Administrador)
                    {
                        modoDelUsuario = enumModoDeAccesoDeDatos.Administrador;
                        break;
                    }
                    else
                    {

                        if (modoDelUsuario != enumModoDeAccesoDeDatos.Gestor && modoLeido.Gestor)
                            modoDelUsuario = enumModoDeAccesoDeDatos.Gestor;
                        else
                        if (modoLeido.Consultor && modoDelUsuario == enumModoDeAccesoDeDatos.SinPermiso)
                            modoDelUsuario = enumModoDeAccesoDeDatos.Consultor;
                    }
                }

                if (modoDelUsuario != enumModoDeAccesoDeDatos.SinPermiso && !NegocioActivo(negocio))
                    return enumModoDeAccesoDeDatos.Consultor;

                cache[indice] = modoDelUsuario;
            }

            return (enumModoDeAccesoDeDatos)cache[indice];
        }

        private bool NegocioActivo(enumNegocio negocio)
        {
            var gestorDeNegocio = Gestores<TContexto, NegocioDtm, NegocioDto>.Obtener(Contexto, Mapeador, $"Negocio.GestorDeNegocios");
            var negocioActivo = gestorDeNegocio.GetType().GetMethod("NegocioActivo");
            var estaActivo = (bool)negocioActivo.Invoke(gestorDeNegocio, new object[] { negocio });
            return estaActivo;
        }


        private List<ModoDeAccesoAlNegocioDtm> ModosDeAccesoAlNegocio(int idUsuario, enumNegocio negocio)
        {
            var nombreNegocio = negocio.ToNombre();

            var modosDeAcceso = Contexto
               .ModoAccesoAlNegocio
               .FromSqlInterpolated($@"
                                       SELECT ID
                                       , ADMINISTRADOR
                                       , GESTOR
                                       , CONSULTOR
                                       , IDUSUA
                                       , IDPERMISO
                                       , ORIGEN
                                       FROM NEGOCIO.MODO_ACCESO_AL_NEGOCIO_POR_USUARIO({nombreNegocio},{idUsuario})
                                      "
                                    ).ToList();
            return modosDeAcceso;
        }


        #endregion


        public string ObtenerTabla()
        {
            var entityType = Contexto.Model.FindEntityType(typeof(TRegistro));
            var schema = entityType.GetSchema();
            var tableName = entityType.GetTableName();
            return $"{schema}.{tableName}";
        }

        public void Dispose()
        {

        }

        public static class ltrJoinAudt
        {
            public static readonly string IncluirUsuarioDtm = nameof(IncluirUsuarioDtm);
        }


        #region codigo creo que obsoleto





        #endregion


    }

}

