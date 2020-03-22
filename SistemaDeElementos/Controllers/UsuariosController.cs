using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Elementos.Entorno;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;

namespace MVCSistemaDeElementos.Controllers
{
    public class UsuariosController : EntidadController<CtoEntorno, rUsuario, eUsuario>
    {

        public UsuariosController(GestorDeUsuarios gestorDeUsuarios, GestorDeErrores gestorDeErrores)
        :base
        (
          gestorDeUsuarios, 
          gestorDeErrores, 
          new CrudUsuario(ModoDescriptor.Mantenimiento)
        )
        {
        }

        
        public IActionResult IraMantenimientoUsuario(string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(orden));
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }

        
        public IActionResult IraCrearUsuario()
        {
            return View(GestorDelCrud.Creador.Vista, new eUsuario());
        }

        public IActionResult IraDetalleUsuario(int? id)
        {
            GestorDelCrud.Detalle.AsignarTituloDetalle("Inscripciones");
            return View(GestorDelCrud.Detalle.Vista, LeerDetalle(id));
        }

        public IActionResult IraBorrarUsuario(int? id)
        {
            return View(GestorDelCrud.Supresor.Vista, LeerUsuario(id));
        }

        public IActionResult IraEditarUsuario(int? id)
        {
            return View(GestorDelCrud.Editor.Vista, LeerUsuario(id));
        }


        [HttpPost, ActionName(nameof(CrearUsuario))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearUsuario([Bind("ID,Apellido,Nombre,Alta")] eUsuario usuario)
        {
            return await CrearObjeto(usuario);
        }



        [HttpPost, ActionName(nameof(ModificarUsuario))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarUsuario(int id, [Bind("Id,Apellido,Nombre,Alta")] eUsuario usuario)
        {
            return await ModificarObjeto(id, usuario);
        }



        [HttpPost, ActionName(nameof(BorrarUsuario))]
        [ValidateAntiForgeryToken]
        public IActionResult BorrarUsuario(int id)
        {

            GestorDeElementos.BorrarPorId(id);
            return IraMantenimientoUsuario("");
        }

        private eUsuario LeerUsuario(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del usuario no puede ser nulo");
            }

            var usuario = (eUsuario)GestorDeElementos.LeerElementoPorId((int)id);
            if (usuario == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del usuario no se pudo localizar");
            }

            return usuario;
        }

        private eUsuario LeerDetalle(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del usuario no puede ser nulo");
            }

            var usuario = (eUsuario)GestorDeElementos.LeerElementoConDetalle((int)id);
            if (usuario == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del usuario no se pudo localizar");
            }

            return usuario;
        }


    }

}
