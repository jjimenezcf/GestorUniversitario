﻿

using Enumerados;

namespace ModeloDeDto.Seguridad
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5)]
    public class PermisosDeUnRolDto : ElementoDto
    {
        public static string ExpresionElemento = nameof(Permiso); 

        [IUPropiedad(Etiqueta = "Rol",
            Ayuda = "permisos de un rol",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            MostrarExpresion = nameof(Rol),
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
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(PermisoDto),
            GuardarEn = nameof(IdPermiso),
            Fila = 1,
            Columna = 0,
            Ordenar = true
            )
        ]
        public string Permiso { get; set; }
    }


}
