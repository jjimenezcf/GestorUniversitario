using Enumerados;

namespace ModeloDeDto.Callejero
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5, MostrarExpresion = "[Codigo]")]
    public class CodigoPostalDto : ElementoDto
    {
        [IUPropiedad(
            Etiqueta = "CP",
            Ayuda = "Código postal",
            Tipo = typeof(string),
            Fila = 0,
            Columna = 0,
            Ordenar = true,
            Obligatorio = true,
            LongitudMaxima = 5,
            Alineada = Aliniacion.derecha
          )
        ]
        public string Codigo { get; set; }
    }
}
