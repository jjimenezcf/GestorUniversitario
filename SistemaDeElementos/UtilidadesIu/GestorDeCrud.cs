using ModeloDeDto;
using MVCSistemaDeElementos.Descriptores;

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
