using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using Gestor.Elementos.Seguridad;
using GestorDeSeguridad.ModeloIu;

namespace MVCSistemaDeElementos.Controllers
{
    public class PuestoDeUnUsuarioController :  EntidadController<ContextoSe, PuestosDeUsuarioDtm, PuestoDeUnUsuarioDto>
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

        public IActionResult CrudPuestoDeUnUsuario(string filtroUsuario, string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(filtroUsuario, orden), cantidadPorLeer: 5, posicionInicial: 0);
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }
    }
}
