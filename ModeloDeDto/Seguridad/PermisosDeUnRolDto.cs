

using Enumerados;

namespace ModeloDeDto.Seguridad
{
    [IUDto(ExpresionNombre = "[Permiso]")]
    public class PermisosDeUnRolDto : ElementoDto
    {
        [IUPropiedad(Etiqueta = "Rol",
            Ayuda = "permisos de un rol",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdRol { get; set; }

        [IUPropiedad(
            Etiqueta = "rol",
            SiempreVisible = false
            )
        ]
        public string Rol { get; set; }


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
            PorAnchoMnt = 15
            )
        ]
        public string Permiso { get; set; }
    }


}
