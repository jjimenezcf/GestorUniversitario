using Gestor.Elementos.ModeloIu;
using Gestor.Elementos.Seguridad;

namespace GestorDeSeguridad.ModeloIu
{
    [IUDto]
    public class RolesDeUnPuestoDto : ElementoDto
    {
        [IUPropiedad(Etiqueta = "Puesto",
            Ayuda = "Roles de un puesto",
            TipoDeControl = TipoControl.RestrictorDeEdicion,
            Fila = 0,
            Columna = 0,
            VisibleEnGrid = false
            )
        ]
        public int IdPuesto { get; set; }

        [IUPropiedad(
            Etiqueta = "Puesto de trabajo",
            Visible = false
            )
        ]
        public string Puesto { get; set; }

        [IUPropiedad(
            Etiqueta = "Id del rol",
            Visible = false
            )
        ]
        public int IdRol { get; set; }


        [IUPropiedad(
            Etiqueta = "Rol",
            Ayuda = "Indique el rol",
            TipoDeControl = TipoControl.ListaDinamica,
            SeleccionarDe = nameof(RolDto),
            GuardarEn = nameof(IdRol),
            MostrarPropiedad = nameof(RolDto.Nombre),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Rol { get; set; }


    }
}
