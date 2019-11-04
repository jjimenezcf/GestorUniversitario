namespace UniversidadDeMurcia.Objetos
{
    public class EstudianteEnlace
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

    }
}
