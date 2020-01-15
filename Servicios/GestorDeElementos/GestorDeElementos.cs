using AutoMapper;
using Extensiones.String;
using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.ModeloIu;
using Gestor.Errores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Gestor.Elementos
{

    public static class Auditoria
    {
        public static IQueryable<TRegistro> LogSql<TRegistro>(this IQueryable<TRegistro> consulta, ContextoDeElementos contexto, Dictionary<string, string> filtros) where TRegistro : class
        {
            var a = consulta.ToSql();
            contexto.Traza.AnotarTrazaSql(a, "--");
            return consulta;
        }
    }

    public static class IQueryableExtensions
    {
        public static string ToSql<TRegistro>(this IQueryable<TRegistro> query) where TRegistro : class
        {
            var enumerator = query.Provider.Execute<IEnumerable<TRegistro>>(query.Expression).GetEnumerator();
            var enumeratorType = enumerator.GetType();
            var selectFieldInfo = enumeratorType.GetField("_selectExpression", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException($"cannot find field _selectExpression on type {enumeratorType.Name}");
            var sqlGeneratorFieldInfo = enumeratorType.GetField("_querySqlGeneratorFactory", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException($"cannot find field _querySqlGeneratorFactory on type {enumeratorType.Name}");
            var selectExpression = selectFieldInfo.GetValue(enumerator) as SelectExpression ?? throw new InvalidOperationException($"could not get SelectExpression");
            var factory = sqlGeneratorFieldInfo.GetValue(enumerator) as IQuerySqlGeneratorFactory ?? throw new InvalidOperationException($"could not get IQuerySqlGeneratorFactory");
            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);
            var sql = command.CommandText;
            return sql;
        }
    }


    public static class Filtros
    {
        public const string FiltroPorId = "PorId";

        public static IQueryable<TRegistro> Filtro<TRegistro>(this IQueryable<TRegistro> set, Dictionary<string, string> filtros) where TRegistro : RegistroBase
        {
            if (filtros.ContainsKey(FiltroPorId))
                return set.Where(x => x.Id == filtros[FiltroPorId].Entero());

            return set;
        }
    }

    public enum Ordenacion { Ascendente, Descendente };

    public static class Ordenaciones
    {
        public const string OrdenPorId = "PorId";

        public static IQueryable<TRegistro> Orden<TRegistro>(this IQueryable<TRegistro> set, Dictionary<string, Ordenacion> orden) where TRegistro : RegistroBase
        {
            if (orden.ContainsKey(OrdenPorId))
            {
                if (orden[OrdenPorId] == Ordenacion.Ascendente)
                    return set.OrderBy(x => x.Id);

                if (orden[OrdenPorId] == Ordenacion.Descendente)
                    return set.OrderByDescending(x => x.Id);
            }

            return set;
        }
    }


    public abstract class GestorDeElementos<TContexto, TRegistro, TElemento> where TRegistro : RegistroBase where TElemento : ElementoBase where TContexto : ContextoDeElementos
    {
        protected ClaseDeElemetos<TRegistro, TElemento> Metadatos;
        public TContexto Contexto;
        private GestorDeErrores _gestorDeErrores;
        public IMapper Mapeador;

        protected abstract TRegistro LeerConDetalle(int Id);

        public GestorDeElementos(TContexto contexto, IMapper mapeador)
        {
            Mapeador = mapeador;
            IniciarClase(contexto);
        }

        public void AsignarGestores(GestorDeErrores gestorErrores)
        {
            _gestorDeErrores = gestorErrores;
        }

        protected virtual void IniciarClase(TContexto contexto)
        {
            Contexto = contexto;
            Metadatos = ClaseDeElemetos<TRegistro, TElemento>.ObtenerGestorDeLaClase();
        }

        public async Task InsertarElementoAsync(TElemento elemento)
        {
            TRegistro elementoBD = MapearRegistro(elemento);
            Contexto.Add(elementoBD);
            await Contexto.SaveChangesAsync();
        }

        public async Task ModificarElementoAsync(TElemento elemento)
        {
            TRegistro elementoBD = MapearRegistro(elemento);
            Contexto.Update(elementoBD);
            await Contexto.SaveChangesAsync();
        }

        public bool ExisteObjetoEnBd(int id)
        {
            return Contexto.Set<TRegistro>().Any(e => e.Id == id);
        }


        public (IEnumerable<TElemento>, int) Leer(int posicion, int cantidad, Dictionary<string, Ordenacion> orden)
        {
            List<TRegistro> elementosDeBd;
            var filtros = new Dictionary<string, string>();

            //filtros[Filtros.FiltroPorId] = "1";
            IQueryable<TRegistro> registros = IncluirFiltros(Contexto.Set<TRegistro>(), filtros);

            var total = registros.LogSql(Contexto, filtros).Count();

            registros = AplicarOrden(registros, orden);

            elementosDeBd = registros.Skip(posicion).Take(cantidad).AsNoTracking().ToList();
            return (MapearElementos(elementosDeBd), total);
        }

        protected virtual IQueryable<TRegistro> AplicarOrden(IQueryable<TRegistro> registros, Dictionary<string, Ordenacion> orden)
        {
            return registros.Orden(orden);
        }

        protected virtual IQueryable<TRegistro> IncluirFiltros(IQueryable<TRegistro> registros, Dictionary<string, string> filtros)
        {
            return registros.Filtro(filtros);
        }

        public (IEnumerable<TElemento>,int) LeerTodos()
        {
            var elementosDeBd = Contexto.Set<TRegistro>().AsNoTracking().ToList();
            return (MapearElementos(elementosDeBd), Contexto.Set<TRegistro>().Count());
        }

        public TElemento LeerElementoPorId(int id)
        {
            var elementoDeBd = Contexto.Set<TRegistro>().AsNoTracking().FirstOrDefault(m => m.Id == id);
            return MapearElemento(elementoDeBd);
        }

        public TRegistro LeerRegistroPorId(int id)
        {
            return Contexto.Set<TRegistro>().AsNoTracking().FirstOrDefault(m => m.Id == id);
        }


        public TElemento LeerElementoConDetalle(int id)
        {
            var elementoLeido = LeerConDetalle(id);
            return MapearElemento(elementoLeido);
        }

        public void BorrarPorId(int id)
        {
            var registro = LeerRegistroPorId(id);
            Contexto.Remove(registro);
            Contexto.SaveChangesAsync();
        }

        private TRegistro MapearRegistro(TElemento elemento)
        {
            var registro = Metadatos.NuevoElementoBd();
            PropertyInfo[] propiedadesBd = typeof(TRegistro).GetProperties();
            PropertyInfo[] propiedadesIu = typeof(TElemento).GetProperties();

            foreach (PropertyInfo pBd in propiedadesBd)
            {
                foreach (PropertyInfo pIu in propiedadesIu)
                {
                    if (pIu.Name == pBd.Name)
                    {
                        pBd.SetValue(registro, pIu.GetValue(elemento));
                        break;
                    }
                }
            }
            return registro;
        }

        private IEnumerable<TElemento> MapearElementos(List<TRegistro> registros)
        {
            var lista = new List<TElemento>();
            foreach (var registro in registros)
            {

                var elemento = MapearElemento(registro);
                lista.Add(elemento);
            }
            return lista.AsEnumerable();
        }

        public TElemento MapearElemento(TRegistro registro, List<string> excluirPropiedad = null)
        {
            var elemento = (TElemento)Mapeador.Map(registro, typeof(TRegistro), typeof(TElemento));
            return elemento;
        }

        public TElemento NuevoElemento()
        {
            return Metadatos.NuevoElementoIu();
        }

    }
}
