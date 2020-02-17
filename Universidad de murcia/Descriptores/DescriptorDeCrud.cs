using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using UniversidadDeMurcia.Descriptores;
using Utilidades;
using UtilidadesParaIu;

namespace UniversidadDeMurcia.Descriptores
{
    public enum TipoControl
    {
        Selector,
        Editor,
        Desplegable,
        GridModal,
        TablaBloque,
        Bloque,
        ZonaDeOpciones,
        ZonaDeGrid,
        ZonaDeFiltro,
        VistaCrud,
        DescriptorDeCrud,
        Opcion,
        Label,
        Referencia,
        Lista,
        Fecha
    }

    public enum ModoDescriptor { Mantenimiento, Seleccion}


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

    public abstract class ControlHtml
    {
        public string Id { get; private set; }
        public string IdHtml => Id.ToLower();
        public string Etiqueta { get; private set; }
        public string Propiedad { get; private set; }
        public string Ayuda { get; private set; }
        public bool DeFiltrado { get; set; } = false;
        public Posicion Posicion { get; private set; }

        public TipoControl Tipo { get; protected set; }

        public ControlHtml Padre { get; set; }

        public ControlHtml(ControlHtml padre, string id, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        {
            Padre = padre;
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

        public abstract string RenderControl();

    }

    public class Selector<Tseleccionado> : ControlHtml
    {
        public string propiedadParaFiltrar { get; private set; }
        public string propiedadParaMostrar { get; private set; }
        public GridModal<Tseleccionado> Modal { get; set; }

        public Selector(Bloque padre, string etiqueta, string propiedad, string ayuda, Posicion posicion, string paraFiltrar, string paraMostrar, DescriptorDeCrud<Tseleccionado> descriptor)
        : base(
              padre: padre
              , id: $"{typeof(Tseleccionado).Name.Replace("Elemento", "")}_{TipoControl.Selector}"
              , etiqueta
              , propiedad
              , ayuda
              , posicion
              )
        {
            Tipo = TipoControl.Selector;
            propiedadParaFiltrar = paraFiltrar.ToLower();
            propiedadParaMostrar = paraMostrar.ToLower();
            Modal = new GridModal<Tseleccionado>(padre, this, descriptor);
            padre.AnadirSelector(this);
            DeFiltrado = true;
        }


        public string RenderSelector()
        {
            return $@"<div class=¨input-group mb-3¨>
                       <input id=¨{IdHtml}¨ type = ¨text¨ class=¨form-control¨ {(DeFiltrado?$"filtro=¨S¨":"")} placeholder=¨{Ayuda}¨>
                       <div class=¨input-group-append¨>
                            <button class=¨btn btn-outline-secondary¨ type=¨button¨ data-toggle=¨modal¨ data-target=¨#{Modal.IdHtml}¨ >Seleccionar</button>
                       </div>
                    </div>
                  ";
        }

        public override string RenderControl()
        {
            return RenderSelector();
        }
    }

    public class Editor : ControlHtml
    {
        public Editor(Bloque padre, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(padre: padre
              , id: $"{padre.Id}_{propiedad}"
              , etiqueta
              , propiedad
              , ayuda
              , posicion
              )
        {
            Tipo = TipoControl.Editor;
            DeFiltrado = true;
            padre.AnadirControl(this);
        }

        public override string RenderControl()
        {
            return RenderInput();
        }

        public string RenderInput()
        {
            return $@"<div class=¨input-group mb-3¨>
                         <input id=¨{IdHtml}¨ type = ¨text¨ class=¨form-control¨ {(DeFiltrado ? $"filtro=¨S¨" : "")}  placeholder=¨{Ayuda}¨>
                      </div>
                  ";
        }
    }

    public class Desplegable : ControlHtml
    {
        public ICollection<Valor> valores { get; set; }

        public Desplegable(ControlHtml padre, string id, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(padre: padre
              , id: $"{padre.Id}_ddl"
              , etiqueta
              , propiedad
              , ayuda
              , posicion
              )
        {
            Tipo = TipoControl.Desplegable;
        }

        public override string RenderControl()
        {
            throw new NotImplementedException();
        }
    }

    public class GridModal<Tseleccionado> : ControlHtml
    {
        public Selector<Tseleccionado> Selector { get; set; }
        public DescriptorDeCrud<Tseleccionado> Descriptor { get; set; }

        public string Titulo => Ayuda;

        public GridModal(ControlHtml padre, Selector<Tseleccionado> selectorAsociado, DescriptorDeCrud<Tseleccionado> descriptor)
        : base(
          padre: padre,
          id: $"Modal_{(selectorAsociado.Id.Replace("_"+TipoControl.Selector.ToString(),""))}",
          etiqueta: $"Seleccionar {selectorAsociado.propiedadParaMostrar}",
          propiedad: selectorAsociado.propiedadParaMostrar,
          ayuda: selectorAsociado.Ayuda,
          posicion: null
        )
        {
            Tipo = TipoControl.GridModal;
            Selector = selectorAsociado;
            Selector.Modal = this;
            Descriptor = descriptor;
        }


        private string RenderGridModal()
        {
            var s = Selector;

            const string _alAbrirLaModal = @"
                                         $('#idModal').on('show.bs.modal', function (event) {
                                            AlAbrir('{IdGrid}', '{idSelector}', '{columnaId}', '{columnaMostrar}')
                                          })
                                      ";
            const string _alCerrarLaModal = @"
                                         $('#idModal').on('hidden.bs.modal', function (event) {
                                            AlCerrar('{idModal}', '{idGrid}', '{nameSelCheck}')
                                          })
                                      ";

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
                     <button type = ¨button¨ class=¨btn btn-primary¨ data-dismiss=¨modal¨ onclick=¨AlSeleccionar('{idSelector}', '{idGrid}', '{nameSelCheck}')¨>Seleccionar</button>
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

            var jsAbrirModal = _alAbrirLaModal
                .Replace("idModal", IdHtml)
                .Replace("{IdGrid}", Descriptor.Grid.IdHtml)
                .Replace("{idSelector}", s.IdHtml)
                .Replace("{columnaId}", s.propiedadParaFiltrar)
                .Replace("{columnaMostrar}", s.propiedadParaMostrar);

            var jsCerrarModal = _alCerrarLaModal
                .Replace("idModal", IdHtml)
                .Replace("{IdGrid}", Descriptor.Grid.IdHtml)
                .Replace("{nameSelCheck}", nombreCheckDeSeleccion);


            return _htmlModalSelector
                    .Replace("idModal", IdHtml)
                    .Replace("titulo", Titulo)
                    .Replace("{idSelector}", s.IdHtml)
                    .Replace("{idGrid}", Descriptor.Grid.IdHtml)
                    .Replace("{nameSelCheck}", $"{nombreCheckDeSeleccion}")
                    .Replace("{columnaId}", s.propiedadParaFiltrar)
                    .Replace("{columnaMostrar}", s.propiedadParaMostrar)
                    .Replace("{idContenedor}", $"{s.Modal.IdHtml}_contenedor")
                    .Replace("{gridDeElementos}", Descriptor.RenderControl())
                    .Replace("AlAbrirLaModal", jsAbrirModal)
                    .Replace("AlCerrarLaModal", jsCerrarModal)
                    .Render();
        }

        public override string RenderControl()
        {
            return RenderGridModal();
        }
    }

    public class TablaBloque : ControlHtml
    {
        public Dimension Dimension { get; private set; }
        public ICollection<ControlHtml> Controles { get; set; }

        public TablaBloque(ControlHtml padre, string identificador, Dimension dimension, ICollection<ControlHtml> controles)
        : base(
          padre: padre ,
          id: $"{padre.Id}_Tabla",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.TablaBloque;
            Dimension = dimension;
            Controles = controles;
        }
        
        public override string RenderControl()
        {
            return RenderTabla();
        }

        private string RenderTabla()
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
            foreach (ControlHtml c in Controles)
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

    public class Bloque : ControlHtml
    {
        public TablaBloque Tabla { get; set; }

        public ICollection<ControlHtml> Controles => Tabla.Controles;


        public Bloque(ZonaDeFiltro padre, string titulo, Dimension dimension)
        : base(
          padre: padre,
          id: $"{padre.Id}_{padre.Bloques.Count}_bloque",
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Bloque;
            Tabla = new TablaBloque(this, $"{Id}", dimension, new List<ControlHtml>());
            padre.Bloques.Add(this);
        }


        public void AnadirControl(ControlHtml c)
        {
            Controles.Add(c);
        }

        public void AnadirSelector<T>(Selector<T> s)
        {
            Controles.Add(s);
            Controles.Add(s.Modal);
        }
        public ControlHtml ObtenerControl(string id)
        {

            foreach (ControlHtml c in Controles)
            {
                if (c.Id == id)
                    return c;
            }

            throw new Exception($"El control {id} no está en la zona de filtrado");
        }

        private string RenderBloque()
        {
            string htmlBloque = $@"<div id = ¨{IdHtml}¨>     
                                     tabla 
                                    </div>";
            string htmlTabla = Tabla.RenderControl();

            return htmlBloque.Replace("tabla", htmlTabla);
        }

        public string RenderModalesBloque()
        {
            var htmlModalesEnBloque = "";
            foreach (ControlHtml c in Controles)
            {
                if (c.Tipo == TipoControl.GridModal)
                    htmlModalesEnBloque =
                        $"{htmlModalesEnBloque}{(htmlModalesEnBloque.IsNullOrEmpty() ? "" : Environment.NewLine)}" +
                        $"{c.RenderControl()}";

            }
            return htmlModalesEnBloque;
        }

        public override string RenderControl()
        {
            return RenderBloque();
        }
    }

    public class ZonaDeMenu<Telemento> : ControlHtml
    {
        public ICollection<Opcion<Telemento>> Opciones { get; private set; } = new List<Opcion<Telemento>>();

        public ZonaDeMenu(DescriptorDeCrud<Telemento> padre, VistaCrud vista)
        : base(
          padre: padre,
          id: $"{padre.Id}_Menu",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.ZonaDeOpciones;
            new Opcion<Telemento>(this, vista.Ruta, vista.Accion, vista.Etiqueta);
        }

        private string RenderOpcionesMenu()
        {
            var htmlRef = "<div id=¨{idOpc}¨>{newLine}<a href =¨/{ruta}/{accion}¨>{titulo}</a>{newLine}</div>";
            var htmlMenu = "<div id=¨{idMenu}¨>{hmlOpciones}</div>";
            var htmlOpciones = "";
            foreach (Opcion<Telemento> o in Opciones)
            {
                htmlOpciones = htmlOpciones + htmlRef
                                             .Replace("{idOpc}", o.IdHtml)
                                             .Replace("{ruta}", o.Ruta)
                                             .Replace("{accion}", o.Accion)
                                             .Replace("{titulo}", o.Etiqueta)
                                             .Replace("{newLine}", Environment.NewLine) +
                                             Environment.NewLine;
            }

            return htmlMenu.Replace("{idMenu}", IdHtml).Replace("{hmlOpciones}", $"{Environment.NewLine}{htmlOpciones}");
        }

        public override string RenderControl()
        {
           return RenderOpcionesMenu();
        }
    }

    public class ZonaDeGrid<TElemento> : ControlHtml
    {
        public List<ColumnaDelGrid> Columnas { get; private set; } = new List<ColumnaDelGrid>();

        public List<FilaDelGrid> Filas { get; private set; } = new List<FilaDelGrid>();

        public int CantidadPorLeer { get; set; } = 5;
        public int PosicionInicial { get; set; }

        public int TotalEnBd { get; set; }
        public ZonaDeGrid(DescriptorDeCrud<TElemento> padre)
        : base(
          padre: padre,
          id: $"{padre.Id}_Grid",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.ZonaDeGrid;
        }

        private string RenderGrid()
        {

            var idHtmlZonaFiltro = ((DescriptorDeCrud<TElemento>)Padre).Filtro.IdHtml;
            const string htmlDiv = @"<div id = ¨{idGrid}¨
                                      seleccionables =2
                                      seleccionados =¨¨
                                      zonaDeFiltro = ¨{idFiltro}¨
                                    >     
                                     tabla_Navegador 
                                    </div>";
            var htmlContenedor = htmlDiv.Replace("{idGrid}", $"{IdHtml}")
                                        .Replace("{idFiltro}", idHtmlZonaFiltro)
                                        .Replace("tabla_Navegador", RenderDelGrid());
            return htmlContenedor;
        }

        public string RenderDelGrid()
        {
            var grid = new Grid(IdHtml, Columnas, Filas, PosicionInicial, CantidadPorLeer)
            {
                Controlador = ((DescriptorDeCrud<TElemento>)Padre).Ruta,
                TotalEnBd = TotalEnBd
            };
            var htmlGrid = grid.ToHtml();
            return htmlGrid.Render();
        }

        public override string RenderControl()
        {
            return RenderGrid();
        }
    }

    public class ZonaDeFiltro : ControlHtml
    {
        public ICollection<Bloque> Bloques { get; private set; } = new List<Bloque>();

        public ZonaDeFiltro(ControlHtml padre)
        : base(
          padre: padre,
          id: $"{padre.Id}_Filtro",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.ZonaDeFiltro;
            var b1 = new Bloque(this, "General", new Dimension(1, 2));
            new Bloque(this, "Común", new Dimension(1, 2));

            new Editor(padre: b1, etiqueta: "Nombre", propiedad: "Nombre", ayuda: "buscar por nombre", new Posicion { fila = 0, columna = 0 });
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

        private string RenderFiltro()
        {
            var htmlFiltro = $@"<div id = ¨{IdHtml}¨ style=¨width:100%¨>     
                                     bloques 
                                </div>";

            var htmlBloques = "";
            foreach (Bloque b in Bloques)
                htmlBloques = $"{htmlBloques}{(htmlBloques.IsNullOrEmpty() ? "" : Environment.NewLine)}{b.RenderControl()}";

            return htmlFiltro.Replace("bloques", htmlBloques);
        }


        private string RenderModalesFiltro()
        {
            var htmlModalesEnFiltro = "";
            foreach (Bloque b in Bloques)
                htmlModalesEnFiltro = $"{htmlModalesEnFiltro}{(htmlModalesEnFiltro.IsNullOrEmpty() ? "" : Environment.NewLine)}{b.RenderModalesBloque()}";

            return htmlModalesEnFiltro;
        }

        public override string RenderControl()
        {
            return RenderFiltro() + Environment.NewLine + RenderModalesFiltro();
        }
    }

    public class VistaCrud : ControlHtml
    {
        public string Ruta { get; private set; }
        public string Accion { get; private set; }

        public VistaCrud(ControlHtml padre, string id, string ruta, string vista, string texto)
        : base(
          padre: padre,
          id: $"{padre.Id}_{id}",
          etiqueta: texto,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.VistaCrud;
            Ruta = ruta;
            Accion = vista;
        }

        public override string RenderControl()
        {
            throw new NotImplementedException();
        }
    }

    public class DescriptorDeCrud<TElemento> : ControlHtml
    {
        public VistaCrud VistaMnt { get; private set; }
        public VistaCrud VistaCreacion { get; private set; }

        public ZonaDeMenu<TElemento> Menu { get; set; }
        public ZonaDeFiltro Filtro { get; private set; }
        public ZonaDeGrid<TElemento> Grid { get; set; }
        public string Ruta { get; private set; }
        public ModoDescriptor Modo { get; private set; }



        public DescriptorDeCrud(string ruta, string vista, string titulo, ModoDescriptor modo)
        : base(
          padre: null,
          id: typeof(TElemento).Name.Replace("Elemento", ""),
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.DescriptorDeCrud;
            VistaMnt = new VistaCrud(this, "Mnt", ruta, vista, titulo);
            Filtro = new ZonaDeFiltro(this);
            Grid = new ZonaDeGrid<TElemento>(this);
            Ruta = ruta;
            Modo = modo;
        }


        protected void DefinirVistaDeCreacion(string accion, string textoMenu)
        {
            VistaCreacion = new VistaCrud(this, "Crear", Ruta, accion, textoMenu);
            Menu = new ZonaDeMenu<TElemento>(this, VistaCreacion);
        }

        private string RenderDescriptor()
        {
            var htmlCrud = ModoDescriptor.Mantenimiento == Modo 
                   ?
                   RenderTitulo() + Environment.NewLine +
                   Menu.RenderControl() + Environment.NewLine +
                   Filtro.RenderControl() + Environment.NewLine +
                   Grid.RenderControl() + Environment.NewLine
                   :
                   Filtro.RenderControl() + Environment.NewLine +
                   Grid.RenderControl() + Environment.NewLine;

            return htmlCrud.Render();
        }

        private string RenderTitulo()
        {
            var htmlCabecera = $"<h2>{this.Etiqueta}</h2>";
            return htmlCabecera;
        }

        protected virtual void DefinirColumnasDelGrid()
        {
        }

        public virtual void MapearElementosAlGrid((IEnumerable<TElemento> elementos, int totalEnBd) leidos)
        {
            Grid.TotalEnBd = leidos.totalEnBd;
        }

        public override string RenderControl()
        {
            return RenderDescriptor();
        }
    }

    public class Valor
    {
        public string Nombreestudiante { get; set; }
        public string Fechadeinscripción { get; set; }
    }


    public class Opcion<Telemento> : ControlHtml
    {
        public string Ruta { get; private set; }
        public string Accion { get; private set; }

        public Opcion(ZonaDeMenu<Telemento> padre, string ruta, string accion, string titulo)
        : base(
          padre: padre,
          id: $"{padre.Id}_{padre.Opciones.Count}_opc",
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Opcion;
            Ruta = ruta;
            Accion = accion;
            ((ZonaDeMenu<Telemento>)Padre).Opciones.Add(this);
        }

        public override string RenderControl()
        {
            throw new NotImplementedException();
        }
    }


}



