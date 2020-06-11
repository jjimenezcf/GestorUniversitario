using Gestor.Elementos.ModeloIu;
using System;
using System.Collections.Generic;

namespace Gestor.Elementos.Entorno
{

    [IUDto(AnchoEtiqueta = 20
         , AnchoSeparador = 5)]
    public class VistaMvcDto : Elemento
    {
        [IUPropiedad(
            Etiqueta = "Vista",
            Ayuda = "Nombre de la vista",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Nombre { get; set; }

        [IUPropiedad(
            Etiqueta = "Controlador",
            Ayuda = "Nombre del controlador",
            Tipo = typeof(string),
            Fila = 2,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Controlador { get; set; }

        [IUPropiedad(
            Etiqueta = "Accion",
            Ayuda = "Nombre de la acción",
            Tipo = typeof(string),
            Fila = 3,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Accion { get; set; }


        [IUPropiedad(
            Etiqueta = "Parametros",
            Ayuda = "Lista de parámetros de entrada",
            Tipo = typeof(string),
            Fila = 4,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15,
            Obligatorio = false,
            VisibleEnGrid =false
            )
        ]
        public string Parametros { get; set; }

        [IUPropiedad(
            Etiqueta = "Mostrar en modal",
            Ayuda = "indica si se ha de mostrar en modal la creación o edición",
            VisibleEnGrid = false,
            Obligatorio = true,
            TipoDeControl = TipoControl.Check,
            ValorPorDefecto = false,
            Tipo = typeof(bool),
            Fila = 4,
            Columna = 1
            )
        ]
        public string MostrarEnModal { get; set; }

        [IUPropiedad(Visible = false)]
        public List<MenuDto> Menus { get; set; }
    }
}
