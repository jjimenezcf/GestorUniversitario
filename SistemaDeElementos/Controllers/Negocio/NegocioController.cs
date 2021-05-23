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

        public NegocioController(GestorDeNegocios gestorDeNegocios, GestorDeErrores gestorDeErrores)
        :base
        (
          gestorDeNegocios, 
          gestorDeErrores
        )
        {

        }

        
        public IActionResult CrudDeNegocios()
        {
            Descriptor = new DescriptorDeNegocio(Contexto, ModoDescriptor.Mantenimiento);
            return ViewCrud();
        }


    }

}
