using Gestor.Elementos;
using GestorUniversitario.ModeloBd;
using GestorUniversitario.ContextosDeBd;
using GestorUniversitario.ModeloIu;
using System.Reflection;

namespace GestorUniversitario
{
    public class GestorDeCursos : GestorDeElementos<ContextoUniversitario, RegistroDeCurso, ElementoCurso>
    {

        public GestorDeCursos(ContextoUniversitario contexto)
            : base(contexto)
        {
        }
               
        protected override RegistroDeCurso LeerConDetalle(int Id)
        {
            return null;
        }

        protected override void MapearDetalleParaLaIu(RegistroDeCurso registro, ElementoCurso elemento)
        {

        }

        protected override void MapearElemento(RegistroDeCurso registro, ElementoCurso elemento, PropertyInfo propiedad)
        {

        }

    }
}
