

namespace ModeloDeDto.Seguridad
{
    [IUDto(ExpresionNombre = "[Permiso]")]
    public class PermisosDeUnPuestoDto : ElementoDto
    {
        [IUPropiedad(Etiqueta = "Id Puesto",
            Ayuda = "permisos de un puesto",
            TipoDeControl = TipoControl.RestrictorDeEdicion,
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
            TipoDeControl = TipoControl.ListaDinamica,
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
            TipoDeControl = TipoControl.Editor,
            PorAnchoMnt = 60
            )
        ]
        public string Roles { get; set; }
    }


}
