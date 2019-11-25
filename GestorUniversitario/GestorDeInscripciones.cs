using GestorDeElementos;
using GestorUniversitario.BdModelo;
using GestorUniversitario.ContextosDeBd;
using GestorUniversitario.IuModelo;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GestorUniversitario
{
    public class GestorDeInscripciones : GestorDeElementos<ContextoUniversitario, RegistroDeInscripcion, ElementoInscripcion>
    {

        public GestorDeInscripciones(ContextoUniversitario contexto)
            : base(contexto)
        {
        }



        protected override RegistroDeInscripcion LeerConDetalle(int Id)
        {
            return null;
        }



        protected override void MapearDetalleParaLaIu(ElementoInscripcion iuElemento, RegistroDeInscripcion bdElemento, PropertyInfo propiedadOrigen)
        {

        }

    }
}
