﻿using Microsoft.AspNetCore.Mvc;
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
    public class AuditoriaController : BaseController
    {
        private ContextoSe Contexto { get; }
        private IMapper Mapeador { get; }
        private DescriptorDeAuditoria Descriptor {get;}

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
                    return RenderMensaje($"Solicite al menos permisos de consulta sobre los elementos de negocio {Descriptor.Negocio}");
            }


            Descriptor.GestorDeNegocio = GestorDeNegocios.Gestor(Contexto, Mapeador);
            Descriptor.Negocio = NegociosDeSe.ParsearNegocio(negocio);
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
                var opcionesDeMapeo = new Dictionary<string, object>();
                var elementos = LeerAuditoria(pos, can, filtro, orden, opcionesDeMapeo);
                //si no he leido nada por estar al final, vuelvo a leer los últimos
                if (pos > 0 && elementos.Count() == 0)
                {
                    pos = pos - can;
                    if (pos < 0) pos = 0;
                    elementos = LeerAuditoria(pos, can, filtro, orden, opcionesDeMapeo);
                    r.Mensaje = "No hay más elementos";
                }

                var infoObtenida = new ResultadoDeLectura();

                infoObtenida.registros = ElementosLeidos(elementos.ToList());
                infoObtenida.total = accion == epAcciones.buscar.ToString() ? Contar(filtro) : Recontar(filtro);
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

        private IEnumerable<AuditoriaDtm> LeerAuditoria(int pos, int can, string filtro, string orden, Dictionary<string, object> opcionesDeMapeo)
        {
            List<ClausulaDeFiltrado> filtros = JsonConvert.DeserializeObject<List<ClausulaDeFiltrado>>(filtro);
            enumNegocio negocio = enumNegocio.No_Definido;
            var idElemento  = 0;

            foreach (var f in filtros)
            {
                if (f.Clausula == "negocio")
                    negocio = NegociosDeSe.ParsearNegocio(f.Valor);
                if (f.Clausula == "idElemento")
                    idElemento = f.Valor.Entero();
            }

            if (negocio == enumNegocio.No_Definido)
               GestorDeErrores.Emitir("Debe indicar el negocio del que se quiere obtener la auditoria");
            if (idElemento == 0)
                GestorDeErrores.Emitir("Debe indicar el elemento del que se quiere obtener la auditoria");

            var a = new AuditoriaDeElementos(Contexto, negocio);
            return a.LeerRegistros(idElemento);
        }

        private int Recontar(string filtro)
        {
            throw new NotImplementedException();
        }

        private int Contar(string filtro)
        {
            throw new NotImplementedException();
        }

        private List<Dictionary<string, object>> ElementosLeidos(List<AuditoriaDtm> auditorias)
        {
            var listaDeElementos = new List<Dictionary<string, object>>();
            if (auditorias.Count > 0)
            {
                PropertyInfo[] propiedades = auditorias[0].GetType().GetProperties();

                foreach (AuditoriaDtm elemento in auditorias)
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