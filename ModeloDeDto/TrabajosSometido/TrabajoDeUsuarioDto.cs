using System;
using Enumerados;
using ModeloDeDto.Entorno;

namespace ModeloDeDto.TrabajosSometidos
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5)]
    public class TrabajoDeUsuarioDto : ElementoDto
    {
        public static string ExpresionElemento = nameof(Trabajo);

        [IUPropiedad(
            Etiqueta = "Id del sometedor",
            SiempreVisible = false
            )
        ]
        public int IdSometedor { get; set; }

        [IUPropiedad(
            Etiqueta = "Sometido por",
            Ayuda = "Usuario sometedor",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = nameof(UsuarioDto),
            GuardarEn = nameof(IdSometedor),
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public string Sometedor { get; set; }

        [IUPropiedad(
            Etiqueta = "Id del usuario ejecutor",
            SiempreVisible = false
            )
        ]
        public int? IdEjecutor { get; set; }
        [IUPropiedad(
            Etiqueta = "Ejecutado por",
            Ayuda = "Usuario ejecutor",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = nameof(UsuarioDto),
            GuardarEn = nameof(IdEjecutor),
            Fila = 0,
            Columna = 1,
            VisibleEnGrid = false,
            EditableAlEditar = false
            )
        ]
        public string Ejecutor { get; set; }
       
        [IUPropiedad(
            Etiqueta = "Id del trabajo",
            SiempreVisible = false
            )
        ]
        public int IdTrabajo { get; set; }
        [IUPropiedad(
            Etiqueta = "Trabajo",
            Ayuda = "trabajo sometido",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = nameof(TrabajoSometidoDto),
            GuardarEn = nameof(IdTrabajo),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 50,
            EditableAlEditar = false
          )
        ]
        public string Trabajo { get; set; }

        [IUPropiedad(
           Etiqueta = "Estado",
           Ayuda = "estado del trabajo",
           TipoDeControl = enumTipoControl.RestrictorDeEdicion,
           Fila = 2,
           Columna = 0,
           VisibleEnGrid = true,
           EditableAlEditar = false,
           VisibleAlCrear = false
           )
        ]
        public string Estado { get; set; }

        [IUPropiedad(
           Etiqueta = "Entró",
           Ayuda = "Fecha de entrada",
           TipoDeControl = enumTipoControl.SelectorDeFecha,
           Fila = 3,
           Columna = 0,
           VisibleEnGrid = true,
           EditableAlCrear =false,
           EditableAlEditar =false           
           )
        ]
        public DateTime Encolado { get; set; }

        [IUPropiedad(
           Etiqueta = "Se iniciará",
           Ayuda = "Fecha planificada de ejecución",
           TipoDeControl = enumTipoControl.SelectorDeFecha,
           Fila = 3,
           Columna = 1,
           VisibleEnGrid = true
           )
        ]
        public DateTime Planificado { get; set; }

        [IUPropiedad(
           Etiqueta = "Iniciado",
           Ayuda = "Fecha de inicio",
           TipoDeControl = enumTipoControl.SelectorDeFecha,
           Fila = 4,
           Columna = 0,
           VisibleEnGrid = true,
           EditableAlCrear = false,
           EditableAlEditar = false
           )
        ]
        public DateTime? Iniciado { get; set; }

        [IUPropiedad(
           Etiqueta = "Terminado",
           Ayuda = "Fecha de fin",
           TipoDeControl = enumTipoControl.SelectorDeFecha,
           Fila = 4,
           Columna = 1,
           VisibleEnGrid = true,
           EditableAlCrear = false,
           EditableAlEditar = false
           )
        ]
        public DateTime? Terminado { get; set; }

        [IUPropiedad(
           Etiqueta = "Parametros",
           Ayuda = "Json con los parámetros de ejecución",
           TipoDeControl = enumTipoControl.RestrictorDeEdicion,
           Fila = 5,
           Columna = 0,
           VisibleEnGrid = false,
           EditableAlCrear = true,
           EditableAlEditar = true
           )
        ]
        public string Parametros { get; set; }

        [IUPropiedad(
           Etiqueta = "Periodicidad",
           Ayuda = "Tiempo en segundos de resometimiento",
           TipoDeControl = enumTipoControl.RestrictorDeEdicion,
           Fila = 6,
           Columna = 0,
           VisibleEnGrid = false,
           EditableAlCrear = true,
           EditableAlEditar = true,
           ConSpanEnColumnas = false
           )
        ]
        public int Periodicidad { get; set; }
    }
}
