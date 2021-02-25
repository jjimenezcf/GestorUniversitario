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
using Newtonsoft.Json;
using System;

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

        public JsonResult epIniciarTrabajoDeUsuario(int idTrabajoUsuario)
        {
            var r = new Resultado();

            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                GestorDeTrabajosDeUsuario.Iniciar(GestorDeElementos.Contexto, idTrabajoUsuario);
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = "Trabajo iniciado";
            }
            catch (Exception e)
            {
                r.Estado = enumEstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = $"Error al iniciar el trabajo. {(e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true ? e.Message : "")}";
            }

            return new JsonResult(r);
        }
        public JsonResult epBloquearTrabajoDeUsuario(int idTrabajoUsuario)
        {
            var r = new Resultado();

            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                GestorDeTrabajosDeUsuario.Bloquear(GestorDeElementos.Contexto, idTrabajoUsuario);
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = "Trabajo bloqueado";
            }
            catch (Exception e)
            {
                r.Estado = enumEstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = $"Error al bloquear el trabajo. {(e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true ? e.Message : "")}";
            }

            return new JsonResult(r);
        }
        public JsonResult epDesbloquearTrabajoDeUsuario(int idTrabajoUsuario)
        {
            var r = new Resultado();

            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                GestorDeTrabajosDeUsuario.Desbloquear(GestorDeElementos.Contexto, idTrabajoUsuario);
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = "Trabajo bloqueado";
            }
            catch (Exception e)
            {
                r.Estado = enumEstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = $"Error al desbloquear el trabajo. {(e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true ? e.Message : "")}";
            }

            return new JsonResult(r);
        }

    }

}
