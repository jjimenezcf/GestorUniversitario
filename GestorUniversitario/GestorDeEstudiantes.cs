using GestorDeElementos;
using GestorUniversitario.ModeloBd;
using GestorUniversitario.ContextosDeBd;
using GestorUniversitario.ModeloIu;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GestorUniversitario
{
    public class GestorDeEstudiantes : GestorDeElementos<ContextoUniversitario, RegistroDeEstudiante, ElementoEstudiante>
    {

        public GestorDeEstudiantes(ContextoUniversitario contexto)
            : base(contexto)
        {

        }

        protected override RegistroDeEstudiante LeerConDetalle(int Id)
        {
            return _Contexto.Set<RegistroDeEstudiante>()
                            .Include(i => i.Inscripciones)
                            .ThenInclude(e => e.Curso)
                            .AsNoTracking()
                            .FirstOrDefault(m => m.Id == Id);
        }

        protected override void MapearDetalleParaLaIu(RegistroDeEstudiante registro, ElementoEstudiante elemento)
        {
            var gestor = new GestorDeInscripciones(_Contexto);

            if (registro.Inscripciones == null)
                return;

            elemento.Inscripciones = new Collection<ElementoInscripcionesDeUnEstudiante>();
            foreach (var registroDeInscripcion in registro.Inscripciones)
            {
                var elemetoInscripcion = gestor.MapearElemento(registroDeInscripcion, new List<string> {"Estudiante"} );
                elemento.Inscripciones.Add(elemetoInscripcion);
            }
        }

        protected override void MapearElemento(RegistroDeEstudiante registro, ElementoEstudiante elemtoEstudiante, PropertyInfo propiedad)
        { 
        }

    }


}
