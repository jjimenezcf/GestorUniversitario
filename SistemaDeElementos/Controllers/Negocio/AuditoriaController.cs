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
using ModeloDeDto.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    public class AuditoriaController : BaseController<AuditoriaDto>
    {
        public AuditoriaController(ContextoSe contexto, IMapper mapeador, GestorDeErrores gestorDeErrores)
        : base(gestorDeErrores, contexto, mapeador)
        {
        }


        public IActionResult CrudDeAuditoria(string negocio)
        {

            var descriptor = new DescriptorDeAuditoria(Contexto, ModoDescriptor.Mantenimiento);

            ApiController.CumplimentarDatosDeUsuarioDeConexion(Contexto, Mapeador, HttpContext);
            descriptor.GestorDeUsuario = GestorDeUsuarios.Gestor(Contexto, Mapeador);
            descriptor.UsuarioConectado = descriptor.GestorDeUsuario.LeerRegistroCacheado(nameof(UsuarioDtm.Login), DatosDeConexion.Login);

            var destino = $"{(descriptor.RutaBase.IsNullOrEmpty() ? "" : $"../{descriptor.RutaBase}/")}{descriptor.Vista}";
            if (!this.ExisteLaVista(destino))
                return RenderMensaje($"La vista {destino} no está definida");

            string nombreDeLaVista = ControllerContext.RouteData.Values["action"].ToString();
            string nombreDelControlador = ControllerContext.RouteData.Values["controller"].ToString();

            if (!descriptor.UsuarioConectado.EsAdministrador)
            {
                try
                {
                    var hayPermisos = descriptor.GestorDeUsuario.TienePermisoFuncional(descriptor.UsuarioConectado, $"{nombreDelControlador}.{nombreDeLaVista}");
                    if (!hayPermisos)
                        GestorDeErrores.Emitir($"Solicite permisos de acceso a {destino}");

                    hayPermisos = descriptor.GestorDeUsuario.TienePermisoDeDatos(descriptor.UsuarioConectado, enumModoDeAccesoDeDatos.Consultor, descriptor.Negocio);
                    if (!hayPermisos)
                        GestorDeErrores.Emitir($"Solicite al menos permisos de consulta sobre los elementos de negocio {NegociosDeSe.ToString(descriptor.Negocio)}");
                }
                catch(Exception e)
                {
                    return RenderMensaje(e.Message);
                }
            }


            descriptor.GestorDeNegocio = GestorDeNegocios.Gestor(Contexto, Mapeador);
            descriptor.negocioDtm = GestorDeNegocios.LeerNegocio(Contexto, NegociosDeSe.Negocio(negocio));

            ViewBag.DatosDeConexion = DatosDeConexion;

            return base.View(destino, descriptor);
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
                var negocioDtm = GestorDeNegocios.LeerNegocio(Contexto, restrictor.idNegocio);
                var modoAcceso = LeerModoAccesoAlNegocio(Contexto.DatosDeConexion.IdUsuario, NegociosDeSe.Negocio(negocioDtm.Nombre));
                if (modoAcceso == enumModoDeAccesoDeDatos.SinPermiso)
                    GestorDeErrores.Emitir($"El usuario {Contexto.DatosDeConexion.Login} no tiene acceso a los datos de auditoría del negocio {negocioDtm.Nombre}");

                var datos = ApiController.LeerDatosParaElGrid(
                    () => AuditoriaDeNegocio.LeerElementos(Contexto, NegociosDeSe.Negocio(negocioDtm.Nombre), restrictor.idElemento, restrictor.usuarios, pos, can)
                  , () => AuditoriaDeNegocio.ContarElementos(Contexto, NegociosDeSe.Negocio(negocioDtm.Nombre), restrictor.idElemento, restrictor.usuarios));

                var infoObtenida = new ResultadoDeLectura();
                infoObtenida.registros = ApiController.ElementosLeidos(Contexto, datos.elementos.ToList(), () => { return enumModoDeAccesoDeDatos.Consultor; });
                infoObtenida.total = datos.total;
                infoObtenida.posicion = pos;
                infoObtenida.cantidad = can;
                r.Datos = infoObtenida;
                r.Estado = enumEstadoPeticion.Ok;
                if (pos > 0 && datos.elementos.Count() == 0)
                    r.Mensaje = "No hay más elementos";
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e, r, "No se ha podido recuperar datos para el grid.");
            }

            var a = new JsonResult(r);
            return a;
        }

        /// <summary>
        /// por ahora no veo en Negocio.Negocio si tiene permiso de consulta de auditoría
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="negocio"></param>
        /// <returns></returns>
        protected override  enumModoDeAccesoDeDatos LeerModoAccesoAlNegocio(int idUsuario, enumNegocio negocio)
        {
            return base.LeerModoAccesoAlNegocio(idUsuario, negocio) == enumModoDeAccesoDeDatos.Administrador
                ? enumModoDeAccesoDeDatos.Consultor
                : enumModoDeAccesoDeDatos.SinPermiso;
        }

        protected override AuditoriaDto LeerPorId(int id, Dictionary<string,object> parametros)
        {
            if (!parametros.Keys.Contains(NegocioPor.idNegocio))
                GestorDeErrores.Emitir("Debe definir el negocio del que ha de leerse la auditroría");

            var id32 = Convert.ToInt32(parametros[NegocioPor.idNegocio]);

            var negocioDtm = GestorDeNegocios.LeerNegocio(Contexto, id32);

            return AuditoriaDeNegocio.LeerElemento(Contexto, NegociosDeSe.Negocio(negocioDtm.Nombre), id);
        }
        protected override enumModoDeAccesoDeDatos LeerModoDeAccesoAlElemento(AuditoriaDto elemento)
        {
            return base.LeerModoDeAccesoAlElemento(elemento) == enumModoDeAccesoDeDatos.Administrador
                ? enumModoDeAccesoDeDatos.Consultor
                : enumModoDeAccesoDeDatos.SinPermiso;
        }


        private static (int idNegocio, int idElemento, List<int> usuarios) ObtenerRestrictores(List<ClausulaDeFiltrado> filtros)
        {
            var idNegocio = 0;
            var idElemento = 0;
            var usuarios = new List<int>();
            foreach (var f in filtros)
            {
                if (f.Clausula == NegocioPor.idNegocio)
                    idNegocio = f.Valor.Entero();
                if (f.Clausula == nameof(AuditoriaDto.IdElemento).ToLower())
                    idElemento = f.Valor.Entero();
                if (f.Clausula == UsuariosPor.AlgunUsuario)
                    usuarios.Incluir(f.Valor);
            }

            if (idNegocio == 0)
                GestorDeErrores.Emitir("Debe indicar el negocio del que se quiere obtener la auditoria");
            if (idElemento == 0)
                GestorDeErrores.Emitir("Debe indicar el elemento del que se quiere obtener la auditoria");
            return (idNegocio, idElemento, usuarios);
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


    }

}
