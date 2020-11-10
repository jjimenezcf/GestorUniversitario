

namespace ModeloDeDto.Seguridad
{
    [IUDto(ExpresionNombre = "[Permiso]")]
    public class PermisosDeUnPuestoDto : ElementoDto
    {
        [IUPropiedad(Etiqueta = "Puesto",
            Ayuda = "permisos de un usuario",
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
            MostrarPropiedad = nameof(PermisoDto.Nombre),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 30,
            VisibleEnGrid = true,
            VisibleEnEdicion = false
            )
        ]
        public string Permiso { get; set; }

        [IUPropiedad(
            Etiqueta = "Origen",
            Ayuda = "Origen del permiso",
            VisibleEnGrid = true,
            VisibleEnEdicion = false,
            TipoDeControl = TipoControl.Editor,
            PorAnchoMnt = 60
            )
        ]
        public string Origen { get; set; }
    }


}
