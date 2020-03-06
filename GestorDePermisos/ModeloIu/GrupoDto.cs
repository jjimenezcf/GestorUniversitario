using Gestor.Elementos.ModeloIu;
using System.Collections.Generic;

namespace Gestor.Elementos.Permiso
{
    public static class GrupoPor
    {
        public static string Nombre = FiltroPor.Nombre;
        public static string PermisoDeUnRol = nameof(PermisoDeUnRol).ToLower();
    }

    public class GrupoDto : Elemento
    {
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<ElementoInscripcionesDeUnEstudiante> Inscripciones { get; set; }
    }
}
