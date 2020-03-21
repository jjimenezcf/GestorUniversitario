using Gestor.Elementos.ModeloIu;
using System;
using System.Collections.Generic;

namespace Gestor.Elementos.Entorno
{
    public class Funcion: Elemento
    {
        public string Nombre { get; set; }

        public string Icono { get; set; }

        public string Descripcion { get; set; }

        public List<Funcion> Opciones { get; set; }
        
        public EleAccion Accion { get; set; }

        public bool Activo { get; set; }
    }
}
