using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversidadDeMurcia.UtilidadesIu
{

    public class EdicionCrud<T> : BaseCrud<T>
    {
        public EdicionCrud() :
        base("Editar")
        {
            AsignarTitulo($"Edición de {NombreDelObjeto}");
            AsignarAccion($"Modificar{NombreDelObjeto}");
            AsignarFormulario("Modificar");
        }
    }

}
