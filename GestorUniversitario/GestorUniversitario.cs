using GestorDeElementos;
using GestorUniversitario.BdModelo;
using GestorUniversitario.ContextosDeBd;
using GestorUniversitario.IuModelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorUniversitario
{
    public class GestorUniversitario: GestorDeElementos<ContextoUniversitario, BdEstudiante, IuEstudiante>
    {

        public GestorUniversitario(ContextoUniversitario cnx)
            :base(cnx)
        {

        }

    }
}
