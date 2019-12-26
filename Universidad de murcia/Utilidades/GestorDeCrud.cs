using Extensiones.Html;
using System;
using System.Collections.Generic;

namespace UniversidadDeMurcia.Utilidades
{

    public class Opcion
    {
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public string Accion { get; set; }
    }

    public class BaseCrud<T>
    {
        protected string NombreDelObjeto => typeof(T).Name.Replace("Elemento","");
        private string _verbo;
        private string _accion;
        private string _formulario;

        public string Ruta { get; set; }
        public string Vista => $"{_verbo}{NombreDelObjeto}";
        public string Titulo { get; set; }
        public string Ir => $"Ira{_verbo}{NombreDelObjeto}";

        public string Accion => _accion ?? $"{_verbo}{NombreDelObjeto}";

        public string Formulario => _formulario ?? _verbo;

        public BaseCrud(string verbo)
        {
            _verbo = verbo;
        }

        public void AsignarTitulo(string titulo)
        {
            Titulo = titulo;
        }

        public void AsignarAccion(string accion)
        {
            _accion = accion;
        }

        public void AsignarFormulario(string formulario)
        {
            _formulario = formulario;
        }

    }

    public class MantenimientoCrud<T> : BaseCrud<T>
    {

        private List<Opcion> _opcionesGenerales;
        public List<ColumnaDelGrid> ColumnasDelGrid { get;}
        public List<FilaDelGrid> FilasDelGrid {private get; set; }
        public List<Opcion> OpcionesGenerales
        {
            get => _opcionesGenerales;
            set
            {
                if (_opcionesGenerales == null)
                    _opcionesGenerales = value;
                else
                    _opcionesGenerales.AddRange(value);
            }
          }

        public Dictionary<string, SelectorModal> Modales { get; set; }

       private string IdGrid => $"T_{Vista}";

        public MantenimientoCrud(Func<List<ColumnaDelGrid>> definirColumnasDelGrid, Func<List<Opcion>> definirOpcionesGenerales)
        :base("Mantenimiento")
        {
            AsignarTitulo($"Mantenimiento de {NombreDelObjeto}s");
            ColumnasDelGrid = definirColumnasDelGrid == null ? renderDeColumnasVacio() : definirColumnasDelGrid();
            OpcionesGenerales = definirOpcionesGenerales();
        }

        private List<ColumnaDelGrid> renderDeColumnasVacio()
        {
             return new List<ColumnaDelGrid>();
        }

        public string Render()
        {
            //IEnumerable<ColumnaGrid> columnasGrid = DefinirColumnasDelGrid();

            var htmlMantenimiento =
                RenderCabecera() +
                RenderFiltro() +
                RenderGrid(ColumnasDelGrid, FilasDelGrid) +
                RenderPie();

            return htmlMantenimiento;
        }
        
        private string RenderCabecera()
        {
            //
            var htmlCabecera = $"<h2>{Titulo}</h2>" +
                RenderOpcionesComunes();
            return htmlCabecera;
        }

        private string RenderOpcionesComunes()
        {
            var htmlOpcionesGenerales = "";
            var htmlOpcion = $@"
                                       <p>
                                           <a href=¨/ruta/accion¨>nombreAccion</a>
                                       </p>
                                      ";

            foreach (var opcion in OpcionesGenerales)
            {
                var html = htmlOpcion.Replace("ruta", opcion.Ruta).Replace("accion", opcion.Accion).Replace("nombreAccion", opcion.Nombre);
                htmlOpcionesGenerales = htmlOpcionesGenerales + html.Render();
            }

            return htmlOpcionesGenerales;
        }

        private string RenderFiltro()
        {
            var htmlFiltro = "";
            foreach (var modal in Modales.Keys)
            {
                htmlFiltro = htmlFiltro + Modales[modal].RenderSelector();
            }

            return htmlFiltro;
        }

        private string RenderGrid(List<ColumnaDelGrid> columnas, List<FilaDelGrid> filas)
        {
            var grid = new Grid(IdGrid, columnas, filas);
            var htmlGrid = grid.ToHtml();
            return htmlGrid;
        }

        private string RenderPie()
        {
            var htmlPie = "";
            foreach (var modal in Modales.Keys)
            {
                htmlPie = htmlPie + Modales[modal].RenderModal();
            }

            return htmlPie;
        }

    }


    public class CreacionCrud<T> : BaseCrud<T>
    {
        public CreacionCrud() :
        base("Crear")
        {
            AsignarTitulo($"Creación de {NombreDelObjeto}");
        }
    }


    public class EdicionCrud<T> : BaseCrud<T>
    {
        public EdicionCrud() :
        base("Editar")
        {
            AsignarTitulo($"Edición de {NombreDelObjeto}");
            AsignarAccion($"Modificar{NombreDelObjeto}");
            AsignarFormulario("Modificar");
        }
    }


    public class DetalleCrud<T> : BaseCrud<T>
    {
        public string TituloDetalle { get; private set; }
        public DetalleCrud() :
        base("Detalle")
        {
            AsignarTitulo($"Detalle de {NombreDelObjeto}");
        }

        public void AsignarTituloDetalle(string titulo)
        {
            TituloDetalle = titulo;
        }
    }

    public class BorradoCrud<T> : BaseCrud<T>
    {
        public BorradoCrud() :
        base("Borrar")
        {
            AsignarTitulo($"Borrado de {NombreDelObjeto}");
        }
    }

    public class GestorCrud<T>
    {

        private string _ruta;

        public string NombreDelObjeto => typeof(T).Name;
        public string Ruta
        {
            get { return _ruta ?? $"{NombreDelObjeto.Replace("Elemento","")}s"; }
            set { _ruta = value; }
        }
        public string Titulo { get; set; }
        public Dictionary<string, SelectorModal> Modales = new Dictionary<string, SelectorModal>();

        public MantenimientoCrud<T> Mantenimiento { get; }
        public CreacionCrud<T> Creador { get; }
        public EdicionCrud<T> Editor { get; }
        public DetalleCrud<T> Detalle { get; }
        public BorradoCrud<T> Supresor { get; }

        public GestorCrud(Func<List<ColumnaDelGrid>> definirColumnasDelGrid, Func<List<Opcion>> definirOpcionesGenerales)
        {
            Titulo = $"Gestor de {NombreDelObjeto}";
            Creador = new CreacionCrud<T>();

            var opciones = new List<Opcion>() { new Opcion() { Nombre = Creador.Titulo, Ruta = Ruta, Accion = Creador.Ir } };
            Mantenimiento = new MantenimientoCrud<T>(definirColumnasDelGrid, definirOpcionesGenerales) { Ruta = Ruta, OpcionesGenerales = opciones, Modales = Modales };

            Editor = new EdicionCrud<T>();
            Detalle = new DetalleCrud<T>();
            Supresor = new BorradoCrud<T>();
        }
    }
}
