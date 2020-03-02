namespace Gestor.Elementos.ModeloIu
{
    public static class FiltroPor
    {
        public static string Nombre = nameof(Nombre).ToLower();
        public static string Id = nameof(Id).ToLower();
    }


    public class Elemento
    {
        public int Id { get; set; }
    }
}