using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using Gestor.Elementos.Seguridad;
using GestorDeSeguridad.ModeloIu;
using Gestor.Elementos.Entorno;

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
        public IActionResult CrudPuestoDeUnUsuario(string restrictor, string orden)
        {
            var elementosDto = LeerOrdenados(restrictor, orden);
            GestorDelCrud.Descriptor.MapearElementosAlGrid(elementosDto, cantidadPorLeer: 5, posicionInicial: 0);
            GestorDelCrud.Descriptor.TotalEnBd(Contar());
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
