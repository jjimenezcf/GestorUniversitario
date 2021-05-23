using System;
using System.Collections.Generic;
using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Entorno;
using ModeloDeDto.Entorno;
using GestoresDeNegocio.Entorno;
using GestorDeElementos;

namespace MVCSistemaDeElementos.Controllers
{
    public class MenusController : EntidadController<ContextoSe, MenuDtm, MenuDto>
    {
        public GestorDeMenus GestorDeMenus { get; set; }

        public MenusController(GestorDeMenus gestorDeMenus, GestorDeErrores gestorDeErrores)
        : base
        (
          gestorDeMenus,
          gestorDeErrores          
        )
        {
            GestorDeMenus = gestorDeMenus;
        }

        public IActionResult CrudMenu()
        {
            Descriptor = new DescriptorDeMenu(Contexto, ModoDescriptor.Mantenimiento);
            return ViewCrud();
        }


        protected override dynamic CargarLista(string claseElemento, enumNegocio negocio, List<ClausulaDeFiltrado> filtro)
        {
            if (claseElemento == nameof(MenuDto))
                return ((GestorDeMenus)GestorDeElementos).LeerPadres();

            return base.CargarLista(claseElemento, negocio, filtro);
        }


        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, ClausulaDeFiltrado filtro)
        {
            if (claseElemento == nameof(VistaMvcDto))
                return GestorDeVistaMvc.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerVistas(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            if (claseElemento == nameof(MenuDto))
                return GestorDeMenus.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerMenus(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);

        }


    }
}