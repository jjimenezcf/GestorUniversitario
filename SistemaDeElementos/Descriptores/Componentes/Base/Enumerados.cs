
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum enumClaseOpcionMenu
    {
        DeVista,
        DeElemento,
        Basico
    }

    public static class ClaseOpcionMenu
    {
        public static string Render(enumClaseOpcionMenu clase)
        {
            switch (clase)
            {
                case enumClaseOpcionMenu.DeElemento: return "de-elemento";
                case enumClaseOpcionMenu.DeVista: return "de-vista";
                case enumClaseOpcionMenu.Basico: return "basico";
            }
            return "";
        }
    }

    public partial class ModoDeAccesoDeDatos
    {
        public static string Render(enumModoDeAccesoDeDatos modoDeAcceso)
        {
            return ModoDeAcceso.ToString(modoDeAcceso).ToLower();
        }
    }
}