using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enumerados;

namespace ModeloDeDto.Negocio
{
    public static class NegocioPor
    {
        public static string idNegocio = nameof(idNegocio).ToLower();
    }

    [IUDto(AnchoEtiqueta = 20, AnchoSeparador = 5, OpcionDeCrear = false, OpcionDeBorrar = false)]
    public class AuditoriaDto : ElementoDto
    {
        //--------------------------------------------
        [IUPropiedad(
            Etiqueta = "Elemento",
            Ayuda = "Auditorial del elemento",
            TipoDeControl = enumTipoControl.RestrictorDeEdicion,
            MostrarExpresion = nameof(Elemento),
            Fila = 0,
            Columna = 0,
            EditableAlCrear = false,
            EditableAlEditar = false,
            VisibleEnGrid = false
            )
        ]
        public int IdElemento { get; set; }

        //--------------------------------------------
        [IUPropiedad(Visible = false)]
        public string Elemento { get; set; }
       
        //--------------------------------------------
        [IUPropiedad(Visible = false)]
        public int IdUsuario { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Usuario",
           Ayuda = "La operación la hizo el usuario",
           Visible = false,
           VisibleEnGrid = true, 
           VisibleAlEditar = true,
           TipoDeControl = enumTipoControl.Editor,
           Fila = 1,
           Columna = 0)
        ]
        public string Usuario { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Operación",
           Ayuda = "Operación auditada",
           Visible = false,
           VisibleEnGrid = true,
           EditableAlEditar = false,
           TipoDeControl = enumTipoControl.Editor,
           Fila = 2,
           Columna = 0)
        ]
        public string Operacion { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Información",
           Ayuda = "Información anterior",
           TipoDeControl = enumTipoControl.AreaDeTexto,
           Fila = 3,
           Columna = 0,
           VisibleEnGrid = false,
           EditableAlEditar = false,
           VisibleAlCrear = false,
           Obligatorio = false,
           NumeroDeFilas = 5
           )
        ]
        public string registroJson { get; set; }

        //--------------------------------------------
        [IUPropiedad(
           Etiqueta = "Fecha",
           Ayuda = "Fecha de auditoría",
           TipoDeControl = enumTipoControl.SelectorDeFechaHora,
           Fila = 2,
           Columna = 1,
           VisibleEnGrid = true,
           EditableAlCrear = false,
           EditableAlEditar = false
           )
        ]
        public DateTime AuditadoEl { get; set; }
    }

}
