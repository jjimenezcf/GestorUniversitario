using Enumerados;
using ModeloDeDto.Entorno;

namespace ModeloDeDto.Seguridad
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5)]
    public class UsuariosDeUnPuestoDto: ElementoDto
    {
        public static string ExpresionElemento = nameof(Usuario);

        [IUPropiedad(
            Etiqueta = "Puesto",
            Ayuda = "Usuarios de un puesto",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            MostrarExpresion = nameof(Puesto),
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdPuesto { get; set; }


        [IUPropiedad(
            Etiqueta = "Puesto",
            Visible = false
            )
        ]
        public string Puesto { get; set; }

        [IUPropiedad(
            Etiqueta = "Id del usuario",
            Visible = false
            )
        ]
        public int IdUsuario { get; set; }

        [IUPropiedad(
            Etiqueta = "Usuario",
            Ayuda = "Indique el usuario",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(UsuarioDto),
            GuardarEn = nameof(IdUsuario),
            Fila = 1,
            Columna = 0,
            Ordenar = true
            )
        ]
        public string Usuario { get; set; }

    }
}
