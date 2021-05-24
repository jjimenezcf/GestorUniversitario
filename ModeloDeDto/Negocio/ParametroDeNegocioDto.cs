using Enumerados;

namespace ModeloDeDto.Entorno
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5)]
    public class ParametroDeNegocioDto : ElementoDto
    {
        //--------------------------------------------
        [IUPropiedad(
            Etiqueta = "Negocio",
            Ayuda = "negocio del parámetro",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            MostrarExpresion = nameof(Negocio),
            Fila = 0,
            Columna = 0,
            EditableAlCrear = false,
            EditableAlEditar = false,
            VisibleEnGrid = false
            )
        ]
        public int IdNegocio { get; set; }

        //--------------------------------------------
        [IUPropiedad(Visible = false)]
        public string Negocio { get; set; }

        //--------------------------------------------
        [IUPropiedad(
          Etiqueta = "Parametro",
          Ayuda = "Indique el nombre del parámetro",
          Tipo = typeof(string),
          Fila = 0,
          Columna = 0,
          Ordenar = true,
          PorAnchoMnt = 50
          )
        ]
        public string Nombre { get; set; }

        //--------------------------------------------
        [IUPropiedad(
          Etiqueta = "Valor",
          Ayuda = "Asigne un valor al parámetro",
          Tipo = typeof(string),
          Fila = 2,
          Columna = 0
          )
        ]
        public string Valor { get; set; }
    }
}
