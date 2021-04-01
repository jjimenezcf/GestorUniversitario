using Enumerados;

namespace ModeloDeDto.Seguridad
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5)]
    public class PuestosDeUnUsuarioDto: ElementoDto
    {
        public static string ExpresionElemento = nameof(Puesto);

        [IUPropiedad(
            Etiqueta = "Usuario",
            Ayuda = "Puestos del usuario",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            MostrarExpresion = nameof(Usuario),
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdUsuario { get; set; }


        [IUPropiedad(
            Etiqueta = "usuario",
            Visible = false
            )
        ]
        public string Usuario { get; set; }

        [IUPropiedad(
            Etiqueta = "Id del puesto de trabajo",
            Visible = false
            )
        ]
        public int IdPuesto { get; set; }

        [IUPropiedad(
            Etiqueta = "Puesto",
            Ayuda = "Indique el puesto de trabajo",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(PuestoDto),
            GuardarEn = nameof(IdPuesto),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Puesto { get; set; }

        [IUPropiedad(
            Etiqueta ="Roles del puesto",
            Visible = false,
            VisibleEnGrid = true)]
        public string RolesDeUnPuesto { get; set; }

    }
}
