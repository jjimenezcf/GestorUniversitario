using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using AutoMapper;
using ModeloDeDto;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;
using Utilidades;

namespace GestorDeElementos
{
    public static class Utilidades
    {
        public static string DescargarUrlDeArchivo(int id, string nombreFichero, string almacenadoEn)
        {
            var rutaUrlBase = "/Archivos";
            var archivo = DescargarArchivo(id, nombreFichero, almacenadoEn);
            string urlArchivoRelativa = $@"{rutaUrlBase}/{Path.GetFileName(archivo)}";
            return urlArchivoRelativa;
        }

        public static string DescargarArchivo(int id, string nombreFichero, string almacenadoEn)
        {
            var rutaDeDescarga = $@".\wwwroot\Archivos";
            var ficheroCacheado = $"{id}.se";
            var ficheroConRutaEnLaGd = $@"{almacenadoEn}\{ficheroCacheado}";
            var ficheroConRutaCacheado = $@"{rutaDeDescarga}\{ficheroCacheado}";

            if (!File.Exists(ficheroConRutaEnLaGd))
                return $@"{rutaDeDescarga}\FicheroNoEncontrado.png";


            if (!File.Exists(ficheroConRutaCacheado))
            {
                if (!CopiarFichero(ficheroConRutaEnLaGd, ficheroConRutaCacheado))
                    return $@"{rutaDeDescarga}\FicheroBloqueado.png";
            }

            var ficherpParaDevolverConRuta = $@"{rutaDeDescarga}\{Path.GetFileNameWithoutExtension(nombreFichero)}_{DateTime.Now.Ticks}{Path.GetExtension(nombreFichero)}";
            if (!CopiarFichero(ficheroConRutaCacheado, ficherpParaDevolverConRuta))
                return $@"{rutaDeDescarga}/FicheroBloqueado.png";

            return ficherpParaDevolverConRuta;
        }

        private static bool EstaEnUso(string ficheroConRuta)
        {
            bool usando = false;
            try
            {
                var f = File.OpenWrite(ficheroConRuta);
                f.Close();
            }
            catch
            {
                usando = true;
            }
            return usando;
        }

        private static bool CopiarFichero(string ficheroConRutaOrigen, string ficheroConRutaDestino)
        {
            var contadorEspera = 0;
            var copiado = false;

            while (contadorEspera <= 2 && !copiado)
            {
                try
                {
                    File.Copy(ficheroConRutaOrigen, ficheroConRutaDestino, true);
                    copiado = true;
                }
                catch
                {
                    contadorEspera += 1;
                    System.Threading.Thread.Sleep(500);
                }
            }
            return copiado;
        }

        public static PropertyInfo[] PropiedadesDelObjeto(Type tipo)
        {
            var indice = tipo.FullName;
            var cache = ServicioDeCaches.Obtener(nameof(Type.GetProperties));
            if (!cache.ContainsKey(indice))
            {
                Type t = tipo.GetType();
                cache[indice] = t.GetProperties();
            }
            PropertyInfo[] props = (PropertyInfo[])cache[indice];
            return props;
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
