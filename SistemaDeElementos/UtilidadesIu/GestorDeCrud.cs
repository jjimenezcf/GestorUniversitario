using Gestor.Elementos.ModeloIu;
using MVCSistemaDeElementos.Descriptores;
using MVCSistemaDeElementos.UtilidadesIu;

namespace UtilidadesParaIu
{

    public class GestorCrud<T> where T : Elemento
    {
        public DescriptorDeCrud<T> Descriptor { get; }


        public GestorCrud(DescriptorDeCrud<T> descriptor)
        {
            Descriptor = descriptor;    
        }

    }
}
