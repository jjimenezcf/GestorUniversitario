using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversidadDeMurcia.UtilidadesIu
{
    public class PeticionMvc
    {
        public string Nombre { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
    }

    public class Control
    {
        

    }

    public class ListaDeControles
    {
        public List<Control> Controles = new List<Control>();

    }

    public class ZonaDeFiltro
    {
        public ListaDeControles Controles;

    }
}
