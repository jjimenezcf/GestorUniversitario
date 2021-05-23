using Microsoft.AspNetCore.Mvc;
using ServicioDeDatos;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using GestoresDeNegocio.TrabajosSometidos;

namespace MVCSistemaDeElementos.Controllers
{
    public class CorreosController : EntidadController<ContextoSe, CorreoDtm, CorreoDto>
    {

        public CorreosController(GestorDeCorreos gestorDeCorreos, GestorDeErrores gestorDeErrores)
        :base
        (
          gestorDeCorreos, 
          gestorDeErrores
        )
        {

        }
                
        public IActionResult CrudDeCorreos()
        {
            return ViewCrud(new DescriptorDeCorreos(Contexto, ModoDescriptor.Mantenimiento));
        }

     

    }

}
