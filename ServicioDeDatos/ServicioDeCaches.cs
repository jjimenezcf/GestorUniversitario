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

        public static void Eliminar(string nombreCache)
        {
            if (CachesDeSe.ContainsKey(nombreCache))
                CachesDeSe[nombreCache].Clear();
        }
    }
}
