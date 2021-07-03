﻿using Enumerados;
using ServicioDeDatos.Callejero;

namespace ModeloDeDto.Callejero
{

    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5)]
    public class CpsDeUnaProvinciaDto : ElementoDto
    {
        public static string ExpresionElemento = nameof(CodigoPostal);

        //----------------------------------------------------------
        [IUPropiedad(Etiqueta = "Provincia",
            Ayuda = "códigos postales de una provincia",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            MostrarExpresion = nameof(Provincia),
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdProvincia { get; set; }

        [IUPropiedad(
            Etiqueta = "Provincia",
            Visible = false
            )
        ]
        public string Provincia { get; set; }

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
            RestringidoPor = nameof(IdProvincia),
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
