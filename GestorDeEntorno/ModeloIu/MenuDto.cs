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
            TipoDeControl = TipoControl.SelectorDeElemento,
            SeleccionarDe = nameof(MenuDto),
            GuardarEn = nameof(idPadre),
            MostrarPropiedad = nameof(Nombre),
            Fila = 0,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
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
            Ayuda = "icono de presentación",
            Tipo = typeof(string),
            Fila = 2,
            Columna = 0,
            Ordenar = true,
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
            Visible = false
            )
        ]
        public VistaMvcDto VistaMvc { get; set; }

        [IUPropiedad(
            Visible = false
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
