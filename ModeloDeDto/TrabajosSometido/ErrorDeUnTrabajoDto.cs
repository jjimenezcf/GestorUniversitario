using System;
using Enumerados;
using ModeloDeDto.Entorno;

namespace ModeloDeDto.TrabajosSometidos
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5)]
    public class ErrorDeUnTrabajoDto : ElementoDto
    {
        public static string ExpresionElemento = nameof(TrabajoDeUsuario);

        //--------------------------------------------
        [IUPropiedad(
            Etiqueta = "Trabajo del usuario",
            Ayuda = "Trabajo sometido de un usuario",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            MostrarExpresion = nameof(TrabajoDeUsuario),
            Fila = 0,
            Columna = 0,
            EditableAlCrear = false,
            EditableAlEditar = false,
            VisibleEnGrid = false
            )
        ]
        public int IdTrabajoDeUsuario { get; set; }

        //--------------------------------------------
        [IUPropiedad(
            Etiqueta = "trabajo de usuario",
            SiempreVisible = false
            )
        ]
        public string TrabajoDeUsuario { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Fecha",
           Ayuda = "Fecha del error",
           TipoDeControl = enumTipoControl.SelectorDeFechaHora,
           Fila = 1,
           Columna = 0,
           VisibleEnGrid = true,
           EditableAlCrear = false,
           EditableAlEditar = false
           )
        ]
        public DateTime Fecha { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Error",
           Ayuda = "error del trabajo",
           TipoDeControl = enumTipoControl.Editor,
           Fila = 2,
           Columna = 0,
           VisibleEnGrid = true,
           EditableAlEditar = false,
           VisibleAlCrear = false
           )
        ]
        public string Error { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Detalle",
           Ayuda = "detalle del error",
           TipoDeControl = enumTipoControl.AreaDeTexto,
           Fila = 3,
           Columna = 0,
           VisibleEnGrid = false,
           EditableAlEditar = false,
           VisibleAlCrear = false,
           Obligatorio = false,
           NumeroDeFilas = 5
           )
        ]
        public string Detalle { get; set; }

    }
}
