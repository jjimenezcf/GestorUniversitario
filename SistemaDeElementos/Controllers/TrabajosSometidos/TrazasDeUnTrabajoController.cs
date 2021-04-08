using Microsoft.AspNetCore.Mvc;
using ServicioDeDatos;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using GestoresDeNegocio.TrabajosSometidos;
using System;
using GestorDeElementos;

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

        protected override ParametrosDeNegocio AntesDeEjecutar_ModificarPorId(TrazaDeUnTrabajoDto elemento)
        {
            throw new Exception("Los mensajes de la traza de un trabajo no son modificables");
        }

        protected override ParametrosDeNegocio AntesDeEjecutar_CrearElemento(TrazaDeUnTrabajoDto elemento)
        {
            throw new Exception("No se pueden crear trazas en un trabajo");
        }
    }
}
