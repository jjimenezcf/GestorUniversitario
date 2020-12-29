using System;

namespace ServicioDeDatos.Seguridad
{
    public enum enumClaseDePermiso
    {
        Tipo,
        Estado,
        Transicion,
        CentroGestor,
        Negocio,
        Elemento,
        Funcion,
        Vista,
        Menu
    }

    public enum enumModoDeAccesoDeDatos
    {
        Administrador,
        Gestor,
        Consultor,
        SinPermiso
    }

    public enum enumModoDeAccesoFuncional
    {
        Acceso,
        SinAcceso
    }

    public static class ModoDeAcceso
    {
        public static string ToString(enumModoDeAccesoDeDatos modoDeAcceso)
        {
            switch (modoDeAcceso)
            {
                case enumModoDeAccesoDeDatos.Administrador: return enumModoDeAccesoDeDatos.Administrador.ToString();
                case enumModoDeAccesoDeDatos.Gestor: return enumModoDeAccesoDeDatos.Gestor.ToString();
                case enumModoDeAccesoDeDatos.Consultor: return enumModoDeAccesoDeDatos.Consultor.ToString();
                case enumModoDeAccesoDeDatos.SinPermiso: return enumModoDeAccesoDeDatos.SinPermiso.ToString();
            }

            throw new Exception($"El modo de acceso de datos '{modoDeAcceso}' no está definido, no se puede parsear");
        }

        public static string ToString(enumModoDeAccesoFuncional modoDeAcceso)
        {
            switch (modoDeAcceso)
            {
                case enumModoDeAccesoFuncional.Acceso: return enumModoDeAccesoFuncional.Acceso.ToString();
                case enumModoDeAccesoFuncional.SinAcceso: return enumModoDeAccesoFuncional.SinAcceso.ToString();
            }

            throw new Exception($"El modo de acceso funcional '{modoDeAcceso}' no está definido, no se puede parsear");
        }
    }



}
