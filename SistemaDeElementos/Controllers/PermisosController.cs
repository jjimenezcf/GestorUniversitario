using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using Gestor.Elementos.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVCSistemaDeElementos.Controllers
{

    public class PermisosController : EntidadController<CtoSeguridad, PermisoDtm, PermisoDto>
    {
        public PermisosController(GestorDePermisos gestorDePermisos, GestorDeErrores gestorDeErrores)
        : base
        (
         gestorDePermisos,
         gestorDeErrores,
         new CrudPermiso(ModoDescriptor.Mantenimiento)
        )
        {
        }

        public IActionResult CrudPermiso(string orden)
        {
            GestorDelCrud.Descriptor.MapearElementosAlGrid(LeerOrdenados(orden), cantidadPorLeer: 5, posicionInicial: 0);
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }

        protected override dynamic LeerTodos(string claseElemento)
        {
            if (claseElemento == nameof(ClasePermisoDto))
                return ((GestorDePermisos)GestorDeElementos).LeerClases();

            if (claseElemento == nameof(TipoPermisoDto))
                return ((GestorDePermisos)GestorDeElementos).LeerTipos();

            return null;
        }

    }
}
