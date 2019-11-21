using GestorDeElementos.BdModelo;
using GestorDeElementos.IuModelo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestorDeElementos
{
    public class GestorDeElementos<Tbd, Tiu> where Tbd : BdElemento where Tiu : IuElemento
    {
        protected ClaseDeElemetos<Tbd, Tiu> Metadatos;
        private DbContext _Contexto;


        public GestorDeElementos(DbContext contexto, Tbd bdClase, Tiu iuClase)
        {
            _Contexto = contexto;
            Metadatos = ClaseDeElemetos<Tbd, Tiu>.ObtenerGestorDeLaClase(bdClase, iuClase);
        }


        public async Task InsertarElementoAsync(IuElemento iuElemento)
        {
            BdElemento elementoBD = MapearModeloParaLaBd(iuElemento);
            _Contexto.Add(elementoBD);
            await _Contexto.SaveChangesAsync();
        }

        public async Task ModificarElementoAsync(IuElemento iuElemento)
        {
            BdElemento elementoBD = MapearModeloParaLaBd(iuElemento);
            _Contexto.Update(elementoBD);
            await _Contexto.SaveChangesAsync();
        }

        public bool ExisteObjetoEnBd(int id)
        {
            return true; //_Contexto.Elementos<T>().Any(e => e.Id == id);
        }


        public IEnumerable<IuElemento> LeerTodos()
        {
            throw new NotImplementedException();

            //await _Contexto.Elementos
            //.AsNoTracking()
        }

        public IuElemento LeerPorId(int id)
        {

            BdElemento bdElemento = null;

            //await _Contexto.Elementos
            //.AsNoTracking()
            //.FirstOrDefaultAsync(m => m.Id == id);

            return MapearModeloParaLaIu(bdElemento);
        }

        public IuElemento LeerTodoPorId(int id)
        {
            BdElemento bdElemento = null;

            //await _Contexto.Elementos
            //.Include(i => i.Inscripciones)
            //.ThenInclude(e => e.Curso)
            //.AsNoTracking()
            //.FirstOrDefaultAsync(m => m.Id == id);

            return MapearModeloParaLaIu(bdElemento);
        }

        public async Task BorrarPorId(int id)
        {
            var bdElemto = LeerPorId(id);
            _Contexto.Remove(bdElemto);
            await _Contexto.SaveChangesAsync();
        }

        private BdElemento MapearModeloParaLaBd(IuElemento iuElemento)
        {
            var bdElemento = Metadatos.NuevoElementoBd();
            return bdElemento;
        }

        private IuElemento MapearModeloParaLaIu(BdElemento bdElemento)
        {
            var iuElemento = Metadatos.NuevoElementoIu();
            return iuElemento;
        }


    }
}
