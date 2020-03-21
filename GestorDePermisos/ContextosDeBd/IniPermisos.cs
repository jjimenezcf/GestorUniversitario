using System.Linq;

namespace Gestor.Elementos.Seguridad
{
    public class IniPermisos
    {

        public static void CrearDatosIniciales(CtoPermisos ctoPermisos)
        {

            if (!ctoPermisos.Permisos.Any())
            {
                var permisos = new RegPermiso[]
                {

                };
                foreach (RegPermiso permiso in permisos)
                {
                    ctoPermisos.Permisos.Add(permiso);
                }
                ctoPermisos.SaveChanges();
            }

            if (!ctoPermisos.PermisosDeUnRol.Any())
            {
                var permisosDeUnRol = new RegRolPermisos[]
                {
                };
                foreach (RegRolPermisos permisoDelRol in permisosDeUnRol)
                {
                    ctoPermisos.PermisosDeUnRol.Add(permisoDelRol);
                }
                ctoPermisos.SaveChanges();
            }
        }
    }
}
