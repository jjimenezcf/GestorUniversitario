using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Elementos.Universitario.ModeloIu;
using UniversidadDeMurcia.Objetos;
using Gestor.Elementos.Universitario.ModeloBd;
using System.Collections.Generic;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Gestor.Errores;
using Gestor.Elementos.Universitario;
using UtilidadesParaIu;
using Componentes;
using Extensiones.String;

namespace UniversidadDeMurcia.Controllers
{
    public class EstudiantesController : EntidadController<ContextoUniversitario, RegistroDeEstudiante, ElementoEstudiante>
    {
        public EstudiantesController(GestorDeEstudiantes gestorDeEstudiantes, GestorDeErrores gestorDeErrores) :
            base(gestorDeEstudiantes, gestorDeErrores)
        {
            GestorDelCrud.Creador.AsignarTitulo("Crear un nuevo estudiante");
            GestorDelCrud.Modales[nameof(SelectorDeCurso)] = new SelectorDeCurso(gestorDeEstudiantes.Contexto, gestorDeEstudiantes.Mapeador).Selector;
        }

        protected override List<ColumnaDelGrid>DefinirColumnasDelGrid()
        {
            var columnasDelGrid = base.DefinirColumnasDelGrid().ToList();

            var columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(ElementoEstudiante.Id), Tipo = typeof(int), Visible = false };
            columnasDelGrid.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(ElementoEstudiante.Apellido), Ordenar = true, Ruta = "Estudiantes", Accion = nameof(IraMantenimientoEstudiante)};
            columnasDelGrid.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid {Nombre = nameof(ElementoEstudiante.Nombre)};
            columnasDelGrid.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid
            {
                Nombre = nameof(ElementoEstudiante.InscritoEl),
                Tipo = typeof(DateTime),
                Alineada = Aliniacion.centrada,          
                Ordenar = true,
                Ruta = "Estudiantes",
                Accion = nameof(IraMantenimientoEstudiante)
            };
            columnasDelGrid.Add(columnaDelGrid);

            return columnasDelGrid;
        }

        protected override List<FilaDelGrid> MapearElementosAlGrid(IEnumerable<ElementoEstudiante> elementos)
        {
            var listaDeEstudiantes = base.MapearElementosAlGrid(elementos);
            var columnasDelGrid = GestorDelCrud.Mantenimiento.ColumnasDelGrid;

            foreach (var estudiante in elementos)
            {
                var fila = new FilaDelGrid();
                foreach (ColumnaDelGrid columna in columnasDelGrid)
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
                listaDeEstudiantes.Add(fila);
            }
            return listaDeEstudiantes;
        }

        public IActionResult IraMantenimientoEstudiante(string orden)
        {
            var (estudiantes, total) = LeerOrdenados(0,10,orden);

            GestorDelCrud.Mantenimiento.FilasDelGrid = MapearElementosAlGrid(estudiantes);

            return View(GestorDelCrud.Mantenimiento.Vista, estudiantes.ToList());
        }


        private void PrepararProximoOrden(string orden)
        {
            ViewData[nameof(ElementoEstudiante.Apellido)] = orden.IsNullOrEmpty() || orden == $"{nameof(ElementoEstudiante.Apellido)}Asc"
                                                        ? $"{nameof(ElementoEstudiante.Apellido)}Des"
                                                        : $"{nameof(ElementoEstudiante.Apellido)}Asc";

            ViewData[nameof(ElementoEstudiante.InscritoEl)] = orden.IsNullOrEmpty() || orden == $"{nameof(ElementoEstudiante.InscritoEl)}Asc"
                                                        ? $"{nameof(ElementoEstudiante.InscritoEl)}Des"
                                                        : $"{nameof(ElementoEstudiante.InscritoEl)}Asc";
        }

        private IEnumerable<ElementoEstudiante> OrdenarListaDeEstudiantes(IEnumerable<ElementoEstudiante> estudiantes, string orden)
        {
            foreach (var columna in GestorDelCrud.Mantenimiento.ColumnasDelGrid)
            {
                if (!columna.Ordenar)
                    continue;
                if (orden != null && orden.Contains(columna.Nombre) && columna.Nombre == nameof(ElementoEstudiante.Apellido))
                {
                    if (orden.EndsWith("Des"))
                    {
                        columna.Sentido = "Asc";
                        return estudiantes.OrderByDescending(c => c.Apellido);
                    }
                    columna.Sentido = "Des";
                    return estudiantes.OrderBy(c => c.Apellido);
                }

                if (orden != null && orden.Contains(columna.Nombre) && columna.Nombre == nameof(ElementoEstudiante.InscritoEl))
                {
                    if (orden.EndsWith("Des"))
                    {
                        columna.Sentido = "Asc";
                        return estudiantes.OrderByDescending(c => c.InscritoEl);
                    }
                    columna.Sentido = "Des";
                    return estudiantes.OrderBy(c => c.InscritoEl);
                }
            }

            return estudiantes.OrderBy(s => s.Apellido);
        }

        public IActionResult IraCrearEstudiante()
        {
            return View(GestorDelCrud.Creador.Vista, new ElementoEstudiante());
        }

        public IActionResult IraDetalleEstudiante(int? id)
        {
            GestorDelCrud.Detalle.AsignarTituloDetalle("Inscripciones");
            return View(GestorDelCrud.Detalle.Vista, LeerDetalle(id));
        }

        public IActionResult IraBorrarEstudiante(int? id)
        {
            return View(GestorDelCrud.Supresor.Vista, LeerEstudiante(id));
        }

        public IActionResult IraEditarEstudiante(int? id)
        {
            return View(GestorDelCrud.Editor.Vista, LeerEstudiante(id));
        }


        [HttpPost, ActionName(nameof(CrearEstudiante))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEstudiante([Bind("ID,Apellido,Nombre,InscritoEl")] ElementoEstudiante estudiante)
        {
            return await CrearObjeto(estudiante);
        }



        [HttpPost, ActionName(nameof(ModificarEstudiante))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarEstudiante(int id, [Bind("Id,Apellido,Nombre,InscritoEl")] ElementoEstudiante estudiante)
        {
            return await ModificarObjeto(id, estudiante);
        }



        [HttpPost, ActionName(nameof(BorrarEstudiante))]
        [ValidateAntiForgeryToken]
        public IActionResult BorrarEstudiante(int id)
        {

            GestorDeElementos.BorrarPorId(id);
            return RedirectToAction(GestorDelCrud.Mantenimiento.Ir);
        }

        private ElementoEstudiante LeerEstudiante(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del estudiante no puede ser nulo");
            }

            var estudiante = (ElementoEstudiante)GestorDeElementos.LeerElementoPorId((int)id);
            if (estudiante == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del estudiante no se pudo localizar");
            }

            return estudiante;
        }

        private ElementoEstudiante LeerDetalle(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del estudiante no puede ser nulo");
            }

            var estudiante = (ElementoEstudiante)GestorDeElementos.LeerElementoConDetalle((int)id);
            if (estudiante == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del estudiante no se pudo localizar");
            }

            return estudiante;
        }


    }

}
