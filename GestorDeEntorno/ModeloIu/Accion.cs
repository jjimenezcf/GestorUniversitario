using Gestor.Elementos.ModeloIu;
using System;
using System.Collections.Generic;

namespace Gestor.Elementos.Entorno
{
    public class EleAccion: Elemento
    {
        public string Nombre { get; set; }
        public string Controlador {get; set; }
        public string Accion { get; set; }
        public string Parametros { get; set; }
    }
}