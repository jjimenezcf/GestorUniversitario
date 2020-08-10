using Microsoft.AspNetCore.Mvc;
using ServicioDeDatos;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Entorno;
using GestoresDeNegocio.Entorno;
using ModeloDeDto.Entorno;

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

        
        public IActionResult CrudUsuario()
        {
            return ViewCrud();
        }


    }

}
