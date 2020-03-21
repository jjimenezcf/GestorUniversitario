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
    public class FuncionalidadController : EntidadController<CtoEntorno, Fun_Elemento, FuncionalidadDto>
    {

        public FuncionalidadController(GestorDeFuncionalidad gestorDeFuncionalidad, GestorDeErrores gestorDeErrores)
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