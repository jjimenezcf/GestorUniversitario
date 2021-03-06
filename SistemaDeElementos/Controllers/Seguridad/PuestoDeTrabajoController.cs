﻿using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using ModeloDeDto.Seguridad;
using GestorDeElementos;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Controllers
{
    public class PuestoDeTrabajoController : EntidadController<ContextoSe, PuestoDtm, PuestoDto>
    {

        public PuestoDeTrabajoController(GestorDePuestosDeTrabajo gestorDePuesto, GestorDeErrores gestorDeErrores)
         : base
         (
           gestorDePuesto,
           gestorDeErrores
         )
        {
        }


        public IActionResult CrudPuestoDeTrabajo()
        {
            return ViewCrud(new DescriptorDePuestoDeTrabajo(Contexto, ModoDescriptor.Mantenimiento));
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
           return ((GestorDePuestosDeTrabajo)GestorDeElementos).LeerPuestos(posicion, cantidad, filtros);
        }

    }
}
