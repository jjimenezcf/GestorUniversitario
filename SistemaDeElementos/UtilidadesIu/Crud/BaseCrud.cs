using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversidadDeMurcia.UtilidadesIu
{
    public class BaseCrud<T>
    {
        protected string NombreDelObjeto => typeof(T).Name.Replace("Elemento", "");
        protected string Controlador;
        private string _verbo;
        private string _accion;
        private string _formulario;

        public string Ruta { get; set; }
        public int TotalEnBd { get; set; }
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
}
