using Gestor.Elementos.ModeloIu;
using System;
using System.Collections.Generic;

namespace Gestor.Elementos.Entorno
{


    [IUDto(AnchoEtiqueta = 20
         , AnchoSeparador = 5)]
    public class MenuDto : Elemento
    {

        [IUPropiedad(
            Etiqueta = "Id del menú padre",
            Visible = false
            )
        ]
        public int? idPadre { get; set; }

        [IUPropiedad(
            Etiqueta = "Padre",
            Ayuda = "Indique el menú padre",
            TipoDeControl = TipoControl.ListaDeElemento,
            SeleccionarDe = nameof(MenuDto),
            GuardarEn = nameof(idPadre),
            MostrarPropiedad = nameof(Nombre),
            Fila = 0,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15,
            Obligatorio = false
            )
        ]
        public  string Padre { get; set; }

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


        [IUPropiedad(
            Etiqueta = "Icono",
            Ayuda = "Seleccione un icono",
            TipoDeControl = TipoControl.UrlDeArchivo,
            ExtensionesValidas = ".svg",
            RutaDestino = "/images/menu",
            Tipo = typeof(string),
            Fila = 2,
            Columna = 0,
            PorAnchoMnt = 15
            )
        ]
        public string Icono { get; set; }

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

        [IUPropiedad(
            Visible = false
            )
        ]
        public List<MenuDto> Submenus { get; set; }


        [IUPropiedad(
            TipoDeControl = TipoControl.Editor,
            Visible = true,
            Etiqueta = nameof(VistaMvc),
            EditableAlEditar = false,
            VisibleAlCrear = false,
            Fila = 4,
            Columna = 0,
            Obligatorio = false
            )
        ]
        public string VistaMvc { get; set; }

        [IUPropiedad(
            Etiqueta ="Opción activa",
            VisibleEnGrid = false,
            Obligatorio = true,
            Fila = 5,
            Columna = 0,
            TipoDeControl = TipoControl.Check,
            ValorPorDefecto = false
            )
        ]
        public bool Activo { get; set; }
    }


    public class VistaMvcDto : Elemento
    {
        public string Nombre { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string Parametros { get; set; }

        public List<MenuDto> Menus { get; set; }
    }


}
