using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Seguridad
{
    public static class PermisoPor
    {
        public static string Nombre = FiltroPor.Nombre;
        public static string PermisoDeUnRol = nameof(PermisoDeUnRol).ToLower();
        public static string PermisosDeUnUsuario = nameof(PermisosDeUnUsuario).ToLower();
    }

    [IUDto(AnchoEtiqueta = 20
          ,AnchoSeparador = 5
          ,EdicionEnModal = true)]
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
            TipoDeControl = TipoControl.ListaDeElemento,
            SeleccionarDe = nameof(ClasePermisoDto),
            GuardarEn = nameof(IdClase),
            MostrarPropiedad = nameof(Nombre),
            CargaDinamica = true,
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
            TipoDeControl = TipoControl.ListaDeElemento,
            SeleccionarDe = nameof(TipoPermisoDto),
            GuardarEn = nameof(IdTipo),
            MostrarPropiedad = nameof(Nombre),
            Fila = 1,
            Columna = 1,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Tipo { get; set; }

    }
}
