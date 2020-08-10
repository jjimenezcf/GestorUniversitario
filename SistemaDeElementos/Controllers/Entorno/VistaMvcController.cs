using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Entorno;
using Gestor.Elementos.Entorno;
using ModeloDeDto.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class VistaMvcController : EntidadController<ContextoSe, VistaMvcDtm, VistaMvcDto>
    {

        public VistaMvcController(GestorDeVistaMvc gestorDeVistas, GestorDeErrores gestorDeErrores)
         : base
         (
           gestorDeVistas,
           gestorDeErrores,
           new DescriptorDeVistaMvc(ModoDescriptor.Mantenimiento)
         )
        {
        }


        public IActionResult CrudVistaMvc()
        {
            return ViewCrud();
        }
    }
}
