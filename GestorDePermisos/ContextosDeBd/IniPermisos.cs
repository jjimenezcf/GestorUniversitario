using System.Linq;

namespace Gestor.Elementos.Seguridad
{
    public class IniPermisos
    {

        public static void CrearDatosIniciales(CtoSeguridad ctoPermisos)
        {

            if (!ctoPermisos.Permisos.Any())
            {
                var permisos = new PermisoDtm[]
                {

                };
                foreach (PermisoDtm permiso in permisos)
                {
                    ctoPermisos.Permisos.Add(permiso);
                }
                ctoPermisos.SaveChanges();
            }

            if (!ctoPermisos.PermisosDeUnRol.Any())
            {
                var permisosDeUnRol = new RolPermisoDtm[]
                {
                };
                foreach (RolPermisoDtm permisoDelRol in permisosDeUnRol)
                {
                    ctoPermisos.PermisosDeUnRol.Add(permisoDelRol);
                }
                ctoPermisos.SaveChanges();
            }
        }
    }
}
