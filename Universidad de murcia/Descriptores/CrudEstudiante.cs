using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.Universitario.ModeloIu;

namespace UniversidadDeMurcia.Descriptores
{
    public class CrudEstudiante : DescriptorDeCrud
    {
        public CrudEstudiante()
        : base(nameof(ElementoEstudiante), ruta: "Estudiantes", titulo: "Mantenimiento de estudiantes")
        {

            var bloque = new Bloque($"{Filtro.Id}_b3", "Específico", new Dimension(1, 2));

            var selector = new Selector(idModal: "selector_curso",
                                        etiqueta: "Curso",
                                        propiedad: "cursoInscrito",
                                        ayuda: "seleccionar curso",
                                        posicion: new Posicion() { fila = 0, columna = 0 },
                                        paraFiltrar: nameof(ElementoCurso.Id),
                                        paraMostrar: nameof(ElementoCurso.Titulo));

            bloque.AnadirControl(selector);

            Filtro.AnadirBloque(bloque);
        }
    }
}
