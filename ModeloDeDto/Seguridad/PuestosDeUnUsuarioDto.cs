namespace ModeloDeDto.Seguridad
{
    [IUDto(ExpresionNombre = "[Puesto]")]
    public class PuestosDeUnUsuarioDto: ElementoDto
    {

        [IUPropiedad(
            Etiqueta = "Usuario",
            Ayuda = "Puestos del usuario",
            TipoDeControl = TipoControl.RestrictorDeEdicion,
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdUsuario { get; set; }


        [IUPropiedad(
            Etiqueta = "usuario",
            SiempreVisible = false
            )
        ]
        public string Usuario { get; set; }

        [IUPropiedad(
            Etiqueta = "Id del puesto de trabajo",
            SiempreVisible = false
            )
        ]
        public int IdPuesto { get; set; }

        [IUPropiedad(
            Etiqueta = "Puesto",
            Ayuda = "Indique el puesto de trabajo",
            TipoDeControl = TipoControl.ListaDinamica,
            SeleccionarDe = nameof(PuestoDto),
            GuardarEn = nameof(IdPuesto),
            MostrarPropiedad = nameof(PuestoDto.Nombre),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Puesto { get; set; }

        [IUPropiedad(
            Etiqueta ="Roles del puesto",
            SiempreVisible = false,
            VisibleEnGrid = true)]
        public string RolesDeUnPuesto { get; set; }

    }
}
