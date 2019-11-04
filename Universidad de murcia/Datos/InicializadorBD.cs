using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversidadDeMurcia.Datos;
using UniversidadDeMurcia.Models;

namespace UniversidadDeMurcia.Datos
{
    public class InicializadorBD
    {

        public static void Inicializar(ContextoUniversitario context)
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();

            // Look for any students.
            if (context.Estudiantes.Any())
            {
                return;   // DB has been seeded
            }

            var estudiantes = new Estudiante[]
            {
            new Estudiante{Apellido="Carson",Nombre="Alexander",InscritoEl=DateTime.Parse("2005-09-01")},
            new Estudiante{Apellido="Meredith",Nombre="Alonso",InscritoEl=DateTime.Parse("2002-09-01")},
            new Estudiante{Apellido="Arturo",Nombre="Anand",InscritoEl=DateTime.Parse("2003-09-01")},
            new Estudiante{Apellido="Gytis",Nombre="Barzdukas",InscritoEl=DateTime.Parse("2002-09-01")},
            new Estudiante{Apellido="Yan",Nombre="Li",InscritoEl=DateTime.Parse("2002-09-01")},
            new Estudiante{Apellido="Peggy",Nombre="Justice",InscritoEl=DateTime.Parse("2001-09-01")},
            new Estudiante{Apellido="Laura",Nombre="Norman",InscritoEl=DateTime.Parse("2003-09-01")},
            new Estudiante{Apellido="Nino",Nombre="Olivetto",InscritoEl=DateTime.Parse("2005-09-01")}
            };
            foreach (Estudiante estudiante in estudiantes)
            {
                context.Estudiantes.Add(estudiante);
            }
            context.SaveChanges();

            var courses = new Curso[]
            {
            new Curso{Titulo="Chemistry",Creditos=3},
            new Curso{Titulo="Microeconomics",Creditos=3},
            new Curso{Titulo="Macroeconomics",Creditos=3},
            new Curso{Titulo="Calculus",Creditos=4},
            new Curso{Titulo="Trigonometry",Creditos=4},
            new Curso{Titulo="Composition",Creditos=3},
            new Curso{Titulo="Literature",Creditos=4}
            };
            foreach (Curso curso in courses)
            {
                context.Cursos.Add(curso);
            }
            context.SaveChanges();

            var enrollments = new Inscripcion[]
            {
            new Inscripcion{EstudianteID=1,CursoID=1050,Grado=Grado.A},
            new Inscripcion{EstudianteID=1,CursoID=4022,Grado=Grado.C},
            new Inscripcion{EstudianteID=1,CursoID=4041,Grado=Grado.B},
            new Inscripcion{EstudianteID=2,CursoID=1045,Grado=Grado.B},
            new Inscripcion{EstudianteID=2,CursoID=3141,Grado=Grado.F},
            new Inscripcion{EstudianteID=2,CursoID=2021,Grado=Grado.F},
            new Inscripcion{EstudianteID=3,CursoID=1050},
            new Inscripcion{EstudianteID=4,CursoID=1050},
            new Inscripcion{EstudianteID=4,CursoID=4022,Grado=Grado.F},
            new Inscripcion{EstudianteID=5,CursoID=4041,Grado=Grado.C},
            new Inscripcion{EstudianteID=6,CursoID=1045},
            new Inscripcion{EstudianteID=7,CursoID=3141,Grado=Grado.A},
            };
            foreach (Inscripcion inscripcion in enrollments)
            {
                context.Inscripciones.Add(inscripcion);
            }
            context.SaveChanges();

        }
    }
}
