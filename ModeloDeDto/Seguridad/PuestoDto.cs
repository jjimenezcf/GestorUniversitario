namespace ModeloDeDto.Seguridad
{
    [IUDto(AnchoEtiqueta = 20
      , AnchoSeparador = 5)]
    public class PuestoDto : ElementoDto
    {
        public const string MostrarPuesto = "[Nombre]";

        [IUPropiedad(
            Etiqueta = "Puesto",
            Ayuda = "Nombre al puesto de trabajo",
            Tipo = typeof(string),
            Fila = 0,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 50
            )
        ]
        public string Nombre { get; set; }


        [IUPropiedad(
            Etiqueta = "Descripción",
            Ayuda = "Descripción del puesto de trabajo",
            SiempreVisible = false,
            VisibleEnEdicion = true,
            Tipo = typeof(string),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 50
            )
        ]
        public string Descripcion { get; set; }

        [IUPropiedad(
            Etiqueta = "Roles del puesto",
            SiempreVisible = false,
            VisibleEnGrid = true)]
        public string RolesDeUnPuesto { get; set; }
    }
}
