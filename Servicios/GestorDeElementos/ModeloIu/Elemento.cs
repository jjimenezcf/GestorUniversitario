using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Utilidades;

namespace Gestor.Elementos.ModeloIu
{
    public static class TipoControl
    {
        public const string Selector = "selector";
        public const string SelectorDeElemento = "selector-de-elemento";
        public const string Editor = "editor";
        public const string Archivo = "archivo";
        public const string Desplegable = "desplegable";
        public const string GridModal = "grid-modal";
        public const string TablaBloque = "tabla-bloque";
        public const string Bloque = "bloque";
        public const string ZonaDeOpciones = "zona-de-opciones";
        public const string ZonaDeDatos = "zona-de-datos";
        public const string ZonaDeFiltro = "zona-de-filtro";
        public const string VistaCrud = "vista-crud";
        public const string DescriptorDeCrud = "descriptor-crud";
        public const string Opcion = "opcion";
        public const string Label = "label";
        public const string Referencia = "referencia";
        public const string Lista = "lista";
        public const string Fecha = "fecha";
        public const string Plantilla = "plantilla";
        public const string Mantenimiento = "mantenimiento";
        public const string pnlCreador = "panel-creador";
        public const string pnlEditor = "panel-editor";
        public const string pnlBorrado = "panel-borrado";
        public const string ZonaMenu = "zona-menu";
        public const string Menu = "menu";
    }

    public enum LadoDeRenderizacion { izquierdo, derecho }
    public enum ModoDeTrabajo { Nuevo, Consulta, Edicion }

    public enum Aliniacion { no_definida, izquierda, centrada, derecha, justificada };

    public class IUPropiedadAttribute : Attribute
    {
        private string etiquetaGrid;

        private bool _visibleEnGrid = true;

        public string EtiquetaGrid
        {
            get
            {
                if (etiquetaGrid.IsNullOrEmpty())
                    return Etiqueta;
                return etiquetaGrid;
            }
            set { etiquetaGrid = value; }
        }

        public string Etiqueta { get; set; } = "";
        public string Ayuda { get; set; } = "";
        public bool Visible { get; set; } = true;
        public bool VisibleEnGrid { get { return _visibleEnGrid && Visible; } set { _visibleEnGrid = value; } }
        public bool VisibleAlCrear { get; set; } = true;
        public bool VisibleAlEditar { get; set; } = true;
        public bool VisibleAlConsultar { get; set; } = true;
        public bool EditableAlCrear { get; set; } = true;
        public bool EditableAlEditar { get; set; } = true;
        public bool Obligatorio { get; set; } = true;
        public Type Tipo { get; set; } = typeof(string);
        public short Fila { get; set; }
        public short Columna { get; set; }
        public short Posicion { get; set; } = 0;
        public string ValorPorDefecto { get; set; }
        public bool Ordenar { get; set; } = false;
        public int PosicionEnGrid { get; set; } = -1;
        public Aliniacion Alineada
        {
            get
            {
                if (Tipo == typeof(string)) return Aliniacion.izquierda;
                if (Tipo == typeof(int)) return Aliniacion.derecha;
                if (Tipo == typeof(DateTime)) return Aliniacion.centrada;
                return Aliniacion.izquierda;
            }
        }
        public int PorAnchoMnt { get; set; } = 0;
        public int PorAnchoSel { get; set; } = 0;

        public string TipoDeControl { get; set; } = TipoControl.Editor;

        public string SeleccionarDe { get; set; }

        public string GuardarEn { get; set; }

        public string MostrarPropiedad { get; set; }

        public bool EsVisible(ModoDeTrabajo modo)
        {
            if (Visible)
            {
                if (modo == ModoDeTrabajo.Edicion)
                    return VisibleAlEditar;
                else
                if (modo == ModoDeTrabajo.Nuevo)
                    return VisibleAlCrear;
                else
                if (modo == ModoDeTrabajo.Consulta)
                    return VisibleAlConsultar;
            }

            return false;
        }
        public bool EsEditable(ModoDeTrabajo modo)
        {
            if (EsVisible(modo))
            {
                if (modo == ModoDeTrabajo.Edicion)
                    return EditableAlEditar;
                else
                if (modo == ModoDeTrabajo.Nuevo)
                    return EditableAlCrear;
            }

            return false;
        }



    }

    public class IUDtoAttribute : Attribute
    {
        /// <summary>
        /// Ancho que se les da a las etiquetas en la iu
        /// </summary>
        public short AnchoEtiqueta { get; set; } = 15;
        /// <summary>
        /// Separación entre la etiqueta y el contro que muestra el dato
        /// </summary>
        public short AnchoSeparador { get; set; } = 2;
    }


    public static class FiltroPor
    {
        public static string Nombre = nameof(Nombre).ToLower();
        public static string Id = nameof(Id).ToLower();
    }


    public class Elemento
    {
        [IUPropiedad(
            Etiqueta = "Id",
            Ayuda = "id del elemento",
            Tipo = typeof(int),
            Visible = false
            )
        ]
        public int Id { get; set; }


        public static IUPropiedadAttribute ObtenerAtributos(PropertyInfo propiedad)
        {
            var iEnumerableAtrb = propiedad.GetCustomAttributes(typeof(IUPropiedadAttribute));
            if (iEnumerableAtrb == null || iEnumerableAtrb.ToList().Count == 0)
                Errores.GestorDeErrores.Emitir($"No se puede definir el descriptor para el tipo {propiedad.DeclaringType} por no tener definidas los atributos {typeof(IUPropiedadAttribute)}");

            var listaAtrb = iEnumerableAtrb.ToList();            

            if (listaAtrb.Count != 1)
                Errores.GestorDeErrores.Emitir($"No se puede definir el descriptor para el tipo {propiedad.DeclaringType} por tener mas de una definición para {typeof(IUPropiedadAttribute)}");

            var atributos = (IUPropiedadAttribute)propiedad.GetCustomAttributes(typeof(IUPropiedadAttribute)).ToList()[0];
            return atributos;
        }

        public static object ValorDelAtributo(Type clase, string nombreAtributo, bool obligatorio = true)
        {
            Attribute[] atributosDeDto = System.Attribute.GetCustomAttributes(clase);

            if (atributosDeDto == null || atributosDeDto.Length == 0)
                Errores.GestorDeErrores.Emitir($"No hay definido descriptores para el dto {clase.Name}");

            foreach (Attribute propiedad in atributosDeDto)
            {
                if (propiedad is IUDtoAttribute)
                {
                    IUDtoAttribute a = (IUDtoAttribute)propiedad;
                    switch (nombreAtributo)
                    {
                        case nameof(IUDtoAttribute.AnchoEtiqueta):
                            return a.AnchoEtiqueta;

                        case nameof(IUDtoAttribute.AnchoSeparador):
                            return a.AnchoSeparador;
                    }
                    if (obligatorio)
                        throw new Exception($"Se ha solicitado el atributo {nameof(IUDtoAttribute)}.{nombreAtributo} de la clase {clase} y no está definido");
                }
            }

            return null;

        }
    }
}