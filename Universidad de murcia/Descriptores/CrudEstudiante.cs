using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.Universitario.ModeloIu;

namespace UniversidadDeMurcia.Descriptores
{
    public class CrudEstudiante: DescriptorDeCrud<ElementoEstudiante>
    {
        public CrudEstudiante()
        :base()
        {
           var b = new Bloque($"{Filtro.Id}_b3", "Específico", new Dimension(1, 4));
           b.Controles.Add(new Selector("SelectorCurso"));
           Filtro.Add(b);            
        }
    }
}
