using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Concurrent;
using Gestor.Errores;
using System.Reflection.Metadata.Ecma335;

namespace Gestor.Elementos
{
    public class Generador<TContexto, IMapper> 
    {
        public static ConcurrentDictionary<string, object> cacheObjetos = new ConcurrentDictionary<string, object>();

        public static object ObtenerGestor(string dll, string nombreClase, object[] parametros)
        {
            if (!dll.EndsWith(".dll")) dll = dll + ".dll";
            var indice = dll + '-' + nombreClase;

            if (!cacheObjetos.ContainsKey(indice) || cacheObjetos[indice] == null)
            {
                var clase = ReferenciarClase(dll, nombreClase);
                var constructorConParametros = clase.GetConstructor(new[] { typeof(TContexto), typeof(IMapper) });
                var objetoConParametros = constructorConParametros.Invoke(parametros);

                cacheObjetos[indice] = objetoConParametros;
            }
            return cacheObjetos[indice];
        }

        public static object CachearGestor(Type tipo, Func<object> creador)
        {
            var dll = tipo.Assembly.CodeBase;
            var nombreClase = tipo.Name;
            //if (!dll.EndsWith(".dll")) dll = dll + ".dll";
            var indice = dll + '-' + nombreClase;

            if (!cacheObjetos.ContainsKey(indice) || cacheObjetos[indice] == null)
                cacheObjetos[indice] = creador();

            return cacheObjetos[indice];
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
