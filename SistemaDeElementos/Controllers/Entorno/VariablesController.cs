using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Entorno;
using Gestor.Elementos.Entorno;
using ModeloDeDto.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class VariablesController : EntidadController<ContextoSe, VariableDtm, VariableDto>
    {

        public VariablesController(GestorDeVariables gestorDeVariables, GestorDeErrores gestorDeErrores)
         : base
         (
           gestorDeVariables,
           gestorDeErrores,
           new DescriptorDeVariable(ModoDescriptor.Mantenimiento)
         )
        {
        }


        public IActionResult CrudVariable()
        {
            return ViewCrud();
        }
    }
}
