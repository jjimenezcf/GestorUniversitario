﻿using Enumerados;

namespace ModeloDeDto.Negocio
{
    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5, OpcionDeCrear = false)]
    public class NegocioDto : ElementoDto
    {
        [IUPropiedad(
          Etiqueta = "Negocio",
          Ayuda = "Indique el nombre del negocio",
          Tipo = typeof(string),
          Fila = 0,
          Columna = 0,
          Ordenar = true,
          PorAnchoMnt = 50,
          EditableAlCrear = false
          )
        ]
        public string Nombre { get; set; }
       
        //-------------------------------------------------        
        [IUPropiedad(
          Etiqueta = "Enumerado",
          Ayuda = "Enumerado asociado al negocio",
          Tipo = typeof(string),
          Fila = 0,
          Columna = 1,
          EditableAlCrear = false,
          EditableAlEditar = false
          )
          ]
        public string Enumerado { get; set; }

        //-------------------------------------------------
        [IUPropiedad(
          Etiqueta = "Elemento Dto",
          Ayuda = "Elemento de la vista",
          Tipo = typeof(string),
          Fila = 1,
          Columna = 0
          )
        ]
        public string ElementoDto { get; set; }

        //-------------------------------------------------
        [IUPropiedad(
          Etiqueta = "Elemento Dtm",
          Ayuda = "Elemento de la BD",
          Tipo = typeof(string),
          Fila = 1,
          Columna = 1
          )
        ]
        public string ElementoDtm { get; set; }

        //-------------------------------------------------
        [IUPropiedad(
            Etiqueta = "Icono",
            Ayuda = "Seleccione un icono",
            TipoDeControl = enumTipoControl.UrlDeArchivo,
            ExtensionesValidas = ".svg",
            RutaDestino = "/images/menu",
            Tipo = typeof(string),
            Fila = 2,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public string Icono { get; set; }


        //-------------------------------------------------
        [IUPropiedad(
            Etiqueta = "Administrador",
            Ayuda = "Permiso de administrador",
            EditableAlEditar = false,
            Tipo = typeof(string),
            Fila = 3,
            Columna = 0,
            Obligatorio = false,
            VisibleEnGrid = false,
            VisibleAlEditar = true,
            VisibleAlConsultar = true,
            VisibleAlCrear = false
            )
        ]
        public string PermisoDeAdministrador { get; set; }


        //-------------------------------------------------
        [IUPropiedad(
            Etiqueta = "Gestión",
            Ayuda = "Permiso de gestión",
            EditableAlEditar = false,
            Tipo = typeof(string),
            Fila = 4,
            Columna = 0,
            Obligatorio = false,
            VisibleEnGrid = false,
            VisibleAlEditar = true,
            VisibleAlConsultar = true,
            VisibleAlCrear = false
            )
        ]
        public string PermisoDeGestor { get; set; }

        //-------------------------------------------------
        [IUPropiedad(
            Etiqueta = "Consulta",
            Ayuda = "Permiso de consulta",
            EditableAlEditar = false,
            Tipo = typeof(string),
            Fila = 5,
            Columna = 0,
            Obligatorio = false,
            VisibleEnGrid = false,
            VisibleAlEditar = true,
            VisibleAlConsultar = true,
            VisibleAlCrear = false
            )
        ]
        public string PermisoDeConsultor { get; set; }

        //-------------------------------------------------
        [IUPropiedad(
            Etiqueta = "Negocio activo",
            Ayuda = "indica si el negocio está activo",
            VisibleEnGrid = false,
            Obligatorio = true,
            Fila = 6,
            Columna = 0,
            TipoDeControl = enumTipoControl.Check,
            ValorPorDefecto = false
            )
        ]
        public bool Activo { get; set; }

        //-------------------------------------------------
        [IUPropiedad(
            Etiqueta = "Usa seguridad",
            Ayuda = "indica si el negocio está activo",
            VisibleEnGrid = false,
            Obligatorio = true,
            Fila = 6,
            Columna = 1,
            TipoDeControl = enumTipoControl.Check,
            ValorPorDefecto = false
            )
        ]
        public bool UsaSeguridad { get; set; }

        //-------------------------------------------------
        [IUPropiedad(
            Etiqueta = "Es de parametrización",
            Ayuda = "indica si el negocio está activo",
            VisibleEnGrid = false,
            Obligatorio = true,
            Fila = 7,
            Columna = 0,
            TipoDeControl = enumTipoControl.Check,
            ValorPorDefecto = false
            )
        ]
        public bool EsDeParametrizacion { get; set; }
    }
}
