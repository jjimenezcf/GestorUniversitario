using System;

namespace Gestor.Errores
{
    public class GestorDeErrores
    {
        public enum Datos {Mostrar}

        public GestorDeErrores()
        {
        }

        public static string Detalle(Exception e)
        {
            var retorno = Concatenar(e);

            var s = e.StackTrace;
            retorno = retorno + Environment.NewLine + s;
            return retorno;
        }

        public static string Concatenar(Exception e)
        {
            var retorno = "";
            while (e != null)
            {
                if (!e.Message.Contains("See the inner exception for details"))
                {
                    retorno += e.Message + (e.InnerException != null ? Environment.NewLine : "");
                }
                e = e.InnerException;

            }

            return retorno;
        }

        public static void Emitir(string error, Exception e = null)
        {
            if (e != null)
                RegistrarExcepcion(error,e);

            var exc = new Exception(error,e);
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
            var mensajeDeError = Detalle(e);
            Correo.GestorDeCorreo.EnviarCorreo("juan.jimenez@emuasa.es", $"{asunto} en {e.TargetSite.DeclaringType.Name}.{e.TargetSite.Name}", mensajeDeError);
        }

    }
}
