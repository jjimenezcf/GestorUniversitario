using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCSistemaDeElementos.UtilidadesIu
{
    public class DetalleCrud<T> : BaseCrud<T>
    {
        public string TituloDetalle { get; private set; }
        public DetalleCrud() :
        base("Detalle")
        {
            AsignarTitulo($"Detalle de {NombreDelObjeto}");
        }

        public void AsignarTituloDetalle(string titulo)
        {
            TituloDetalle = titulo;
        }
    }

}
