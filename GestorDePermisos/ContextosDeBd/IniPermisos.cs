using System.Linq;

namespace Gestor.Elementos.Seguridad
{
    public class IniPermisos
    {

        public static void CrearDatosIniciales(CtoPermisos ctoPermisos)
        {

            if (!ctoPermisos.Permisos.Any())
            {
                var permisos = new rPermiso[]
                {

                };
                foreach (rPermiso permiso in permisos)
                {
                    ctoPermisos.Permisos.Add(permiso);
                }
                ctoPermisos.SaveChanges();
            }

            if (!ctoPermisos.PermisosDeUnRol.Any())
            {
                var permisosDeUnRol = new rRolPermiso[]
                {
                };
                foreach (rRolPermiso permisoDelRol in permisosDeUnRol)
                {
                    ctoPermisos.PermisosDeUnRol.Add(permisoDelRol);
                }
                ctoPermisos.SaveChanges();
            }
        }
    }
}
