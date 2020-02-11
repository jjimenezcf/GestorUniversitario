using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using Utilidades;
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

        public Control(string id, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        {
            Id = id.ToLower();
            Etiqueta = etiqueta;
            Propiedad = propiedad;
            Ayuda = ayuda;
            Posicion = posicion;
        }

        public string RenderLabel()
        {
            return $@"<div class=¨input-group mb-3¨>
                         {Etiqueta}
                      </div>
                  ";
        }

        public string RenderControl()
        {
            switch (Tipo)
            {
                case TipoControl.Selector: return((Selector)this).RenderSelector();
                case TipoControl.Editor: return ((Editor)this).RenderInput();
            }
            throw new Exception($"El tipo {this.Tipo} de control no está definido");
        }

    }

    public class Selector : Control
    {
        public string propiedadParaFiltrar { get; private set; }
        public string propiedadParaMostrar { get; private set; }
        public PanelDeSeleccion Modal { get; set; }

        public Selector(string idModal, string etiqueta, string propiedad, string ayuda, Posicion posicion, string paraFiltrar, string paraMostrar)
        :base($"{idModal}.Selector", etiqueta, propiedad,ayuda, posicion)
        {

        Tipo = TipoControl.Selector;
            propiedadParaFiltrar = paraFiltrar.ToLower();
            propiedadParaMostrar = paraMostrar.ToLower();
            Modal = new PanelDeSeleccion(idModal, this);
        }

        public string RenderSelector()
        {
            return $@"<div class=¨input-group mb-3¨>
                       <input id=¨{Id}¨ type = ¨text¨ class=¨form-control¨ placeholder=¨{Ayuda}¨>
                       <div class=¨input-group-append¨>
                            <button class=¨btn btn-outline-secondary¨ type=¨button¨ data-toggle=¨modal¨ data-target=¨#{Modal.Id}¨ >Seleccionar</button>
                       </div>
                    </div>
                  ";
        }
    }

    public class Editor : Control
    {
        public Editor(string id, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(id, etiqueta, propiedad, ayuda, posicion)
        {
            Tipo = TipoControl.Editor;
        }

        public string RenderInput()
        {
            return $@"<div class=¨input-group mb-3¨>
                         <input id=¨{Id}¨ type = ¨text¨ class=¨form-control¨ placeholder=¨{Ayuda}¨>
                      </div>
                  ";
        }
    }

    public class Desplegable: Control
    {
        public ICollection<Valor> valores { get; set; }
        public Desplegable(string idSelector, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(idSelector, etiqueta, propiedad, ayuda, posicion)
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

        public string RenderTabla()
        {

            var htmlTabla = $@"<table id=¨{Id}¨ width=¨100%¨
                                  filas
                               </table>";
            var htmlFilas = "";
            for (var i = 0; i < Dimension.Filas; i++)
                htmlFilas = $"{htmlFilas}{(htmlFilas.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderFila(i)}";

            return htmlTabla.Replace("filas", htmlFilas);
        }

        private string RenderFila(int i)
        {
            var idFila = $"{Id}_{i}";
            var htmlFila = $@"<tr id=¨{idFila}¨>
                                 columnas
                              </tr>";
            var htmlColumnas = "";
            for (var j = 0; j < Dimension.Columnas; j++)
                htmlColumnas = $"{htmlColumnas}{(htmlColumnas.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderColumnasControl(idFila, i, j)}";


            return htmlFila.Replace("columnas", htmlColumnas);
        }

        private string RenderColumnasControl(string idFila, int i, int j)
        {
            var idColumna = $"{idFila}_{j}";
            var htmlColumnaEtiqueta = $@"<td id=¨{idColumna}_e¨ style=¨width:15%¨>
                                            etiqueta
                                         </td>";
            var htmlColumnaControl = $@"<td id=¨{idColumna}_c¨ style=¨width:35%¨>
                                           control
                                        </td>";
            var htmlControl = "";
            var htmlEtiqueta = "";
            foreach (Control c in Controles)
            {
                if (c.Posicion.fila == i && c.Posicion.columna == j)
                    htmlEtiqueta = $"{c.RenderLabel()}";

                if (c.Posicion.fila == i && c.Posicion.columna == j)
                    htmlControl = $"{c.RenderControl()}";
            }


            return htmlColumnaEtiqueta.Replace("etiqueta", htmlEtiqueta) +
                   Environment.NewLine +
                   htmlColumnaControl.Replace("control", htmlControl);
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
        public void AnadirControl(Control c)
        {
            Controles.Add(c);
        }
        public Control ObtenerControl(string id)
        {

            foreach(Control c in Controles)
            {
                if (c.Id == id)
                    return c;
            }
            
            throw new Exception($"El control {id} no está en la zona de filtrado");
        }

        public string RenderBloque()
        {
            string htmlBloque = $@"<div id = ¨{Id}¨>     
                                     tabla 
                                    </div>";
            string htmlTabla = Tabla.RenderTabla();

            return htmlBloque.Replace("tabla", htmlTabla);
        }
    }

    public class ZonaDeOpciones
    {
        public string Id { get; private set; }
        public ICollection<Opcion> Opciones { get; private set; } = new List<Opcion>();
        public ZonaDeOpciones(string identificador, string ruta)
        {
            Id = $"opc_{identificador}";
            var crear = new Opcion($"{Id}.Crear", ruta, "IraCrearEstudiante", $"Nuevo estudiante");
            Opciones.Add(crear);
        }

        public string RenderOpcionesMenu()
        {
            var htmlRef = "<div id=¨id¨> <a href =¨/{ruta}/{accion}¨>{titulo}</a> </div>";
            var htmlOpciones = "";
            foreach (Opcion o in Opciones)
            {
                htmlOpciones = htmlOpciones + htmlRef.Replace("{id}", o.Id).Replace("{ruta}", o.Ruta).Replace("{accion}", o.Accion).Replace("{titulo}", o.Titulo);
            }

            return htmlOpciones;
        }

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
            Id = $"flt.{identificador}";

            var editor = new Editor(id: $"{Id}.b1.filtro", etiqueta: "Nombre", propiedad: "Nombre", ayuda: "buscar por nombre", new Posicion { fila= 0, columna = 0 });

            var b1 = new Bloque($"{Id}.b1", "General", new Dimension(1, 2));
            var b2 = new Bloque($"{Id}.b2", "Común", new Dimension(1, 2));

            b1.AnadirControl(editor);

            Bloques.Add(b1);
            Bloques.Add(b2);
        }

        public void AnadirBloque(Bloque bloque)
        {
            Bloques.Add(bloque);
        }
        
        public Bloque ObtenerBloque(string identificador)
        {
            foreach(Bloque b in Bloques)
            {
                if (b.Id == identificador)
                    return b;
            }

            throw new Exception($"El bloque {identificador} no está en la zona de filtrado");
        }

        public string RenderFiltro()
        {
            var htmlFiltro = $@"<div id = ¨{Id}¨ style=¨width:100%¨>     
                                     bloques 
                                </div>";

            var htmlBloques = "";
            foreach (Bloque b in Bloques)
                htmlBloques = $"{htmlBloques}{(htmlBloques.IsNullOrEmpty() ? "" : Environment.NewLine)}{b.RenderBloque()}";

            return htmlFiltro.Replace("bloques", htmlBloques);
        }

    }

    public class DescriptorDeCrud
    {
        public string Elemento { get; private set; }

        public string Titulo { get; private set; }

        public ZonaDeOpciones Menu { get; set; }
        public ZonaDeFiltro Filtro { get; private set; }
        public ZonaDeGrid Grid { get; set; }

        public DescriptorDeCrud(string elemento, string ruta, string titulo)
        {
            Elemento = elemento.Replace("Elemento","");
            Titulo = titulo;
            Menu = new ZonaDeOpciones(Elemento, ruta);
            Filtro = new ZonaDeFiltro(Elemento);

        }

        public string Render()
        {
            var htmlCrud =
                   RenderTitulo() + Environment.NewLine +
                   Menu.RenderOpcionesMenu() + Environment.NewLine +
                   Filtro.RenderFiltro() + Environment.NewLine +
                   HtmlRender.RenderModalesFiltro(Filtro);
                   //+ Environment.NewLine +
                   //RenderGrid(crud.Grid) + Environment.NewLine +
                   //RenderPie();

            return htmlCrud.Render();
        }

        private string RenderTitulo()
        {
            var htmlCabecera = $"<h2>{Titulo}</h2>";
            return htmlCabecera;
        }

    }
          
    public class Valor
    {
        public string Nombreestudiante { get; set; }
        public string Fechadeinscripción { get; set; }
    }


    public class Opcion
    {
        public string Id {get; private set; }
        public string Ruta { get; private set; }
        public string Accion { get; private set; }
        public string Titulo { get; private set;  }

        public Opcion(string id, string ruta, string accion, string titulo)
        {
            this.Id = id;
            this.Ruta = ruta;
            this.Accion = accion;
            this.Titulo = titulo;
        }
    }




}
