using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using Gestor.Elementos.Seguridad;
using GestorDeSeguridad.ModeloIu;

namespace MVCSistemaDeElementos.Controllers
{
    public class PuestoDeUnUsuarioController :  EntidadController<ContextoSe, PuestoDeUnUsuarioDtm, PuestoDeUnUsuarioDto>
    {

        public PuestoDeUnUsuarioController(GestorDePuestoDeUnUsuario gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores,
           new DescriptorDePuestoDeUnUsuario(ModoDescriptor.Mantenimiento)
         )
        {
        }

        [HttpPost]
        public IActionResult CrudPuestoDeUnUsuario(string restrictor, string orden)
        {
            var elementosDto = LeerOrdenados(restrictor, orden);
            GestorDelCrud.Descriptor.MapearElementosAlGrid(elementosDto, cantidadPorLeer: 5, posicionInicial: 0);
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }
    }
}
