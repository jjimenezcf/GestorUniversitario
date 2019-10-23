using System;

namespace Extensiones
{
    public static class Cadenas
    {
        public static bool IsNullOrEmpty(this string str, bool bQuitarBlancos = true)
        {
            if (str == null)
                return true;

            return string.IsNullOrEmpty(bQuitarBlancos ? str.Trim() : str);
        }
    }
}
