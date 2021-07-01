using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Callejero;
using GestoresDeNegocio.Callejero;
using ModeloDeDto.Callejero;

namespace MVCSistemaDeElementos.Controllers
{
    public class TiposDeViaController : EntidadController<ContextoSe, TipoDeViaDtm, TipoDeViaDto>
    {
        public TiposDeViaController(GestorDeTiposDeVia gestorDeTiposDeVia, GestorDeErrores gestorDeErrores)
         : base
         (
           gestorDeTiposDeVia,
           gestorDeErrores
         )
        {
        }


        public IActionResult CrudTiposDeVia()
        {
            return ViewCrud(new DescriptorTiposDeVia(Contexto, ModoDescriptor.Mantenimiento));
        }
    }
}
