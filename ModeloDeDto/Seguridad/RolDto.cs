﻿namespace ModeloDeDto.Seguridad
{
    [IUDto(AnchoEtiqueta = 20
      , AnchoSeparador = 5)]
    public class RolDto : ElementoDto
    {
        [IUPropiedad(
            Etiqueta = "Rol",
            Ayuda = "Nombre del rol",
            Tipo = typeof(string),
            Fila = 0,
            Columna = 0,
            Ordenar = true
            )
        ]
        public string Nombre { get; set; }


        [IUPropiedad(
            Etiqueta = "Descripción",
            Ayuda = "Descripción del rol",
            Tipo = typeof(string),
            Fila = 0,
            Columna = 0,
            Ordenar = true
            )
        ]
        public string Descripcion { get; set; }
    }
}
