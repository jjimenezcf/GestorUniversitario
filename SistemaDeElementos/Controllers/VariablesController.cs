using System;
using System.Collections.Generic;
using Gestor.Elementos.Entorno;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using Utilidades;

namespace MVCSistemaDeElementos.Controllers
{
    public class VariablesController : EntidadController<CtoEntorno, VariableDtm, VariableDto>
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
