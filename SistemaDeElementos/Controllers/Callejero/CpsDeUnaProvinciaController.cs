using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Callejero;
using ModeloDeDto.Callejero;
using GestoresDeNegocio.Callejero;

namespace MVCSistemaDeElementos.Controllers
{
    public class CpsDeUnaProvinciaController :  RelacionController<ContextoSe, CpsDeUnaProvinciaDtm, CpsDeUnaProvinciaDto>
    {

        public CpsDeUnaProvinciaController(GestorDeCpsDeUnaProvincia gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores
         )
        {
        }

        [HttpPost]
        public IActionResult CrudCpsDeUnaProvincia()
        {
            return ViewCrud(new DescriptorDeCpsDeUnaProvincia(Contexto, ModoDescriptor.Mantenimiento));
        }

    }
}
