using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Callejero;
using GestoresDeNegocio.Callejero;
using ModeloDeDto.Callejero;
using MVCSistemaDeElementos.Descriptores.Callejero;

namespace MVCSistemaDeElementos.Controllers.Callejero
{
    public class PaisesController : EntidadController<ContextoSe, PaisDtm, PaisDto>
    {

        public PaisesController(GestorDePaises gestorDePaises, GestorDeErrores gestorDeErrores)
         : base
         (
           gestorDePaises,
           gestorDeErrores,
           new DescriptorDePais(ModoDescriptor.Mantenimiento)
         )
        {
        }


        public IActionResult CrudPaises()
        {
            return ViewCrud();
        }
    }
}
