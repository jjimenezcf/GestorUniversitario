using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.Universitario.ModeloIu;

namespace UniversidadDeMurcia.Descriptores
{
    public class CrudEstudiante: DescriptorDeCrud
    {
        public CrudEstudiante()
        :base(nameof(ElementoEstudiante))
        {
           var b = new Bloque($"{Filtro.Id}_b3", "Específico", new Dimension(1,2));
           b.Controles.Add(new Selector("Des_Se_Cur", "Curso","cursoInscrito","seleccionar curso", new Posicion(){fila=0, columna=0}, nameof(ElementoCurso.Id),nameof(ElementoCurso.Titulo)));
           Filtro.Add(b);            
        }
    }
}
