using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;

namespace UniversidadDeMurcia.Descriptores
{
    public class Posicion
    {
        int fila { get; set; }
        int columna { get; set; }
    }

    public class Dimension
    {
        public int Filas { get; private set; }
        public int Columnas { get; private set; }

        public Dimension(int filas, int columnas)
        {
            Filas = filas;
            Columnas = columnas;
        }
    }

    public class Columnas
    {
        public string etiqueta { get; set; }
        public string propiedad { get; set; }
        public string visible { get; set; }
        public string tipo { get; set; }
    }

    public class Control
    {
        public string id { get; set; }
        public string tipo { get; set; }
        public string etiqueta { get; set; }
        public string propiedad { get; set; }
        public string ayuda { get; set; }
        public Posicion posicion { get; set; }
        public ICollection<Valor> valores { get; set; }
    }

    public class Selector : Control
    {
        public string propiedadParaFiltrar { get; set; } = "Id";
        public string propiedadParaMostrar { get; set; }
        public PanelDeSeleccion modal { get; set; }

        public Selector(string idSelector)
        {
            tipo = nameof(Selector);
            id = idSelector;
        }
    }

    public class PanelDeSeleccion : Control
    {
        public Selector selector { get; set; }
        public string gestorDeElementos { get; set; }
        public string claseDeElemento { get; set; }
        public ICollection<Columnas> columnasDelGrid { get; set; }
        public string Registros { get; set; }

        public PanelDeSeleccion(Selector selectorAsociado)
        {
            selector = selectorAsociado;
            selector.modal = this;
        }
    }
    
    public class TablaBloque
    {
        public string Id { get; private set; }
        public Dimension Dimension { get; private set; }
        public ICollection<Control> Controles { get; set; }

        public TablaBloque(string identificador, Dimension dimension, ICollection<Control> controles)
        {
            Id = identificador;
            Dimension = dimension;
            Controles = controles;
        }

    }

    public class Bloque
    {
        public string Id { get; private set; }

        public string Titulo { get; private set; }

        public TablaBloque Tabla { get; set; }

        public ICollection<Control> Controles => Tabla.Controles;

        public Bloque(string identificador, string titulo, Dimension dimension)
        {
            Id = identificador;
            Titulo = titulo;
            Tabla = new TablaBloque($"{Id}_Tbl", dimension, new List<Control>());
        }
    }

    public class ZonaDeOpciones
    {
        public ICollection<Opcion> Opciones { get; set; }
    }

    public class ZonaDeGrid
    {
        public ICollection<Columnas> columnas { get; set; }
        public string Registros { get; set; }
    }

    public class ZonaDeFiltro
    {
        public ICollection<Bloque> Bloques { get; private set; } = new List<Bloque>();

        public string Id  { get; private set; }

        public ZonaDeFiltro(string identificador)
        {
            Id = $"flt_{identificador}";

            var b1 = new Bloque($"{Id}_b1", "General", new Dimension(1, 4));
            var b2 = new Bloque($"{Id}_b2", "Común", new Dimension(1, 4));

            Bloques.Add(b1);
            Bloques.Add(b2);
        }

        public void Add(Bloque bloque)
        {
            Bloques.Add(bloque);
        }

        public Bloque Get(string identificador)
        {
            foreach(Bloque b in Bloques)
            {
                if (b.Id == identificador)
                    return b;
            }

            throw new Exception($"El bloque {identificador} no está en la zona de filtrado");
        }
    }

    public class DescriptorDeCrud<TElemento>
    where TElemento : ElementoBase
    {
        public string Elemento { get; private set; }
        public ZonaDeFiltro Filtro { get; private set; }
        public ZonaDeGrid Grid { get; set; }
        public ZonaDeOpciones Opciones { get; set; }

        public DescriptorDeCrud()
        {
            Elemento = typeof(TElemento).ToString().Replace("Elemento","");
            Filtro = new ZonaDeFiltro(Elemento);
        }

    }
          
    public class Valor
    {
        public string Nombreestudiante { get; set; }
        public string Fechadeinscripción { get; set; }
    }

    
    public class Opcion
    {
    }




}
