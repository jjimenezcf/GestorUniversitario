using Gestor.Elementos.ModeloIu;
using System;
using System.Linq;

namespace Gestor.Elementos.Entorno
{
    [IUDto(AnchoEtiqueta = 20
          , AnchoSeparador = 5)]
    public class VariableDto : ElementoDto
    {
        [IUPropiedad(
          Etiqueta = "Variable",
          Ayuda = "Indique el nombre de la variable",
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
          Ayuda = "Describa el uso de la variable",
          Tipo = typeof(string),
          Fila = 1,
          Columna = 0
          )
        ]
        public string Descripcion { get; set; }

        [IUPropiedad(
          Etiqueta = "Valor",
          Ayuda = "Asigne un valor a la variable",
          Tipo = typeof(string),
          Fila = 2,
          Columna = 0
          )
        ]
        public string Valor { get; set; }
    }
}
