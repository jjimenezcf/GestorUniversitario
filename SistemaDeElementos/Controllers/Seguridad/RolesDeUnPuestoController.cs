using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Seguridad;
using ModeloDeDto.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{
    public class RolesDeUnPuestoController :  EntidadController<ContextoSe, RolesDeUnPuestoDtm, RolesDeUnPuestoDto>
    {

        public RolesDeUnPuestoController(GestorDeRolesDeUnPuesto gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores,
           new DescriptorDeRolesDeUnPuesto(ModoDescriptor.Mantenimiento)
         )
        {
        }

        [HttpPost]
        public IActionResult CrudRolesDeUnPuesto(string restrictor, string orden)
        {
            //var elementosDto = LeerOrdenados(restrictor, orden);
            //GestorDelCrud.Descriptor.MapearElementosAlGrid(elementosDto, cantidadPorLeer: 5, posicionInicial: 0);
            //GestorDelCrud.Descriptor.TotalEnBd(Contar());
            return ViewCrud();
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        {
            if (claseElemento == nameof(PuestoDto))
                return ((GestorDeRolesDeUnPuesto)GestorDeElementos).LeerPuestos(posicion, cantidad, filtro);

            if (claseElemento == nameof(RolDto))
                return ((GestorDeRolesDeUnPuesto)GestorDeElementos).LeerRoles(posicion, cantidad, filtro);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }
    }
}
