using Gestor.Elementos.ModeloIu;
using System.Collections.Generic;

namespace Gestor.Elementos.Usuario.ModeloIu
{
    public static class CursoPor
    {
        public static string Nombre = FiltroPor.Nombre;
        public static string EstudianteInscrito = nameof(EstudianteInscrito).ToLower();
    }

    public class ElementoCurso : Elemento
    {
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<ElementoInscripcionesDeUnEstudiante> Inscripciones { get; set; }
    }
}
