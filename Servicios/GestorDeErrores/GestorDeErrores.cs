using System;

namespace Gestor.Errores
{
    public class GestorDeErrores
    {
        public enum Datos {Mostrar}

        public GestorDeErrores()
        {
        }

        public static string Concatenar(Exception e)
        {
            var retorno = "";
            var s = e.StackTrace;
            while (e != null)
            {
                if (!e.Message.Contains("See the inner exception for details"))
                {
                    retorno += e.Message + (e.InnerException != null ? Environment.NewLine : "");
                }
                e = e.InnerException;

            }

            retorno = retorno + Environment.NewLine + s;
            return retorno;
        }

        public static void Emitir(string error, Exception e = null)
        {
            if (e != null)
                RegistrarExcepcion(error,e);

            var exc = new Exception(error);
            exc.Data[Datos.Mostrar] = true;
            throw exc;
        }

        private static void RegistrarExcepcion(string error, Exception e)
        {
            /* registrar en el logger de excepciones 
             
            - Fecha Hora
            - Usuario
            - error
            - Excepción            
             
             */
            
        }

        public static void EnviarExcepcionPorCorreo(string asunto, Exception e)
        {
            var mensajeDeError = Concatenar(e);
            Correo.GestorDeCorreo.EnviarCorreo("juan.jimenez@emuasa.es", $"{asunto} en {e.TargetSite.DeclaringType.Name}.{e.TargetSite.Name}", mensajeDeError);
        }

    }
}
