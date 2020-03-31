using Gestor.Elementos.ModeloIu;
using System;
using System.Collections.Generic;

namespace Gestor.Elementos.Entorno
{
    public class MenuDto : Elemento
    {
        public  MenuDto Padre { get; set; }

        public string Nombre { get; set; }

        public string Icono { get; set; }

        public string Descripcion { get; set; }

        public List<MenuDto> Submenus { get; set; }

        public VistaMvcDto VistaMvc { get; set; }

        public bool Activo { get; set; }
    }


    public class VistaMvcDto : Elemento
    {
        public string Nombre { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string Parametros { get; set; }

        public List<MenuDto> Menus { get; set; }
    }


}
