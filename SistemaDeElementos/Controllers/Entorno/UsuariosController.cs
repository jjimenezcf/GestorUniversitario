using Microsoft.AspNetCore.Mvc;
using ServicioDeDatos;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Entorno;
using Gestor.Elementos.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class UsuariosController : EntidadController<ContextoSe, UsuarioDtm, UsuarioDto>
    {

        public UsuariosController(GestorDeUsuarios gestorDeUsuarios, GestorDeErrores gestorDeErrores)
        :base
        (
          gestorDeUsuarios, 
          gestorDeErrores, 
          new DescriptorDeUsuario(ModoDescriptor.Mantenimiento)
        )
        {

        }

        
        public IActionResult CrudUsuario(string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(null, orden), cantidadPorLeer: 5, posicionInicial: 0);
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }


    }

}
