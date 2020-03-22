using Gestor.Elementos.ModeloIu;
using System;
using System.Collections.Generic;

namespace Gestor.Elementos.Entorno
{
    public class E_Menu : Elemento
    {
        public  E_Menu Padre { get; set; }

        public string Nombre { get; set; }

        public string Icono { get; set; }

        public string Descripcion { get; set; }

        public List<E_Menu> Opciones { get; set; }

        public E_VistaMvc VistaMvc { get; set; }

        public bool Activo { get; set; }
    }


    public class E_VistaMvc : Elemento
    {
        public string Nombre { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string Parametros { get; set; }
    }


}
