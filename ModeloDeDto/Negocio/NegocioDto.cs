using Enumerados;

namespace ModeloDeDto.Negocio
{
    [IUDto(AnchoEtiqueta = 20
          , AnchoSeparador = 5)]
    public class NegocioDto : ElementoDto
    {
        [IUPropiedad(
          Etiqueta = "Negocio",
          Ayuda = "Indique el nombre del negocio",
          Tipo = typeof(string),
          Fila = 0,
          Columna = 0,
          Ordenar = true,
          PorAnchoMnt = 50
          )
        ]
        public string Nombre { get; set; }

        [IUPropiedad(
          Etiqueta = "Elemento",
          Ayuda = "Elemento de BD",
          Tipo = typeof(string),
          Fila = 0,
          Columna = 1
          )
        ]
        public string Elemento { get; set; }

        [IUPropiedad(
            Etiqueta = "Icono",
            Ayuda = "Seleccione un icono",
            TipoDeControl = enumTipoControl.UrlDeArchivo,
            ExtensionesValidas = ".svg",
            RutaDestino = "/images/menu",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 0,
            PorAnchoMnt = 15
            )
        ]
        public string Icono { get; set; }


        [IUPropiedad(
            Etiqueta = "Administrador",
            Ayuda = "Permiso de administrador",
            EditableAlEditar = false,
            Tipo = typeof(string),
            Fila = 2,
            Columna = 0,
            Obligatorio = false,
            VisibleEnGrid = false,
            VisibleAlEditar = true,
            VisibleAlConsultar = true,
            VisibleAlCrear = false
            )
        ]
        public string PermisoDeAdministrador { get; set; }


        [IUPropiedad(
            Etiqueta = "Gestión",
            Ayuda = "Permiso de gestión",
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
        public string PermisoDeGestor { get; set; }

        [IUPropiedad(
            Etiqueta = "Consulta",
            Ayuda = "Permiso de consulta",
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
        public string PermisoDeConsultor { get; set; }



        [IUPropiedad(
            Etiqueta = "Negocio activo",
            Ayuda = "indica si el negocio está activo",
            VisibleEnGrid = false,
            Obligatorio = true,
            Fila = 5,
            Columna = 1,
            TipoDeControl = enumTipoControl.Check,
            ValorPorDefecto = false
            )
        ]
        public bool Activo { get; set; }
    }
}
