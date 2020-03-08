using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Seguridad
{
    public static class PermisoPor
    {
        public static string Nombre = FiltroPor.Nombre;
        public static string PermisoDeUnRol = nameof(PermisoDeUnRol).ToLower();
    }

    public class PermisoDto : Elemento
    {
        public string Nombre { get; set; }
        public int Clase { get; set; }
        public int Permiso { get; set; }

    }
}
