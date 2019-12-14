using Extensiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversidadDeMurcia.Utilidades
{
    public class BaseCrud<T>
    {
        protected string NombreDelObjeto => typeof(T).Name.Replace("Elemento","");
        private string _verbo;
        private string _accion;
        private string _formulario;

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
        public MantenimientoCrud() :
        base("Mantenimiento")
        {
            AsignarTitulo($"Mantenimiento de {NombreDelObjeto}s");
        }

        public string Render()
        {
            var htmlMantenimiento =
                RenderCabecera() +
                RenderFiltro() +
                RenderGrid() +
                RenderPie();

            return htmlMantenimiento;
        }
        
        private string RenderCabecera()
        {
            var htmlCabecera = RenderOpcionesComunes();
            return htmlCabecera;
        }

        private string RenderOpcionesComunes()
        {
            var htmlOpcionesComunes = "";
            return htmlOpcionesComunes;
        }

        private string RenderFiltro()
        {
            var htmlFiltro = "";
            return htmlFiltro;
        }

        private string RenderGrid()
        {
           var htmlGrid = 
                RenderCabeceraGrid() +
                RenderDetalleGrid()+
                RenderNavegadorGrid() +
                RenderOpcionesGrid();

            return htmlGrid;
        }

        private string RenderCabeceraGrid()
        {
            var htmlCabeceraGrid = "";
            return htmlCabeceraGrid;
        }

        private string RenderDetalleGrid()
        {
            var htmlDetalleGrid = "";
            return htmlDetalleGrid;
        }

        private string RenderNavegadorGrid()
        {
            var htmlNavegadorGrid = "";
            return htmlNavegadorGrid;
        }

        private string RenderOpcionesGrid()
        {
            var htmlOpcionesGrid = "";
            return htmlOpcionesGrid;
        }

        private string RenderPie()
        {
            var htmlPie = "";
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
        public string TituloDetalle;
        public DetalleCrud(string titulo) :
        base("Detalle")
        {
            TituloDetalle = titulo;
            AsignarTitulo($"Detalle de {NombreDelObjeto}");
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
        public string NombreDelObjeto => typeof(T).Name;
        public string Titulo { get; set; }
        public Dictionary<string, SelectorModal> Modales = new Dictionary<string, SelectorModal>();

        public MantenimientoCrud<T> Mantenimiento { get; }
        public CreacionCrud<T> Creador { get; }
        public EdicionCrud<T> Editor { get; }
        public DetalleCrud<T> Detalle { get; }
        public BorradoCrud<T> Supresor { get; }

        public GestorCrud(string titulo, string tituloDetalle)
        {
            Titulo = titulo;
            Mantenimiento = new MantenimientoCrud<T>();
            Creador = new CreacionCrud<T>();
            Editor = new EdicionCrud<T>();
            Detalle = new DetalleCrud<T>(tituloDetalle);
            Supresor = new BorradoCrud<T>();
        }
    }
}
