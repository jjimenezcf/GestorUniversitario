using Gestor.Elementos.ModeloIu;

namespace Gestor.Elementos.Seguridad
{
    [IUDto]
    public class ClasePermisoDto: Elemento
    {
        [IUPropiedad(
            Etiqueta = "Clase",
            Ayuda = "Clase de permiso",
            Ordenar = true
            )
        ]
        public string Nombre { get; set; }
    }
}
