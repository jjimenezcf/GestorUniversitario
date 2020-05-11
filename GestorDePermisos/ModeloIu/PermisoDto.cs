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
            Etiqueta = "Permiso",
            Ayuda = "De un nombre al permiso",
            Tipo = typeof(string),
            Fila = 0,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt =50
            )
        ]
        public string Nombre { get; set; }

        [IUPropiedad(
            Etiqueta = "Id de la clase de permiso",
            Visible = false
            )
        ]
        public string IdClase { get; set; }

        [IUPropiedad(
            Etiqueta = "Clase",
            Ayuda = "Indique clase de permiso",
            TipoDeControl = TipoControl.SelectorDeElemento,
            SeleccionarDe = nameof(ClasePermisoDto),
            GuardarEn = nameof(IdClase),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Clase { get; set; }

        [IUPropiedad(
            Etiqueta = "Id del tipo de permiso",
            Visible = false
            )
        ]
        public string IdTipo { get; set; }

        [IUPropiedad(
            Etiqueta = "Tipo",
            Ayuda = "Indique el tipo a aplicar",
            TipoDeControl = TipoControl.SelectorDeElemento,
            SeleccionarDe = nameof(TipoPermisoDto),
            GuardarEn = nameof(IdTipo),
            Fila = 1,
            Columna = 1,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Tipo { get; set; }

    }
}
