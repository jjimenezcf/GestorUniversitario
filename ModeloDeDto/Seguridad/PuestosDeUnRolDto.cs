using Enumerados;

namespace ModeloDeDto.Seguridad
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5)]
    public class PuestosDeUnRolDto : ElementoDto
    {
        public static string ExpresionElemento = nameof(Puesto);

        [IUPropiedad(Etiqueta = "Rol",
            Ayuda = "Puesto de un rol",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            MostrarExpresion = nameof(Rol),
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdRol { get; set; }

        [IUPropiedad(
            Etiqueta = "Rol",
            Visible = false
            )
        ]
        public string Rol { get; set; }

        [IUPropiedad(
            Etiqueta = "Id del puesto",
            Visible = false
            )
        ]
        public int IdPuesto { get; set; }


        [IUPropiedad(
            Etiqueta = "Puesto",
            Ayuda = "Indique el puesto",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(RolDto),
            GuardarEn = nameof(IdRol),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Puesto { get; set; }


    }
}
