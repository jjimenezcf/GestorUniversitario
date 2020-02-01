using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilidadesParaIu;

namespace UniversidadDeMurcia.UtilidadesIu
{
    public class MantenimientoCrud<T>
    {
        protected string NombreDelObjeto => typeof(T).Name.Replace("Elemento", "");
        private string _modo;
        private string _accion;
        private string _formulario;
        protected string Controlador { get; private set; }

        public string Ruta { get; set; }
        public int TotalEnBd { get; set; }
        public string Vista => $"{_modo}{NombreDelObjeto}";
        public string Titulo { get; set; }
        public string Ir => $"Ira{_modo}{NombreDelObjeto}";

        public string Accion => _accion ?? $"{_modo}{NombreDelObjeto}";

        public string Formulario => _formulario ?? _modo;

        public string ClaseDeElemento { get; private set; }
        
        private List<PeticionMvc> _peticionesComunes;
        public List<ColumnaDelGrid> ColumnasDelGrid { get; }
        public List<FilaDelGrid> FilasDelGrid { private get; set; }
        public List<PeticionMvc> PeticionesComunes
        {
            get => _peticionesComunes;
            set
            {
                if (_peticionesComunes == null)
                    _peticionesComunes = value;
                else
                    _peticionesComunes.AddRange(value);
            }
        }

        public Dictionary<string, SelectorModal> Modales { get; set; }

        private string IdGrid => $"GridMnt_{ClaseDeElemento}".ToLower();
        public int PosicionInicial { get; set; }
        public int CantidadPorLeer { get; set; }


        public MantenimientoCrud(string controlador, string modo, string claseDeElemento, Func<List<ColumnaDelGrid>> definirColumnasDelGrid, Func<List<PeticionMvc>> definirOpcionesGenerales)
        {
            Controlador = controlador;
            _modo = modo;
            ClaseDeElemento = claseDeElemento;
            AsignarTitulo($"{modo} de {ClaseDeElemento}s");
            ColumnasDelGrid = definirColumnasDelGrid == null ? renderDeColumnasVacio() : definirColumnasDelGrid();
            PeticionesComunes = definirOpcionesGenerales();
        }

        public string Render()
        {
            //IEnumerable<ColumnaGrid> columnasGrid = DefinirColumnasDelGrid();

            var htmlMantenimiento =
                RenderCabecera() +
                RenderFiltro() +
                RenderGrid(ColumnasDelGrid, FilasDelGrid) +
                RenderPie();

            return htmlMantenimiento.Render();
        }

        public string RenderGridSiguiente(string idGrid)
        {
            var grid = new Grid(idGrid, ColumnasDelGrid, FilasDelGrid, PosicionInicial, CantidadPorLeer) { Controlador = Ruta, TotalEnBd = TotalEnBd };
            var htmlGrid = grid.ToHtml();
            return htmlGrid.Render();
        }

        private List<ColumnaDelGrid> renderDeColumnasVacio()
        {
            return new List<ColumnaDelGrid>();
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

            foreach (var opcion in PeticionesComunes)
            {
                var html = htmlOpcion.Replace("ruta", opcion.Controlador).Replace("accion", opcion.Accion).Replace("nombreAccion", opcion.Nombre);
                htmlOpcionesGenerales = htmlOpcionesGenerales + html;
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
            const string htmlDiv = @"<div id = ¨idContenedor¨>     
                                     contenido 
                                    </div>";

            var grid = new Grid(IdGrid, columnas, filas, PosicionInicial, CantidadPorLeer) { Controlador = Ruta, TotalEnBd = TotalEnBd };
            var htmlGrid = grid.ToHtml();
            var htmlContenedor = htmlDiv.Replace("idContenedor", $"contenedor.{grid.Id}").Replace("contenido", htmlGrid);
            return htmlContenedor;
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


}
