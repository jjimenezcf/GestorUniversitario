using Gestor.Elementos.ModeloIu;
using System;
using System.Collections.Generic;

namespace Gestor.Elementos.Universitario.ModeloIu
{
    
    public static class UsuariosPor
    {
       public static string NombreCompleto = nameof(NombreCompleto).ToLower();
       public static string CursosInscrito = nameof(CursosInscrito).ToLower();
    }

    public class UsuarioDto: Elemento
    {
        public string Login { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime Alta { get; set; }

        public ICollection<ElementoInscripcionesDeUnEstudiante> Inscripciones { get; set; }
    }
}
