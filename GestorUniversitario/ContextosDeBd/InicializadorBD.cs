using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using GestorUniversitario.BdModelo;

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

            var estudiantes = new BdEstudiante[]
            {
            new BdEstudiante{Apellido="Carson",Nombre="Alexander",InscritoEl=DateTime.Parse("2005-09-01")},
            new BdEstudiante{Apellido="Meredith",Nombre="Alonso",InscritoEl=DateTime.Parse("2002-09-01")},
            new BdEstudiante{Apellido="Arturo",Nombre="Anand",InscritoEl=DateTime.Parse("2003-09-01")},
            new BdEstudiante{Apellido="Gytis",Nombre="Barzdukas",InscritoEl=DateTime.Parse("2002-09-01")},
            new BdEstudiante{Apellido="Yan",Nombre="Li",InscritoEl=DateTime.Parse("2002-09-01")},
            new BdEstudiante{Apellido="Peggy",Nombre="Justice",InscritoEl=DateTime.Parse("2001-09-01")},
            new BdEstudiante{Apellido="Laura",Nombre="Norman",InscritoEl=DateTime.Parse("2003-09-01")},
            new BdEstudiante{Apellido="Nino",Nombre="Olivetto",InscritoEl=DateTime.Parse("2005-09-01")}
            };
            foreach (BdEstudiante estudiante in estudiantes)
            {
                context.Estudiantes.Add(estudiante);
            }
            context.SaveChanges();

            var courses = new BdCurso[]
            {
            new BdCurso{Titulo="Chemistry",Creditos=3},
            new BdCurso{Titulo="Microeconomics",Creditos=3},
            new BdCurso{Titulo="Macroeconomics",Creditos=3},
            new BdCurso{Titulo="Calculus",Creditos=4},
            new BdCurso{Titulo="Trigonometry",Creditos=4},
            new BdCurso{Titulo="Composition",Creditos=3},
            new BdCurso{Titulo="Literature",Creditos=4}
            };
            foreach (BdCurso curso in courses)
            {
                context.Cursos.Add(curso);
            }
            context.SaveChanges();

            var enrollments = new BdInscripcion[]
            {
            new BdInscripcion{EstudianteID=1,CursoID=1,Grado=Grado.A},
            new BdInscripcion{EstudianteID=1,CursoID=2,Grado=Grado.C},
            new BdInscripcion{EstudianteID=1,CursoID=3,Grado=Grado.B},
            new BdInscripcion{EstudianteID=2,CursoID=2,Grado=Grado.B},
            new BdInscripcion{EstudianteID=2,CursoID=3,Grado=Grado.F},
            new BdInscripcion{EstudianteID=2,CursoID=4,Grado=Grado.F},
            new BdInscripcion{EstudianteID=3,CursoID=5},
            new BdInscripcion{EstudianteID=4,CursoID=6},
            new BdInscripcion{EstudianteID=4,CursoID=5,Grado=Grado.F},
            new BdInscripcion{EstudianteID=5,CursoID=1,Grado=Grado.C},
            new BdInscripcion{EstudianteID=6,CursoID=3},
            new BdInscripcion{EstudianteID=7,CursoID=2,Grado=Grado.A},
            };
            foreach (BdInscripcion inscripcion in enrollments)
            {
                context.Inscripciones.Add(inscripcion);
            }
            context.SaveChanges();

        }
    }
}
