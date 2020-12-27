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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Utilidades;

namespace GestorDeElementos
{
    public enum CriteriosDeFiltrado { igual, mayor, menor, esNulo, noEsNulo, contiene, comienza, termina, mayorIgual, menorIgual, diferente, esAlgunoDe }
    public enum ModoDeOrdenancion { ascendente, descendente }
    public enum TipoOperacion { Insertar, Modificar, Leer, NoDefinida, Eliminar, Contar };


    #region Extensiones para filtrar, hacer joins y ordenar
    public class ClausulaDeJoin
    {
        public Type Dtm { get; set; }
    }
    public class ClausulaDeFiltrado
    {
        public string Clausula { get; set; }
        public CriteriosDeFiltrado Criterio { get; set; }

        private string _valor = "";
        public string Valor { get { return _valor.Trim(); } set { _valor = value; } }
    }

    public class ClausulaDeOrdenacion
    {
        public static string PorDefecto = nameof(PorDefecto);

        public string Criterio { get; set; }
        public ModoDeOrdenancion Modo { get; set; }
        public bool TablaPrincipal { get; set; } = true;
    }

    #endregion

    #region Extensiones a pasar a las operaciones a realizar

    public enum EnumParametro { Creado }


    public class ParametrosDeNegocio
    {
        public TipoOperacion Operacion { get; private set; }
        public Dictionary<EnumParametro, object> Parametros = new Dictionary<EnumParametro, object>();

        public ParametrosDeNegocio(TipoOperacion tipo)
        {
            Operacion = tipo;
        }
    }

    public class ParametrosDeMapeo
    {
        public bool AnularMapeo = false;
        public Dictionary<string, object> parametros = new Dictionary<string, object>();
    }

    #endregion

    public class GestorDeElementos<TContexto, TRegistro, TElemento>
        where TRegistro : Registro
        where TElemento : ElementoDto
        where TContexto : ContextoSe
    {
        public TContexto Contexto;
        public IMapper Mapeador;

        private static readonly ConcurrentDictionary<string, bool> _CacheDeRecuentos = new ConcurrentDictionary<string, bool>();

        public TRegistro RegistroEnBD { get; private set; }

        protected bool InvertirMapeoDeRelacion { get; set; } = false;
        public bool HayFiltroPorId { get; private set; } = false;

        public GestorDeElementos(TContexto contexto, IMapper mapeador)
        {
            Mapeador = mapeador;

            IniciarClase(contexto);
        }


        public GestorDeElementos(Func<TContexto> generadorDeContexto, IMapper mapeador)
        : this(generadorDeContexto(), mapeador)
        {
        }

        protected virtual void IniciarClase(TContexto contexto)
        {
            Contexto = contexto;
        }

        #region ASYNC

        #region Métodos de inserción ASYN

        public async Task InsertarElementoAsync(TElemento elemento, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Insertar);

            TRegistro elementoBD = MapearRegistro(elemento, parametros);
            Contexto.Add(elementoBD);
            await Contexto.SaveChangesAsync();
        }

        #endregion

        #region Métodos de modificación

        public async Task ModificarElementoAsync(TElemento elemento, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Modificar);

            TRegistro registro = MapearRegistro(elemento, parametros);
            await ModificarRegistroAsync(registro);
        }

        protected async Task ModificarRegistroAsync(TRegistro registro, ParametrosDeNegocio parametros = null)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Modificar);

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
            if (!registro.RegistroDeRelacion)
                throw new Exception($"El registro {typeof(TRegistro)} no es de relación.");

            var filtros = new List<ClausulaDeFiltrado>();
            DefinirFiltroDeRelacion(registro, filtros, idElemento1, idElemento2);
            var registros = LeerRegistros(filtros).ToList();
            if (registros.Count != 0)
                return $"El registro {registro} ya existe";

            MapearDatosDeRelacion(registro, idElemento1, idElemento2);
            PersistirRegistro(registro, new ParametrosDeNegocio(TipoOperacion.Insertar));

            return "";
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
                if (propiedad.Name == registro.NombreDeLaPropiedadDelIdElemento1)
                    c.Valor = InvertirMapeoDeRelacion ? idElemento2.ToString() : idElemento1.ToString();
                if (propiedad.Name == registro.NombreDeLaPropiedadDelIdElemento2)
                    c.Valor = InvertirMapeoDeRelacion ? idElemento1.ToString() : idElemento2.ToString();

                if (c.Valor.Entero() > 0)
                    filtros.Add(c);
            }
        }

        protected void PersistirRegistro(TRegistro registro, ParametrosDeNegocio parametros)
        {
            var registros = new List<TRegistro> { registro };
            PersistirRegistros(registros, parametros);
        }

        protected void PersistirRegistros(List<TRegistro> registros, ParametrosDeNegocio parametros)
        {
            var transaccion = Contexto.IniciarTransaccion();
            try
            {
                foreach (var registro in registros)
                {
                    AntesDePersistir(registro, parametros);

                    if (parametros.Operacion == TipoOperacion.Insertar)
                        Contexto.Add(registro);
                    else
                    if (parametros.Operacion == TipoOperacion.Modificar)
                        Contexto.Update(registro);
                    else
                    if (parametros.Operacion == TipoOperacion.Eliminar)
                        Contexto.Remove(registro);
                    else
                        throw new Exception($"Solo se pueden persistir operaciones del tipo {TipoOperacion.Insertar} o  {TipoOperacion.Modificar} o {TipoOperacion.Eliminar}");
                    Contexto.SaveChanges();

                    DespuesDePersistir(registro, parametros);
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
                if (propiedad.Name == nameof(registro.Nombre))
                    ServicioDeCaches.EliminarElemento(typeof(TRegistro).FullName, $"{nameof(registro.Nombre)}-{registro.Nombre}");
                if (propiedad.Name == nameof(registro.Id))
                    ServicioDeCaches.EliminarElemento(typeof(TRegistro).FullName, $"{nameof(registro.Id)}-{registro.Id}");
            }

        }

        protected virtual void AntesDePersistir(TRegistro registro, ParametrosDeNegocio parametros)
        {
            AntesDePersistirValidarRegistro(registro, parametros);

            if (parametros.Operacion != TipoOperacion.Insertar)
                RegistroEnBD = LeerRegistroPorId(registro.Id);
        }
        protected virtual void AntesDePersistirValidarRegistro(TRegistro registro, ParametrosDeNegocio parametros)
        {
            ValidarPermisosDePersistencia(Contexto.DatosDeConexion.IdUsuario, parametros.Operacion, NegociosDeSe.ParsearDto(registro.GetType().Name.Replace("Dtm", "Dto")));

            if ((parametros.Operacion == TipoOperacion.Insertar || parametros.Operacion == TipoOperacion.Modificar) && registro.NombreObligatorio)
            {
                var propiedades = PropiedadesDelObjeto(registro);
                foreach (var propiedad in propiedades)
                {
                    if (propiedad.Name == nameof(registro.Nombre))
                    {
                        if (((string)propiedad.GetValue(registro)).IsNullOrEmpty())
                            GestorDeErrores.Emitir($"El nombre del objeto {typeof(TRegistro).Name} es obligatorio");
                        break;
                    }
                }
            }

            if (parametros.Operacion == TipoOperacion.Modificar || parametros.Operacion == TipoOperacion.Eliminar)
            {
            }
        }
        protected void PersistirElementoDtm(ElementoDtm elementoDtm, ParametrosDeNegocio parametros) => PersistirElementosDtm(new List<ElementoDtm> { elementoDtm }, parametros);

        protected void PersistirElementosDtm(List<ElementoDtm> elementosDtm, ParametrosDeNegocio parametros)
        {

            foreach (var elementoDtm in elementosDtm)
            {
                if (parametros.Operacion == TipoOperacion.Insertar)
                {
                    elementoDtm.IdUsuaCrea = Contexto.DatosDeConexion.IdUsuario;
                    elementoDtm.FechaCreacion = DateTime.Now;
                }
                else
                if (parametros.Operacion == TipoOperacion.Modificar)
                {
                    elementoDtm.IdUsuaModi = Contexto.DatosDeConexion.IdUsuario;
                    elementoDtm.FechaModificacion = DateTime.Now;
                }

                PersistirRegistro(elementoDtm as TRegistro, parametros);
            }
        }

        #endregion

        #region Métodos de lectura

        public IEnumerable<TElemento> LeerElementos(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden)
        {
            List<TRegistro> elementosDeBd = LeerRegistros(posicion, cantidad, filtros, orden);

            // (IEnumerable<TElemento>)Mapeador.Map(elementosDeBd, typeof(IEnumerable<TRegistro>), typeof(IEnumerable<TElemento>));

            return MapearElementos(elementosDeBd);
        }

        public List<TElemento> ProyectarElementos(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden, ParametrosDeNegocio parametros = null)
        {
            IQueryable<TRegistro> registros = DefinirConsulta(posicion, cantidad, filtros, orden, null, parametros);

            return Mapeador.ProjectTo<TElemento>(registros).AsNoTracking().ToList();
        }

        public TRegistro LeerRegistroPorId(int? id, bool usarLaCache = true)
        {
            if (!usarLaCache)
                return LeerRegistro(nameof(Registro.Id), id.ToString(), errorSiNoHay: true, errorSiHayMasDeUno: true, traqueado: !usarLaCache);

            return LeerRegistroCacheado(nameof(Registro.Id), id.ToString());
        }

        public TRegistro LeerRegistroCacheado(string propiedad, string valor, bool errorSiNoHay = true, bool errorSiHayMasDeUno = true)
        {
            var indice = $"{propiedad}-{valor}";
            var cache = ServicioDeCaches.Obtener(typeof(TRegistro).FullName);
            if (!cache.ContainsKey(indice))
            {
                var a = LeerRegistro(propiedad, valor, errorSiNoHay, errorSiHayMasDeUno, traqueado: false);
                if (a == null)
                    return null;

                cache[indice] = a;
            }
            return (TRegistro)cache[indice];
        }

        public TRegistro LeerRegistro(string propiedad, string valor, bool errorSiNoHay, bool errorSiHayMasDeUno, bool traqueado)
        {
            List<TRegistro> registros = LeerRegistroInterno(propiedad, valor, traqueado);

            if (errorSiNoHay && registros.Count == 0)
                GestorDeErrores.Emitir($"No se ha localizado el registro solicitada para el valor {valor} en la clase {typeof(TRegistro).Name}");

            if (errorSiHayMasDeUno && registros.Count > 1)
                GestorDeErrores.Emitir($"Hay más de un registro para el valor {valor} en la clase {typeof(TRegistro).Name}");

            return registros.Count == 1 ? registros[0] : null;
        }

        private List<TRegistro> LeerRegistroInterno(string propiedad, string valor, bool traqueado)
        {
            var filtro = new ClausulaDeFiltrado()
            {
                Criterio = CriteriosDeFiltrado.igual,
                Clausula = propiedad,
                Valor = valor
            };
            var filtros = new List<ClausulaDeFiltrado>() { filtro };
            IQueryable<TRegistro> registros = DefinirConsulta(0, -1, filtros, null, null, null);
            if (!traqueado)
                registros = registros.AsNoTracking();
            return registros.ToList();
        }

        public List<TRegistro> LeerRegistros(List<ClausulaDeFiltrado> filtros)
        {
            return LeerRegistros(0, 0, filtros);
        }

        public List<TRegistro> LeerRegistros(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros = null, List<ClausulaDeOrdenacion> orden = null, List<ClausulaDeJoin> joins = null, ParametrosDeNegocio parametros = null)
        {
            List<TRegistro> elementosDeBd;

            IQueryable<TRegistro> registros = DefinirConsulta(posicion, cantidad, filtros, orden, joins, parametros);

            elementosDeBd = registros.AsNoTracking().ToList();

            return elementosDeBd;
        }

        private IQueryable<TRegistro> DefinirConsulta(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> orden, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            if (parametros == null)
                parametros = new ParametrosDeNegocio(TipoOperacion.Leer);

            if (joins == null)
                joins = new List<ClausulaDeJoin>();

            if (filtros == null)
                filtros = new List<ClausulaDeFiltrado>();

            IQueryable<TRegistro> registros = Contexto.Set<TRegistro>();

            if (parametros.Operacion == TipoOperacion.Leer)
                AplicarOrdenTablaPrincipal(ref orden, ref registros);

            registros = AplicarJoins(registros, filtros, joins, parametros);

            if (filtros.Count > 0)
                registros = AplicarFiltros(registros, filtros, parametros);

            registros = registros.Skip(posicion);

            if (cantidad > 0)
            {
                registros = registros.Take(cantidad);
            }

            if (parametros.Operacion == TipoOperacion.Leer && orden.Count > 0)
                AplicarOrden(registros, orden);

            return registros;
        }

        private void AplicarOrdenTablaPrincipal(ref List<ClausulaDeOrdenacion> orden, ref IQueryable<TRegistro> registros)
        {
            if (orden == null || orden.Count == 0)
            {
                orden = new List<ClausulaDeOrdenacion>();
                orden.Add(new ClausulaDeOrdenacion { TablaPrincipal = true, Criterio = ClausulaDeOrdenacion.PorDefecto, Modo = ModoDeOrdenancion.ascendente });
            }

            List<ClausulaDeOrdenacion> ordenAntesDeJoin = new List<ClausulaDeOrdenacion>();
            for (var i = orden.Count - 1; i >= 0; i--)
            {
                var c = orden[i];
                if (c.TablaPrincipal)
                {
                    ordenAntesDeJoin.Add(new ClausulaDeOrdenacion { Criterio = c.Criterio, TablaPrincipal = true, Modo = ModoDeOrdenancion.ascendente });
                    orden.RemoveAt(i);
                }
            }

            registros = AplicarOrden(registros, ordenAntesDeJoin);
        }

        /// <summary>
        /// se indican que joins se han de montar cuando se defina la consulta en función de los filtros y los parámetros de negocio
        /// </summary>
        /// <param name="filtros">filtros que se van a aplicar</param>
        /// <param name="joins">join a incluir</param>
        /// <param name="parametros">parámetros de negocio que modifican el comportamiento</param>
        protected virtual IQueryable<TRegistro> AplicarOrden(IQueryable<TRegistro> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            foreach (var orden in ordenacion)
            {
                if (orden.Criterio.ToLower() == nameof(Registro.Id).ToLower())
                    return registros = OrdenPorId(registros, orden);

                if (orden.Criterio.ToLower() == nameof(Registro.Nombre).ToLower())
                    return registros = OrdenPorNombre(registros, orden);
            }

            return registros;
        }
        private static IQueryable<TRegistro> OrdenPorId(IQueryable<TRegistro> registros, ClausulaDeOrdenacion orden)
        {
            return orden.Modo == ModoDeOrdenancion.ascendente
                ? registros.OrderBy(x => x.Id)
                : registros.OrderByDescending(x => x.Id);
        }
        private static IQueryable<TRegistro> OrdenPorNombre(IQueryable<TRegistro> registros, ClausulaDeOrdenacion orden)
        {
            return orden.Modo == ModoDeOrdenancion.ascendente
                ? registros.OrderBy(x => x.Nombre)
                : registros.OrderByDescending(x => x.Nombre);
        }

        protected virtual IQueryable<TRegistro> AplicarFiltros(IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            return FiltrosBasicos(registros, filtros);

            //if (HayFiltroPorId(registros))
            //    return registros;

            //return registros; //.FiltrarPorNombre(filtros);
        }

        private IQueryable<TRegistro> FiltrosBasicos(IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros)
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(Registro.Id).ToLower() && filtro.Valor.Entero() > 0)
                {
                    HayFiltroPorId = true;
                    return registros.Where(x => x.Id == filtro.Valor.Entero());
                }

                if (filtro.Clausula.ToLower() == nameof(Registro.Nombre).ToLower() && !filtro.Valor.IsNullOrEmpty())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.contiene)
                        return registros.Where(x => x.Nombre.Contains(filtro.Valor));

                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        return registros.Where(x => x.Nombre == filtro.Valor);
                }

            }
            return registros;
        }

        protected virtual IQueryable<TRegistro> AplicarJoins(IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
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
                parametros = new ParametrosDeNegocio(TipoOperacion.Contar);

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

        protected TRegistro MapearRegistro(TElemento elemento, ParametrosDeNegocio opciones)
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
            if (TipoOperacion.Insertar == opciones.Operacion)
                registro.Id = 0;
        }

        private void AntesMapearRegistro(TElemento elemento, ParametrosDeNegocio opciones)
        {

            if (opciones.Operacion == TipoOperacion.Insertar)
                AntesMapearRegistroParaInsertar(elemento, opciones);
            else
            if (opciones.Operacion == TipoOperacion.Modificar)
                AntesMapearRegistroParaModificar(elemento, opciones);
            else
            if (opciones.Operacion == TipoOperacion.Eliminar)
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

            TElemento elemento = null;
            elemento = Mapeador.Map<TRegistro, TElemento>(registro,
                opt =>
                {
                    opt.BeforeMap((registro, elemento) => AntesDeMapearElemento(registro, parametros));
                    opt.AfterMap((registro, elemento) => DespuesDeMapearElemento(registro, elemento, parametros));
                }
                );

            if (parametros.AnularMapeo)
                elemento = null;

            return elemento;
        }

        protected virtual void DespuesDeMapearElemento(TRegistro registro, TElemento elemento, ParametrosDeMapeo parametros)
        {

        }

        private void MapearDatosDeRelacion(TRegistro registro, int idElemento1, int idElemento2)
        {
            var propiedades = PropiedadesDelObjeto(registro);
            foreach (var propiedad in propiedades)
            {
                if (propiedad.Name == registro.NombreDeLaPropiedadDelIdElemento1)
                    propiedad.SetValue(registro, InvertirMapeoDeRelacion ? idElemento2 : idElemento1);
                if (propiedad.Name == registro.NombreDeLaPropiedadDelIdElemento2)
                    propiedad.SetValue(registro, InvertirMapeoDeRelacion ? idElemento1 : idElemento2);
            }

            //throw new Exception($"El gestor: {this} no tiene definida la función de {nameof(MapearDatosDeRelacion)}.");
        }

        #endregion

        #region  Métodos de seguridad

        public bool ValidarPermisosDePersistencia(int idUsuario, TipoOperacion operacion, enumNegocio negocio)
        {
            if (Contexto.DatosDeConexion.EsAdministrador || !NegociosDeSe.UsaSeguridad(negocio))
                return true;

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

        public enumModoDeAcceso LeerModoDeAcceso(TElemento elemento)
        {
            var m = ModoDeAcceso(Contexto.DatosDeConexion.IdUsuario, NegociosDeSe.ParsearDto(elemento.GetType().Name));
            return m;
        }

        private enumModoDeAcceso ModoDeAcceso(int idUsuario, enumNegocio negocio)
        {
            enumModoDeAcceso modo = enumModoDeAcceso.SinAcceso;

            if (!NegociosDeSe.UsaSeguridad(negocio))
                return enumModoDeAcceso.Administrador;

            var gestorDeNegocio = Gestores<TContexto, NegocioDtm, NegocioDto>.Obtener(Contexto, Mapeador, "Negocio.GestorDeNegocio");

            var negocioActivo = gestorDeNegocio.GetType().GetMethod("NegocioActivo");
            var estaActivo = (bool)negocioActivo.Invoke(gestorDeNegocio, new object[] { negocio });

            if (Contexto.DatosDeConexion.EsAdministrador)
                return estaActivo ?
                    enumModoDeAcceso.Administrador :
                    enumModoDeAcceso.Consultor;

            var leerModoDeAccesoAlNegocio = gestorDeNegocio.GetType().GetMethod("LeerModoDeAccesoAlNegocio");
            var modosDeAcceso = (List<ModoDeAccesoAlNegocioDtm>)leerModoDeAccesoAlNegocio.Invoke(gestorDeNegocio, new object[] { negocio, idUsuario });

            foreach (var modoDeAcceso in modosDeAcceso)
            {
                if (modoDeAcceso.Administrador)
                {
                    modo = enumModoDeAcceso.Administrador;
                    break;
                }

                if (modo != enumModoDeAcceso.Gestor && modoDeAcceso.Gestor)
                    modo = enumModoDeAcceso.Gestor;
                else
                if (modoDeAcceso.Consultor)
                    modo = enumModoDeAcceso.Consultor;
            }
            return modo;
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

        public TElemento LeerElementoPorId(int id)
        {
            TRegistro elementoDeBd;
            try
            {
                elementoDeBd = LeerRegistroPorId(id);
            }
            catch (Exception e)
            {
                throw new Exception($"No existe en la base de datos un registro de {typeof(TRegistro).Name} con Id {id}", e);
            }

            return MapearElemento(elementoDeBd);
        }




        #endregion


    }

}

