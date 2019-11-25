using GestorDeElementos.ModeloBd;
using GestorDeElementos.ModeloIu;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GestorDeElementos
{
    public abstract class GestorDeElementos<TContexto, TRegistro, TElemento> where TRegistro : RegistroBase where TElemento : ElementoBase where TContexto : DbContext
    {
        protected ClaseDeElemetos<TRegistro, TElemento> Metadatos;
        public TContexto _Contexto;

        protected abstract TRegistro LeerConDetalle(int Id);
        protected abstract void MapearDetalleParaLaIu(TRegistro registro, TElemento elemento);
        protected abstract void MapearElemento(TRegistro registro, TElemento elemento, PropertyInfo propiedad);

        public GestorDeElementos()
        {
            
        }

        public GestorDeElementos(TContexto contexto)
        {
            IniciarClase(contexto);
        }

        protected virtual void IniciarClase(TContexto contexto)
        {
            _Contexto = contexto;
            Metadatos = ClaseDeElemetos<TRegistro, TElemento>.ObtenerGestorDeLaClase();
        }

        public async Task InsertarElementoAsync(TElemento elemento)
        {
            RegistroBase elementoBD = MapearRegistro(elemento);
            _Contexto.Add(elementoBD);
            await _Contexto.SaveChangesAsync();
        }

        public async Task ModificarElementoAsync(TElemento elemento)
        {
            RegistroBase elementoBD = MapearRegistro(elemento);
            _Contexto.Update(elementoBD);
            await _Contexto.SaveChangesAsync();
        }

        public bool ExisteObjetoEnBd(int id)
        {
            return _Contexto.Set<TRegistro>().Any(e => e.Id == id);
        }


        public IEnumerable<ElementoBase> LeerTodos()
        {
            var elementosDeBd = _Contexto.Set<TRegistro>().AsNoTracking().ToList();
            return MapearElementosParaLaIu(elementosDeBd);
        }

        private IEnumerable<ElementoBase> MapearElementosParaLaIu(List<TRegistro> registros)
        {
            var lista = new List<TElemento>();
            foreach (var registro in registros)
            {
                lista.Add(MapearElemento(registro));
            }
            return lista.AsEnumerable();
        }

        public ElementoBase LeerElementoPorId(int id)
        {
            var elementoDeBd = _Contexto.Set<TRegistro>().AsNoTracking().FirstOrDefault(m => m.Id == id);
            return MapearElemento(elementoDeBd);
        }

        public RegistroBase LeerRegistroPorId(int id)
        {
            return _Contexto.Set<TRegistro>().AsNoTracking().FirstOrDefault(m => m.Id == id);
        }


        public ElementoBase LeerElementoConDetalle(int id)
        {
            var elementoLeido = LeerConDetalle(id);
            return MapearElemento(elementoLeido);
        }

        public void BorrarPorId(int id)
        {
            var registro = LeerRegistroPorId(id);
            _Contexto.Remove(registro);
            _Contexto.SaveChangesAsync();
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

        public TElemento MapearElemento(TRegistro registro, List<string> excluirPropiedad = null)
        {
            TElemento elemento = Metadatos.NuevoElementoIu(); 
            PropertyInfo[] propiedadesBd = typeof(TRegistro).GetProperties();
            PropertyInfo[] propiedadesIu = typeof(TElemento).GetProperties();

            foreach (PropertyInfo propiedadOrigen in propiedadesBd)
            {
                foreach (PropertyInfo propiedadDestino in propiedadesIu)
                {
                    if (excluirPropiedad != null && excluirPropiedad.Contains(propiedadDestino.Name))
                        break;

                    if (propiedadDestino.Name == propiedadOrigen.Name)
                    {
                        if (typeof(ICollection<>).Name == propiedadOrigen.PropertyType.Name)
                            MapearDetalleParaLaIu(registro, elemento);
                        else
                        if (propiedadOrigen.PropertyType.BaseType.Name.Equals("RegistroBase"))
                            MapearElemento(registro, elemento, propiedadOrigen);
                        else
                        if (propiedadOrigen.GetValue(registro) != null)
                        {
                            var valor = propiedadOrigen.GetValue(registro);
                            propiedadDestino.SetValue(elemento, valor);
                        }
                        break;
                    }
                }
            }
            return elemento;
        }
        
        public  TElemento NuevoElemento()
        {
            return Metadatos.NuevoElementoIu();
        }

    }
}
