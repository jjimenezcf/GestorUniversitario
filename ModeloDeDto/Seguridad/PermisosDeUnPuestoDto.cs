

using Enumerados;

namespace ModeloDeDto.Seguridad
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5)]
    public class PermisosDeUnPuestoDto : ElementoDto
    {
        public static string ExpresionElemento = nameof(Permiso);

        [IUPropiedad(Etiqueta = "Id Puesto",
            Ayuda = "permisos de un puesto",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdPuesto { get; set; }

        [IUPropiedad(
            Etiqueta = "Puesto",
            SiempreVisible = false
            )
        ]
        public string Puesto { get; set; }


        [IUPropiedad(
            Etiqueta = "Id del permiso",
            SiempreVisible = false
            )
        ]
        public int IdPermiso { get; set; }

        [IUPropiedad(
            Etiqueta = "Permiso",
            Ayuda = "Indique el permiso",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = nameof(PermisoDto),
            GuardarEn = nameof(IdPermiso),
            MostrarExpresion = nameof(PermisoDto.Nombre),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 30,
            VisibleEnEdicion = false,
            VisibleEnGrid = true
            )
        ]
        public string Permiso { get; set; }

        [IUPropiedad(
            Etiqueta = "Roles",
            Ayuda = "Origen del permiso",
            VisibleEnEdicion = false,
            VisibleEnGrid = true,
            TipoDeControl = enumTipoControl.Editor,
            PorAnchoMnt = 60
            )
        ]
        public string Roles { get; set; }
    }


}
