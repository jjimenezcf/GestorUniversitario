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

        public EstudiantesController(ContextoUniversitario context, Gestor.Errores.Errores gestorErrores):
            base(context, gestorErrores, nameof(Estudiante))
        {
        }

        public IActionResult Index()
        {
            return RedirectToAction(CrudDelObjeto.IrAlCrud);
        }

        public async Task<IActionResult> IraMntEstudiantes(string orden)
        {
            ViewData[Estudiante.Parametro.Nombre] = orden.IsNullOrEmpty() || orden == Estudiante.OrdenadoPor.NombreAsc ? Estudiante.OrdenadoPor.NombreDes : Estudiante.OrdenadoPor.NombreAsc;
            ViewData[Estudiante.Parametro.InscritoEl] = orden == Estudiante.OrdenadoPor.InscritoElAsc ? Estudiante.OrdenadoPor.InscritoElDes : Estudiante.OrdenadoPor.InscritoElAsc;

            var estudiantes = from s in ContextoDeBd.Estudiantes select s;
            estudiantes = orden switch
            {
                Estudiante.OrdenadoPor.NombreAsc => estudiantes.OrderBy(s => s.Apellido),
                Estudiante.OrdenadoPor.NombreDes => estudiantes.OrderByDescending(s => s.Apellido),
                Estudiante.OrdenadoPor.InscritoElDes => estudiantes.OrderByDescending(s => s.InscritoEl),
                Estudiante.OrdenadoPor.InscritoElAsc => estudiantes.OrderBy(s => s.InscritoEl),
                _ => estudiantes.OrderBy(s => s.Apellido),
            };
            return View(CrudDelObjeto.VistaDelCrud, await estudiantes.AsNoTracking().ToListAsync());
        }

        public IActionResult IraCrearEstudiante()
        {
            return View(CrudDelObjeto.VistaDeCreacion);
        }

        public async Task<IActionResult> IraDetalleEstudiante(int? id)
        {
            return View(CrudDelObjeto.VistaDeDetalle, await LeerDetalleAsync(id));
        }

        public async Task<IActionResult> IraBorrarEstudiante(int? id)
        {
            return View(CrudDelObjeto.VistaDeBorrado, await LeerEstudianteAsync(id));
        }

        public async Task<IActionResult> IraEditarEstudiante(int? id)
        {
            return View(CrudDelObjeto.VistaDeEdicion, await LeerEstudianteAsync(id));
        }


        [HttpPost, ActionName(nameof(CrearEstudiante))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEstudiante([Bind("ID,Apellido,Nombre,InscritoEl")] Estudiante estudiante)
        {
            return await CrearObjeto(estudiante);
        }



        [HttpPost, ActionName(nameof(ModificarEstudiante))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarEstudiante(int id, [Bind("ID,Apellido,Nombre,InscritoEl")] Estudiante estudiante)
        {
            return await ModificarObjeto(id, estudiante);
        }



        [HttpPost, ActionName("EliminarEstudiante")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarEstudiante(int id)
        {
            var estudiante = await ContextoDeBd.Estudiantes.FindAsync(id);
            ContextoDeBd.Estudiantes.Remove(estudiante);
            await ContextoDeBd.SaveChangesAsync();
            return RedirectToAction(nameof(IraMntEstudiantes));
        }

        private bool EstudianteExists(int id)
        {
            return ContextoDeBd.Estudiantes.Any(e => e.ID == id);
        }

        private async Task<Estudiante> LeerEstudianteAsync(int? id)
        {
            if (id == null)
            {
                GestorErrores.LanzarExcepcion("El id del estudiante no puede ser nulo");
            }

            var estudiante = await ContextoDeBd.Estudiantes.FirstOrDefaultAsync(m => m.ID == id);
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

            var estudiante = await ContextoDeBd.Estudiantes
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
