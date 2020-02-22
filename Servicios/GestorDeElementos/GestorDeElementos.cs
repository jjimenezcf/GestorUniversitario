using AutoMapper;
using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.ModeloIu;
using Gestor.Errores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Utilidades;

namespace Gestor.Elementos
{

    public class ClausulaDeFiltrado
    {
        public string Propiedad { get; set; }
        public string Criterio { get; set; }
        public string Valor { get; set; }
    }


    public static class RegistroBaseFiltros
    {
        public const string FiltroPorId = "Id";

        public static IQueryable<TRegistro> AplicarFiltroId<TRegistro>(this IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros) where TRegistro : RegistroBase
        {
            foreach(ClausulaDeFiltrado filtro in filtros)
                if (filtro.Propiedad.ToLower() == FiltroPorId.ToLower())
                   return registros.Where(x => x.Id == filtro.Valor.Entero());

            return registros;
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


    public abstract class GestorDeElementos<TContexto, TRegistro, TElemento> 
        where TRegistro : RegistroBase 
        where TElemento : ElementoBase 
        where TContexto : ContextoDeElementos
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


        public IEnumerable<TElemento> Leer(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, Dictionary<string, Ordenacion> orden)
        {
            List<TRegistro> elementosDeBd;

            IQueryable<TRegistro> registros = AplicarFiltros(Contexto.Set<TRegistro>(), filtros);

            registros = AplicarOrden(registros, orden);

            registros = registros.Skip(posicion);

            if (cantidad > 0)
            {
                registros = registros.Take(cantidad);
            }

            elementosDeBd = registros.AsNoTracking().ToList();

            return MapearElementos(elementosDeBd) ;
        }

        public int Contar(List<ClausulaDeFiltrado> filtros)
        {

            IQueryable<TRegistro> registros = AplicarFiltros(Contexto.Set<TRegistro>(), filtros);

            var total = registros.Count();

            return total;
        }

        protected virtual IQueryable<TRegistro> AplicarOrden(IQueryable<TRegistro> registros, Dictionary<string, Ordenacion> orden)
        {
            return registros.Orden(orden);
        }

        protected virtual IQueryable<TRegistro> AplicarFiltros(IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros)
        {
            return registros.AplicarFiltroId(filtros);
        }

        public (IEnumerable<TElemento>, int) LeerTodos()
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
