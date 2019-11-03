using System;
using System.Linq;
using System.Threading.Tasks;
using Extensiones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversidadDeMurcia.Datos;
using UniversidadDeMurcia.Models;

namespace UniversidadDeMurcia.Controllers
{

    public class EstudiantesController : BaseController
    {
        private readonly ContextoUniversitario _context;

        public EstudiantesController(ContextoUniversitario context, Gestor.Errores.Errores gestorErrores):
            base(gestorErrores)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(IraMntEstudiantes));
        }

        public async Task<IActionResult> IraMntEstudiantes(string orden)
        {
            ViewBag.Mantenimiento = nameof(IraMntEstudiantes);
            ViewData[Estudiante.Parametro.Nombre] = orden.IsNullOrEmpty()  || orden == Estudiante.OrdenadoPor.NombreAsc ? Estudiante.OrdenadoPor.NombreDes: Estudiante.OrdenadoPor.NombreAsc;
            ViewData[Estudiante.Parametro.InscritoEl] = orden == Estudiante.OrdenadoPor.InscritoElAsc ? Estudiante.OrdenadoPor.InscritoElDes: Estudiante.OrdenadoPor.InscritoElAsc;

            var estudiantes = from s in _context.Estudiantes select s;
            estudiantes = orden switch
            {
                Estudiante.OrdenadoPor.NombreAsc => estudiantes.OrderBy(s => s.Apellido),
                Estudiante.OrdenadoPor.NombreDes => estudiantes.OrderByDescending(s => s.Apellido),
                Estudiante.OrdenadoPor.InscritoElDes => estudiantes.OrderByDescending(s => s.InscritoEl),
                Estudiante.OrdenadoPor.InscritoElAsc => estudiantes.OrderBy(s => s.InscritoEl),
                _ => estudiantes.OrderBy(s => s.Apellido),
            };
            return View("MntEstudiantes", await estudiantes.AsNoTracking().ToListAsync());
        }

        public IActionResult IraCrearEstudiante()
        {
            return View("CrearEstudiante");
        }

        public async Task<IActionResult> IraDetalleEstudiante(int? id)
        {
            return View("DetalleEstudiante", await LeerDetalleAsync(id));
        }

        public async Task<IActionResult> IraBorrarEstudiante(int? id)
        {
            return View("EliminarEstudiante", await LeerEstudianteAsync(id));
        }

        public async Task<IActionResult> IraEditarEstudiante(int? id)
        {
            return View("EditarEstudiante", await LeerEstudianteAsync(id));
        }


        [HttpPost, ActionName("CrearEstudiante")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEstudiante([Bind("ID,Apellido,Nombre,InscritoEl")] Estudiante estudiante)
        {

           try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(estudiante);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(IraMntEstudiantes));
                }
            }
            catch(DbUpdateException e)
            {
                ModelState.AddModelError("", $"No es posible crear el registro.");
                GestorErrores.Enviar("Error al crear un estudiante", e);
            }
            return View("CrearEstudiante",estudiante);
        }




        [HttpPost, ActionName("ModificarEstudiante")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarEstudiante(int id, [Bind("ID,Apellido,Nombre,InscritoEl")] Estudiante estudiante)
        {
            if (id != estudiante.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteExists(estudiante.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IraMntEstudiantes));
            }

            return View( "EditarEstudiante", estudiante);
        }

        [HttpPost, ActionName("EliminarEstudiante")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IraMntEstudiantes));
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiantes.Any(e => e.ID == id);
        }

        private async Task<Estudiante> LeerEstudianteAsync(int? id)
        {
            if (id == null)
            {
                GestorErrores.LanzarExcepcion("El id del estudiante no puede ser nulo");
            }

            var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(m => m.ID == id);
            if (estudiante == null)
            {
                GestorErrores.LanzarExcepcion($"El id {id} del estudiante no se pudo localizar");
            }

            return estudiante;
        }

        private async Task<Estudiante> LeerDetalleAsync(int? id)
        {
            if (id == null)
            {
                GestorErrores.LanzarExcepcion("El id del estudiante no puede ser nulo");
            }

            var estudiante = await _context.Estudiantes
                .Include(i => i.Inscripciones)
                .ThenInclude(e => e.Curso)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (estudiante == null)
            {
                GestorErrores.LanzarExcepcion($"El id {id} del estudiante no se pudo localizar");
            }

            return estudiante;
        }

    }
}
