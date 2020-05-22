using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Entorno;
using Gestor.Elementos.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class VariablesController : EntidadController<ContextoSe, VariableDtm, VariableDto>
    {

        public VariablesController(GestorDeVariables gestorDeVariables, GestorDeErrores gestorDeErrores)
         : base
         (
           gestorDeVariables,
           gestorDeErrores,
           new CrudVariable(ModoDescriptor.Mantenimiento)
         )
        {
        }


        public IActionResult CrudVariable(string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(orden), cantidadPorLeer: 5, posicionInicial: 0);
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }
    }
}
