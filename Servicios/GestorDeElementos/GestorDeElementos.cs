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
    public abstract class GestorDeElementos<Tctx, Tbd, Tiu> where Tbd : BdElemento where Tiu : IuElemento where Tctx : DbContext
    {
        protected ClaseDeElemetos<Tbd, Tiu> Metadatos;
        protected Tctx _Contexto;

        protected abstract Tbd LeerConDetalle(int Id);

        public GestorDeElementos()
        {
            
        }

        public GestorDeElementos(Tctx contexto)
        {
            IniciarClase(contexto);
        }

        protected virtual void IniciarClase(Tctx contexto)
        {
            _Contexto = contexto;
            Metadatos = ClaseDeElemetos<Tbd, Tiu>.ObtenerGestorDeLaClase();
        }

        public async Task InsertarElementoAsync(Tiu iuElemento)
        {
            BdElemento elementoBD = MapearElementoParaLaBd(iuElemento);
            _Contexto.Add(elementoBD);
            await _Contexto.SaveChangesAsync();
        }

        public async Task ModificarElementoAsync(Tiu iuElemento)
        {
            BdElemento elementoBD = MapearElementoParaLaBd(iuElemento);
            _Contexto.Update(elementoBD);
            await _Contexto.SaveChangesAsync();
        }

        public bool ExisteObjetoEnBd(int id)
        {
            return _Contexto.Set<Tbd>().Any(e => e.Id == id);
        }


        public IEnumerable<IuElemento> LeerTodos()
        {
            var elementosDeBd = _Contexto.Set<Tbd>().AsNoTracking().ToList();
            return MapearElementosParaLaIu(elementosDeBd);
        }

        private IEnumerable<IuElemento> MapearElementosParaLaIu(List<Tbd> elementosDeBd)
        {
            var lista = new List<Tiu>();
            foreach (var elementoBd in elementosDeBd)
            {
                lista.Add(MaperaElementoParaLaIu(elementoBd));
            }
            return lista.AsEnumerable();
        }

        public IuElemento LeerElementoPorId(int id)
        {
            var elementoDeBd = _Contexto.Set<Tbd>().AsNoTracking().FirstOrDefault(m => m.Id == id);
            return MaperaElementoParaLaIu(elementoDeBd);
        }

        public BdElemento LeerRegistroPorId(int id)
        {
            return _Contexto.Set<Tbd>().AsNoTracking().FirstOrDefault(m => m.Id == id);
        }


        public IuElemento LeerElementoConDetalle(int id)
        {
            var elementoLeido = LeerConDetalle(id);
            return MaperaElementoParaLaIu(elementoLeido);
        }

        public void BorrarPorId(int id)
        {
            var bdElemeto = LeerRegistroPorId(id);
            _Contexto.Remove(bdElemeto);
            _Contexto.SaveChangesAsync();
        }

        private Tbd MapearElementoParaLaBd(Tiu iuElemento)
        {
            var bdElemento = Metadatos.NuevoElementoBd();
            PropertyInfo[] propiedadesBd = typeof(Tbd).GetProperties();
            PropertyInfo[] propiedadesIu = typeof(Tiu).GetProperties();

            foreach (PropertyInfo pBd in propiedadesBd)
            {
                foreach (PropertyInfo pIu in propiedadesIu)
                {
                    if (pIu.Name == pBd.Name)
                    {
                        pBd.SetValue(bdElemento, pIu.GetValue(iuElemento));
                        break;
                    }
                }
            }
            return bdElemento;
        }

        private Tiu MaperaElementoParaLaIu(Tbd bdElemento)
        {
            var iuElemento = Metadatos.NuevoElementoIu();
            PropertyInfo[] propiedadesBd = typeof(Tbd).GetProperties();
            PropertyInfo[] propiedadesIu = typeof(Tiu).GetProperties();

            foreach (PropertyInfo pBd in propiedadesBd)
            {
                foreach (PropertyInfo pIu in propiedadesIu)
                {
                    if (pIu.Name == pBd.Name)
                    {
                        pIu.SetValue(iuElemento, pBd.GetValue(bdElemento));
                        break;
                    }
                }
            }
            return iuElemento;
        }

   
    }
}
