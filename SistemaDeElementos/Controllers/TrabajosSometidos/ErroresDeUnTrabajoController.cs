﻿using Microsoft.AspNetCore.Mvc;
using ServicioDeDatos;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using GestoresDeNegocio.TrabajosSometidos;
using System;

namespace MVCSistemaDeElementos.Controllers
{
    public class ErroresDeUnTrabajoController : EntidadController<ContextoSe, ErrorDeUnTrabajoDtm, ErrorDeUnTrabajoDto>
    {

        public ErroresDeUnTrabajoController(GestorDeErroresDeUnTrabajo gestorDeNegocios, GestorDeErrores gestorDeErrores)
        : base
        (
          gestorDeNegocios,
          gestorDeErrores,
          new DescriptorDeErroresDeUnTrabajo(ModoDescriptor.Mantenimiento)
        )
        {

        }

        public IActionResult CrudDeErroresDeUnTrabajo()
        {
            return ViewCrud();
        }

        protected override void AntesDeEjecutar_ModificarPorId(string elementoJson)
        {
            throw new Exception("Los mensajes de errores de un trabajo no son modificables");
        }

        protected override void AntesDeEjecutar_CrearElemento(string elementoJson)
        {
            throw new Exception("No se pueden crear errores en un trabajo");
        }

    }
}