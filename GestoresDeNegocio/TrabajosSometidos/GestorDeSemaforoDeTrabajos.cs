using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.TrabajosSometidos;
using ServicioDeDatos.Utilidades;

namespace GestoresDeNegocio.TrabajosSometidos
{
    class GestorDeSemaforoDeTrabajos
    {

        public static void PonerSemaforo(TrabajoDeUsuarioDtm tu)
        {
            var sentencia = new ConsultaSql<SemaforoDeTrabajosDtm>(SemaforoDeTrabajosSql.CrearSemaforo);

            var valores = new Dictionary<string, object> {
                { $"@{nameof(TrabajoDeUsuarioDtm.Id)}", tu.Id }, 
                { $"@{nameof(TrabajoDeUsuarioDtm.Iniciado)}", DateTime.Now }, 
                { $"@{nameof(TrabajoDeUsuarioDtm.Sometedor.Login)}", tu.Sometedor.Login } };
            var semaforo = 0;
            if (CacheDeVariable.Cfg_HayQueDebuggar)
                semaforo = sentencia.DebuggarSentencia($"{nameof(PonerSemaforo)}.txt", new DynamicParameters(valores));
            else
                semaforo = sentencia.EjecutarSentencia(new DynamicParameters(valores));

            if (semaforo == 0)
                throw new Exception($"No se ha podido bloquear el trabajo {tu.Trabajo.Nombre} del usuario {tu.Sometedor.Login}");
        }

        public static void QuitarSemaforo(TrabajoDeUsuarioDtm tu)
        {
            var sentencia = new ConsultaSql<SemaforoDeTrabajosDtm>(SemaforoDeTrabajosSql.BorrarSemaforo);

            var valores = new Dictionary<string, object> {{ $"@{nameof(TrabajoDeUsuarioDtm.Id)}", tu.Id } };
            var semaforo = 0;
            if (CacheDeVariable.Cfg_HayQueDebuggar)
                semaforo = sentencia.DebuggarSentencia($"{nameof(QuitarSemaforo)}.txt", new DynamicParameters(valores));
            else
                semaforo = sentencia.EjecutarSentencia(new DynamicParameters(valores));

            if (semaforo == 0)
                throw new Exception($"No se ha podido bloquear el trabajo {tu.Trabajo.Nombre} del usuario {tu.Sometedor.Login}");
        }

    }
}
