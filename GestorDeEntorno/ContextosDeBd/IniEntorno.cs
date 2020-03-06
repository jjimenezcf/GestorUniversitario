using System;
using System.Linq;

namespace Gestor.Elementos.Entorno
{
    public class IniEntorno
    {

        public static void CrearDatosIniciales(CtoEntorno contexto)
        {
            // Look for any students.
            if (!contexto.Usuarios.Any())
            {
                var usuarios = new UsuarioReg[]
                {
                    new UsuarioReg{Login="alexande.carson@se.es",Apellido="Carson",Nombre="Alexander",Alta=DateTime.Parse("2005-09-01")},
                    new UsuarioReg{Login="alfonso.meredith@se.es",Apellido="Meredith",Nombre="Alonso",Alta=DateTime.Parse("2002-09-01")},
                    new UsuarioReg{Login="arturo.anand@se.es",Apellido="Anand",Nombre="Arturo",Alta=DateTime.Parse("2003-09-01")},
                    new UsuarioReg{Login="luis.sanchez@se.es",Apellido="Sánchez",Nombre="Luis",Alta=DateTime.Parse("2002-09-01")},
                    new UsuarioReg{Login="antonio.carrillo@se.es",Apellido="Carrillo",Nombre="Antonio",Alta=DateTime.Parse("2002-09-01")},
                    new UsuarioReg{Login="pepe.perez@se.es",Apellido="Pérez",Nombre="Pepe",Alta=DateTime.Parse("2001-09-01")},
                    new UsuarioReg{Login="javier.ros@se.es",Apellido="Ros",Nombre="Javier",Alta=DateTime.Parse("2003-09-01")},
                    new UsuarioReg{Login="ricardo.aguilera@se.es",Apellido="Aguilera",Nombre="Ricardo",Alta=DateTime.Parse("2005-09-01")}
                };
                foreach (UsuarioReg usuario in usuarios)
                {
                    contexto.Usuarios.Add(usuario);
                }
                contexto.SaveChanges();
            }
        }
    }
}
