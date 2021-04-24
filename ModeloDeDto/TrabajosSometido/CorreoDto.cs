using System;
using Enumerados;
using ModeloDeDto.Entorno;

namespace ModeloDeDto.TrabajosSometido
{


    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5, OpcionDeBorrar = false)]
    public class CorreoDto : ElementoDto
    {
        public static readonly string ExpresionElemento = $"{nameof(Creado)}: {nameof(Asunto)}";
        public static readonly string receptores = nameof(receptores);
        public static readonly string asunto = nameof(asunto);
        public static readonly string cuerpo = nameof(cuerpo);


        //--------------------------------------------
        [IUPropiedad(
            Etiqueta = "Creador",
            Ayuda = "mensaje creado por",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            MostrarExpresion = nameof(Creador),
            Fila = 0,
            Columna = 0,
            EditableAlCrear = false,
            EditableAlEditar = false,
            VisibleEnGrid = true
            )
        ]
        public int IdUsuario { get; set; }

        //--------------------------------------------
        [IUPropiedad(
            Etiqueta = "Creador",
            Visible = false
            )
        ]
        public string Creador { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Emisor",
           Ayuda = "Emisor del mensaje",
           TipoDeControl = enumTipoControl.Editor,
           Fila = 1,
           Columna = 0,
           VisibleEnGrid = true,
           EditableAlEditar = false,
           VisibleAlCrear = false
           )
        ]
        public string Emisor { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Receptores",
           Ayuda = "receptores del mensaje",
           TipoDeControl = enumTipoControl.Editor,
           Fila = 2,
           Columna = 0,
           VisibleEnGrid = false,
           EditableAlEditar = false,
           VisibleAlCrear = false
           )
        ]
        public string Receptores { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Asunto",
           Ayuda = "asunto del mensaje",
           TipoDeControl = enumTipoControl.Editor,
           Fila = 3,
           Columna = 0,
           VisibleEnGrid = true,
           EditableAlEditar = false,
           VisibleAlCrear = false
           )
        ]
        public string Asunto { get; set; }


        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Cuerpo",
           Ayuda = "cuerpo del mensaje",
           TipoDeControl = enumTipoControl.AreaDeTexto,
           Fila = 4,
           Columna = 0,
           VisibleEnGrid = false,
           EditableAlEditar = false,
           VisibleAlCrear = false
           )
        ]
        public string Cuerpo { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Elementos",
           Ayuda = "elementos anexos",
           TipoDeControl = enumTipoControl.AreaDeTexto,
           Fila = 5,
           Columna = 0,
           VisibleEnGrid = false,
           EditableAlEditar = false,
           VisibleAlCrear = false
           )
        ]
        public string Elementos { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Archivos",
           Ayuda = "archivos anexos",
           TipoDeControl = enumTipoControl.AreaDeTexto,
           Fila = 6,
           Columna = 0,
           VisibleEnGrid = false,
           EditableAlEditar = false,
           VisibleAlCrear = false
           )
        ]
        public string Archivos { get; set; }

        //--------------------------------------------
        [IUPropiedad(
            Etiqueta = "Creado",
            Ayuda = "fecha de creación",
            TipoDeControl = enumTipoControl.SelectorDeFechaHora,
            Fila = 1,
            Columna = 1,
            VisibleEnGrid = false,
            EditableAlCrear = false,
            EditableAlEditar = false,
            Obligatorio = true,
            Ordenar = false
           )
        ]
        public DateTime Creado { get; set; }


        //--------------------------------------------
        [IUPropiedad(
            Etiqueta = "Enviado",
            Ayuda = "fecha de envío",
            TipoDeControl = enumTipoControl.SelectorDeFechaHora,
            Fila = 1,
            Columna = 2,
            VisibleEnGrid = true,
            EditableAlCrear = false,
            EditableAlEditar = false,
            Obligatorio = false,
            Ordenar = true
           )
        ]
        public DateTime? Enviado { get; set; }

    }
}
