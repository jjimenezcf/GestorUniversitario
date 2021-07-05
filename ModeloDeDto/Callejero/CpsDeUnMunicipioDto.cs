using Enumerados;
using ServicioDeDatos.Callejero;

namespace ModeloDeDto.Callejero
{

    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5)]
    public class CpsDeUnMunicipioDto : ElementoDto
    {
        public static string ExpresionElemento = nameof(CodigoPostal);

        //----------------------------------------------------------
        [IUPropiedad(Etiqueta = "Municipio",
            Ayuda = "códigos postales de un municipio",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            MostrarExpresion = nameof(Municipio),
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdMunicipio { get; set; }

        [IUPropiedad(
            Etiqueta = "Municipio",
            Visible = false
            )
        ]
        public string Municipio { get; set; }

        //----------------------------------------------------------
        [IUPropiedad(
            Etiqueta = "Id del código postal",
            Visible = false
            )
        ]
        public int IdCp { get; set; }


        [IUPropiedad(
            Etiqueta = "Código Postal",
            Ayuda = "Indique el CP",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(CodigoPostalDto),
            GuardarEn = nameof(IdCp),
            RestringidoPor = nameof(IdMunicipio),
            BuscarPor = nameof(CodigoPostalDtm.Codigo),
            CriterioDeBusqueda = CriteriosDeFiltrado.comienza,
            Fila = 1,
            Columna = 0,
            Ordenar = true
            )
        ]
        public string CodigoPostal { get; set; }


    }
}
