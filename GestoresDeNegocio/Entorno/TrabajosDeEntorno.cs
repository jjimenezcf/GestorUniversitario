using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Enumerados;
using Gestor.Errores;
using GestorDeElementos;
using GestoresDeNegocio.TrabajosSometidos;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto;
using Newtonsoft.Json;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.TrabajosSometidos;
using ServicioDeDatos.Utilidades;
using Utilidades;

namespace GestoresDeNegocio.Entorno
{
    class TrabajosDeEntorno
    {
        public static void SometerBorrarTrazas(ContextoSe contexto)
        {
            var dll = Assembly.GetExecutingAssembly().GetName().Name;
            var clase = typeof(TrabajosDeEntorno).FullName;
            var ts = GestorDeTrabajosSometido.Obtener(contexto, "Borrar trazas", dll, clase, nameof(SometerBorrarTrazas).Replace("Someter", ""));
            
            GestorDeTrabajosDeUsuario.CrearSiNoEstaPendiente(contexto, ts, new Dictionary<string, object> { { nameof(TrabajoDeUsuarioDtm.Planificado),DateTime.Now.AddDays(1)} });
        }

        public static void BorrarTrazas(EntornoDeTrabajo entorno)
        {
            entorno.CrearTraza("Inicio del proceso");
            var ficheros = Directory.GetFiles(TrazaSql.ruta);
            var borrados = 0;
            var errores = 0;
            var trazaDtm =  entorno.CrearTraza("Fichero a borrar");
            foreach (var fichero in ficheros)
            {
                if (File.Exists(fichero))
                    try
                    {
                        entorno.ActualizarTraza(trazaDtm, $"Borrando el fichero {Path.GetFileName(fichero)}");
                        File.Delete(fichero);
                        borrados++;
                    }
                    catch(Exception e)
                    {
                        entorno.AnotarError($"Error al borrar el fichero {Path.GetFileName(fichero)}",e);
                        errores++;
                    }
            }
            entorno.CrearTraza($"Se han borrado {borrados} y {errores} no se han podido borrar");
        }
    }
}
