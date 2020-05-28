using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gestor.Elementos
{
    public class Utilidades
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
}
