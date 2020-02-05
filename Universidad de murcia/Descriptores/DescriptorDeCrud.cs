using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using UtilidadesParaIu;

namespace UniversidadDeMurcia.Descriptores
{
    public enum TipoControl { Selector, Editor, Label, Referencia, Desplegable, Lista, Fecha}

    public class Posicion
    {
        public int fila { get; set; }
        public int columna { get; set; }
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
        public string Id { get; private set; }
        public string Etiqueta { get; private set; }
        public string Propiedad { get; private set; }
        public string Ayuda { get; private set; }
        public Posicion Posicion { get; private set; }
        
        public TipoControl Tipo { get; protected set; }

        public Control(string id, string etiqueta, string propiedad, string ayudad, Posicion posicion)
        {
            Id = id.ToLower();
            Etiqueta = etiqueta;
            Propiedad = propiedad;
            Ayuda = ayudad;
            Posicion = posicion;
        }
    }

    public class Selector : Control
    {
        public string propiedadParaFiltrar { get; private set; }
        public string propiedadParaMostrar { get; private set; }
        public PanelDeSeleccion Modal { get; set; }

        public Selector(string idModal, string etiqueta, string propiedad, string ayudad, Posicion posicion, string paraFiltrar, string paraMostrar)
        :base($"{idModal}.Selector", etiqueta, propiedad,ayudad, posicion)
        {
        //private string IdGrid => $"{IdModal}.Grid".ToLower();
        //private string IdSelector => $"{IdModal}.Selector";

        Tipo = TipoControl.Selector;
            propiedadParaFiltrar = paraFiltrar;
            propiedadParaMostrar = paraMostrar;
            Modal = new PanelDeSeleccion(idModal, this);
        }
    }

    public class Desplegable: Control
    {
        public ICollection<Valor> valores { get; set; }
        public Desplegable(string idSelector, string etiqueta, string propiedad, string ayudad, Posicion posicion)
        : base(idSelector, etiqueta, propiedad, ayudad, posicion)
        {
            Tipo = TipoControl.Desplegable;
        }
    }

    public class PanelDeSeleccion : Control
    {
        public Selector selector { get; set; }
        public string gestorDeElementos { get; set; }
        public string claseDeElemento { get; set; }
        public ICollection<Columnas> columnasDelGrid { get; set; }
        public string Registros { get; set; }

        public PanelDeSeleccion(string idModal, Selector selectorAsociado)
        : base(idModal, $"Seleccionar {selectorAsociado.propiedadParaMostrar}", selectorAsociado.propiedadParaMostrar, selectorAsociado.Ayuda, null)
        {
            selector = selectorAsociado;
            selector.Modal = this;
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

        public void Add(Control c, Posicion pos)
        {

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

            var b1 = new Bloque($"{Id}_b1", "General", new Dimension(1, 2));
            var b2 = new Bloque($"{Id}_b2", "Común", new Dimension(1, 2));

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

    public class DescriptorDeCrud
    {
        public string Elemento { get; private set; }
        public ZonaDeFiltro Filtro { get; private set; }
        public ZonaDeGrid Grid { get; set; }
        public ZonaDeOpciones Menu { get; set; }

        public DescriptorDeCrud(string elemento)
        {
            Elemento = elemento.Replace("Elemento","");
            Filtro = new ZonaDeFiltro(Elemento);
        }

        public string Render()
        {
            return HtmlRender.RenderCrud(this);
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
