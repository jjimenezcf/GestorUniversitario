using System;
using Enumerados;

namespace ModeloDeDto.Callejero
{
    [IUDto(AnchoEtiqueta = 20
          , AnchoSeparador = 5
          , MostrarExpresion = "([Codigo]) [Nombre]")]
    public class ProvinciaDto : AuditoriaDto
    {
        [IUPropiedad(
            Etiqueta = nameof(Pais),
            Ayuda = "Seleccione el pais",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(PaisDto),
            GuardarEn = nameof(IdPais),
            Fila = 0,
            Columna = 0,
            Obligatorio = true,
            Ordenar = true
            )
        ]
        public string Pais { get; set; }

        [IUPropiedad(Etiqueta = "Id del pais",
            Visible = false)]
        public int IdPais { get; set; }

        //----------------------------------------------
        [IUPropiedad(
            Etiqueta = "Provincia",
            Ayuda = "Indique el nombre de la provincia",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 1,
            Ordenar = true,
            PorAnchoMnt = 50,
            Obligatorio = true,
            LongitudMaxima = 250
          )
        ]
        public string Nombre { get; set; }

        //----------------------------------------------
        [IUPropiedad(
            Etiqueta = "Código",
            Ayuda = "Código de la provincia",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            Obligatorio = true,
            LongitudMaxima = 2,
            Alineada = Aliniacion.derecha
          )
        ]
        public string Codigo { get; set; }

        //----------------------------------------------


        [IUPropiedad(
            Etiqueta = "Sigla",
            Ayuda = "Sigla de la provincia",
            Tipo = typeof(string),
            Fila = 2,
            Columna = 0,
            Obligatorio = true,
            LongitudMaxima = 3,
            Alineada = Aliniacion.derecha
          )
        ]
        public string Sigla { get; set; }

        //----------------------------------------------

        [IUPropiedad(
            Etiqueta = "Prefijo telefónico",
            Ayuda = "Asigne el prefijo telefónico de la provincia",
            Tipo = typeof(string),
            Fila = 2,
            Columna = 1,
            Obligatorio = true,
            LongitudMaxima = 10,
            Alineada = Aliniacion.derecha
          )
        ]
        public string Prefijo { get; set; }

    }
}
