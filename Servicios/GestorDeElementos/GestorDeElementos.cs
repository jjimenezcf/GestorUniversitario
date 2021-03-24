using AutoMapper;
using Gestor.Errores;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto;
using ModeloDeDto.Entorno;
using ModeloDeDto.Negocio;
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
        public Dictionary<string, object> Parametros = new Dictionary<string, object>();

        public ParametrosDeNegocio(enumTipoOperacion tipo)
        {
            Operacion = tipo;
        }
    }

    public class ParametrosDeMapeo
    {
        public bool AnularMapeo = false;
        public Dictionary<string, object> Opciones = new Dictionary<string, object>();
    }

    #endregion

    public class GestorDeElementos<TContexto, TRegistro, TElemento>
        where TRegistro : Registro
        where TElemento : ElementoDto
        where TContexto : ContextoSe
    {
        public TContexto Contexto;
        public IMapper Mapeador => Contexto.Mapeador;

        private static readonly ConcurrentDictionary<string, bool> _CacheDeRecuentos = new ConcurrentDictionary<string, bool>();

        public TRegistro RegistroEnBD { get; private set; }

        protected bool InvertirMapeoDeRelacion { get; set; } = false;
        public bool HayFiltroPorId { get; private set; } = false;

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

        #region Métodos de ayuda

        private static PropertyInfo[] PropiedadesDelObjeto(TRegistro registro)
        {
            var indice = typeof(TRegistro).FullName;
            var cache = ServicioDeCaches.Obtener(nameof(Type.GetProperties));
            if (!cache.ContainsKey(indice))
            {
                Type t = registro.GetType();
                cache[indice] = t.GetProperties();
            }
            PropertyInfo[] props = (PropertyInfo[])cache[indice];
            return props;
        }

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

        public string CrearRelacion(int idElemento1, int idElemento2)
        {
            var registro = Registro.RegistroVacio<TRegistro>();
            if (!registro.ImplementaUnaRelacion())
                throw new Exception($"El registro {typeof(TRegistro)} no es de relación.");

            var filtros = new List<ClausulaDeFiltrado>();
            DefinirFiltroDeRelacion(registro, filtros, idElemento1, idElemento2);
            var registros = ValidarAntesDeRelacionar(filtros).ToList();
            if (registros.Count != 0)
                return $"El registro {registro} ya existe";

            MapearDatosDeRelacion(registro, idElemento1, idElemento2);
            PersistirRegistro(registro, new ParametrosDeNegocio(enumTipoOperacion.Insertar));

            return "";
        }

        public List<TRegistro> ValidarAntesDeRelacionar(List<ClausulaDeFiltrado> filtros)
        {
            return LeerRegistros(0, 1, filtros, null, null, null);
        }

        private void DefinirFiltroDeRelacion(TRegistro registro, List<ClausulaDeFiltrado> filtros, int idElemento1, int idElemento2)
        {
            var propiedades = PropiedadesDelObjeto(registro);
            foreach (var propiedad in propiedades)
            {
                var c = new ClausulaDeFiltrado
                {
                    Clausula = propiedad.Name,
                    Criterio = CriteriosDeFiltrado.igual
                };

                if (propiedad.Name == registro.ValorPropiedad(nameof(IRelacion.NombreDeLaPropiedadDelIdElemento1)).ToString())
                    c.Valor = InvertirMapeoDeRelacion ? idElemento2.ToString() : idElemento1.ToString();

                if (propiedad.Name == registro.ValorPropiedad(nameof(IRelacion.NombreDeLaPropiedadDelIdElemento2)).ToString())
                    c.Valor = InvertirMapeoDeRelacion ? idElemento1.ToString() : idElemento2.ToString();

                if (c.Valor.Entero() > 0)
                    filtros.Add(c);
            }
        }

        public TRegistro PersistirRegistro(TRegistro registro, ParametrosDeNegocio parametros)
        {
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

        protected virtual void DespuesDePersistir(TRegistro registro, ParametrosDeNegocio parametros)
        {
            var indice = typeof(TRegistro).FullName;
            _CacheDeRecuentos[indice] = true;

            var propiedades = PropiedadesDelObjeto(registro);
            foreach (var propiedad in propiedades)
            {
                if (typeof(TRegistro).ImplementaNombre() && propiedad.Name == nameof(INombre.Nombre))
                    ServicioDeCaches.EliminarElemento(typeof(TRegistro).FullName, $"{nameof(INombre.Nombre)}-{registro.ValorPropiedad(nameof(INombre.Nombre))}");

                if (propiedad.Name == nameof(registro.Id))
                    ServicioDeCaches.EliminarElemento(typeof(TRegistro).FullName, $"{nameof(registro.Id)}-{registro.Id}");
            }

            ServicioDeCaches.EliminarCache($"{typeof(TRegistro).FullName}-ak");
        }

        protected virtual void AntesDePersistir(TRegistro registro, ParametrosDeNegocio parametros)
        {
            if (parametros.Operacion != enumTipoOperacion.Insertar)
                RegistroEnBD = LeerRegistroPorId(registro.Id);

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
                    elemento.IdUsuaCrea = ((IElementoDtm)RegistroEnBD).IdUsuaCrea;
                    elemento.FechaCreacion = ((IElementoDtm)RegistroEnBD).FechaCreacion;
                    elemento.IdUsuaModi = Contexto.DatosDeConexion.IdUsuario;
                    elemento.FechaModificacion = DateTime.Now;
                }
            }
        }
        protected virtual void AntesDePersistirValidarRegistro(TRegistro registro, ParametrosDeNegocio parametros)
        {
            var negocio = NegociosDeSe.ParsearDtm(registro.GetType().Name);
            ValidarPermisosDePersistencia(Contexto.DatosDeConexion.IdUsuario, parametros.Operacion, negocio);

            if ((parametros.Operacion == enumTipoOperacion.Insertar || parametros.Operacion == enumTipoOperacion.Modificar) && registro.ImplementaNombre())
            {
                var propiedades = PropiedadesDelObjeto(registro);
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

            elementoDtm = LeerRegistroPorId(id);
            return MapearElemento(elementoDtm, parametros);
        }

        public IEnumerable<TElemento> LeerElementos(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden, Dictionary<string, object> opcionesDeMapeo)
        {
            if (!opcionesDeMapeo.ContainsKey(nameof(ParametrosDeNegocio.Operacion)))
                opcionesDeMapeo.Add(nameof(ParametrosDeNegocio.Operacion), enumTipoOperacion.LeerSinBloqueo.ToString());
            var to = opcionesDeMapeo[nameof(ParametrosDeNegocio.Operacion)].ToTipoOperacion();
            var p = new ParametrosDeNegocio(to);

            List<TRegistro> elementosDeBd = LeerRegistros(posicion, cantidad, filtros, orden, null, p);

            ParametrosDeMapeo parametrosDelMapeo = opcionesDeMapeo.Count > 0 ? new ParametrosDeMapeo() { Opciones = opcionesDeMapeo } : null;

            return MapearElementos(elementosDeBd, parametrosDelMapeo);
        }

        public List<TElemento> ProyectarElementos(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden, ParametrosDeNegocio parametros = null)
        {
            IQueryable<TRegistro> registros = DefinirConsulta(posicion, cantidad, filtros, orden, null, parametros);

            return Mapeador.ProjectTo<TElemento>(registros).AsNoTracking().ToList();
        }

        public TRegistro LeerRegistroPorId(int? id, bool usarLaCache = true, bool conBloqueo = false)
        {
            if (!usarLaCache)
                return LeerRegistro(nameof(Registro.Id), id.ToString(), errorSiNoHay: true, errorSiHayMasDeUno: true, traqueado: !usarLaCache, conBloqueo);

            return LeerRegistroCacheado(nameof(Registro.Id), id.ToString());
        }


        public TRegistro LeerRegistroCacheado(List<ClausulaDeFiltrado> filtros, bool errorSiNoHay = true, bool errorSiHayMasDeUno = true)
        {
            string indice = "";
            foreach (var filtro in filtros)
                indice = indice.IsNullOrEmpty() ? filtro.Clausula : $"{indice}-{filtro.Clausula}";
            var cache = ServicioDeCaches.Obtener($"{typeof(TRegistro).FullName}-ak");
            if (!cache.ContainsKey(indice))
            {
                var registros = LeerRegistros(0, -1, filtros);

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
        public TRegistro LeerRegistroCacheado(string propiedad, string valor, bool errorSiNoHay = true, bool errorSiHayMasDeUno = true)
        {
            var indice = $"{propiedad}-{valor}";
            var cache = ServicioDeCaches.Obtener(typeof(TRegistro).FullName);
            if (!cache.ContainsKey(indice))
            {
                var a = LeerRegistro(propiedad, valor, errorSiNoHay, errorSiHayMasDeUno, traqueado: false, conBloqueo: false);
                if (a == null)
                    return null;

                cache[indice] = a;
            }
            return (TRegistro)cache[indice];
        }

        public TRegistro LeerRegistro(string propiedad, string valor, bool errorSiNoHay, bool errorSiHayMasDeUno, bool traqueado, bool conBloqueo)
        {
            List<TRegistro> registros = LeerRegistroInterno(propiedad, valor, traqueado, conBloqueo);

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

        private List<TRegistro> LeerRegistroInterno(string propiedad, string valor, bool traqueado, bool ConBloqueo)
        {
            var filtro = new ClausulaDeFiltrado()
            {
                Criterio = CriteriosDeFiltrado.igual,
                Clausula = propiedad,
                Valor = valor
            };
            var filtros = new List<ClausulaDeFiltrado>() { filtro };
            var parametros = new ParametrosDeNegocio(ConBloqueo ? enumTipoOperacion.LeerConBloqueo : enumTipoOperacion.LeerSinBloqueo);
            IQueryable<TRegistro> registros = DefinirConsulta(0, -1, filtros, null, null, parametros);
            if (!traqueado)
                registros = registros.AsNoTracking();
            return registros.ToList();
        }


        public List<TRegistro> LeerRegistrosPorNombre(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros = null)
        {
            if (!typeof(TRegistro).ImplementaNombre())
                throw new Exception($"se ha solicitado leer registros por nombre, el tipo {typeof(TRegistro).Name} no tiene dicho campo");

            List<ClausulaDeOrdenacion> orden = new List<ClausulaDeOrdenacion>();
            orden.Add(new ClausulaDeOrdenacion() { OrdenarPor = nameof(INombre.Nombre), Modo = ModoDeOrdenancion.ascendente });

            return LeerRegistros(posicion, cantidad, filtros, orden);
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
                    elementosDeBd = registros.AsNoTracking().ToList();
                    transactionScope.Complete();
                }
            }
            else
                elementosDeBd = registros.AsNoTracking().ToList();

            return elementosDeBd;
        }

        private IQueryable<TRegistro> DefinirConsulta(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            if (joins == null)
                joins = new List<ClausulaDeJoin>();

            if (filtros == null)
                filtros = new List<ClausulaDeFiltrado>();

            IQueryable<TRegistro> registros = Contexto.Set<TRegistro>();

            registros = AplicarJoins(registros, filtros, joins, parametros);

            if (filtros.Count > 0)
                registros = AplicarFiltros(registros, filtros, parametros);

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
                if (filtro.Clausula.ToLower() == nameof(Registro.Id).ToLower() && filtro.Criterio == CriteriosDeFiltrado.igual)
                {
                    HayFiltroPorId = filtro.Criterio == CriteriosDeFiltrado.igual;
                    return registros.AplicarFiltroPorIdentificador(filtro, nameof(Registro.Id));
                }
            }

            return registros.AplicarFiltroPorPropiedades(filtros);
        }

        protected virtual IQueryable<TRegistro> AplicarJoins(IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            if (RegistroExtensiones.ImplementaUnElemento(typeof(TRegistro)))
            {
                registros = registros.Include(e => ((IElementoDtm)e).UsuarioCreador);
                registros = registros.Include(e => ((IElementoDtm)e).UsuarioModificador);
            }
            return registros;
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

        private void MapearDatosDeRelacion(TRegistro registro, int idElemento1, int idElemento2)
        {
            var propiedades = PropiedadesDelObjeto(registro);
            foreach (var propiedad in propiedades)
            {
                if (propiedad.Name == registro.ValorPropiedad(nameof(IRelacion.NombreDeLaPropiedadDelIdElemento1)).ToString())
                    propiedad.SetValue(registro, InvertirMapeoDeRelacion ? idElemento2 : idElemento1);

                if (propiedad.Name == registro.ValorPropiedad(nameof(IRelacion.NombreDeLaPropiedadDelIdElemento2)).ToString())
                    propiedad.SetValue(registro, InvertirMapeoDeRelacion ? idElemento1 : idElemento2);
            }

            //throw new Exception($"El gestor: {this} no tiene definida la función de {nameof(MapearDatosDeRelacion)}.");
        }

        #endregion

        #region  Métodos de seguridad

        public bool ValidarPermisosDePersistencia(int idUsuario, enumTipoOperacion operacion, enumNegocio negocio)
        {
            if (Contexto.DatosDeConexion.EsAdministrador || negocio == enumNegocio.No_Definido || !NegociosDeSe.UsaSeguridad(negocio))
                return true;

            if (!Contexto.DatosDeConexion.EsAdministrador && NegociosDeSe.EsDeParametrizacion(negocio))
                GestorDeErrores.Emitir($"El usuario {Contexto.DatosDeConexion.Login} no tiene permisos de parametrización sobre el negocio {NegociosDeSe.ToString(negocio)}");

            var gestorDeNegocio = Gestores<TContexto, NegocioDtm, NegocioDto>.Obtener(Contexto, Mapeador, "Negocio.GestorDeNegocio");
            var negocioDtm = gestorDeNegocio.LeerRegistroCacheado(nameof(NegocioDtm.Nombre), NegociosDeSe.ToString(negocio));
            var cache = ServicioDeCaches.Obtener($"{nameof(GestorDeElementos)}.{nameof(ValidarPermisosDePersistencia)}");
            var indice = $"Usuario:{idUsuario} Permiso:{negocioDtm.IdPermisoDeGestor}";

            if (!cache.ContainsKey(indice))
            {
                var gestorDePermisosDeUnUsuario = Gestores<TContexto, PermisosDeUnUsuarioDtm, PermisosDeUnUsuarioDto>.Obtener(Contexto, Mapeador, "Entorno.GestorDePermisosDeUnUsuario");
                var filtros = new List<ClausulaDeFiltrado>();
                filtros.Add(new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdUsuario), Criterio = CriteriosDeFiltrado.igual, Valor = idUsuario.ToString() });
                filtros.Add(new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdPermiso), Criterio = CriteriosDeFiltrado.esAlgunoDe, Valor = $"{negocioDtm.IdPermisoDeGestor},{negocioDtm.IdPermisoDeAdministrador}" });

                if (gestorDePermisosDeUnUsuario.Contar(filtros) == 0)
                    GestorDeErrores.Emitir($"El usuario {Contexto.DatosDeConexion.Login} no tiene permisos para {operacion.ToString().ToLower()} los datos de {NegociosDeSe.ToString(negocio)}");

                cache[indice] = true;
            }
            return (bool)cache[indice];
        }

        public enumModoDeAccesoDeDatos LeerModoDeAccesoAlElemento(int idUsuario, enumNegocio negocio, int id)
        {
            var m = LeerModoDeAccesoAlNegocio(idUsuario, negocio);

            //analizar modo de acceso al elemento (por ahora sólo negocio)

            return m;
        }

        public enumModoDeAccesoDeDatos LeerModoDeAccesoAlNegocio(int idUsuario, enumNegocio negocio)
        {
            enumModoDeAccesoDeDatos modoDelUsuario = enumModoDeAccesoDeDatos.SinPermiso;

            if (!NegociosDeSe.UsaSeguridad(negocio) || negocio == enumNegocio.No_Definido)
                return enumModoDeAccesoDeDatos.Administrador;

            if (NegociosDeSe.EsDeParametrizacion(negocio) && !Contexto.DatosDeConexion.EsAdministrador)
                return enumModoDeAccesoDeDatos.Consultor;

            if (Contexto.DatosDeConexion.EsAdministrador)
                return enumModoDeAccesoDeDatos.Administrador;

            if (!NegocioActivo(negocio))
                return enumModoDeAccesoDeDatos.Consultor;

            var modosLeidos = ModosDeAccesoAlNegocio(idUsuario, negocio);
            foreach (var modoLeido in modosLeidos)
            {
                if (modoLeido.Administrador)
                    return enumModoDeAccesoDeDatos.Administrador;

                if (modoDelUsuario != enumModoDeAccesoDeDatos.Gestor && modoLeido.Gestor)
                    modoDelUsuario = enumModoDeAccesoDeDatos.Gestor;
                else
                if (modoLeido.Consultor)
                    modoDelUsuario = enumModoDeAccesoDeDatos.Consultor;
            }

            return modoDelUsuario;
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
            var nombreNegocio = NegociosDeSe.ToString(negocio);

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

        #region codigo creo que obsoleto





        #endregion


    }

}

