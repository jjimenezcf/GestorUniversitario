using System;
using System.Collections.Generic;

namespace Gestor.Elementos.ModeloIu
{
    public enum LadoDeRenderizacion {izquierdo, derecho}

    public class IUPropiedadAttribute : Attribute
    {
        public string Etiqueta { get; set; } = "";
        public string Ayuda { get; set; } = "";
        public bool Visible { get; set; } = true;
        public bool Editable { get; set; } = true;
        public Type Tipo { get; set; }
        public short Fila { get; set; } 
        public short Columna { get; set; }
        public short Posicion { get; set; } = 0;
        public string ClaseCss { get; set; } = "controlDeCreacion";
        public string ConcatenarClaseCss { set { ClaseCss = $"{ClaseCss} {value}"; } }
        public string ValorPorDefecto { get; set; }

    }

    public class IUDtoAttribute : Attribute
    {
        public short AnchoEtiqueta { get; set; }
        public short AnchoSeparador { get; set; }
        public string AlMostrar { get; set; } = "Crud.Crear.Mostrar()";

        public string AlCerrar { get; set; } = "Crud.Crear.Cerrar()";

        public string AlAceptar { get; set; } = "Crud.Crear.Aceptar()";

    }


    public static class FiltroPor
    {
        public static string Nombre = nameof(Nombre).ToLower();
        public static string Id = nameof(Id).ToLower();
    }


    public class Elemento
    {
        [IUPropiedad(
            Etiqueta = "Id",
            Ayuda = "id del elemento",
            Tipo = typeof(int),
            Visible = false,
            ClaseCss = "controlNoVisible"
            )
        ]
        public int Id { get; set; }
    }
}