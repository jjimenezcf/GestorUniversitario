using System;

namespace Extensiones.String
{
    public static class Cadenas
    {
        public static bool IsNullOrEmpty(this string str, bool bQuitarBlancos = true)
        {
            if (str == null)
                return true;

            return string.IsNullOrEmpty(bQuitarBlancos ? str.Trim() : str);
        }

        public static int Entero(this string str)
        {
            int numero = 0;
            if (str.IsNullOrEmpty())
                return numero;
            
            int.TryParse(str, out numero);
            return numero;
        }
    }


}
