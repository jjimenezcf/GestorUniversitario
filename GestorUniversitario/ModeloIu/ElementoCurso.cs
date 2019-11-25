﻿using GestorDeElementos.ModeloIu;
using System.Collections.Generic;

namespace GestorUniversitario.ModeloIu
{
    public class ElementoCurso : ElementoBase
    {
        public string Titulo { get; set; }
        public int Creditos { get; set; }

        public ICollection<ElementoInscripcionesDeUnEstudiante> Inscripciones { get; set; }
    }
}
