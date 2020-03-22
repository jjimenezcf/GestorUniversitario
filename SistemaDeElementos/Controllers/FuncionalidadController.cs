using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.Entorno;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Controllers;
using MVCSistemaDeElementos.Descriptores;
using Utilidades;

namespace MVCSistemaDeElementos.Controllers
{
    public class FuncionalidadController : EntidadController<CtoEntorno, R_Menu, E_Menu>
    {

        public FuncionalidadController(GestorDeMenus gestorDeFuncionalidad, GestorDeErrores gestorDeErrores)
        : base
        (
          gestorDeFuncionalidad,
          gestorDeErrores,
          new CrudFuncionalidad(ModoDescriptor.Mantenimiento)
        )
        {
        }
    }
}