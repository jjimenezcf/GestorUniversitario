using System;
using System.Collections.Generic;
using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Entorno;
using ModeloDeDto.Entorno;
using GestoresDeNegocio.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class MenusController : EntidadController<ContextoSe, MenuDtm, MenuDto>
    {
        public GestorDeMenus GestorDeMenus { get; set; }

        public MenusController(GestorDeMenus gestorDeMenus, GestorDeErrores gestorDeErrores)
        : base
        (
          gestorDeMenus,
          gestorDeErrores,
          new DescriptorDeMenu(ModoDescriptor.Mantenimiento)
        )
        {
            GestorDeMenus = gestorDeMenus;
        }

        public IActionResult CrudMenu()
        {
            return ViewCrud();
        }


        protected override dynamic CargarLista(string claseElemento)
        {
            if (claseElemento == nameof(MenuDto))
                return ((GestorDeMenus)GestorDeElementos).LeerPadres();

            return null;
        }


        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        {
            if (claseElemento == nameof(VistaMvcDto))
                return ((GestorDeMenus)GestorDeElementos).LeerVistas(posicion, cantidad, filtro);

            if (claseElemento == nameof(MenuDto))
                return ((GestorDeMenus)GestorDeElementos).LeerMenus(posicion, cantidad, filtro);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);

        }


    }
}