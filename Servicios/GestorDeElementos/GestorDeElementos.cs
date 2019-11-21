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
        private Tctx _Contexto;

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
            return true; //_Contexto.Elementos<T>().Any(e => e.Id == id);
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

        public IuElemento LeerPorId(int id)
        {

            Tbd bdElemento = null;

            //await _Contexto.Elementos
            //.AsNoTracking()
            //.FirstOrDefaultAsync(m => m.Id == id);

            return MaperaElementoParaLaIu(bdElemento);
        }

        public IuElemento LeerTodoPorId(int id)
        {
            Tbd bdElemento = null;

            //await _Contexto.Elementos
            //.Include(i => i.Inscripciones)
            //.ThenInclude(e => e.Curso)
            //.AsNoTracking()
            //.FirstOrDefaultAsync(m => m.Id == id);

            return MaperaElementoParaLaIu(bdElemento);
        }

        public async Task BorrarPorId(int id)
        {
            var bdElemto = LeerPorId(id);
            _Contexto.Remove(bdElemto);
            await _Contexto.SaveChangesAsync();
        }

        private Tbd MapearElementoParaLaBd(Tiu iuElemento)
        {
            var bdElemento = Metadatos.NuevoElementoBd();
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
