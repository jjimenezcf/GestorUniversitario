using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversidadDeMurcia.UtilidadesIu
{

    public class CreacionCrud<T> : BaseCrud<T>
    {
        public CreacionCrud() :
        base("Crear")
        {
            AsignarTitulo($"Creación de {NombreDelObjeto}");
        }
    }
}
