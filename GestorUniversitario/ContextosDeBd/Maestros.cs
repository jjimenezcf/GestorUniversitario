using System;
using System.Linq;
using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario.ModeloBd.Enumerados;

namespace Gestor.Elementos.Universitario
{
    public class Maestros
    {

        public static void CrearDatosIniciales(ContextoUniversitario contexto)
        {
            // Look for any students.
            if (!contexto.Estudiantes.Any())
            {
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
                    contexto.Estudiantes.Add(estudiante);
                }
                contexto.SaveChanges();
            }

            if (!contexto.Cursos.Any())
            {
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
                    contexto.Cursos.Add(curso);
                }
                contexto.SaveChanges();
            }

            if (!contexto.Inscripciones.Any())
            {
                var inscripciones = new RegistroDeInscripcion[]
                {
                    new RegistroDeInscripcion{EstudianteId=1,CursoId=1,Grado=Grado.A},
                    new RegistroDeInscripcion{EstudianteId=1,CursoId=2,Grado=Grado.C},
                    new RegistroDeInscripcion{EstudianteId=1,CursoId=3,Grado=Grado.B},
                    new RegistroDeInscripcion{EstudianteId=2,CursoId=2,Grado=Grado.B},
                    new RegistroDeInscripcion{EstudianteId=2,CursoId=3,Grado=Grado.F},
                    new RegistroDeInscripcion{EstudianteId=2,CursoId=4,Grado=Grado.F},
                    new RegistroDeInscripcion{EstudianteId=3,CursoId=5},
                    new RegistroDeInscripcion{EstudianteId=4,CursoId=6},
                    new RegistroDeInscripcion{EstudianteId=4,CursoId=5,Grado=Grado.F},
                    new RegistroDeInscripcion{EstudianteId=5,CursoId=1,Grado=Grado.C},
                    new RegistroDeInscripcion{EstudianteId=6,CursoId=3},
                    new RegistroDeInscripcion{EstudianteId=7,CursoId=2,Grado=Grado.A},
                };
                foreach (RegistroDeInscripcion inscripcion in inscripciones)
                {
                    contexto.Inscripciones.Add(inscripcion);
                }
                contexto.SaveChanges();
            }
        }
    }
}
