using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using Gestor.Elementos.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{
    public class PuestoDeTrabajoController : EntidadController<ContextoSe, PuestoDtm, PuestoDto>
    {

        public PuestoDeTrabajoController(GestorDePuestosDeTrabajo gestorDePuesto, GestorDeErrores gestorDeErrores)
         : base
         (
           gestorDePuesto,
           gestorDeErrores,
           new DescriptorDePuestoDeTrabajo(ModoDescriptor.Mantenimiento)
         )
        {
        }


        public IActionResult CrudPuestoDeTrabajo(string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(null, orden), cantidadPorLeer: 5, posicionInicial: 0);
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }
    }
}
