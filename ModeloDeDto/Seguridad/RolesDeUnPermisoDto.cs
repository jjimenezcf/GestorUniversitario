﻿
namespace ModeloDeDto.Seguridad
{
    [IUDto(ExpresionNombre = "[Rol]")]
    public class RolesDeUnPermisoDto : ElementoDto
    {
        [IUPropiedad(Etiqueta = "Permiso",
            Ayuda = "Roles de un permiso",
            TipoDeControl = TipoControl.RestrictorDeEdicion,
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdPermiso { get; set; }

        [IUPropiedad(
            Etiqueta = "permiso",
            Visible = false
            )
        ]
        public string Permiso { get; set; }

        [IUPropiedad(
            Etiqueta = "Id del rol",
            Visible = false
                     )]
        public int IdRol { get; set; }

        [IUPropiedad(
            Etiqueta = "Rol",
            Ayuda = "Indique el rol",
            TipoDeControl = TipoControl.ListaDinamica,
            SeleccionarDe = nameof(RolDto),
            GuardarEn = nameof(IdRol),
            MostrarPropiedad = nameof(RolDto.Nombre),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Rol { get; set; }
    }


}