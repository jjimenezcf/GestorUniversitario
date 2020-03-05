using MVCSistemaDeElementos.Descriptores;
using MVCSistemaDeElementos.UtilidadesIu;

namespace UtilidadesParaIu
{

    public class GestorCrud<T>
    {

        public string NombreDelObjeto => typeof(T).Name;
        public string ClaseDeElemento => NombreDelObjeto.Replace("Elemento", "");
                
        public CreacionCrud<T> Creador { get; }
        public EdicionCrud<T> Editor { get; }
        public DetalleCrud<T> Detalle { get; }
        public BorradoCrud<T> Supresor { get; }
        public DescriptorDeCrud<T> Descriptor { get; }

        public string IrAlCrud { get; set; }

        private string _Ruta => $"{NombreDelObjeto.Replace("Elemento", "")}s";

        public GestorCrud(DescriptorDeCrud<T> descriptor)
        {
            Descriptor = descriptor;    
            Creador = new CreacionCrud<T>();            
            Editor = new EdicionCrud<T>();
            Detalle = new DetalleCrud<T>();
            Supresor = new BorradoCrud<T>();
        }

    }
}
