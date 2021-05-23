using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Entorno;
using GestoresDeNegocio.Entorno;
using ModeloDeDto.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class VistaMvcController : EntidadController<ContextoSe, VistaMvcDtm, VistaMvcDto>
    {

        public VistaMvcController(GestorDeVistaMvc gestorDeVistas, GestorDeErrores gestorDeErrores)
         : base
         (
           gestorDeVistas,
           gestorDeErrores
         )
        {
        }


        public IActionResult CrudVistaMvc()
        {
            return ViewCrud(new DescriptorDeVistaMvc(Contexto, ModoDescriptor.Mantenimiento));
        }
    }
}
