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

    public class UsuarioDto : Elemento
    {
        [IUPropiedad(
            Etiqueta = "Usuario",
            Ayuda = "Usuario de conexión", 
            Tipo = typeof(string), 
            Visible = true, 
            Fila = 0, 
            Columna = 0
            )
        ]
        public string Login { get; set; }


        [IUPropiedad(
            Etiqueta = "Apellido y nombre",
            Ayuda = "Apellidos",
            Tipo = typeof(string),
            Visible = true,
            Fila = 1,
            Columna = 0
            )
        ]
        public string Apellido { get; set; }


        [IUPropiedad(
            Ayuda = "Nombre",
            Tipo = typeof(string),
            Visible = true,
            Fila = 1,
            Columna = 1,
            Posicion = 1
            )
        ]
        public string Nombre { get; set; }


        [IUPropiedad(
            Etiqueta = "Fecha de alta",
            Tipo = typeof(DateTime),
            Visible = true,
            Editable = false,
            ValorPorDefecto="AlMostrar(GetDate())",
            Fila = 2,
            Columna = 0
            )
        ]
        public DateTime Alta { get; set; }

    }



}
