using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AutoMapper;
using ModeloDeDto;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;

namespace GestorDeElementos
{
    public static class Utilidades
    {
        public static string DescargarArchivo(int id, string nombreFichero, string almacenadoEn)
        {
            var rutaDeDescarga = $@".\wwwroot\Archivos";
            var ficheroCacheado = $"{id}.se";


            if (!File.Exists($@"{rutaDeDescarga}\{ficheroCacheado}"))
                if (File.Exists($@"{almacenadoEn}\{ficheroCacheado}"))
                    File.Copy($@"{almacenadoEn}\{ficheroCacheado}", $@"{rutaDeDescarga}\{ficheroCacheado}");
                else
                    return "";


            File.Copy($@"{rutaDeDescarga}\{ficheroCacheado}", $@"{rutaDeDescarga}\{nombreFichero}", true);

            var rutaUrlBase = "/Archivos";
            return $@"{rutaUrlBase}/{nombreFichero}";
        }
    }


    public class Gestores<TContexto, TRegistro, TElemento>
        where TRegistro : Registro
        where TElemento : ElementoDto
        where TContexto : ContextoSe
    {
        public static GestorDeElementos<TContexto, TRegistro, TElemento> Obtener(TContexto contexto, IMapper mapeador, string clase)
        {
            /*
             * No puedo cachear el objeto gestorDeElementos ya que el contexto es disposable y si un proceso coge el objeto y otro hace un dispose de la conexión mientras se usa
             * entonces fallaría el programa. Por tanto solo cacheo el objeto ConstructorInfo
             */

            var cache = ServicioDeCaches.Obtener(nameof(Gestor));
            clase = $"GestoresDeNegocio.{clase}";
            if (!cache.ContainsKey(clase))
            {
                string ruta = Assembly.GetExecutingAssembly().GetName().CodeBase;
                ruta = ruta.Substring(8); /* Quitamos FILE://// */
                ruta = Path.GetDirectoryName(ruta);

                var ensamblado = Assembly.LoadFrom(Path.Combine(ruta, "GestoresDeNegocio.dll"));
                var type = ensamblado.GetType(clase);

                List<Type> tipos = new List<Type> { typeof(TContexto), typeof(IMapper) };
                var constructorSinParametros = type.GetConstructor(tipos.ToArray());
                cache[clase] = constructorSinParametros;
            }
            return (GestorDeElementos<TContexto, TRegistro, TElemento>)((ConstructorInfo)cache[clase]).Invoke(new object[] { contexto, mapeador });

            //    cache[clase] = constructorSinParametros.Invoke(new object[] { contexto, mapeador });
            ////}
            ////else
            ////    ((GestorDeElementos<TContexto, TRegistro, TElemento>)cache[clase]).Contexto = contexto;

            //return (GestorDeElementos<TContexto, TRegistro, TElemento>)cache[clase];
        }
    }
}
