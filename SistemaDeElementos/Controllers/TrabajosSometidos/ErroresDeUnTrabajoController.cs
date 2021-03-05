using Microsoft.AspNetCore.Mvc;
using ServicioDeDatos;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using GestoresDeNegocio.TrabajosSometidos;


namespace MVCSistemaDeElementos.Controllers
{
    public class ErroresDeUnTrabajoController : EntidadController<ContextoSe, ErrorDeUnTrabajoDtm, ErrorDeUnTrabajoDto>
    {

        public ErroresDeUnTrabajoController(GestorDeErroresDeUnTrabajo gestorDeNegocios, GestorDeErrores gestorDeErrores)
        : base
        (
          gestorDeNegocios,
          gestorDeErrores,
          new DescriptorDeErroresDeUnTrabajo(ModoDescriptor.Mantenimiento)
        )
        {

        }

        public IActionResult CrudDeErroresDeUnTrabajo()
        {
            return ViewCrud();
        }
    }
}
