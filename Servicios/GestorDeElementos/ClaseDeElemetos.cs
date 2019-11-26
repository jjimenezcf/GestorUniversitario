using System;

namespace Gestor.Elementos
{
    public class ClaseDeElemetos<Tbd,Tiu>
    {
        public ClaseDeElemetos()
        {
        }

        internal static ClaseDeElemetos<Tbd,Tiu> ObtenerGestorDeLaClase()
        {
            return new ClaseDeElemetos<Tbd,Tiu> ();
        }

        internal Tbd NuevoElementoBd()
        {
            return Activator.CreateInstance<Tbd>();
        }

        internal Tiu NuevoElementoIu()
        {
            return Activator.CreateInstance<Tiu>();
        }
    }
}