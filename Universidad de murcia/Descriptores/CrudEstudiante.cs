using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.Universitario.ModeloIu;
using UtilidadesParaIu;

namespace UniversidadDeMurcia.Descriptores
{
    public class CrudEstudiante : DescriptorDeCrud<ElementoEstudiante>
    {
        public CrudEstudiante(ModoDescriptor modo)
        : base(ruta: "Estudiantes", vista: "MantenimientoEstudiante", titulo: "Mantenimiento de estudiantes", modo: modo)
        {
            if (modo == ModoDescriptor.Mantenimiento)
                new Selector<ElementoCurso>(padre: new Bloque(Filtro, titulo: "Específico", dimension: new Dimension(1, 2)),
                                        etiqueta: "Curso",
                                        propiedad: "cursoInscrito",
                                        ayuda: "Seleccionar curso",
                                        posicion: new Posicion() { fila = 0, columna = 0 },
                                        paraFiltrar: nameof(ElementoCurso.Id),
                                        paraMostrar: nameof(ElementoCurso.Titulo),
                                        descriptor: new CrudCurso(ModoDescriptor.Seleccion));

            DefinirVistaDeCreacion(accion: "IraCrearEstudiante", textoMenu: "Crear estudiante");

            DefinirColumnasDelGrid();
        }


        protected override void DefinirColumnasDelGrid()
        {
            base.DefinirColumnasDelGrid();

            var columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(ElementoEstudiante.Id), Tipo = typeof(int), Visible = false };
            Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(ElementoEstudiante.Apellido), Ordenar = true, Ruta = "Estudiantes", Accion = "IraMantenimientoEstudiante" };
            Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(ElementoEstudiante.Nombre) };
            Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid
            {
                Nombre = nameof(ElementoEstudiante.InscritoEl),
                Tipo = typeof(DateTime),
                Alineada = Aliniacion.centrada,
                Ordenar = true,
                Ruta = "Estudiantes",
                Accion = "IraMantenimientoEstudiante"
            };
            Grid.Columnas.Add(columnaDelGrid);
        }

        public override void MapearElementosAlGrid((IEnumerable<ElementoEstudiante> elementos, int totalEnBd) leidos)
        {
            base.MapearElementosAlGrid(leidos);
            foreach (var estudiante in leidos.elementos)
            {
                var fila = new FilaDelGrid();
                foreach (ColumnaDelGrid columna in Grid.Columnas)
                {
                    CeldaDelGrid celda = new CeldaDelGrid(columna);
                    if (columna.Nombre == nameof(ElementoEstudiante.Id))
                        celda.Valor = estudiante.Id.ToString();
                    else
                    if (columna.Nombre == nameof(ElementoEstudiante.Apellido))
                        celda.Valor = estudiante.Apellido;
                    else
                    if (columna.Nombre == nameof(ElementoEstudiante.Nombre))
                        celda.Valor = estudiante.Nombre.ToString();
                    else
                    if (columna.Nombre == nameof(ElementoEstudiante.InscritoEl))
                        celda.Valor = estudiante.InscritoEl.ToString();

                    fila.Celdas.Add(celda);
                }
                Grid.Filas.Add(fila);
            }
        }

    }
}
