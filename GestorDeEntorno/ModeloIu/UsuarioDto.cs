using Gestor.Elementos.ModeloIu;
using System;
using System.Linq;

/*
 * ClaseTypeScriptDeCreacion = "Usuarios.CrudCreacionUsuario"
         , ClaseTypeScriptDeEdicion = "Usuarios.CrudEdicionUsuario"
 * */

namespace Gestor.Elementos.Entorno
{

    public static class UsuariosPor
    {
       public static string NombreCompleto = nameof(NombreCompleto).ToLower();
       public static string Permisos = nameof(Permisos).ToLower();
    }

    [IUDto(AnchoEtiqueta =20
         , AnchoSeparador =5)]
    public class UsuarioDto : Elemento
    {
        [IUPropiedad(
            Etiqueta = "Usuario",
            Ayuda = "Usuario de conexión", 
            Tipo = typeof(string), 
            Fila = 0, 
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
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 45
            )
        ]
        public string Apellido { get; set; }


        [IUPropiedad(
            Etiqueta = "Nombre",
            Ayuda = "Nombre",
            Tipo = typeof(string),
            Fila = 1,
            Columna = 0,
            Posicion = 0
            )
        ]
        public string Nombre { get; set; }


        [IUPropiedad(
            Etiqueta = "Fecha de alta",
            EtiquetaGrid = "Alta",
            VisibleAlCrear = false,
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
            Etiqueta = "Fotografía",
            Ayuda = "Fotografía adjunta",
            Tipo = typeof(int),
            TipoDeControl= TipoControl.Archivo,
            Fila = 4,
            Columna = 0)]
        public int? IdArchivo { get; set; }

        [IUPropiedad(
            TipoDeControl = TipoControl.ImagenDelCanvas
            )]
        public string UrlDelArchivo { get; set; }

    }



}
