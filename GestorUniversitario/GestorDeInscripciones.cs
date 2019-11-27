using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Elementos.Universitario.ModeloIu;
using System.Reflection;
using AutoMapper;

namespace Gestor.Elementos.Universitario
{
    public class GestorDeInscripciones : GestorDeElementos<ContextoUniversitario, RegistroDeInscripcion, ElementoInscripcionesDeUnEstudiante>
    {

        public GestorDeInscripciones(ContextoUniversitario contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }



        protected override RegistroDeInscripcion LeerConDetalle(int Id)
        {
            return null;
        }



        protected override void MapearDetalleParaLaIu(RegistroDeInscripcion registroDeInscripcion, ElementoInscripcionesDeUnEstudiante elementoInscripcion )
        {

        }


        protected override void MapearElemento(RegistroDeInscripcion registro, ElementoInscripcionesDeUnEstudiante elemento, PropertyInfo propiedad)
        {
            if (propiedad.Name == elemento.PropiedadCurso)
            {
                var gestor = new GestorDeCursos(_Contexto, _mapeador);
                elemento.Curso = gestor.MapearElemento(registro.Curso);
            }

            //if (propiedad.Name == "Estudiante")
            //{
            //    var gestor = new GestorDeEstudiantes(_Contexto);
            //    elemento.Estudiante = gestor.MapearElemento(registro.Estudiante);
            //}

        }

    }
}
