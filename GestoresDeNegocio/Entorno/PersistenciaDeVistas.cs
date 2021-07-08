using ModeloDeDto.Callejero;
using ModeloDeDto.Entorno;
using ServicioDeDatos.Entorno;
using ModeloDeDto.Seguridad;
using ModeloDeDto.TrabajosSometidos;
using ModeloDeDto.Negocio;

namespace GestoresDeNegocio.Entorno
{
    public static class PersistenciaDeVistas
    {
        public static void PersistirVistas(GestorDeVistaMvc gestor)
        {
            gestor.Contexto.IniciarTraza(nameof(PersistirVistas));
            try
            {
                gestor.Contexto.DatosDeConexion.CreandoModelo = true;
                CrearVistaSiNoExiste(gestor, "Usuarios del sistema", "Usuarios", "CrudUsuario", true, typeof(UsuarioDto).FullName);
                CrearVistaSiNoExiste(gestor, "Menú definidos", "Menus", "CrudMenu", false, typeof(MenuDto).FullName);
                CrearVistaSiNoExiste(gestor, "Permisos", "Permisos", "CrudPermiso", true, typeof(PermisoDto).FullName);
                CrearVistaSiNoExiste(gestor, "Variables de entorno", "Variables", "CrudVariable", true, typeof(VariableDto).FullName);
                CrearVistaSiNoExiste(gestor, "Vistas del sistema", "VistaMvc", "CrudVistaMvc", true, typeof(VistaMvcDto).FullName);
                CrearVistaSiNoExiste(gestor, "Puestos de trabajo", "PuestoDeTrabajo", "CrudPuestoDeTrabajo", true, typeof(PuestoDto).FullName);
                CrearVistaSiNoExiste(gestor, "Clases de permisos", "ClaseDePermiso", "CrudClaseDePermiso", true, typeof(ClasePermisoDto).FullName);
                CrearVistaSiNoExiste(gestor, "Puestos de trabajo de un usuario", "PuestosDeUnUsuario", "CrudPuestosDeUnUsuario", true, typeof(PuestosDeUnUsuarioDto).FullName);
                CrearVistaSiNoExiste(gestor, "Roles", "Rol", "CrudRol", true, typeof(RolDto).FullName);
                CrearVistaSiNoExiste(gestor, "Roles de un puesto", "RolesDeUnPuesto", "CrudRolesDeUnPuesto", true, typeof(RolesDeUnPuestoDto).FullName);
                CrearVistaSiNoExiste(gestor, "Permisos de un rol", "PermisosDeUnRol", "CrudPermisosDeUnRol", true, typeof(PermisosDeUnRolDto).FullName);
                CrearVistaSiNoExiste(gestor, "Permisos de un usuario", "PermisosDeUnUsuario", "CrudPermisosDeUnUsuario ", true, typeof(PermisosDeUnUsuarioDto).FullName);
                CrearVistaSiNoExiste(gestor, "Roles de un permiso", "RolesDeUnPermiso", "CrudRolesDeUnPermiso", false, typeof(RolesDeUnPermisoDto).FullName);
                CrearVistaSiNoExiste(gestor, "Puestos de un rol", "PuestosDeUnRol", "CrudPuestosDeUnRol ", false, typeof(PuestosDeUnRolDto).FullName);
                CrearVistaSiNoExiste(gestor, "Usuarios de un puesto", "UsuariosDeUnPuesto", "CrudUsuariosDeUnPuesto ", false, typeof(UsuariosDeUnPuestoDto).FullName);
                CrearVistaSiNoExiste(gestor, "Permisos de un puesto de trabajo", "PermisosDeUnPuesto", "CrudPermisosDeUnPuesto", false, typeof(PermisosDeUnPuestoDto).FullName);
                CrearVistaSiNoExiste(gestor, "Negocios de SE", "Negocio", "CrudDeNegocios", false, typeof(NegocioDto).FullName);
                CrearVistaSiNoExiste(gestor, "Paises", "Paises", "CrudPaises", true, typeof(PaisDto).FullName);
                CrearVistaSiNoExiste(gestor, "Trabajos sometidos", "TrabajosSometido", "CrudDeTrabajosSometido", true, typeof(TrabajoSometidoDto).FullName);
                CrearVistaSiNoExiste(gestor, "Trabajos de usuario", "TrabajosDeUsuario", "CrudDeTrabajosDeUsuario", false, typeof(TrabajoDeUsuarioDto).FullName);
                CrearVistaSiNoExiste(gestor, "Errores de un trabajo", "ErroresDeUnTrabajo", "CrudDeErroresDeUnTrabajo", true, typeof(ErrorDeUnTrabajoDto).FullName);
                CrearVistaSiNoExiste(gestor, "Traza de un trabajo", "TrazasDeUnTrabajo", "CrudDeTrazasDeUnTrabajo", true, typeof(TrazaDeUnTrabajoDto).FullName);
                CrearVistaSiNoExiste(gestor, "Provincias", "Provincias", "CrudProvincias", false, typeof(ProvinciaDto).FullName);
                CrearVistaSiNoExiste(gestor, "Auditoría de elementos", "Auditoria", "CrudDeAuditoria", false, typeof(AuditoriaDto).FullName);
                CrearVistaSiNoExiste(gestor, "Correos de usuario", "Correos", "CrudDeCorreos", false, typeof(CorreoDto).FullName);
                CrearVistaSiNoExiste(gestor, "Parámetros de Negocio", "ParametrosDeNegocio", "CrudDeParametrosDeNegocio", true, typeof(ParametroDeNegocioDto).FullName);
                CrearVistaSiNoExiste(gestor, "Municipios", "Municipios", "CrudMunicipios", true, typeof(MunicipioDto).FullName);
                CrearVistaSiNoExiste(gestor, "Tipos de vía", "TiposDeVia", "CrudTiposDeVia", true, typeof(TipoDeViaDto).FullName);
                CrearVistaSiNoExiste(gestor, "Codigos postales", "CodigosPostales", "CrudCodigosPostales", true, typeof(CodigoPostalDto).FullName);
                CrearVistaSiNoExiste(gestor, "Cps de una provincia", "CpsDeUnaProvincia", "CrudCpsDeUnaProvincia", true, typeof(CpsDeUnaProvinciaDto).FullName);
                CrearVistaSiNoExiste(gestor, "Cps de un municipio", "CpsDeUnMunicipio", "CrudCpsDeUnMunicipio", true, typeof(CpsDeUnMunicipioDto).FullName);
                CrearVistaSiNoExiste(gestor, "Calles", "Calles", "CrudCalles", true, typeof(CalleDto).FullName);
            }
            finally
            {
                gestor.Contexto.DatosDeConexion.CreandoModelo = false;
            }
        }

        private static VistaMvcDtm CrearVistaSiNoExiste(GestorDeVistaMvc gestor, string nombre, string controlador, string accion, bool modal, string elementoDto)
        {
            var v = gestor.LeerRegistroCacheado(nameof(VistaMvcDtm.Nombre), nombre, false, true, false);
            if (v == null)
                v = gestor.CrearVistaMvc(nombre, controlador, accion, modal, elementoDto);
            return v;
        }
    }
}
