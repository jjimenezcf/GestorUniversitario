namespace UniversidadDeMurcia.Objetos
{
    public class EstudianteEnlace
    {
        public static class Parametro
        {
            public const string Apellido = nameof(Apellido);
            public const string InscritoEl = nameof(InscritoEl);
        }

        public static class OrdenadoPor
        {
            internal const string ApellidoDes = nameof(ApellidoAsc);
            internal const string ApellidoAsc = nameof(ApellidoDes);
            internal const string InscritoElDes = nameof(InscritoElDes);
            internal const string InscritoElAsc = nameof(InscritoElAsc);
        }

    }
}
