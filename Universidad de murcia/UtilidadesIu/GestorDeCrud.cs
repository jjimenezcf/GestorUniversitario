using System;
using System.Collections.Generic;
using UniversidadDeMurcia.UtilidadesIu;

namespace UtilidadesParaIu
{

    public class GestorCrud<T>
    {

        public string NombreDelObjeto => typeof(T).Name;
        public string ClaseDeElemento => NombreDelObjeto.Replace("Elemento", "");
        
        public string Titulo { get; set; }
        public Dictionary<string, SelectorModal> Modales = new Dictionary<string, SelectorModal>();

        public MantenimientoCrud<T> Mantenimiento { get; }
        public CreacionCrud<T> Creador { get; }
        public EdicionCrud<T> Editor { get; }
        public DetalleCrud<T> Detalle { get; }
        public BorradoCrud<T> Supresor { get; }

        private string _Ruta => $"{NombreDelObjeto.Replace("Elemento", "")}s";

        public GestorCrud(string controlador,  Func<List<ColumnaDelGrid>> definirColumnasDelGrid, Func<List<PeticionMvc>> definirOpcionesGenerales)
        {
            Titulo = $"Gestor de {NombreDelObjeto}";
            Creador = new CreacionCrud<T>();

            var opciones = new List<PeticionMvc>() { new PeticionMvc() { Nombre = Creador.Titulo, Ruta = _Ruta, Accion = Creador.Ir } };
            Mantenimiento = new MantenimientoCrud<T>(controlador, "Mantenimiento", ClaseDeElemento, definirColumnasDelGrid, definirOpcionesGenerales)
            {
                Ruta = _Ruta
               ,OpcionesGenerales = opciones
               ,Modales = Modales
               ,PosicionInicial = 0 //todo: --> recuperar de BD
               ,CantidadPorLeer = 5 //todo: --> recuperar de BD
            };

            Editor = new EdicionCrud<T>();
            Detalle = new DetalleCrud<T>();
            Supresor = new BorradoCrud<T>();
        }
    }
}
