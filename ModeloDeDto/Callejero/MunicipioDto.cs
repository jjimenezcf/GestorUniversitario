using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enumerados;

namespace ModeloDeDto.Callejero
{
    [IUDto(AnchoEtiqueta = 20
         , AnchoSeparador = 5
         , MostrarExpresion = "([Codigo]) [Nombre]")]
    public class MunicipioDto : AuditoriaDto
    {
        [IUPropiedad(
            Etiqueta = nameof(Provincia),
            Ayuda = "Seleccione la provincia",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(ProvinciaDto),
            GuardarEn = nameof(IdProvincia),
            Fila = 0,
            Columna = 0,
            Obligatorio = true
            )
        ]
        public string Provincia { get; set; }

        [IUPropiedad(Etiqueta = "Id de la provincia", Visible = false)]
        public int IdProvincia { get; set; }

        //----------------------------------------------
        [IUPropiedad(
            Etiqueta = "Código",
            Ayuda = "Código del municipio",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            Obligatorio = true,
            LongitudMaxima = 3,
            Alineada = Aliniacion.derecha
          )
        ]
        public string Codigo { get; set; }

        //----------------------------------------------

        [IUPropiedad(
            Etiqueta = "DC",
            Ayuda = "Dígito postal",
            Tipo = typeof(string),
            Fila = 2,
            Columna = 0,
            Obligatorio = true,
            LongitudMaxima = 3,
            Alineada = Aliniacion.derecha
          )
        ]
        public string DC { get; set; }

        //----------------------------------------------

        [IUPropiedad(
            Etiqueta = "Municipio",
            Ayuda = "Indique el nombre del municipio",
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


    }
}
