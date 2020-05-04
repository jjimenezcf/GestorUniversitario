using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Seguridad
{
    public static class PermisoPor
    {
        public static string Nombre = FiltroPor.Nombre;
        public static string PermisoDeUnRol = nameof(PermisoDeUnRol).ToLower();
        public static string PermisosDeUnUsuario = nameof(PermisosDeUnUsuario).ToLower();
    }

    [IUDto(ClaseTypeScriptDeCreacion = "CrudCreacionPermiso"
     , AnchoEtiqueta = 20
     , AnchoSeparador = 5)]
    public class PermisoDto : Elemento
    {
        [IUPropiedad(
            Etiqueta = "Nombre permiso",
            Ayuda = "De un nombre al permiso",
            Tipo = typeof(string),
            Fila = 0,
            Columna = 0
            )
        ]
        public string Nombre { get; set; }

        [IUPropiedad(
            Etiqueta = "Id de la clase",
            Visible = false
            )
        ]
        public string IdClase { get; set; }

        [IUPropiedad(
            Etiqueta = "Clase",
            Ayuda = "Indique clase de permiso",
            Fila = 1,
            Columna = 0
            )
        ]
        public string Clase { get; set; }

        [IUPropiedad(
            Etiqueta = "Permiso",
            Ayuda = "Indique permiso a aplicar (gestor, consultor o administrador)",
            Fila = 2,
            Columna = 0
            )
        ]
        public string Permiso { get; set; }

    }
}
