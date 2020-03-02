using Gestor.Elementos.ModeloIu;
using System;
using System.Collections.Generic;

namespace Gestor.Elementos.Universitario.ModeloIu
{
    
    public static class EstudiantesPor
    {
       public static string NombreCompleto = nameof(NombreCompleto).ToLower();
       public static string CursosInscrito = nameof(CursosInscrito).ToLower();
    }

    public class ElementoEstudiante: Elemento
    {
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime InscritoEl { get; set; }

        public ICollection<ElementoInscripcionesDeUnEstudiante> Inscripciones { get; set; }
    }
}
