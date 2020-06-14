using System;
using System.Collections.Generic;
using System.Text;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.ModeloIu;
using Gestor.Elementos.Seguridad;

namespace GestorDeSeguridad.ModeloIu
{
    public class PuestoDeUnUsuarioDto: ElementoDto
    {
        [IUPropiedad(
            Etiqueta = "Id del usuario",
            Visible = false
            )
        ]
        public int IdUsuario { get; set; }

        [IUPropiedad(
            Etiqueta = "Usuario",
            Ayuda = "Puestos del usuario",
            TipoDeControl = TipoControl.ListaDinamica,
            SeleccionarDe = nameof(Usuario),
            GuardarEn = nameof(IdUsuario),
            MostrarPropiedad = nameof(UsuarioDto.NombreCompleto),
            Fila = 0,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Usuario { get; set; }

        [IUPropiedad(
            Etiqueta = "Id del puesto de trabajo",
            Visible = false
            )
        ]
        public int IdPuesto { get; set; }

        [IUPropiedad(
            Etiqueta = "Tipo",
            Ayuda = "Indique el tipo a aplicar",
            TipoDeControl = TipoControl.ListaDinamica,
            SeleccionarDe = nameof(PuestoDto),
            GuardarEn = nameof(IdPuesto),
            MostrarPropiedad = nameof(PuestoDto.Nombre),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Tipo { get; set; }

    }
}
