

namespace ModeloDeDto.Seguridad
{
    [IUDto(ExpresionNombre = "[Permiso]")]
    public class PermisosDeUnRolDto : ElementoDto
    {
        [IUPropiedad(Etiqueta = "Rol",
            Ayuda = "permisos de un rol",
            TipoDeControl = TipoControl.RestrictorDeEdicion,
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdRol { get; set; }

        [IUPropiedad(
            Etiqueta = "rol",
            Visible = false
            )
        ]
        public string Rol { get; set; }


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
