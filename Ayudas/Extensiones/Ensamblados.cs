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
        public static void ValidarMetodo(string dll, string clase, string nombreMetodo)
        {
            var assembly = Assembly.LoadFrom(dll);

            var rutaBinarios = Path.GetDirectoryName(dll);
            var nombreEnsamblado = Path.GetFileNameWithoutExtension(dll);
            var nombreClase = $"{nombreEnsamblado}.{clase}";
            var tipo = assembly.GetType(nombreClase);
            if (tipo==null)
                throw new Exception($"la clase {nombreClase} no se encuentra en la dll {nombreEnsamblado} dentro de la ruta de binarios {rutaBinarios}");

            MethodInfo[] metodos = tipo.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var metodo in metodos)
                if (metodo.Name.ToLower() == nombreMetodo.ToLower())
                    return;
            throw new Exception($"Hay que implementar el método estático {nombreEnsamblado}.{clase}.{nombreMetodo} antes de usarlo");
        }

    }
}
