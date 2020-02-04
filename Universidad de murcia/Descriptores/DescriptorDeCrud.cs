using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversidadDeMurcia.Descriptores
{

    public class EstudianteDescriptorDeCrud: DescriptorDeCrud
    {
        public EstudianteDescriptorDeCrud():
        base("Estudiante")
        {
            Filtro = new ZonaDeFiltro();
            var bloque = new Bloque { nombre = "Específico", tablaBloque = new TablaBloque { columnas = 4, filas = 1 }, controles = new List<Control>() };
            Filtro.Bloques.Add(bloque);
        }
    }

    public class DescriptorDeCrud
    {
        public string Mantenimiento { get; set; }
        public ZonaDeFiltro Filtro { get; set; }
        public ZonaDeGrid Grid { get; set; }
        public ZonaDeOpciones Opciones { get; set; }

        public DescriptorDeCrud()
        {
            Mantenimiento = "Estudiante";
            Filtro = new ZonaDeFiltro();
        }

        public DescriptorDeCrud(string nombre)
        {
            Mantenimiento = nombre;
        }
    }

    public class ZonaDeOpciones
    {
        public ICollection<Opcion> Opciones { get; set; }
    }

    public class ZonaDeGrid
    {
        public ICollection<Columnasdelgrid> columnas { get; set; }
        public string Registros { get; set; }
    }

    public class ZonaDeFiltro
    {
        public ICollection<Bloque>  Bloques { get; set; }

        public ZonaDeFiltro()
        {
            Bloques = new List<Bloque>
            {
                new Bloque { nombre = "General", tablaBloque = new TablaBloque {columnas = 4 , filas = 1}, controles = new List<Control>()  },
                new Bloque { nombre = "Común", tablaBloque = new TablaBloque {columnas = 4 , filas = 1}, controles = new List<Control>()  }
            };
        }
    }

    public class TablaBloque
    {
        public int columnas { get; set; }
        public int filas { get; set; }

    }

    public class Bloque
    {
        public string nombre { get; set; }
        public TablaBloque tablaBloque { get; set; }
        public ICollection<Control> controles { get; set; }
    }

    public class Control
    {
        public string tipo { get; set; }
        public string etiqueta { get; set; }
        public string propiedad { get; set; }
        public string ayuda { get; set; }
        public string accion { get; set; }
        public string fila { get; set; }
        public string columna { get; set; }
        public ICollection<Valor> valores { get; set; }
        public string gestorDeElementos { get; set; }
        public string claseDeElemento { get; set; }
        public string propiedadParaFiltrar { get; set; }
        public string propiedadParaMostrar { get; set; }
        public Columnasdelgrid[] columnasDelGrid { get; set; }
        public string Registros { get; set; }
    }

    public class Valor
    {
        public string Nombreestudiante { get; set; }
        public string Fechadeinscripción { get; set; }
    }

    public class Columnasdelgrid
    {
        public string etiqueta { get; set; }
        public string propiedad { get; set; }
        public string visible { get; set; }
        public string tipo { get; set; }
    }

    public class Opcion
    {
    }



}
