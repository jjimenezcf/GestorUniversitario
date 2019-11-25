using GestorDeElementos;
using GestorUniversitario.BdModelo;
using GestorUniversitario.ContextosDeBd;
using GestorUniversitario.IuModelo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GestorUniversitario
{
    public class GestorDeEstudiantes : GestorDeElementos<ContextoUniversitario, BdEstudiante, IuEstudiante>
    {

        public GestorDeEstudiantes(ContextoUniversitario contexto)
            : base(contexto)
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

        protected override void MapearDetalleParaLaIu(IuEstudiante iuElemento, BdEstudiante bdElemento, PropertyInfo propiedadOrigen)
        {
            var gestorDeInscripciones = new GestorDeInscripciones(_Contexto);

            if (bdElemento.Inscripciones == null)
                return;

            foreach (var inscripcion in bdElemento.Inscripciones)
            {
                iuElemento.Inscripciones.Add(gestorDeInscripciones.MaperaElementoParaLaIu(inscripcion));
            }
        }
    }


}
