using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario.ModeloIu;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Gestor.Elementos.Universitario
{
    public class GestorDeEstudiantes : GestorDeElementos<ContextoUniversitario, RegistroDeEstudiante, ElementoEstudiante>
    {

        public GestorDeEstudiantes(ContextoUniversitario contexto, IMapper mapeador)
            : base(contexto, mapeador)
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
            var gestor = new GestorDeInscripciones(_Contexto,_mapeador);

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
