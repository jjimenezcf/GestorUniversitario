namespace Gestor.Elementos.ModeloIu
{
    public static class FiltroPor
    {
        public static string Nombre = nameof(Nombre).ToLower();
        public static string Id = nameof(Id).ToLower();
    }


    public class ElementoBase
    {
        public int Id { get; set; }
    }
}