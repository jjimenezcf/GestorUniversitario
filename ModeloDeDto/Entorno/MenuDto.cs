using System.Collections.Generic;
using Enumerados;

namespace ModeloDeDto.Entorno
{


    [IUDto(AnchoEtiqueta = 20
         , AnchoSeparador = 5)]
    public class MenuDto : ElementoDto
    {
        private const string MostrarPadre = "[Padre].[Nombre]";

        [IUPropiedad(
            Etiqueta = "Id del menú padre",
            Visible = false
            )
        ]
        public int? idPadre { get; set; }

        //----------------------------------------------------------------

        [IUPropiedad(
            Etiqueta = "Padre",
            Ayuda = "Indique el menú padre",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(MenuDto),
            GuardarEn = nameof(idPadre),
            MostrarExpresion = MostrarPadre,
            Fila = 0,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15,
            Obligatorio = false
            )
        ]
        public  string Padre { get; set; }
        //----------------------------------------------------------------

        [IUPropiedad(
            Etiqueta = "Menu",
            Ayuda = "Nombre del menú",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Nombre { get; set; }
        //----------------------------------------------------------------

        [IUPropiedad(
            Etiqueta = "Icono",
            Ayuda = "Seleccione un icono",
            TipoDeControl = enumTipoControl.UrlDeArchivo,
            ExtensionesValidas = ".svg",
            RutaDestino = "/images/menu",
            Tipo = typeof(string),
            Fila = 2,
            Columna = 0,
            PorAnchoMnt = 15
            )
        ]
        public string Icono { get; set; }
        //----------------------------------------------------------------

        [IUPropiedad(
            Etiqueta = "Descripción",
            Ayuda = "Descripción de la opción de menú",
            Tipo = typeof(string),
            Fila = 3,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public string Descripcion { get; set; }
        //----------------------------------------------------------------

        [IUPropiedad(
            Visible = false
            )
        ]
        public List<MenuDto> Submenus { get; set; }

        //--------------------------------------------
        [IUPropiedad(
            Etiqueta = nameof(VistaMvc),
            Ayuda = "Seleccione la vista",
            TipoDeControl = enumTipoControl.ListaDinamica,
            SeleccionarDe = typeof(VistaMvcDto),
            GuardarEn = nameof(idVistaMvc),
            Fila = 4,
            Columna = 0,
            Obligatorio = false
            )
        ]
        public string VistaMvc { get; set; }
        [IUPropiedad(Etiqueta = "Id de la vista",
            Visible = false)]
        public int? idVistaMvc { get; set; }
        //--------------------------------------------

        [IUPropiedad(
            Etiqueta = "Orden",
            Ayuda = "orden del menú",
            Tipo = typeof(int),
            Fila = 5,
            Columna = 0,
            Ordenar = true,
            VisibleEnGrid = true
            )
        ]
        public string Orden { get; set; }
        //----------------------------------------------------------------


        [IUPropiedad(
            Etiqueta ="Opción activa",
            Ayuda = "indica si la opción de menú está activa",
            VisibleEnGrid = true,
            PorAnchoMnt = 15,
            Obligatorio = true,
            Fila = 5,
            Columna = 1,
            TipoDeControl = enumTipoControl.Check,
            ValorPorDefecto = false
            )
        ]
        public bool Activo { get; set; }
        //----------------------------------------------------------------

        //[IUPropiedad(
        //    Etiqueta = "Permiso",
        //    Ayuda = "Permiso de acceso",
        //    EditableAlEditar = false,
        //    Tipo = typeof(string),
        //    Fila = 6,
        //    Columna = 0,
        //    Obligatorio = false,
        //    VisibleEnGrid = false,
        //    VisibleAlEditar = true,
        //    VisibleAlConsultar = true,
        //    VisibleAlCrear = false
        //    )
        //]
        //public string Permiso { get; set; }


    }


}
