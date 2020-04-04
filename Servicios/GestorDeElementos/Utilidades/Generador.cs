using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Gestor.Errores;
using System.Linq;
using System.Collections.Concurrent;

namespace GestorDeElementos
{
    public class Generador 
    {
        public static ConcurrentDictionary<string, object> cacheObjetos = new ConcurrentDictionary<string, object>();

        public static object GenerarObjeto(string dll, string nombreClase, object[] parametros)
        {
            if (!cacheObjetos.ContainsKey(nombreClase) || cacheObjetos[nombreClase] == null)
            {
                var clase = ReferenciarClase(dll, nombreClase);
                cacheObjetos[nombreClase] = clase.InvokeMember("Crear", BindingFlags.InvokeMethod, null, null, parametros); 
            }


            return cacheObjetos[nombreClase];
        }


        private static Type ReferenciarClase(string dll, string nombreClase)
        {
            // buscar ruta del ensamblado que se está ejecutando
            string ruta = Assembly.GetExecutingAssembly().GetName().CodeBase;
            if (ruta.ToUpper().StartsWith("FILE:///"))
                ruta = ruta.Substring(8);
            ruta = Path.GetDirectoryName(ruta);

            return ReferenciarClase(ruta, dll, nombreClase);
        }

        private static Type ReferenciarClase(string ruta, string dll, string nombreClase)
        {
            Assembly ensamblado;
            Type clase;
            try
            {
                var fichero = Path.Combine(ruta, dll);
                if (!File.Exists(fichero))
                    GestorDeErrores.Emitir($"No se puede crear un objeto de la clase {nombreClase} por no localizarse el ensamblado {fichero} ");

                ensamblado = Assembly.LoadFrom(fichero);

                clase = ensamblado.GetTypes().FirstOrDefault(c => c.Name == nombreClase);
                if (clase == null)
                    GestorDeErrores.Emitir($"No se ha encontrado la clase de acceso a datos '{nombreClase}' en la DLL '{dll}'.");
            }
            catch (Exception exc)
            {
                throw new Exception($"{exc.Message}.");
            }


            return clase;
        }

    }
}
