using Enumerados;

namespace ModeloDeDto.Callejero
{
    [IUDto(AnchoEtiqueta = 20
         , AnchoSeparador = 5
         , MostrarExpresion = "Expresion")]
    public class CalleDto : AuditoriaDto
    {
        [IUPropiedad(
            Etiqueta = nameof(Pais),
            Ayuda = "Seleccione el país",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(PaisDto),
            GuardarEn = nameof(IdPais),
            Controlador = "Paises",
            AlSeleccionarBlanquearControl = nameof(Provincia),
            Fila = 0,
            Columna = 0,
            Obligatorio = true,
            Ordenar = true
            )
         ]
        public string Pais { get; set; }

        [IUPropiedad(Etiqueta = "Id del pais", Visible = false)]
        public int IdPais { get; set; }

        //----------------------------------------------
        [IUPropiedad(
            Etiqueta = nameof(Provincia),
            Ayuda = "Seleccione la provincia",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(ProvinciaDto),
            GuardarEn = nameof(IdProvincia),
            Controlador = "Provincias",
            AlSeleccionarBlanquearControl = nameof(Municipio),
            LongitudMinimaParaBuscar = 1,
            RestringidoPor = nameof(Pais),
            Fila = 0,
            Columna = 1,
            Obligatorio = true,
            Ordenar = true
            )
        ]
        public string Provincia { get; set; }

        [IUPropiedad(Etiqueta = "Id de la provincia", Visible = false)]
        public int IdProvincia { get; set; }
        //----------------------------------------------
        
        [IUPropiedad(
            Etiqueta = nameof(Municipio),
            Ayuda = "Seleccione el municipio",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(MunicipioDto),
            GuardarEn = nameof(IdMunicipio),
            Controlador = "Municipios",
            AlSeleccionarBlanquearControl = "",
            LongitudMinimaParaBuscar = 1,
            RestringidoPor = nameof(Provincia),
            Fila = 0,
            Columna = 2,
            Obligatorio = true,
            Ordenar = true
            )
        ]
        public string Municipio { get; set; }

        [IUPropiedad(Etiqueta = "Id del municipio", Visible = false)]
        public int IdMunicipio { get; set; }

        //----------------------------------------------

        [IUPropiedad(
            Etiqueta = nameof(TipoDeVia),
            Ayuda = "Seleccione el tipo de vía",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(TipoDeViaDto),
            GuardarEn = nameof(IdTipoDeVia),
            Fila = 1,
            Columna = 0,
            Obligatorio = true,
            Ordenar = true
            )
        ]
        public string TipoDeVia { get; set; }

        [IUPropiedad(Etiqueta = "Id del tipo de vía", Visible = false)]
        public int IdTipoDeVia { get; set; }

        //----------------------------------------------
        [IUPropiedad(
            Etiqueta = "Calle",
            Ayuda = "Indique el nombre de la calle",
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
            Ayuda = "Código de la calle",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 2,
            Ordenar = true,
            Obligatorio = true,
            LongitudMaxima = 3,
            Alineada = Aliniacion.derecha
          )
        ]
        public string Codigo { get; set; }


    }
}
