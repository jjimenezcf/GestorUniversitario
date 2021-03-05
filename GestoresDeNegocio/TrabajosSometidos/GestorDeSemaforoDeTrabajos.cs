using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.TrabajosSometidos;
using ServicioDeDatos.Utilidades;

namespace GestoresDeNegocio.TrabajosSometidos
{
    class GestorDeSemaforoDeTrabajos
    {

        public static void PonerSemaforo(TrabajoDeUsuarioDtm tu)
        {
            var consulta = new ConsultaSql<SemaforoDeTrabajosDtm>($@"
                                        insert into trabajo.semaforo (ID_TRABAJO, INICIADO, LOGIN)
                                        VALUES({tu.Id},'{DateTime.Now}','{tu.Sometedor.Login}')");
            var semaforo = consulta.EjecutarConsulta();
            if (semaforo == 0)
                throw new Exception($"No se ha podido bloquear el trabajo {tu.Trabajo.Nombre} del usuario {tu.Sometedor.Login}");
        }

        public static void QuitarSemaforo(TrabajoDeUsuarioDtm tu)
        {
            var consulta = new ConsultaSql<SemaforoDeTrabajosDtm>($@"Delete from trabajo.semaforo where ID_TRABAJO = {tu.Id}");
            var semaforo = consulta.EjecutarConsulta();
            if (semaforo == 0)
                throw new Exception($"El trabajo {tu.Trabajo.Nombre} del usuario {tu.Sometedor.Login} no estaba bloqueado");
        }

    }
}
