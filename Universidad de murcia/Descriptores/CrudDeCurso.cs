using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos;
using Gestor.Elementos.Universitario.ModeloIu;
using UtilidadesParaIu;

namespace UniversidadDeMurcia.Descriptores
{
    public class CrudCurso : DescriptorDeCrud<ElementoCurso>
    {
        public CrudCurso(ModoDescriptor modo)
        : base(controlador: "Cursos", vista: "MantenimientoCurso", titulo: "Mantenimiento de cursos", modo: modo)
        {
            if (modo == ModoDescriptor.Mantenimiento)
               new Selector<ElementoEstudiante>(padre: new Bloque(Filtro, "Específico", new Dimension(1, 2)),
                                             etiqueta: "Estudiante",
                                             propiedad: CursoPor.EstudianteInscrito,
                                             ayuda: "Seleccionar estudiante",
                                             posicion: new Posicion() { fila = 0, columna = 0 },
                                             paraFiltrar: nameof(ElementoEstudiante.Id),
                                             paraMostrar: nameof(ElementoEstudiante.Apellido),
                                             descriptor: new CrudEstudiante(ModoDescriptor.Seleccion));

            DefinirVistaDeCreacion(accion: "IraCrearCurso", textoMenu: "Crear curso");

            DefinirColumnasDelGrid();

        }

        protected override void DefinirColumnasDelGrid()
        {
            base.DefinirColumnasDelGrid();
            Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(ElementoCurso.Id), Visible = false, Tipo = typeof(int) });
            Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(ElementoCurso.Titulo), Titulo = "Título", Ordenar = false });
            Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(ElementoCurso.Creditos), Titulo = "Créditos", Tipo = typeof(int) });
        }

        public override void MapearElementosAlGrid(IEnumerable<ElementoCurso> elementos)
        {
            base.MapearElementosAlGrid(elementos);
            foreach (var curso in elementos)
            {
                var fila = new FilaDelGrid();
                foreach (ColumnaDelGrid columna in Grid.Columnas)
                {
                    CeldaDelGrid celda = new CeldaDelGrid(columna);
                    if (columna.Nombre == nameof(ElementoCurso.Id))
                        celda.Valor = curso.Id.ToString();
                    else
                    if (columna.Nombre == nameof(ElementoCurso.Titulo))
                        celda.Valor = curso.Titulo;
                    else
                    if (columna.Nombre == nameof(ElementoCurso.Creditos))
                        celda.Valor = curso.Creditos;
                    fila.Celdas.Add(celda);
                }
                Grid.Filas.Add(fila);
            }
        }

    }
}
