using Enumerados;

namespace ModeloDeDto.Seguridad
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5)]
    public class RolesDeUnPuestoDto : ElementoDto
    {

        public static string ExpresionElemento = nameof(Rol);

        [IUPropiedad(Etiqueta = "Puesto",
            Ayuda = "Roles de un puesto",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            MostrarExpresion = nameof(Puesto),
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdPuesto { get; set; }

        [IUPropiedad(
            Etiqueta = "Puesto de trabajo",
            SiempreVisible = false
            )
        ]
        public string Puesto { get; set; }

        [IUPropiedad(
            Etiqueta = "Id del rol",
            SiempreVisible = false
            )
        ]
        public int IdRol { get; set; }


        [IUPropiedad(
            Etiqueta = "Rol",
            Ayuda = "Indique el rol",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(RolDto),
            GuardarEn = nameof(IdRol),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Rol { get; set; }


    }
}
