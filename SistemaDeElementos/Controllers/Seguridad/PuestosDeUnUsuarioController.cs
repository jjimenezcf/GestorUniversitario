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
    public class PuestosDeUnUsuarioController :  EntidadController<ContextoSe, PuestosDeUnUsuarioDtm, PuestosDeUnUsuarioDto>
    {

        public PuestosDeUnUsuarioController(GestorDePuestosDeUnUsuario gestor, GestorDeErrores errores)
         : base
         (
           gestor,
           errores,
           new DescriptorDePuestosDeUnUsuario(ModoDescriptor.Mantenimiento)
         )
        {
        }

        [HttpPost]
        public IActionResult CrudPuestosDeUnUsuario(string restrictor, string orden)
        {
            return ViewCrud();
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, string filtro)
        {
            if (claseElemento == nameof(UsuarioDto))
                return ((GestorDePuestosDeUnUsuario)GestorDeElementos).LeerUsuarios(posicion, cantidad, filtro);

            if (claseElemento == nameof(PuestoDto))
                return ((GestorDePuestosDeUnUsuario)GestorDeElementos).LeerPuestos(posicion, cantidad, filtro);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }
    }
}
