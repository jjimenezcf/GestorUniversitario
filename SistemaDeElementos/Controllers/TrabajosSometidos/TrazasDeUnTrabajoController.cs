using Microsoft.AspNetCore.Mvc;
using ServicioDeDatos;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using GestoresDeNegocio.TrabajosSometidos;


namespace MVCSistemaDeElementos.Controllers
{
    public class TrazasDeUnTrabajoController : EntidadController<ContextoSe, TrazaDeUnTrabajoDtm, TrazaDeUnTrabajoDto>
    {

        public TrazasDeUnTrabajoController(GestorDeTrazasDeUnTrabajo gestorDeNegocios, GestorDeErrores gestorDeErrores)
        : base
        (
          gestorDeNegocios,
          gestorDeErrores,
          new DescriptorDeTrazasDeUnTrabajo(ModoDescriptor.Mantenimiento)
        )
        {

        }

        public IActionResult CrudDeTrazasDeUnTrabajo()
        {
            return ViewCrud();
        }
    }
}
