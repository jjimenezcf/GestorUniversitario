using Microsoft.AspNetCore.Mvc;
using ServicioDeDatos;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.TrabajosSometidos;
using ModeloDeDto.TrabajosSometidos;
using GestoresDeNegocio.TrabajosSometidos;
using ModeloDeDto.Entorno;
using GestorDeElementos;
using GestoresDeNegocio.Entorno;
using System.Collections.Generic;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Controllers
{
    public class TrabajosDeUsuarioController : EntidadController<ContextoSe, TrabajoDeUsuarioDtm, TrabajoDeUsuarioDto>
    {

        public TrabajosDeUsuarioController(GestorDeTrabajosDeUsuario gestorDeNegocios, GestorDeErrores gestorDeErrores)
        :base
        (
          gestorDeNegocios, 
          gestorDeErrores, 
          new DescriptorDeTrabajosDeUsuario(ModoDescriptor.Mantenimiento)
        )
        {

        }
                
        public IActionResult CrudDeTrabajosDeUsuario()
        {
            return ViewCrud();
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, ClausulaDeFiltrado filtro)
        {
            if (claseElemento == nameof(UsuarioDto))
                return GestorDeUsuarios.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerUsuarios(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            if (claseElemento == nameof(TrabajoSometidoDto))
                return GestorDePuestosDeTrabajo.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPuestos(posicion, cantidad, new List<ClausulaDeFiltrado>() { filtro });

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtro);
        }
    }

}
