using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using GestorUniversitario.ModeloBd;
using GestorUniversitario.ModeloBd.Enumerados;

namespace GestorUniversitario.ContextosDeBd
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

            var estudiantes = new RegistroDeEstudiante[]
            {
            new RegistroDeEstudiante{Apellido="Carson",Nombre="Alexander",InscritoEl=DateTime.Parse("2005-09-01")},
            new RegistroDeEstudiante{Apellido="Meredith",Nombre="Alonso",InscritoEl=DateTime.Parse("2002-09-01")},
            new RegistroDeEstudiante{Apellido="Arturo",Nombre="Anand",InscritoEl=DateTime.Parse("2003-09-01")},
            new RegistroDeEstudiante{Apellido="Gytis",Nombre="Barzdukas",InscritoEl=DateTime.Parse("2002-09-01")},
            new RegistroDeEstudiante{Apellido="Yan",Nombre="Li",InscritoEl=DateTime.Parse("2002-09-01")},
            new RegistroDeEstudiante{Apellido="Peggy",Nombre="Justice",InscritoEl=DateTime.Parse("2001-09-01")},
            new RegistroDeEstudiante{Apellido="Laura",Nombre="Norman",InscritoEl=DateTime.Parse("2003-09-01")},
            new RegistroDeEstudiante{Apellido="Nino",Nombre="Olivetto",InscritoEl=DateTime.Parse("2005-09-01")}
            };
            foreach (RegistroDeEstudiante estudiante in estudiantes)
            {
                context.Estudiantes.Add(estudiante);
            }
            context.SaveChanges();

            var courses = new RegistroDeCurso[]
            {
            new RegistroDeCurso{Titulo="Chemistry",Creditos=3},
            new RegistroDeCurso{Titulo="Microeconomics",Creditos=3},
            new RegistroDeCurso{Titulo="Macroeconomics",Creditos=3},
            new RegistroDeCurso{Titulo="Calculus",Creditos=4},
            new RegistroDeCurso{Titulo="Trigonometry",Creditos=4},
            new RegistroDeCurso{Titulo="Composition",Creditos=3},
            new RegistroDeCurso{Titulo="Literature",Creditos=4}
            };
            foreach (RegistroDeCurso curso in courses)
            {
                context.Cursos.Add(curso);
            }
            context.SaveChanges();

            var enrollments = new RegistroDeInscripcion[]
            {
            new RegistroDeInscripcion{EstudianteID=1,CursoID=1,Grado=Grado.A},
            new RegistroDeInscripcion{EstudianteID=1,CursoID=2,Grado=Grado.C},
            new RegistroDeInscripcion{EstudianteID=1,CursoID=3,Grado=Grado.B},
            new RegistroDeInscripcion{EstudianteID=2,CursoID=2,Grado=Grado.B},
            new RegistroDeInscripcion{EstudianteID=2,CursoID=3,Grado=Grado.F},
            new RegistroDeInscripcion{EstudianteID=2,CursoID=4,Grado=Grado.F},
            new RegistroDeInscripcion{EstudianteID=3,CursoID=5},
            new RegistroDeInscripcion{EstudianteID=4,CursoID=6},
            new RegistroDeInscripcion{EstudianteID=4,CursoID=5,Grado=Grado.F},
            new RegistroDeInscripcion{EstudianteID=5,CursoID=1,Grado=Grado.C},
            new RegistroDeInscripcion{EstudianteID=6,CursoID=3},
            new RegistroDeInscripcion{EstudianteID=7,CursoID=2,Grado=Grado.A},
            };
            foreach (RegistroDeInscripcion inscripcion in enrollments)
            {
                context.Inscripciones.Add(inscripcion);
            }
            context.SaveChanges();

        }
    }
}
