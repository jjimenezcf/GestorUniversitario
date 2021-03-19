﻿namespace ModeloDeDto.Callejero
{
    [IUDto(AnchoEtiqueta = 20
          , AnchoSeparador = 5
          , MostrarExpresion = "([Codigo]) [Nombre]")]
    public class PaisDto : ElementoDto
    {

        [IUPropiedad(
            Etiqueta = "Código",
            Ayuda = "Código de país de 3",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            LongitudMaxima = 3
          )
        ]
        public string Codigo { get; set; }

        //----------------------------------------------

        [IUPropiedad(
            Etiqueta = "País",
            Ayuda = "Indique el nombre del país",
            Tipo = typeof(string),
            Fila = 0,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 50,
            LongitudMaxima = 250
          )
        ]
        public string Nombre { get; set; }

        //----------------------------------------------

        [IUPropiedad(
            Etiqueta = "Iso 2",
            Ayuda = "Código de país de 2",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 1,
            LongitudMaxima = 2
          )
        ]
        public string ISO2 { get; set; }

        //----------------------------------------------

        [IUPropiedad(
            Etiqueta = "Prefijo telefónico",
            Ayuda = "Asigne el prefijo telefónico del país",
            Tipo = typeof(string),
            LongitudMaxima =10,
            Fila = 1,
            Columna = 2
          )
        ]
        public string Prefijo { get; set; }

        //----------------------------------------------

        [IUPropiedad(
            Etiqueta = "Nombre en Inglés",
            Ayuda = "Indique el nombre en inglés",
            Tipo = typeof(string),
            LongitudMaxima = 250,
            Fila = 2,
            Columna = 0,
            VisibleEnGrid = false
          )
        ]
        public string NombreIngles { get; set; }

        //----------------------------------------------


    }
}
