using System;
using System.Linq;

namespace Gestor.Elementos.Permiso
{
    public class IniPermisos
    {

        public static void CrearDatosIniciales(CtoPermisos ctoPermisos)
        {

            if (!ctoPermisos.Permisos.Any())
            {
                var permisos = new PermisoReg[]
                {

                };
                foreach (PermisoReg permiso in permisos)
                {
                    ctoPermisos.Permisos.Add(permiso);
                }
                ctoPermisos.SaveChanges();
            }

            if (!ctoPermisos.PermisosDeUnRol.Any())
            {
                var permisosDeUnRol = new RolPermisoReg[]
                {
                };
                foreach (RolPermisoReg permisoDelRol in permisosDeUnRol)
                {
                    ctoPermisos.PermisosDeUnRol.Add(permisoDelRol);
                }
                ctoPermisos.SaveChanges();
            }
        }
    }
}
