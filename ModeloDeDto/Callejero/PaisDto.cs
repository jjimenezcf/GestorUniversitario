namespace ModeloDeDto.Callejero
{
    [IUDto(AnchoEtiqueta = 20
          , AnchoSeparador = 5
          , MostrarExpresion = nameof(PaisDto.Nombre))]
    public class PaisDto : ElementoDto
    {
        [IUPropiedad(
          Etiqueta = "País",
          Ayuda = "Indique el nombre del país",
          Tipo = typeof(string),
          Fila = 0,
          Columna = 0,
          Ordenar = true,
          PorAnchoMnt = 50
          )
        ]
        public string Nombre { get; set; }


        [IUPropiedad(
          Etiqueta = "Código",
          Ayuda = "Asigne el código de país",
          Tipo = typeof(string),
          Fila = 2,
          Columna = 0
          )
        ]
        public string Codigo { get; set; }
    }
}
