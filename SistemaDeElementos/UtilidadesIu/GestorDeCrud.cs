using Gestor.Elementos.ModeloIu;
using MVCSistemaDeElementos.Descriptores;
using MVCSistemaDeElementos.UtilidadesIu;

namespace UtilidadesParaIu
{

    public class GestorCrud<T> where T : ElementoDto
    {
        public DescriptorDeCrud<T> Descriptor { get; }


        public GestorCrud(DescriptorDeCrud<T> descriptor)
        {
            Descriptor = descriptor;    
        }

    }
}
