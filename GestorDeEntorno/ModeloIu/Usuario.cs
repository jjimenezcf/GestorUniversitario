using Gestor.Elementos.ModeloIu;
using System;

namespace Gestor.Elementos.Entorno
{

    public static class UsuariosPor
    {
       public static string NombreCompleto = nameof(NombreCompleto).ToLower();
       public static string CursosInscrito = nameof(CursosInscrito).ToLower();
    }

    public class EleUsuario: Elemento
    {
        public string Login { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime Alta { get; set; }
    }
}
