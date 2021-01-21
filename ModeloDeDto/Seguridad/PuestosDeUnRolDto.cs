namespace ModeloDeDto.Seguridad
{
    [IUDto(ExpresionNombre = "[Puesto]")]
    public class PuestosDeUnRolDto : ElementoDto
    {
        [IUPropiedad(Etiqueta = "Rol",
            Ayuda = "Puesto de un rol",
            TipoDeControl = TipoControl.RestrictorDeEdicion,
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdRol { get; set; }

        [IUPropiedad(
            Etiqueta = "Rol",
            SiempreVisible = false
            )
        ]
        public string Rol { get; set; }

        [IUPropiedad(
            Etiqueta = "Id del puesto",
            SiempreVisible = false
            )
        ]
        public int IdPuesto { get; set; }


        [IUPropiedad(
            Etiqueta = "Puesto",
            Ayuda = "Indique el puesto",
            TipoDeControl = TipoControl.ListaDinamica,
            SeleccionarDe = nameof(RolDto),
            GuardarEn = nameof(IdRol),
            MostrarExpresion = nameof(RolDto.Nombre),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Puesto { get; set; }


    }
}
