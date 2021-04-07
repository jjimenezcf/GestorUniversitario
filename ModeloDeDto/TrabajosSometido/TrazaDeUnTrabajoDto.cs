using System;
using Enumerados;
using ModeloDeDto.Entorno;

namespace ModeloDeDto.TrabajosSometidos
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5, OpcionDeBorrar = false)]
    public class TrazaDeUnTrabajoDto : ElementoDto
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
            Visible = false
            )
        ]
        public string TrabajoDeUsuario { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Fecha",
           Ayuda = "Fecha de la traza",
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
           Etiqueta = "Traza",
           Ayuda = "Traza del trabajo",
           TipoDeControl = enumTipoControl.Editor,
           Fila = 2,
           Columna = 0,
           VisibleEnGrid = true,
           EditableAlEditar = false,
           VisibleAlCrear = false
           )
        ]
        public string Traza { get; set; }

    }
}
