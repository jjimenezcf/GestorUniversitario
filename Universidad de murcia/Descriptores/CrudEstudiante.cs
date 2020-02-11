﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.Universitario.ModeloIu;
using UtilidadesParaIu;

namespace UniversidadDeMurcia.Descriptores
{
    public class CrudEstudiante : DescriptorDeCrud<ElementoEstudiante>
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

            DefinirColumnasDelGrid();

        }


        protected override void DefinirColumnasDelGrid()
        {
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

        public override void MapearElementosAlGrid(IEnumerable<ElementoEstudiante> elementos)
        {
            base.MapearElementosAlGrid(elementos);

            foreach (var estudiante in elementos)
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
                Grid.filas.Add(fila);
            }
        }

    }
}
