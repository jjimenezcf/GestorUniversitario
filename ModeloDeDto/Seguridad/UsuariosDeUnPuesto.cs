using ModeloDeDto.Entorno;

namespace ModeloDeDto.Seguridad
{
    [IUDto(ExpresionNombre = "[Usuario]")]
    public class UsuariosDeUnPuestoDto: ElementoDto
    {

        [IUPropiedad(
            Etiqueta = "Puesto",
            Ayuda = "Usuarios de un puesto",
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
            Etiqueta = "Id del usuario",
            SiempreVisible = false
            )
        ]
        public int IdUsuario { get; set; }

        [IUPropiedad(
            Etiqueta = "Usuario",
            Ayuda = "Indique el usuario",
            TipoDeControl = TipoControl.ListaDinamica,
            SeleccionarDe = nameof(UsuarioDto),
            GuardarEn = nameof(IdUsuario),
            MostrarExpresion = nameof(UsuarioDto.Nombre),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Usuario { get; set; }

    }
}
