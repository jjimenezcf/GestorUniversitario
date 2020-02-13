using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using Utilidades;
using UtilidadesParaIu;

namespace UniversidadDeMurcia.Descriptores
{
    public enum TipoControl { Selector, Editor, Label, Referencia, Desplegable, Lista, Fecha, GridModal }

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

    public class Control
    {
        public string Id { get; private set; }
        public string IdHtml => Id.ToLower();
        public string Etiqueta { get; private set; }
        public string Propiedad { get; private set; }
        public string Ayuda { get; private set; }
        public Posicion Posicion { get; private set; }

        public TipoControl Tipo { get; protected set; }

        public Control(string id, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        {
            Id = id;
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

        public virtual string RenderControl()
        {
            if (Tipo != TipoControl.Selector && Tipo!= TipoControl.Editor && Tipo!= TipoControl.GridModal)
                throw new Exception($"El tipo {this.Tipo} de control no está definido");
            return "htmlControl";
        }

    }

    public class Selector<Tseleccionado> : Control
    {
        public string propiedadParaFiltrar { get; private set; }
        public string propiedadParaMostrar { get; private set; }
        public GridModal<Tseleccionado> GridModal { get; set; }

        public Selector(Bloque padre, string etiqueta, string propiedad, string ayuda, Posicion posicion, string paraFiltrar, string paraMostrar)
        : base(     
                id: $"{typeof(Tseleccionado).Name.Replace("Elemento","")}_{TipoControl.Selector}"
              , etiqueta
              , propiedad
              , ayuda
              , posicion
              )
        {
            Tipo = TipoControl.Selector;
            propiedadParaFiltrar = paraFiltrar.ToLower();
            propiedadParaMostrar = paraMostrar.ToLower();
            GridModal = new GridModal<Tseleccionado>(this);
            padre.AnadirSelector(this);
        }

        public override string RenderControl()
        {
            return base.RenderControl().Replace("htmlControl",RenderSelector());
        }

        public string RenderSelector()
        {
            return $@"<div class=¨input-group mb-3¨>
                       <input id=¨{IdHtml}¨ type = ¨text¨ class=¨form-control¨ placeholder=¨{Ayuda}¨>
                       <div class=¨input-group-append¨>
                            <button class=¨btn btn-outline-secondary¨ type=¨button¨ data-toggle=¨modal¨ data-target=¨#{GridModal.IdHtml}¨ >Seleccionar</button>
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

        public override string RenderControl()
        {
            return base.RenderControl().Replace("htmlControl", RenderInput());
        }

        public string RenderInput()
        {
            return $@"<div class=¨input-group mb-3¨>
                         <input id=¨{IdHtml}¨ type = ¨text¨ class=¨form-control¨ placeholder=¨{Ayuda}¨>
                      </div>
                  ";
        }
    }

    public class Desplegable : Control
    {
        public ICollection<Valor> valores { get; set; }
        public Desplegable(string idSelector, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(idSelector, etiqueta, propiedad, ayuda, posicion)
        {
            Tipo = TipoControl.Desplegable;
        }
    }

    public class GridModal<Tseleccionado> : Control
    {
        public Selector<Tseleccionado> Selector { get; set; }
        public string gestorDeElementos { get; set; }
        public string claseDeElemento { get; set; }
        public List<ColumnaDelGrid> Columnas { get; set; }
        public string Registros { get; set; }

        public GridModal(Selector<Tseleccionado> selectorAsociado)
        : base(
          id: selectorAsociado.Id.Replace(TipoControl.Selector.ToString(), TipoControl.GridModal.ToString()),
          etiqueta: $"Seleccionar {selectorAsociado.propiedadParaMostrar}", 
          propiedad: selectorAsociado.propiedadParaMostrar, 
          ayuda: selectorAsociado.Ayuda, 
          posicion: null)
        {
            Tipo = TipoControl.GridModal;
            Selector = selectorAsociado;
            Selector.GridModal = this;
        }

        public override string RenderControl()
        {
            return base.RenderControl().Replace("htmlControl", RenderGridModal());
        }

        private string RenderGridModal()
        {
            var s = Selector;

            const string _htmlModalSelector =
            @"
             <div class=¨modal fade¨ id=¨idModal¨ tabindex=¨-1¨ role=¨dialog¨ aria-labelledby=¨exampleModalLabel¨ aria-hidden=¨true¨>
               <div class=¨modal-dialog¨ role=¨document¨>
                 <div class=¨modal-content¨>
                   <div class=¨modal-header¨>
                     <h5 class=¨modal-title¨ id=¨exampleModalLabel¨>titulo</h5>
                   </div>
                   <div id=¨{idContenedor}¨ class=¨modal-body¨>
                     {gridDeElementos}
                   </div>
                   <div class=¨modal-footer¨>
                     <button type = ¨button¨ class=¨btn btn-secondary¨ data-dismiss=¨modal¨>Cerrar</button>
                     <button type = ¨button¨ class=¨btn btn-primary¨ data-dismiss=¨modal¨ onclick=¨AlSeleccionar('{idSelector}', '{idGrid}', '{referenciaChecks}')¨>Seleccionar</button>
                   </div>
                 </div>
               </div>
             </div>
             <script>
               AlAbrirLaModal
               AlCerrarLaModal
             </script>
             ";


            var nombreCheckDeSeleccion = $"chksel.{s.IdHtml}";

            return _htmlModalSelector
                    .Replace("idModal", s.GridModal.IdHtml)
                    .Replace("titulo", s.GridModal.Ayuda)
                    .Replace("{idSelector}", s.IdHtml)
                    //.Replace("{idGrid}", IdGrid)
                    .Replace("{referenciaChecks}", $"{nombreCheckDeSeleccion}")
                    .Replace("{columnaId}", s.propiedadParaFiltrar)
                    .Replace("{columnaMostrar}", s.propiedadParaMostrar)
                    .Replace("{idContenedor}", $"contenedor.{s.GridModal.IdHtml}")
                    .Replace("{gridDeElementos}", "")
                    .Replace("AlAbrirLaModal", "")
                    .Replace("AlCerrarLaModal", "")
                    .Render();
        }
    }

    public class TablaBloque
    {
        public string Id { get; private set; }
        public string IdHtml => Id.ToLower();
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

            var htmlTabla = $@"<table id=¨{IdHtml}¨ width=¨100%¨>
                                  filas
                               </table>";
            var htmlFilas = "";
            for (var i = 0; i < Dimension.Filas; i++)
                htmlFilas = $"{htmlFilas}{(htmlFilas.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderFila(i)}";

            return htmlTabla.Replace("filas", htmlFilas);
        }

        private string RenderFila(int i)
        {
            var idFila = $"{IdHtml}_{i}";
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
                if (c.Posicion == null)
                    continue;

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
        public string IdHtml => Id.ToLower();

        public string Titulo { get; private set; }

        public TablaBloque Tabla { get; set; }

        public ICollection<Control> Controles => Tabla.Controles;

        public Bloque(string identificador, string titulo, Dimension dimension)
        {
            Id = identificador;
            Titulo = titulo;
            Tabla = new TablaBloque($"{IdHtml}_Tbl", dimension, new List<Control>());
        }
        public void AnadirControl(Control c)
        {
            Controles.Add(c);
        }

        public void AnadirSelector<T>(Selector<T> s)
        {
            Controles.Add(s);
            Controles.Add(s.GridModal);
        }
        public Control ObtenerControl(string id)
        {

            foreach (Control c in Controles)
            {
                if (c.Id == id)
                    return c;
            }

            throw new Exception($"El control {id} no está en la zona de filtrado");
        }

        public string RenderBloque()
        {
            string htmlBloque = $@"<div id = ¨{IdHtml}¨>     
                                     tabla 
                                    </div>";
            string htmlTabla = Tabla.RenderTabla();

            return htmlBloque.Replace("tabla", htmlTabla);
        }

        public string RenderModalesBloque()
        {
            var htmlModalesEnBloque = "";
            foreach (Control c in Controles)
            {
                if (c.Tipo == TipoControl.GridModal)
                    htmlModalesEnBloque = 
                        $"{htmlModalesEnBloque}{(htmlModalesEnBloque.IsNullOrEmpty() ? "" : Environment.NewLine)}" +
                        $"{c.RenderControl()}";

            }
            return htmlModalesEnBloque;
        }
    }

    public class ZonaDeOpciones
    {
        private string Id { get; set; }
        public string IdHtml => Id.ToLower();


        public ICollection<Opcion> Opciones { get; private set; } = new List<Opcion>();
        public ZonaDeOpciones(string identificador, VistaCrud vista)
        {
            Id = $"opc_{identificador}";
            var crear = new Opcion($"{IdHtml}_Crear", vista.Ruta, vista.Accion, vista.TextoMenu);
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

    public class ZonaDeGrid<TElemento>
    {
        public List<ColumnaDelGrid> Columnas { get; private set; } = new List<ColumnaDelGrid>();

        public List<FilaDelGrid> Filas { get; private set; } = new List<FilaDelGrid>();

        public int CantidadPorLeer { get; set; } = 5;
        public int PosicionInicial { get; set; }

        public DescriptorDeCrud<TElemento> Crud { get; private set; }
        private string Id { get; set; }

        public string IdHtml => Id.ToLower();

        public int TotalEnBd { get; set; }

        public ZonaDeGrid(DescriptorDeCrud<TElemento> crud)
        {
            Crud = crud;
            Id = $"grid.{crud.Id}";
        }

        public string RenderGrid()
        {
            const string htmlDiv = @"<div id = ¨idContenedor¨>     
                                     contenido 
                                    </div>";
            var htmlContenedor = htmlDiv.Replace("idContenedor", $"contenedor.{IdHtml}").Replace("contenido", RenderFilasDelGrid());
            return htmlContenedor;
        }

        public string RenderFilasDelGrid()
        {
            var grid = new Grid(IdHtml, Columnas, Filas, PosicionInicial, CantidadPorLeer) { Controlador = Crud.Ruta, TotalEnBd = TotalEnBd };
            var htmlGrid = grid.ToHtml();
            return htmlGrid.Render();
        }

    }

    public class ZonaDeFiltro
    {
        public ICollection<Bloque> Bloques { get; private set; } = new List<Bloque>();

        public string Id { get; private set; }
        public string IdHtml => Id.ToLower();

        public ZonaDeFiltro(string identificador)
        {
            Id = $"flt_{identificador}";

            var editor = new Editor(id: $"{Id}_b1_filtro", etiqueta: "Nombre", propiedad: "Nombre", ayuda: "buscar por nombre", new Posicion { fila = 0, columna = 0 });

            var b1 = new Bloque($"{Id}_b1", "General", new Dimension(1, 2));
            var b2 = new Bloque($"{Id}_b2", "Común", new Dimension(1, 2));

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
            foreach (Bloque b in Bloques)
            {
                if (b.Id == identificador)
                    return b;
            }

            throw new Exception($"El bloque {identificador} no está en la zona de filtrado");
        }

        public string RenderFiltro()
        {
            var htmlFiltro = $@"<div id = ¨{IdHtml}¨ style=¨width:100%¨>     
                                     bloques 
                                </div>";

            var htmlBloques = "";
            foreach (Bloque b in Bloques)
                htmlBloques = $"{htmlBloques}{(htmlBloques.IsNullOrEmpty() ? "" : Environment.NewLine)}{b.RenderBloque()}";

            return htmlFiltro.Replace("bloques", htmlBloques);
        }


        public string RenderModalesFiltro()
        {
            var htmlModalesEnFiltro = "";
            foreach (Bloque b in Bloques)
                htmlModalesEnFiltro = $"{htmlModalesEnFiltro}{(htmlModalesEnFiltro.IsNullOrEmpty() ? "" : Environment.NewLine)}{b.RenderModalesBloque()}";

            return htmlModalesEnFiltro;
        }

    }

    public class VistaCrud
    {
        public string Ruta { get; private set; }
        public string Accion { get; private set; }
        public string TextoMenu { get; private set; }

        public VistaCrud(string ruta, string vista, string texto)
        {
            Ruta = ruta;
            Accion = vista;
            TextoMenu = texto;
        }
    }

    public class DescriptorDeCrud<TElemento>
    {
        public VistaCrud VistaMnt { get; private set; }
        public VistaCrud VistaCreacion { get; private set; }

        public string Id { get; private set; }
        public string IdHtml => Id.ToLower();

        public string Titulo { get; private set; }

        public ZonaDeOpciones Menu { get; set; }
        public ZonaDeFiltro Filtro { get; private set; }
        public ZonaDeGrid<TElemento> Grid { get; set; }
        public string Ruta { get; private set; }

        public DescriptorDeCrud(string ruta, string vista, string titulo)
        {
            VistaMnt = new VistaCrud(ruta, vista, titulo);
            Id = typeof(TElemento).Name.Replace("Elemento", "");
            Titulo = titulo;
            Filtro = new ZonaDeFiltro(Id);
            Grid = new ZonaDeGrid<TElemento>(this);
            Ruta = ruta;
        }


        protected void DefinirVistaDeCreacion(string accion, string textoMenu)
        {
            VistaCreacion = new VistaCrud(Ruta, accion, textoMenu);
            Menu = new ZonaDeOpciones(Id, VistaCreacion);
        }

        public string Render()
        {
            var htmlCrud =
                   RenderTitulo() + Environment.NewLine +
                   Menu.RenderOpcionesMenu() + Environment.NewLine +
                   Filtro.RenderFiltro() + Environment.NewLine +
                   Filtro.RenderModalesFiltro() + Environment.NewLine +
                   Grid.RenderGrid() + Environment.NewLine;
            //RenderPie();

            return htmlCrud.Render();
        }

        private string RenderTitulo()
        {
            var htmlCabecera = $"<h2>{Titulo}</h2>";
            return htmlCabecera;
        }

        protected virtual void DefinirColumnasDelGrid()
        {
        }

        public virtual void MapearElementosAlGrid((IEnumerable<TElemento> elementos, int totalEnBd) leidos)
        {
            Grid.TotalEnBd = leidos.totalEnBd;
        }
    }

    public class Valor
    {
        public string Nombreestudiante { get; set; }
        public string Fechadeinscripción { get; set; }
    }


    public class Opcion
    {
        public string Id { get; private set; }
        public string Ruta { get; private set; }
        public string Accion { get; private set; }
        public string Titulo { get; private set; }

        public Opcion(string id, string ruta, string accion, string titulo)
        {
            this.Id = id;
            this.Ruta = ruta;
            this.Accion = accion;
            this.Titulo = titulo;
        }
    }


}



