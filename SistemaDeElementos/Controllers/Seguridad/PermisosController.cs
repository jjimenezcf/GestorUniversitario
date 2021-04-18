using Microsoft.AspNetCore.Mvc;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using GestoresDeNegocio.Seguridad;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos;
using ModeloDeDto.Seguridad;
using GestorDeElementos;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Controllers
{

    public class PermisosController : EntidadController<ContextoSe, PermisoDtm, PermisoDto>
    {
        public PermisosController(GestorDePermisos gestorDePermisos, GestorDeErrores gestorDeErrores)
        : base
        (
         gestorDePermisos,
         gestorDeErrores,
         new DescriptorDePermiso(ModoDescriptor.Mantenimiento)
        )
        {
        }

        public IActionResult CrudPermiso()
        {
            return ViewCrud();
        }

        protected override dynamic CargarLista(string claseElemento, enumNegocio negocio, List<ClausulaDeFiltrado> filtros)
        {
            if (claseElemento == nameof(ClasePermisoDto))
                return ((GestorDePermisos)GestorDeElementos).LeerClases();

            if (claseElemento == nameof(TipoPermisoDto))
                return ((GestorDePermisos)GestorDeElementos).LeerTipos();

            return base.CargarLista(claseElemento, negocio, filtros);
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, ClausulaDeFiltrado filtro)
        {

            if (claseElemento == nameof(ClasePermisoDto))
                return ((GestorDePermisos)GestorDeElementos).LeerClases(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            return null;

        }
    }
}
