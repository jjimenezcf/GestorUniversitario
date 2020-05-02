using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Elementos.Entorno;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using Microsoft.AspNetCore.Hosting;

namespace MVCSistemaDeElementos.Controllers
{
    public class UsuariosController : EntidadController<CtoEntorno, UsuarioDtm, UsuarioDto>
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
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(orden), cantidadPorLeer: 5, posicionInicial: 0);
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }


        [HttpPost, ActionName(nameof(CrearUsuario))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearUsuario([Bind("id,apellido,nombre,alta")] UsuarioDto usuario)
        {
            return await CrearObjeto(usuario);
        }



        [HttpPost, ActionName(nameof(ModificarUsuario))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarUsuario(int id, [Bind("Id,Apellido,Nombre,Alta")] UsuarioDto usuario)
        {
            return await ModificarObjeto(id, usuario);
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


    }

}
