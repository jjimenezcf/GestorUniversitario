using System;

namespace GestorDeElementos
{
    public class ClaseDeElemetos<Tbd,Tiu>
    {
        public Tbd BdClase { get; set; }
        public Tiu IuClase { get; set; }

        ClaseDeElemetos(Tbd bdClase, Tiu iuClase)
        {
            BdClase = bdClase;
            IuClase = iuClase;
        }

        internal static ClaseDeElemetos<Tbd,Tiu> ObtenerGestorDeLaClase(Tbd bdClase, Tiu iuClase)
        {
            return new ClaseDeElemetos<Tbd,Tiu> (bdClase,iuClase);
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