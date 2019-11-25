using GestorDeElementos.BdModelo;
using GestorDeElementos.IuModelo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GestorDeElementos
{
    public abstract class GestorDeElementos<TContexto, TRegistroBase, TElementoBase> where TRegistroBase : RegistroBase where TElementoBase : ElementoBase where TContexto : DbContext
    {
        protected ClaseDeElemetos<TRegistroBase, TElementoBase> Metadatos;
        public TContexto _Contexto;

        protected abstract TRegistroBase LeerConDetalle(int Id);
        protected abstract void MapearDetalleParaLaIu(TElementoBase iuElemento, TRegistroBase bdElemento, PropertyInfo propiedadOrigen);

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
            Metadatos = ClaseDeElemetos<TRegistroBase, TElementoBase>.ObtenerGestorDeLaClase();
        }

        public async Task InsertarElementoAsync(TElementoBase elemento)
        {
            RegistroBase elementoBD = MapearElementoParaLaBd(elemento);
            _Contexto.Add(elementoBD);
            await _Contexto.SaveChangesAsync();
        }

        public async Task ModificarElementoAsync(TElementoBase elemento)
        {
            RegistroBase elementoBD = MapearElementoParaLaBd(elemento);
            _Contexto.Update(elementoBD);
            await _Contexto.SaveChangesAsync();
        }

        public bool ExisteObjetoEnBd(int id)
        {
            return _Contexto.Set<TRegistroBase>().Any(e => e.Id == id);
        }


        public IEnumerable<ElementoBase> LeerTodos()
        {
            var elementosDeBd = _Contexto.Set<TRegistroBase>().AsNoTracking().ToList();
            return MapearElementosParaLaIu(elementosDeBd);
        }

        private IEnumerable<ElementoBase> MapearElementosParaLaIu(List<TRegistroBase> registros)
        {
            var lista = new List<TElementoBase>();
            foreach (var registro in registros)
            {
                lista.Add(MaperaElementoParaLaIu(registro));
            }
            return lista.AsEnumerable();
        }

        public ElementoBase LeerElementoPorId(int id)
        {
            var elementoDeBd = _Contexto.Set<TRegistroBase>().AsNoTracking().FirstOrDefault(m => m.Id == id);
            return MaperaElementoParaLaIu(elementoDeBd);
        }

        public RegistroBase LeerRegistroPorId(int id)
        {
            return _Contexto.Set<TRegistroBase>().AsNoTracking().FirstOrDefault(m => m.Id == id);
        }


        public ElementoBase LeerElementoConDetalle(int id)
        {
            var elementoLeido = LeerConDetalle(id);
            return MaperaElementoParaLaIu(elementoLeido);
        }

        public void BorrarPorId(int id)
        {
            var registro = LeerRegistroPorId(id);
            _Contexto.Remove(registro);
            _Contexto.SaveChangesAsync();
        }

        private TRegistroBase MapearElementoParaLaBd(TElementoBase elemento)
        {
            var registro = Metadatos.NuevoElementoBd();
            PropertyInfo[] propiedadesBd = typeof(TRegistroBase).GetProperties();
            PropertyInfo[] propiedadesIu = typeof(TElementoBase).GetProperties();

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

        public TElementoBase MaperaElementoParaLaIu(TRegistroBase registro)
        {
            var elemento = Metadatos.NuevoElementoIu();
            PropertyInfo[] propiedadesBd = typeof(TRegistroBase).GetProperties();
            PropertyInfo[] propiedadesIu = typeof(TElementoBase).GetProperties();

            foreach (PropertyInfo propiedadOrigen in propiedadesBd)
            {
                foreach (PropertyInfo propiedadDestino in propiedadesIu)
                {
                    if (propiedadDestino.Name == propiedadOrigen.Name)
                    {
                        if (typeof(ICollection<>).Name == propiedadOrigen.PropertyType.Name)
                        {
                            MapearDetalleParaLaIu(elemento, registro, propiedadOrigen);
                        }

                        propiedadDestino.SetValue(elemento, propiedadOrigen.GetValue(registro));
                        break;
                    }
                }
            }
            return elemento;
        }

    }
}
