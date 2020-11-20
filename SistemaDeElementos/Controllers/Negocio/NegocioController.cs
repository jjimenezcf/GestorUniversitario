using Microsoft.AspNetCore.Mvc;
using ServicioDeDatos;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using GestoresDeNegocio.Negocio;
using ModeloDeDto.Negocio;
using ServicioDeDatos.Negocio;

namespace MVCSistemaDeElementos.Controllers
{
    public class NegocioController : EntidadController<ContextoSe, NegocioDtm, NegocioDto>
    {

        public NegocioController(GestorDeNegocio gestorDeNegocios, GestorDeErrores gestorDeErrores)
        :base
        (
          gestorDeNegocios, 
          gestorDeErrores, 
          new DescriptorDeNegocio(ModoDescriptor.Mantenimiento)
        )
        {

        }

        
        public IActionResult CrudDeNegocios()
        {
            return ViewCrud();
        }


    }

}
