﻿using Microsoft.AspNetCore.Mvc;
using ServicioDeDatos;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Entorno;
using GestoresDeNegocio.Entorno;
using ModeloDeDto.Entorno;
using GestorDeElementos;
using ModeloDeDto.Seguridad;
using ServicioDeDatos.Seguridad;
using System.Collections.Generic;
using GestoresDeNegocio.Seguridad;
using System;

namespace MVCSistemaDeElementos.Controllers
{
    public class UsuariosController : EntidadController<ContextoSe, UsuarioDtm, UsuarioDto>
    {
        class UsuarioDeConexion
        {
            public int id { get; set; }
            public string login { get; set; }
            public string mail { get; set; }
            public string administrador { get; set; }
        }

        public UsuariosController(GestorDeUsuarios gestorDeUsuarios, GestorDeErrores gestorDeErrores)
        : base
        (
          gestorDeUsuarios,
          gestorDeErrores
        )
        {

        }


        public IActionResult CrudUsuario()
        {
            return ViewCrud(new DescriptorDeUsuario(Contexto, ModoDescriptor.Mantenimiento));
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            if (claseElemento == nameof(PuestoDto))
                return GestorDePuestosDeTrabajo.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPuestos(posicion, cantidad, filtros);

            if (claseElemento == nameof(RolDto))
                return GestorDeRoles.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerRoles(posicion, cantidad, filtros);

            if (claseElemento == nameof(PermisoDto))
                return GestorDePermisos.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPermisos(posicion, cantidad, filtros);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtros);
        }


        public JsonResult epLeerUsuarioDeConexion()
        {
            var r = new Resultado();
            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                var usuario = GestorDeElementos.LeerRegistroPorId(GestorDeElementos.Contexto.DatosDeConexion.IdUsuario, true, false, false, aplicarJoin: false);
                r.consola = $"registro de usuario de conexión leido correctamente";
                r.Estado = enumEstadoPeticion.Ok;
                r.Datos = new UsuarioDeConexion() { login = usuario.Login, id = usuario.Id, mail = usuario.eMail, administrador = DatosDeConexion.EsAdministrador ? "S" : "N" };
            }
            catch (Exception e)
            {
                r.Estado = enumEstadoPeticion.Error;
                r.consola = GestorDeErrores.Detalle(e);
                r.Mensaje = $"Error al leer el usuario de conexión. {(e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true ? e.Message : "")}";
            }

            return new JsonResult(r);
        }



    }

}
