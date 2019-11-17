using System;
using System.Linq;
using System.Threading.Tasks;
using Extensiones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorUniversitario.ContextosDeBd;
using GestorUniversitario.ModeloDeClases;
using UniversidadDeMurcia.Objetos;

namespace UniversidadDeMurcia.Controllers
{

    public class EstudiantesController : EntidadController<Estudiante>
    {

        public EstudiantesController(ContextoUniversitario context, Gestor.Errores.Errores gestorErrores):
            base(context, gestorErrores)
        {
            GestorDelCrud.GestorDeCreacion.AsignarTitulo("Crear un nuevo estudiante");
        }


        public async Task<IActionResult> IraMantenimientoEstudiante(string orden)
        {
            ViewData[EstudianteEnlace.Parametro.Nombre] = orden.IsNullOrEmpty() || orden == EstudianteEnlace.OrdenadoPor.NombreAsc 
                                                        ? EstudianteEnlace.OrdenadoPor.NombreDes 
                                                        : EstudianteEnlace.OrdenadoPor.NombreAsc;

            ViewData[EstudianteEnlace.Parametro.InscritoEl] = orden == EstudianteEnlace.OrdenadoPor.InscritoElAsc 
                                                        ? EstudianteEnlace.OrdenadoPor.InscritoElDes 
                                                        : EstudianteEnlace.OrdenadoPor.InscritoElAsc;

            var estudiantes = from s in ContextoDeBd.Estudiantes select s;
            estudiantes = orden switch
            {
                EstudianteEnlace.OrdenadoPor.NombreAsc => estudiantes.OrderBy(s => s.Apellido),
                EstudianteEnlace.OrdenadoPor.NombreDes => estudiantes.OrderByDescending(s => s.Apellido),
                EstudianteEnlace.OrdenadoPor.InscritoElDes => estudiantes.OrderByDescending(s => s.InscritoEl),
                EstudianteEnlace.OrdenadoPor.InscritoElAsc => estudiantes.OrderBy(s => s.InscritoEl),
                _ => estudiantes.OrderBy(s => s.Apellido),
            };
            return View(GestorDelCrud.GestorDeMantenimiento.Vista, await estudiantes.AsNoTracking().ToListAsync());
        }

        public IActionResult IraCrearEstudiante()
        {
            return View(GestorDelCrud.GestorDeCreacion.Vista, new Estudiante());
        }

        public async Task<IActionResult> IraDetalleEstudiante(int? id)
        {
            return View(GestorDelCrud.GestorDeDetalle.Vista, await LeerDetalleAsync(id));
        }

        public async Task<IActionResult> IraBorrarEstudiante(int? id)
        {
            return View(GestorDelCrud.GestorDeBorrado.Vista, await LeerEstudianteAsync(id));
        }

        public async Task<IActionResult> IraEditarEstudiante(int? id)
        {
            return View(GestorDelCrud.GestorDeEdicion.Vista, await LeerEstudianteAsync(id));
        }


        [HttpPost, ActionName(nameof(CrearEstudiante))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEstudiante([Bind("ID,Apellido,Nombre,InscritoEl")] Estudiante estudiante)
        {
            return await CrearObjeto(estudiante);
        }



        [HttpPost, ActionName(nameof(ModificarEstudiante))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarEstudiante(int id, [Bind("Id,Apellido,Nombre,InscritoEl")] Estudiante estudiante)
        {
            return await ModificarObjeto(id, estudiante);
        }



        [HttpPost, ActionName(nameof(BorrarEstudiante))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrarEstudiante(int id)
        {
            var estudiante = await ContextoDeBd.Estudiantes.FindAsync(id);
            ContextoDeBd.Estudiantes.Remove(estudiante);
            await ContextoDeBd.SaveChangesAsync();
            return RedirectToAction(GestorDelCrud.GestorDeMantenimiento.Ir);
        }

        private async Task<Estudiante> LeerEstudianteAsync(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del estudiante no puede ser nulo");
            }

            var estudiante = await ContextoDeBd.Estudiantes.FirstOrDefaultAsync(m => m.Id == id);
            if (estudiante == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del estudiante no se pudo localizar");
            }

            return estudiante;
        }

        private async Task<Estudiante> LeerDetalleAsync(int? id)
        {
            if (id == null)
            {
                GestorDeErrores.LanzarExcepcion("El id del estudiante no puede ser nulo");
            }

            var estudiante = await ContextoDeBd.Estudiantes
                .Include(i => i.Inscripciones)
                .ThenInclude(e => e.Curso)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiante == null)
            {
                GestorDeErrores.LanzarExcepcion($"El id {id} del estudiante no se pudo localizar");
            }

            return estudiante;
        }

    }
}
