

namespace ModeloDeDto.Seguridad
{
    [IUDto(ExpresionNombre = "[Permiso]")]
    public class PermisosDeUnUsuarioDto : ElementoDto
    {
        [IUPropiedad(Etiqueta = "Usuario",
            Ayuda = "permisos de un usuario",
            TipoDeControl = TipoControl.RestrictorDeEdicion,
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdUsuario { get; set; }

        [IUPropiedad(
            Etiqueta = "Usuario",
            Visible = false
            )
        ]
        public string Usuario { get; set; }


        [IUPropiedad(
            Etiqueta = "Id del permiso",
            Visible = false
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
            PorAnchoMnt = 15
            )
        ]
        public string Permiso { get; set; }
    }


}
