using System;
using System.Collections.Generic;

namespace UniversidadDeMurcia.Models
{
    public class Estudiante: Objeto
    {
        public static class Parametro
        {
            public const string Nombre = nameof(Nombre);
            public const string InscritoEl = nameof(InscritoEl);
        }

        public static class OrdenadoPor
        {
            internal const string NombreDes = nameof(NombreAsc);
            internal const string NombreAsc = nameof(NombreDes);
            internal const string InscritoElDes = nameof(InscritoElDes);
            internal const string InscritoElAsc = nameof(InscritoElAsc);
        }

        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime InscritoEl { get; set; }

        public ICollection<Inscripcion> Inscripciones { get; set; }
    }
}
