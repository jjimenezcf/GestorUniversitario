using System;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto.Callejero;
using ModeloDeDto.Entorno;
using ModeloDeDto.Negocio;
using ModeloDeDto.Seguridad;
using ModeloDeDto.TrabajosSometidos;
using ServicioDeDatos.Callejero;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Negocio;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos.TrabajosSometidos;

namespace GestoresDeNegocio.Negocio
{
    public static class PersistenciaDeNegocios
    {
        public static void PersistirNegocios(GestorDeNegocios gestor)
        {
            gestor.Contexto.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            gestor.Contexto.IniciarTraza(nameof(PersistirNegocios));
            try
            {
                gestor.Contexto.DatosDeConexion.CreandoModelo = true;
                CrearNegocioSiNoExiste(gestor, enumNegocio.Usuario, "Usuarios", typeof(UsuarioDtm), typeof(UsuarioDto), "usuario.svg");
                CrearNegocioSiNoExiste(gestor, enumNegocio.VistaMvc, "Vistas", typeof(VistaMvcDtm), typeof(VistaMvcDto), "vista.svg");
                CrearNegocioSiNoExiste(gestor, enumNegocio.Variable, "Variables", typeof(VariableDtm), typeof(VariableDto), "cog-solid.svg");
                CrearNegocioSiNoExiste(gestor, enumNegocio.Menu, "Menus", typeof(MenuDtm), typeof(MenuDto), "funcionalidad-3.svg");
                CrearNegocioSiNoExiste(gestor, enumNegocio.Puesto, "Puestos", typeof(PuestoDtm), typeof(PuestoDto), "puestoDeTrabajo.svg");
                CrearNegocioSiNoExiste(gestor, enumNegocio.Permiso, "Permisos", typeof(PermisoDtm), typeof(PermisoDto), "acceso.svg");
                CrearNegocioSiNoExiste(gestor, enumNegocio.Negocio, "Negocios", typeof(NegocioDtm), typeof(NegocioDto), "red.svg");
                CrearNegocioSiNoExiste(gestor, enumNegocio.Rol, "Roles", typeof(RolDtm), typeof(RolDto), "roles.svg");
                CrearNegocioSiNoExiste(gestor, enumNegocio.Pais, "Paises", typeof(PaisDtm), typeof(PaisDto), "paises_1.svg");
                CrearNegocioSiNoExiste(gestor, enumNegocio.Provincia, "Provincias", typeof(ProvinciaDtm), typeof(ProvinciaDto), "provincias_1.svg");
                CrearNegocioSiNoExiste(gestor, enumNegocio.Correo, "Correos", typeof(CorreoDtm), typeof(CorreoDto), "Correo_1.svg");
                CrearNegocioSiNoExiste(gestor, enumNegocio.Municipio, "Municipios", typeof(MunicipioDtm), typeof(MunicipioDto), "");
            }
            finally
            {
                gestor.Contexto.DatosDeConexion.CreandoModelo = false;
            }
        }

        private static NegocioDtm CrearNegocioSiNoExiste(GestorDeNegocios gestor, enumNegocio negocio, string nombre, Type dtm, Type dto, string icono)
        {
            var negocioDtm = gestor.LeerNegocio(negocio, errorSiNoHay: false);
            if (negocioDtm == null)
            {
                negocioDtm = CrearNegocio(gestor, negocio, nombre,  dtm, dto, icono);
            }
            else
            {
                negocioDtm = ActualizarNegocio(gestor, negocioDtm, dtm, dto);
            }
            return negocioDtm;
        }

        private static NegocioDtm CrearNegocio(GestorDeNegocios gestor, enumNegocio negocio, string nombre, Type dtm, Type dto, string icono)
        {
            var negocioDtm = new NegocioDtm();
            negocioDtm.Enumerado = negocio.ToString();
            negocioDtm.Nombre = nombre;
            negocioDtm.ElementoDtm = dtm.FullName;
            negocioDtm.ElementoDto = dto.FullName;
            negocioDtm.Icono = icono;
            negocioDtm.Activo = true;
            var p = new ParametrosDeNegocio(enumTipoOperacion.Insertar);
            return gestor.PersistirRegistro(negocioDtm, p);
        }

        private static NegocioDtm ActualizarNegocio(GestorDeNegocios gestor, NegocioDtm leido, Type dtm, Type dto)
        {
            leido.ElementoDtm = dtm.FullName;
            leido.ElementoDto = dto.FullName;

            var p = new ParametrosDeNegocio(enumTipoOperacion.Modificar);
            p.Parametros[NegociosDeSe.ActualizarSeguridad] = true;
            p.Parametros[GestorDeNegocios.Traqueado] = false;
            return gestor.PersistirRegistro(leido, p);
        }


    }
}
