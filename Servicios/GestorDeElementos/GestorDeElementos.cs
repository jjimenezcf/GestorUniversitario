using AutoMapper;
using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.ModeloIu;
using Gestor.Errores;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Gestor.Elementos
{
    public abstract class GestorDeElementos<TContexto, TRegistro, TElemento> where TRegistro : RegistroBase where TElemento : ElementoBase where TContexto : DbContext
    {
        protected ClaseDeElemetos<TRegistro, TElemento> Metadatos;
        public TContexto _Contexto;
        private GestorDeErrores _gestorDeErrores;
        protected IMapper _mapeador;

        protected abstract TRegistro LeerConDetalle(int Id);

        public GestorDeElementos(TContexto contexto, IMapper mapeador)
        {
            _mapeador = mapeador;
            IniciarClase(contexto);
        }

        public void AsignarGestores(GestorDeErrores gestorErrores)
        {
            _gestorDeErrores = gestorErrores;
        }

        protected virtual void IniciarClase(TContexto contexto)
        {
            _Contexto = contexto;
            Metadatos = ClaseDeElemetos<TRegistro, TElemento>.ObtenerGestorDeLaClase();            
        }

        public async Task InsertarElementoAsync(TElemento elemento)
        {
            TRegistro elementoBD = MapearRegistro(elemento);
            _Contexto.Add(elementoBD);
            await _Contexto.SaveChangesAsync();
        }

        public async Task ModificarElementoAsync(TElemento elemento)
        {
            TRegistro elementoBD = MapearRegistro(elemento);
            _Contexto.Update(elementoBD);
            await _Contexto.SaveChangesAsync();
        }

        public bool ExisteObjetoEnBd(int id)
        {
            return _Contexto.Set<TRegistro>().Any(e => e.Id == id);
        }


        public IEnumerable<TElemento> LeerTodos()
        {
            var elementosDeBd = _Contexto.Set<TRegistro>().AsNoTracking().ToList();
            return MapearElementos(elementosDeBd);
        }

        public TElemento LeerElementoPorId(int id)
        {
            var elementoDeBd = _Contexto.Set<TRegistro>().AsNoTracking().FirstOrDefault(m => m.Id == id);
            return MapearElemento(elementoDeBd);
        }

        public TRegistro LeerRegistroPorId(int id)
        {
            return _Contexto.Set<TRegistro>().AsNoTracking().FirstOrDefault(m => m.Id == id);
        }


        public TElemento LeerElementoConDetalle(int id)
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
            var elemento = (TElemento)_mapeador.Map(registro, typeof(TRegistro), typeof(TElemento));
            return elemento;
        }
        
        public  TElemento NuevoElemento()
        {
            return Metadatos.NuevoElementoIu();
        }

    }
}
