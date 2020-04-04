using System;

namespace Gestor.Errores
{
    public class GestorDeErrores
    {
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
        public static void Emitir(string error)
        {
            new GestorDeErrores().LanzarExcepcion(error);
        }

        public void Enviar(string asunto, Exception e)
        {
            var error = Concatenar(e);
            Enviar($"{asunto} en {e.TargetSite.DeclaringType.Name}.{e.TargetSite.Name}", error);

        }

        public void Enviar(string asunto, string error)
        {
            Gestor.Correo.GestorDeCorreo.EnviarCorreo("juan.jimenez@emuasa.es", asunto, error);
        }


        public static void EnviaError(string asunto, Exception error)
        {
            Gestor.Correo.GestorDeCorreo.EnviarCorreo("juan.jimenez@emuasa.es", asunto, Concatenar(error));
        }


        public void LanzarExcepcion(string error)
        {
            throw new Exception(error);
        }
    }
}
