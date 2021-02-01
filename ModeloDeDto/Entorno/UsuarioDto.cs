﻿using System;
using Enumerados;

namespace ModeloDeDto.Entorno
{

    public static class UsuariosPor
    {
       public static string NombreCompleto = nameof(NombreCompleto).ToLower();
       public static string Permisos = nameof(Permisos).ToLower();
    }

    [IUDto(AnchoEtiqueta = 20
           , AnchoSeparador = 5
           , ExpresionNombre = "([Login]) [Apellido], [Nombre]")]
    public class UsuarioDto : ElementoDto
    {
        [IUPropiedad(SiempreVisible = false)]
        public string NombreCompleto => $"({Login}) {Apellido}, {Nombre}";

        [IUPropiedad(
            Etiqueta = "Usuario",
            Ayuda = "Usuario de conexión", 
            Tipo = typeof(string), 
            Fila = 1, 
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt=25
            )
        ]
        public string Login { get; set; }


        [IUPropiedad(
            Etiqueta = "Apellidos",
            Ayuda = "Apellidos",
            Tipo = typeof(string),
            Fila = 2,
            Columna = 1,
            Ordenar = true,
            PorAnchoMnt = 45
            )
        ]
        public string Apellido { get; set; }


        [IUPropiedad(
            Etiqueta = "Nombre",
            Ayuda = "Nombre",
            Tipo = typeof(string),
            Fila = 2,
            Columna = 0,
            Posicion = 0
            )
        ]
        public string Nombre { get; set; }

        [IUPropiedad(
            Etiqueta = "eMail",
            Ayuda = "eMail",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 1,
            Posicion = 0
            )
        ]
        public string eMail { get; set; }


        [IUPropiedad(
            Etiqueta = "Fecha de alta",
            EtiquetaGrid = "Alta",
            VisibleEnGrid = true,
            VisibleAlCrear = false,
            VisibleEnEdicion = true,
            VisibleAlConsultar = true,
            Tipo = typeof(DateTime),
            EditableAlEditar = false,
            Fila = 3,
            Columna = 0,
            Ordenar = true
            )
        ]
        public DateTime Alta { get; set; }

        [IUPropiedad(
            VisibleEnGrid = false,
            VisibleEnEdicion = true,
            Etiqueta = "Fotografía",
            Ayuda = "Seleccione un fichero",
            Tipo = typeof(int),
            TipoDeControl= enumTipoControl.Archivo,
            ExtensionesValidas = ".png, .jpg",
            UrlDelArchivo = nameof(Foto),
            Fila = 4,
            Columna = 0)]
        public int? IdArchivo { get; set; }

        [IUPropiedad(
            TipoDeControl = enumTipoControl.ImagenDelCanvas
            )]
        public string Foto { get; set; }

    }



}
