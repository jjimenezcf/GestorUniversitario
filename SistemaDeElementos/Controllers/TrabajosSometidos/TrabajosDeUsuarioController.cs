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
using System.ComponentModel.DataAnnotations;

namespace MVCSistemaDeElementos.Controllers
{
    public class TrabajosDeUsuarioController : EntidadController<ContextoSe, TrabajoDeUsuarioDtm, TrabajoDeUsuarioDto>
    {

        public TrabajosDeUsuarioController(GestorDeTrabajosDeUsuario gestorDeTu, GestorDeErrores gestorDeErrores)
        :base
        (
          gestorDeTu, 
          gestorDeErrores
        )
        {

        }
                
        public IActionResult CrudDeTrabajosDeUsuario()
        {
            return ViewCrud(new DescriptorDeTrabajosDeUsuario(Contexto, ModoDescriptor.Mantenimiento));
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            if (claseElemento == nameof(UsuarioDto))
                return GestorDeUsuarios.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerUsuarios(posicion, cantidad,filtros);

            if (claseElemento == nameof(TrabajoSometidoDto))
                return GestorDeTrabajosSometido.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerTrabajos(posicion, cantidad,filtros);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtros);
        }

        public JsonResult epIniciarTrabajoDeUsuario(int idTrabajoUsuario)
        {
            var r = new Resultado();

            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                GestorDeTrabajosDeUsuario.Iniciar(GestorDeElementos.Contexto, idTrabajoUsuario, false);
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = "Trabajo iniciado";
            }
            catch (Exception e)
            {
                r.Estado = enumEstadoPeticion.Error;
                r.consola = GestorDeErrores.Detalle(e);
                if (e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true)
                    r.Mensaje = e.Message;
                else
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
                r.consola = GestorDeErrores.Detalle(e);
                if (e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true)
                    r.Mensaje = e.Message;
                else
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
                r.consola = GestorDeErrores.Detalle(e);
                if (e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true)
                    r.Mensaje = e.Message;
                else
                    r.Mensaje = $"Error al desbloquear el trabajo. {(e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true ? e.Message : "")}";
            }

            return new JsonResult(r);
        }
        public JsonResult epResometerTrabajoDeUsuario(int idTrabajoUsuario)
        {
            var r = new Resultado();

            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                GestorDeTrabajosDeUsuario.Resometer(GestorDeElementos.Contexto, idTrabajoUsuario);
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = "Trabajo resometido";
            }
            catch (Exception e)
            {
                r.Estado = enumEstadoPeticion.Error;
                r.consola = GestorDeErrores.Detalle(e);
                if (e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true)
                    r.Mensaje = e.Message;
                else
                    r.Mensaje = $"Error al resometer el trabajo. {(e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true ? e.Message : "")}";
            }

            return new JsonResult(r);
        }

        protected override ParametrosDeNegocio AntesDeEjecutar_CrearElemento(TrabajoDeUsuarioDto elemento)
        {
           var p = base.AntesDeEjecutar_CrearElemento(elemento);
            p.Parametros[GestorDeTrabajosDeUsuario.SolicitudDeUsuario] = true;
            
            if (elemento.Planificado == null)
               elemento.Planificado = DateTime.Now;

            if (!elemento.IdEjecutor.HasValue || elemento.IdEjecutor == 0)
                elemento.IdEjecutor = elemento.IdSometedor;

            return p;
        }

        protected override void AntesDeEjecutar_Leer(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros, List<ClausulaDeOrdenacion> ordenes)
        {
            base.AntesDeEjecutar_Leer(posicion, cantidad, filtros, ordenes);
            if (!DatosDeConexion.EsAdministrador)
            {
                var f = new ClausulaDeFiltrado { Clausula = nameof(TrabajoDeUsuarioDtm.IdEjecutor), Criterio = ModeloDeDto.CriteriosDeFiltrado.igual, Valor = DatosDeConexion.IdUsuario.ToString() };
                filtros.Add(f);
            }
            if (ordenes.Count == 0)
                ordenes.Add(new ClausulaDeOrdenacion { Modo = ModoDeOrdenancion.descendente, OrdenarPor = nameof(TrabajoDeUsuarioDtm.Planificado) });
        }

    }

}
