using System;
using System.Collections.Generic;
using System.Text;

namespace Extensiones
{
    public static class Excepciones
    {
        public static string MensajeCompleto(this Exception exc, bool mostrarPila = false)
        {
            var result = "";
            var exOrigen = exc;
            while (exc != null)
            {
                var mensaje = exc.Message;
                
                if (!result.Contains(mensaje))
                    result += mensaje + Environment.NewLine;
                exc = exc.InnerException;
            }

            if (mostrarPila)
                result += exOrigen.StackTrace;

            return result;
        }

    }
}
