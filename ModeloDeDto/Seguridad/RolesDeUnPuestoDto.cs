﻿namespace ModeloDeDto.Seguridad
{
    [IUDto(ExpresionNombre = "[Rol]")]
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
            SiempreVisible = false
            )
        ]
        public string Puesto { get; set; }

        [IUPropiedad(
            Etiqueta = "Id del rol",
            SiempreVisible = false
            )
        ]
        public int IdRol { get; set; }


        [IUPropiedad(
            Etiqueta = "Rol",
            Ayuda = "Indique el rol",
            TipoDeControl = TipoControl.ListaDinamica,
            SeleccionarDe = nameof(RolDto),
            GuardarEn = nameof(IdRol),
            MostrarExpresion = nameof(RolDto.Nombre),
            Fila = 1,
            Columna = 0,
            Ordenar = true,
            PorAnchoMnt = 15
            )
        ]
        public string Rol { get; set; }


    }
}
