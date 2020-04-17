using Gestor.Elementos.ModeloIu;
using System;
using System.Linq;

namespace Gestor.Elementos.Entorno
{

    public static class UsuariosPor
    {
       public static string NombreCompleto = nameof(NombreCompleto).ToLower();
       public static string Permisos = nameof(Permisos).ToLower();
    }

    [IUDto(ClaseTypeScriptDeCreacion = "Crud.Usuarios.CrudCreacionUsuario"
         , ClaseTypeScriptDeEdicion = "Crud.Usuarios.CrudEdicionUsuario"
         , AnchoEtiqueta =20
         , AnchoSeparador =5)]
    public class UsuarioDto : Elemento
    {
        [IUPropiedad(
            Etiqueta = "Usuario",
            Ayuda = "Usuario de conexión", 
            Tipo = typeof(string), 
            Fila = 0, 
            Columna = 0
            )
        ]
        public string Login { get; set; }


        [IUPropiedad(
            Etiqueta = "Apellidos",
            Ayuda = "Apellidos",
            Tipo = typeof(string),
            Fila = 2,
            Columna = 0
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
            VisibleAlCrear = false,
            Tipo = typeof(DateTime),
            EditableAlEditar = false,
            Fila = 3,
            Columna = 0
            )
        ]
        public DateTime Alta { get; set; }

    }



}
