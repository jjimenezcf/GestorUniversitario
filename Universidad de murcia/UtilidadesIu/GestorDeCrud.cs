using System;
using System.Collections.Generic;
using UniversidadDeMurcia.Descriptores;
using UniversidadDeMurcia.UtilidadesIu;

namespace UtilidadesParaIu
{

    public class GestorCrud<T>
    {

        public string NombreDelObjeto => typeof(T).Name;
        public string ClaseDeElemento => NombreDelObjeto.Replace("Elemento", "");

        public string Controlador { get; private set; }
        
        public string Titulo { get; set; }
        public CreacionCrud<T> Creador { get; }
        public EdicionCrud<T> Editor { get; }
        public DetalleCrud<T> Detalle { get; }
        public BorradoCrud<T> Supresor { get; }
        public DescriptorDeCrud<T> Descriptor { get; }

        public string IrAlCrud { get; set; }

        private string _Ruta => $"{NombreDelObjeto.Replace("Elemento", "")}s";

        public GestorCrud(string controlador,  Func<List<ColumnaDelGrid>> definirColumnasDelGrid, Func<List<PeticionMvc>> definirOpcionesGenerales, DescriptorDeCrud<T> descriptor = null)
        {
            Controlador = controlador.Replace("Controller", "");
            Titulo = $"Gestor de {NombreDelObjeto}";
            Descriptor = descriptor;            

            Creador = new CreacionCrud<T>();            
            Editor = new EdicionCrud<T>();
            Detalle = new DetalleCrud<T>();
            Supresor = new BorradoCrud<T>();
        }

    }
}
