

using Enumerados;
using ModeloDeDto.Seguridad;

namespace ModeloDeDto.Entorno
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5)]
    public class PermisosDeUnUsuarioDto : ElementoDto
    {
        public static string ExpresionElemento = nameof(Permiso);

        [IUPropiedad(Etiqueta = "Usuario",
            Ayuda = "permisos de un usuario",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            MostrarExpresion = nameof(Usuario),
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdUsuario { get; set; }

        [IUPropiedad(
            Etiqueta = "Usuario",
            SiempreVisible = false
            )
        ]
        public string Usuario { get; set; }


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
            PorAnchoMnt = 30
            )
        ]
        public string Permiso { get; set; }

        [IUPropiedad(
            Etiqueta = "Origen",
            Ayuda = "Origen del permiso",
            TipoDeControl = enumTipoControl.Editor,
            PorAnchoMnt = 60
            )
        ]
        public string Origen { get; set; }
    }


}
