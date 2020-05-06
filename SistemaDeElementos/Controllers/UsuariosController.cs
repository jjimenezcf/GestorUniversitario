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

        
        public IActionResult CrudUsuario(string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(orden), cantidadPorLeer: 5, posicionInicial: 0);
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }


    }

}
