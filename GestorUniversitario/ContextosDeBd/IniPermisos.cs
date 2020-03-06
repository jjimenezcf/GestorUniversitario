using System;
using System.Linq;

namespace Gestor.Elementos.Permiso
{
    public class IniPermisos
    {

        public static void CrearDatosIniciales(CtoPermisos contexto)
        {

            if (!contexto.Cursos.Any())
            {
                var courses = new PermisoReg[]
                {
                  new PermisoReg{Nombre="Chemistry",Clase=3},
                  new PermisoReg{Nombre="Microeconomics",Clase=3},
                  new PermisoReg{Nombre="Macroeconomics",Clase=3},
                  new PermisoReg{Nombre="Calculus",Clase=4},
                  new PermisoReg{Nombre="Trigonometry",Clase=4},
                  new PermisoReg{Nombre="Composition",Clase=3},
                  new PermisoReg{Nombre="Literature",Clase=4}
                };
                foreach (PermisoReg curso in courses)
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
