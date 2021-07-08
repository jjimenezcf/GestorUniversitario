using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AutoMapper;
using Gestor.Errores;
using ModeloDeDto;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;
using Utilidades;
using System.Linq;

namespace GestorDeElementos
{
    public static class Utilidades
    {
        public static string DescargarUrlDeArchivo(int id, string nombreFichero, string almacenadoEn, bool solicitadoPorLaCola)
        {
            var archivo = DescargarArchivo(id, nombreFichero, almacenadoEn, solicitadoPorLaCola);
            return UrlDeArchivo(archivo);
        }

        public static string UrlDeArchivo(string archivo)
        {
            var rutaUrlBase = "/Archivos";
            string urlArchivoRelativa = $@"{rutaUrlBase}/{Path.GetFileName(archivo)}";
            return urlArchivoRelativa;
        }

        public static string DescargarArchivo(int id, string nombreFichero, string almacenadoEn, bool solicitadoPorLaCola)
        {
            var rutaDeDescarga = !solicitadoPorLaCola ? $@".\wwwroot\Archivos": CacheDeVariable.Cfg_RutaDeDescarga;

            if (!Directory.Exists(rutaDeDescarga))
                try
                {
                    Directory.CreateDirectory(rutaDeDescarga);
                }
                catch(Exception e)
                {
                    GestorDeErrores.Emitir($"Error al crear un directorio {rutaDeDescarga}",e);
                }

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

        public static string DescargarArchivo(string ficheroConRutaEnLaGd)
        {
            var rutaDeDescarga = $@".\wwwroot\Archivos";
            var ficheroParaDescargar = $@"{rutaDeDescarga}\{DateTime.Now.Ticks}{Path.GetFileName(ficheroConRutaEnLaGd)}";

            if (!File.Exists(ficheroConRutaEnLaGd))
                return $@"{rutaDeDescarga}\FicheroNoEncontrado.png";


            if (!File.Exists(ficheroParaDescargar))
            {
                if (!CopiarFichero(ficheroConRutaEnLaGd, ficheroParaDescargar))
                    return $@"{rutaDeDescarga}\FicheroBloqueado.png";
            }

            if (!CopiarFichero(ficheroConRutaEnLaGd, ficheroParaDescargar))
                return $@"{rutaDeDescarga}/FicheroBloqueado.png";

            return ficheroParaDescargar;
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

    public static class ApiContextoSe
    {
        public static TSource LeerCacheadoPorId<TSource>(this IQueryable<TSource> source, int id, bool errorSiNoHay = true) where TSource : Registro
        {
            var cache = ServicioDeCaches.Obtener(typeof(TSource).FullName);
            var indice = $"{nameof(Registro.Id)}-{id}-0";
            if (!cache.ContainsKey(indice))
            {
                var registro = source.FirstOrDefault(x => x.Id == id);
                if (registro == null && errorSiNoHay)
                    GestorDeErrores.Emitir($"No se ha localizado el objeto con id {id} buscado en la entidad {typeof(TSource).Name}");
                cache[indice] = registro;
            }
            return (TSource)cache[indice];
        }

        public static TSource LeerCacheadoPorNombre<TSource>(this IQueryable<TSource> source, string nombre, bool errorSiNoHay = true, bool errorSiHayMasDeUno = true) where TSource: INombre
        {
            var cache = ServicioDeCaches.Obtener(typeof(TSource).FullName);
            var indice = $"{nameof(INombre.Nombre)}-{nombre}-0";
            if (!cache.ContainsKey(indice))
            {
                var registros = source.Take(2).Where(x => x.Nombre.Equals(nombre));

                if (registros.Count() == 2 && errorSiNoHay)
                    GestorDeErrores.Emitir($"No se ha localizado el objeto con id {nombre} buscado en la entidad {typeof(TSource).Name}");

                if (registros.Count() == 2 && errorSiHayMasDeUno)
                    GestorDeErrores.Emitir($"No se ha localizado el objeto con id {nombre} buscado en la entidad {typeof(TSource).Name}");

                cache[indice] = registros.ToList()[0];
            }
            return (TSource)cache[indice];
        }

        public static TSource LeerCacheadoPorPropiedad<TSource>(this IQueryable<TSource> source, string nombrePropiedad, string valor, bool errorSiNoHay = true, bool errorSiHayMasDeUno = true) where TSource : INombre
        {
            var cache = ServicioDeCaches.Obtener(typeof(TSource).FullName);
            var indice = $"{nameof(INombre.Nombre)}-{nombrePropiedad}-0";
            if (!cache.ContainsKey(indice))
            {
                var registros = source.Take(2).Where(x => x.Nombre.Equals(nombrePropiedad));
                                
                if (registros.Count() == 0 && errorSiNoHay)
                    GestorDeErrores.Emitir($"No se ha localizado el objeto con {nombrePropiedad} {valor} buscado en la entidad {typeof(TSource).Name}");

                if (registros.Count() == 2 && errorSiHayMasDeUno)
                    GestorDeErrores.Emitir($"Hay más de un objeto con {nombrePropiedad} {valor} buscado en la entidad {typeof(TSource).Name}");

                cache[indice] = registros.ToList()[0];
            }
            return (TSource)cache[indice];
        }
    }
}
