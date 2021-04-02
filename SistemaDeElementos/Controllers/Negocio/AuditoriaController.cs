using Microsoft.AspNetCore.Mvc;
using ServicioDeDatos;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using GestoresDeNegocio.Negocio;
using ModeloDeDto.Negocio;
using ServicioDeDatos.Negocio;
using Utilidades;
using AutoMapper;
using System.Collections.Generic;
using System.Reflection;
using ServicioDeDatos.Seguridad;
using System;
using GestorDeElementos;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Callejero;
using System.Linq;
using Newtonsoft.Json;
using GestoresDeNegocio.Entorno;
using ServicioDeDatos.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class AuditoriaController : BaseController<AuditoriaDto>
    {
        private ContextoSe Contexto { get; }
        private IMapper Mapeador { get; }

        public AuditoriaController(ContextoSe contexto, IMapper mapeador, GestorDeErrores gestorDeErrores)
        : base(gestorDeErrores, contexto.DatosDeConexion)
        {
            Contexto = contexto;
            Mapeador = mapeador;
            Contexto.Mapeador = mapeador;
            Contexto.IniciarTraza();
            Descriptor = new DescriptorDeAuditoria(ModoDescriptor.Mantenimiento);
        }


        public IActionResult CrudDeAuditoria(string negocio)
        {
            ApiController.CumplimentarDatosDeUsuarioDeConexion(Contexto, Mapeador, HttpContext);
            Descriptor.GestorDeUsuario = GestorDeUsuarios.Gestor(Contexto, Mapeador);
            Descriptor.UsuarioConectado = Descriptor.GestorDeUsuario.LeerRegistroCacheado(nameof(UsuarioDtm.Login), DatosDeConexion.Login);

            var destino = $"{(Descriptor.RutaBase.IsNullOrEmpty() ? "" : $"../{Descriptor.RutaBase}/")}{Descriptor.Vista}";
            if (!this.ExisteLaVista(destino))
                return RenderMensaje($"La vista {destino} no está definida");

            string nombreDeLaVista = ControllerContext.RouteData.Values["action"].ToString();
            string nombreDelControlador = ControllerContext.RouteData.Values["controller"].ToString();

            if (!Descriptor.UsuarioConectado.EsAdministrador)
            {
                var hayPermisos = Descriptor.GestorDeUsuario.TienePermisoFuncional(Descriptor.UsuarioConectado, $"{nombreDelControlador}.{nombreDeLaVista}");
                if (!hayPermisos)
                    return RenderMensaje($"Solicite permisos de acceso a {destino}");

                hayPermisos = Descriptor.GestorDeUsuario.TienePermisoDeDatos(Descriptor.UsuarioConectado, enumModoDeAccesoDeDatos.Consultor, Descriptor.Negocio);
                if (!hayPermisos)
                    return RenderMensaje($"Solicite al menos permisos de consulta sobre los elementos de negocio {NegociosDeSe.ToString(Descriptor.Negocio)}");
            }


            Descriptor.GestorDeNegocio = GestorDeNegocios.Gestor(Contexto, Mapeador);
            Descriptor.negocioDtm = GestorDeNegocios.LeerNegocio(Contexto, NegociosDeSe.Negocio(negocio));

            ViewBag.DatosDeConexion = DatosDeConexion;

            return base.View(destino, Descriptor);
        }


        //END-POINT: Desde GridDeDatos.ts
        public JsonResult epLeerDatosParaElGrid(string modo, string accion, string posicion, string cantidad, string filtro, string orden)
        {
            var r = new Resultado();
            int pos = posicion.Entero();
            int can = cantidad.Entero();
            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(Contexto, Mapeador, HttpContext); 
                List<ClausulaDeFiltrado> filtros = JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtro);
                var restrictor = ObtenerRestrictores(filtros);
                var opcionesDeMapeo = new Dictionary<string, object>();
                var negocioDtm = GestorDeNegocios.LeerNegocio(Contexto, restrictor.idNegocio);
                var elementos = AuditoriaDeNegocio.LeerElementos(Contexto, NegociosDeSe.Negocio(negocioDtm.Nombre), restrictor.idElemento, pos, can);
                //si no he leido nada por estar al final, vuelvo a leer los últimos
                if (pos > 0 && elementos.Count() == 0)
                {
                    pos = pos - can;
                    if (pos < 0) pos = 0;
                    elementos = AuditoriaDeNegocio.LeerElementos(Contexto, NegociosDeSe.Negocio(negocioDtm.Nombre), restrictor.idElemento, pos, can);
                    r.Mensaje = "No hay más elementos";
                }

                var infoObtenida = new ResultadoDeLectura();

                infoObtenida.registros = ElementosLeidos(elementos.ToList());
                infoObtenida.total = AuditoriaDeNegocio.ContarElementos(Contexto, NegociosDeSe.Negocio(negocioDtm.Nombre), restrictor.idElemento);
                infoObtenida.posicion = pos;
                infoObtenida.cantidad = can;

                r.Datos = infoObtenida;
                r.Estado = enumEstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                r.Estado = enumEstadoPeticion.Error;
                r.consola = GestorDeErrores.Detalle(e);
                if (e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true)
                    r.Mensaje = e.Message;
                else
                    r.Mensaje = $"No se ha podido recuperar datos para el grid. {(e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true ? e.Message : "")}";
            }

            var a = new JsonResult(r);
            return a;
        }

        private static (int idNegocio, int idElemento) ObtenerRestrictores(List<ClausulaDeFiltrado> filtros)
        {
            var idNegocio = 0;
            var idElemento = 0;
            foreach (var f in filtros)
            {
                if (f.Clausula == NegocioPor.idNegocio)
                    idNegocio = f.Valor.Entero();
                if (f.Clausula == nameof(AuditoriaDto.IdElemento).ToLower())
                    idElemento = f.Valor.Entero();
            }

            if (idNegocio == 0)
                GestorDeErrores.Emitir("Debe indicar el negocio del que se quiere obtener la auditoria");
            if (idElemento == 0)
                GestorDeErrores.Emitir("Debe indicar el elemento del que se quiere obtener la auditoria");
            return (idNegocio, idElemento);
        }

        private List<Dictionary<string, object>> ElementosLeidos(List<AuditoriaDto> auditorias)
        {
            var listaDeElementos = new List<Dictionary<string, object>>();
            if (auditorias.Count > 0)
            {
                PropertyInfo[] propiedades = auditorias[0].GetType().GetProperties();

                foreach (AuditoriaDto elemento in auditorias)
                {
                    var registro = new Dictionary<string, object>();
                    foreach (PropertyInfo propiedad in propiedades)
                    {
                        object valor = elemento.GetType().GetProperty(propiedad.Name).GetValue(elemento);
                        registro[propiedad.Name] = valor == null ? "" : valor;
                    }
                    registro[nameof(Resultado.ModoDeAcceso)] = enumModoDeAccesoDeDatos.Consultor.Render();
                    listaDeElementos.Add(registro);
                }
            }

            return listaDeElementos;
        }

        protected override void Dispose(bool disposing)
        {
            Contexto.CerrarTraza();
            base.Dispose(disposing);
        }

    }

}
