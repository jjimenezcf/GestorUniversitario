using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ServicioDeDatos
{
    public class ServicioDeCaches
    {

        private static ConcurrentDictionary<string, ConcurrentDictionary<string, object>> CachesDeSe = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();

        public static ConcurrentDictionary<string, object> Obtener(string nombreCache)
        {
            if (!CachesDeSe.ContainsKey(nombreCache))
                CachesDeSe[nombreCache] = new ConcurrentDictionary<string, object>();

            return CachesDeSe[nombreCache];
        }

        public static void EliminarCache(string nombreCache)
        {
            if (CachesDeSe.ContainsKey(nombreCache))
                CachesDeSe[nombreCache].Clear();
        }

        public static void EliminarElemento(string cache, string clave)
        {
            var cacheDeRegistros = Obtener(cache);

            foreach (var indice in cacheDeRegistros.Keys)
            {
                if (clave == indice)
                {
                    cacheDeRegistros.TryRemove(clave, out _);
                }
            }
        }
    }
}
