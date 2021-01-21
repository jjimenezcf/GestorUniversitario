using System;
using System.Linq;
using System.Reflection;
using Utilidades;

namespace ModeloDeDto
{

    public static class TipoControl
    {
        public const string Selector = "selector";
        public const string ListaDeElemento = "lista-de-elemento";
        public const string ListaDinamica = "lista-dinamica";
        public const string Editor = "editor";
        public const string RestrictorDeFiltro = "restrictor-filtro";
        public const string RestrictorDeEdicion = "restrictor-edicion";
        public const string Archivo = "archivo";
        public const string Check = "check";
        public const string UrlDeArchivo = "url-archivo";
        public const string VisorDeArchivo = "visor-archivo";
        public const string ImagenDelCanvas = "imagen-de-canva";
        public const string DesplegableDeFiltro = "desplegable-de-filtro";
        public const string GridModal = "grid-modal";
        public const string TablaBloque = "tabla-bloque";
        public const string Bloque = "bloque";
        public const string ZonaDeMenu = "zona-menu";
        public const string ZonaDeDatos = "zona-de-datos";
        public const string ZonaDeFiltro = "zona-de-filtro";
        public const string Menu = "menu";
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
        public const string ModalDeRelacion = "modal-de-relacion";
        public const string ModalDeConsulta = "modal-de-consulta";
    }

    public static class CamposDeFiltrado
    {
        public static string Nombre = nameof(Nombre).ToLower();
        public static string Id = nameof(Id).ToLower();
        public static string PorDefecto = "por-defecto";
    }

    public enum CriteriosDeFiltrado { igual, mayor, menor, esNulo, noEsNulo, contiene, comienza, termina, mayorIgual, menorIgual, diferente, esAlgunoDe }

    public enum LadoDeRenderizacion { izquierdo, derecho }
    public enum ModoDeTrabajo { Nuevo, Consulta, Edicion, Mantenimiento }

    public enum Aliniacion { no_definida, izquierda, centrada, derecha, justificada };

    public class IUPropiedadAttribute : Attribute
    {
        private string etiquetaGrid;


        private bool _visibleEnGrid = true;

        private string _ayuda = "";

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
        public string Ayuda { get { return _ayuda.IsNullOrEmpty() ? Etiqueta : _ayuda;} set { _ayuda = value; } } 
        public bool SiempreVisible { get { return VisibleAlCrear && VisibleAlEditar && VisibleAlConsultar && _visibleEnGrid; } set { VisibleAlCrear = VisibleAlEditar = VisibleAlConsultar = _visibleEnGrid = value; } }
        public bool VisibleEnGrid { get { return _visibleEnGrid && TipoDeControl != TipoControl.UrlDeArchivo; } set { _visibleEnGrid = value; } }
        public bool VisibleEnEdicion { get { return VisibleAlCrear && VisibleAlEditar && VisibleAlConsultar; } set { VisibleAlCrear = VisibleAlEditar = VisibleAlConsultar = value; } }
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
        public object ValorPorDefecto { get; set; }
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

        /// <summary>
        /// Dto del que se van a seleccionar los valores
        /// </summary>
        public string SeleccionarDe { get; set; }

        public CriteriosDeFiltrado CriterioDeBusqueda { get; set; } = CriteriosDeFiltrado.contiene;

        public string GuardarEn { get; set; }
        public string BuscarPor { get; set; } = CamposDeFiltrado.PorDefecto;

        public string MostrarExpresion { get; set; } = ElementoDto.ExpresionPorDefecto;

        public bool CargaDinamica => TipoDeControl == TipoControl.ListaDinamica;

        public string UrlDelArchivo { get; set; }

        public string ExtensionesValidas { get; set; } = "*.*";
        public object RutaDestino { get; set; }

        public bool EsVisible(ModoDeTrabajo modo)
        {
            if (TipoControl.ImagenDelCanvas == TipoDeControl)
                return false;

            if (SiempreVisible)
                return true;

            if (modo == ModoDeTrabajo.Edicion)
                return VisibleAlEditar;
            
            if (modo == ModoDeTrabajo.Nuevo)
                return VisibleAlCrear;
            
            if (modo == ModoDeTrabajo.Consulta)
                return VisibleAlConsultar;
            
            if (modo == ModoDeTrabajo.Mantenimiento)
                return VisibleEnGrid;

            return false;
        }
        public bool EsEditable(ModoDeTrabajo modo)
        {
            if (TipoControl.RestrictorDeEdicion == TipoDeControl)
                return false;

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
        /// Separación entre la etiqueta y el control que muestra el dato
        /// </summary>
        public short AnchoSeparador { get; set; } = 2;

        /// <summary>
        /// indica las propiedades del dto con las que se conforma el nombre
        /// </summary>
        public string ExpresionNombre { get; set; } = ElementoDto.ExpresionPorDefecto;
    }

    public class ElementoDto
    {
        public static string ExpresionPorDefecto = "[nombre]";

        [IUPropiedad(
            Etiqueta = "Id",
            Ayuda = "id del elemento",
            Tipo = typeof(int),
            SiempreVisible = false
            )
        ]
        public int Id { get; set; }


        public static IUPropiedadAttribute ObtenerAtributos(PropertyInfo propiedad)
        {
            var iEnumerableAtrb = propiedad.GetCustomAttributes(typeof(IUPropiedadAttribute));
            if (iEnumerableAtrb == null || iEnumerableAtrb.ToList().Count == 0)
                Gestor.Errores.GestorDeErrores.Emitir($"No se puede definir el descriptor para el tipo {propiedad.DeclaringType} por no tener definidas los atributos {typeof(IUPropiedadAttribute)}");

            var listaAtrb = iEnumerableAtrb.ToList();

            if (listaAtrb.Count != 1)
                Gestor.Errores.GestorDeErrores.Emitir($"No se puede definir el descriptor para el tipo {propiedad.DeclaringType} por tener mas de una definición para {typeof(IUPropiedadAttribute)}");

            var atributos = (IUPropiedadAttribute)propiedad.GetCustomAttributes(typeof(IUPropiedadAttribute)).ToList()[0];
            return atributos;
        }

        public static object ValorDelAtributo(Type clase, string nombreAtributo, bool obligatorio = true)
        {
            Attribute[] atributosDeDto = System.Attribute.GetCustomAttributes(clase);

            if (atributosDeDto == null || atributosDeDto.Length == 0)
                Gestor.Errores.GestorDeErrores.Emitir($"No hay definido descriptores para el dto {clase.Name}");

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

                        case nameof(IUDtoAttribute.ExpresionNombre):
                            return a.ExpresionNombre;
                    }
                    if (obligatorio)
                        throw new Exception($"Se ha solicitado el atributo {nameof(IUDtoAttribute)}.{nombreAtributo} de la clase {clase} y no está definido");
                }
            }

            return null;

        }
    }
}