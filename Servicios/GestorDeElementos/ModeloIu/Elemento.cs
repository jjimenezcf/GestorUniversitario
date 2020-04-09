using System;

namespace Gestor.Elementos.ModeloIu
{
    public class IUCreacionAttribute : Attribute
    {
        public string Etiqueta { get; set; }
        public string Ayuda { get; set; }
        public bool Visible { get; set; }
        public bool Editable { get; set; }
        public Type Tipo { get; set; }
        public short Fila { get; set; }
        public short Columna { get; set; }
        public short Posicion { get; set; } = 0;
    }


    public static class FiltroPor
    {
        public static string Nombre = nameof(Nombre).ToLower();
        public static string Id = nameof(Id).ToLower();
    }


    public class Elemento
    {
        [IUCreacion(
            Etiqueta = "Id",
            Ayuda = "id del elemento",
            Tipo = typeof(int),
            Visible = false
            )
        ]
        public int Id { get; set; }
    }
}