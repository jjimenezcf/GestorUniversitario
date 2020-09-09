using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Seguridad;
using ModeloDeDto.Seguridad;
using ModeloDeDto.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class UsuariosDeUnPuestoController :  EntidadController<ContextoSe, PuestosDeUnUsuarioDtm, UsuariosDeUnPuestoDto>
    {

        public UsuariosDeUnPuestoController(GestorDeUsuariosDeUnPuesto gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores,
           new DescriptorDeUsuariosDeUnPuesto(ModoDescriptor.Mantenimiento)
         )
        {
        }

        [HttpPost]
        public IActionResult CrudUsuariosDeUnPuesto(string restrictor, string orden)
        {
            return ViewCrud();
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        {
            if (claseElemento == nameof(UsuarioDto))
                return ((GestorDeUsuariosDeUnPuesto)GestorDeElementos).LeerUsuarios(posicion, cantidad, filtro);

            if (claseElemento == nameof(PuestoDto))
                return ((GestorDeUsuariosDeUnPuesto)GestorDeElementos).LeerPuestos(posicion, cantidad, filtro);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }
    }
}
