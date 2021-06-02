using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utilidades
{
    public static class Ensamblados
    {
        public static string RutaDeBinarios()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\","");
        }

        public static Type ObtenerType(string dll, string nombreCompletoDeClase)
        {
            var rutaDeBinarios = RutaDeBinarios();
            var assembly = Assembly.LoadFile($@"{RutaDeBinarios()}\{dll}");
            var tipo = assembly.GetType(nombreCompletoDeClase);
            if (tipo == null)
                throw new Exception($"la clase {nombreCompletoDeClase} no se encuentra el tipo {nombreCompletoDeClase} dentro de la ruta de binarios {rutaDeBinarios}");
            return tipo;
        }

        public static MethodInfo ValidarMetodoEstatico(string dll, string nombreCompletoDeClase, string nombreMetodo)
        {
            var assembly = Assembly.LoadFrom(dll.Replace(@"file:\", ""));

            var rutaBinarios = Path.GetDirectoryName(dll);
            var nombreEnsamblado = Path.GetFileNameWithoutExtension(dll);
            var tipo = assembly.GetType(nombreCompletoDeClase);
            if (tipo == null)
                throw new Exception($"la clase {nombreCompletoDeClase} no se encuentra en la dll {nombreEnsamblado} dentro de la ruta de binarios {rutaBinarios}");

            MethodInfo[] metodos = tipo.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var metodo in metodos)
                if (metodo.Name.ToLower() == nombreMetodo.ToLower())
                    return metodo;
            throw new Exception($"Hay que implementar el método estático {nombreMetodo} en la clase {nombreCompletoDeClase} en la ddl {nombreEnsamblado} antes de usarlo");
        }

        public static void EjecutarMetodoEstatico(string dll, string nombreCompletoDeClase, string nombreMetodo, object parametros)
        {
            var metodo = ValidarMetodoEstatico(dll, nombreCompletoDeClase, nombreMetodo);
            metodo.Invoke(null, new object[] { parametros });
        }

        public static PropertyInfo ObtenerPropiedad(string dll, string nombreCompletoDeClase, string nombrePropiedad)
        {
            var assembly = Assembly.LoadFrom(dll);

            var rutaBinarios = Path.GetDirectoryName(dll);
            var nombreEnsamblado = Path.GetFileNameWithoutExtension(dll);
            var tipo = assembly.GetType(nombreCompletoDeClase);
            if (tipo == null)
                throw new Exception($"la clase {nombreCompletoDeClase} no se encuentra en la dll {nombreEnsamblado} dentro de la ruta de binarios {rutaBinarios}");

            PropertyInfo[] propiedades = tipo.GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var propiedad in propiedades)
                if (propiedad.Name.ToLower() == nombrePropiedad.ToLower())
                    return propiedad;
            throw new Exception($"Hay que implementar el método estático {nombrePropiedad} en la clase {nombreCompletoDeClase} en la ddl {nombreEnsamblado} antes de usarlo");
        }

        public static PropertyInfo[] PropiedadesDelObjeto(this object objeto)
        {
            var indice = objeto.GetType().FullName;
            var cache = ServicioDeCaches.Obtener(nameof(Type.GetProperties));
            if (!cache.ContainsKey(indice))
            {
                Type t = objeto.GetType();
                cache[indice] = t.GetProperties();
            }
            PropertyInfo[] props = (PropertyInfo[])cache[indice];
            return props;
        }

        public static bool TienenLaPropiedad(this Type tipo, string propiedad)
        {
            var cache = ServicioDeCaches.Obtener(nameof(Type.GetProperties));
            var indice = tipo.FullName; 
            if (!cache.ContainsKey(indice))
                cache[indice] = tipo.GetProperties();

            foreach(var p in (PropertyInfo[])cache[indice])
            {
                if (p.Name.ToLower() == propiedad.ToLower())
                    return true;
            }

            return false;
        }

        /*
         *             
            //Type d1 = typeof(GestorDeElementos<,,>);
            //Type[] genericos = new Type[] { typeof(ContextoSe), typeof(IRegistro), typeof(ElementoDto) }; 
            //Type constructed = d1.MakeGenericType(genericos);
            //object o = Activator.CreateInstance(constructed,new object[] { contexto, contexto.Mapeador });
            //return (GestorDeElementos<ContextoSe, Registro, ElementoDto>) o;

         * */



    }
}
