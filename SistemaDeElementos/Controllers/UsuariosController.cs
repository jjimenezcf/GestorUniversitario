using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Elementos.Universitario.ModeloIu;
using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario;
using Gestor.Errores;
using UniversidadDeMurcia.Descriptores;

namespace UniversidadDeMurcia.Controllers
{
    public class UsuariosController : EntidadController<ContextoUniversitario, UsuarioReg, UsuarioDto>
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
            return View(GestorDelCrud.Creador.Vista, new UsuarioDto());
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
        public async Task<IActionResult> CrearUsuario([Bind("ID,Apellido,Nombre,Alta")] UsuarioDto usuario)
        {
            return await CrearObjeto(usuario);
        }



        [HttpPost, ActionName(nameof(ModificarUsuario))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarUsuario(int id, [Bind("Id,Apellido,Nombre,Alta")] UsuarioDto usuario)
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

        private UsuarioDto LeerUsuario(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del usuario no puede ser nulo");
            }

            var usuario = (UsuarioDto)GestorDeElementos.LeerElementoPorId((int)id);
            if (usuario == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del usuario no se pudo localizar");
            }

            return usuario;
        }

        private UsuarioDto LeerDetalle(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del usuario no puede ser nulo");
            }

            var usuario = (UsuarioDto)GestorDeElementos.LeerElementoConDetalle((int)id);
            if (usuario == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del usuario no se pudo localizar");
            }

            return usuario;
        }


    }

}
