using GestorDeElementos;
using GestorUniversitario.BdModelo;
using GestorUniversitario.ContextosDeBd;
using GestorUniversitario.IuModelo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestorUniversitario
{
    public class GestorDeEstudiantes: GestorDeElementos<ContextoUniversitario, BdEstudiante, IuEstudiante>
    {

        public GestorDeEstudiantes(ContextoUniversitario contexto)
            :base(contexto)
        {

        }

        protected override BdEstudiante LeerConDetalle(int Id)
        {
            return _Contexto.Set<BdEstudiante>()
                            .Include(i => i.Inscripciones)
                            .ThenInclude(e => e.Curso)
                            .AsNoTracking()
                            .FirstOrDefault(m => m.Id == Id);
        }

    }
}
