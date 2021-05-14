using System;
using System.Linq;
using System.Reflection;
using Utilidades;
using Enumerados;
using ServicioDeDatos.Elemento;
using System.IO;
using System.Collections.Generic;

namespace ModeloDeDto
{
    public static class ltrFiltros
    {
        public static string Nombre = nameof(Nombre).ToLower();
        public static string Id = nameof(Id).ToLower();
        public static string filtro = nameof(filtro).ToLower();
        public static string posicion = nameof(posicion).ToLower();
        public static string orden = nameof(orden).ToLower();
        public static string cantidad = nameof(cantidad).ToLower();
    }

    public enum CriteriosDeFiltrado { igual, mayor, menor, esNulo, noEsNulo, contiene, noContiene, comienza, termina, mayorIgual, menorIgual, diferente, esAlgunoDe, entreFechas }

    public enum LadoDeRenderizacion { izquierdo, derecho }
    public enum ModoDeTrabajo { Nuevo, Consulta, Edicion, Mantenimiento }

    public enum Aliniacion { no_definida, izquierda, centrada, derecha, justificada };

    public class IUPropiedadAttribute : Attribute
    {
        private string etiquetaGrid;


        private bool _visibleEnGrid = true;

        private string _ayuda = "";

        public static string NombreConColSpan => "ConSpanEnColumnas";

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
        public string Ayuda { get { return _ayuda.IsNullOrEmpty() ? Etiqueta : _ayuda; } set { _ayuda = value; } }
        public bool Visible { get { return VisibleAlCrear && VisibleAlEditar && VisibleAlConsultar && _visibleEnGrid; } set { VisibleAlCrear = VisibleAlEditar = VisibleAlConsultar = _visibleEnGrid = value; } }
        public bool VisibleEnGrid { get { return _visibleEnGrid && TipoDeControl != enumTipoControl.UrlDeArchivo; } set { _visibleEnGrid = value; } }
        public bool VisibleEnEdicion { get { return VisibleAlCrear && VisibleAlEditar && VisibleAlConsultar; } set { VisibleAlCrear = VisibleAlEditar = VisibleAlConsultar = value; } }
        public bool VisibleAlCrear { get; set; } = true;
        public bool VisibleAlEditar { get; set; } = true;
        public bool VisibleAlConsultar { get; set; } = true;
        public bool EditableAlCrear { get; set; } = true;
        public bool EditableAlEditar { get; set; } = true;
        public int NumeroDeFilas { get; set; } = 5;
        public bool ConSpanEnColumnas { get; set; } = true;
        public bool Obligatorio { get; set; } = true;
        public Type Tipo { get; set; } = typeof(string);
        public short Fila { get; set; }
        public short Columna { get; set; }
        public short Posicion { get; set; } = 0;
        public object ValorPorDefecto { get; set; }
        public bool Ordenar { get; set; } = false;
        public string OrdenarPor { get; set; }
        public string AnchoMaximo { get; set; } = null;
        public int PosicionEnGrid { get; set; } = -1;
        public Aliniacion Alineada { get; set; } = Aliniacion.no_definida;
        public int PorAnchoMnt { get; set; } = 0;
        public int PorAnchoSel { get; set; } = 0;

        public enumTipoControl TipoDeControl { get; set; } = enumTipoControl.Editor;

        /// <summary>
        /// Dto del que se van a seleccionar los valores
        /// </summary>
        public Type SeleccionarDe { get; set; }

        public CriteriosDeFiltrado CriterioDeBusqueda { get; set; } = CriteriosDeFiltrado.contiene;

        public string GuardarEn { get; set; }
        public string BuscarPor { get; set; } = ltrFiltros.Nombre;

        public string MostrarExpresion { get; set; }

        public bool CargaDinamica => TipoDeControl == enumTipoControl.ListaDinamica;

        public string UrlDelArchivo { get; set; }

        public string ExtensionesValidas { get; set; } = "*.*";
        public object RutaDestino { get; set; }

        public bool EsVisible(ModoDeTrabajo modo)
        {
            if (enumTipoControl.ImagenDelCanvas == TipoDeControl)
                return false;

            if (Visible)
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
            if (enumTipoControl.RestrictorDeEdicion == TipoDeControl)
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

        public int LongitudMaxima { get; set; }

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
        public bool OpcionDeCrear { get; set; } = true;
        public bool OpcionDeEditar { get; set; } = true;
        public bool OpcionDeBorrar { get; set; } = true;
        public bool OpcionDeExportar { get; set; } = true;
        public bool OpcionDeEnviar { get; set; } = true;

        private string _mostrarExpresion = $"[{nameof(INombre.Nombre)}]";
        public string MostrarExpresion
        {
            get { return _mostrarExpresion; }
            set
            {
                _mostrarExpresion = value;
                if (!_mostrarExpresion.Contains('[') && !_mostrarExpresion.Contains(']'))
                    _mostrarExpresion = $"[{_mostrarExpresion}]";
            }
        }
    }

    public interface IAuditadoDto
    {
        [IUPropiedad(
            Visible = false
          )
        ]
        public DateTime CreadoEl { get; set; }
        [IUPropiedad(
            Visible = false
          )
        ]
        public DateTime? ModificadoEl { get; set; }
        [IUPropiedad(
            Visible = false
          )
        ]
        public string Creador { get; set; }
        [IUPropiedad(
            Visible = false
          )
        ]
        public string Modificador { get; set; }
    }

    public interface IElementoDto
    {

    }
    public class ElementoDto: IElementoDto
    {
        public static string DescargarGestionDocumental = "descargar-gestion-documental";

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
                Gestor.Errores.GestorDeErrores.Emitir($"No hay definido descriptores {nameof(IUDtoAttribute)} para el dto {clase.Name}");

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

                        case nameof(IUDtoAttribute.MostrarExpresion):
                            return a.MostrarExpresion;

                        case nameof(IUDtoAttribute.OpcionDeCrear):
                            return a.OpcionDeCrear;

                        case nameof(IUDtoAttribute.OpcionDeBorrar):
                            return a.OpcionDeBorrar;

                        case nameof(IUDtoAttribute.OpcionDeEditar):
                            return a.OpcionDeEditar;

                        case nameof(IUDtoAttribute.OpcionDeExportar):
                            return a.OpcionDeExportar;

                        case nameof(IUDtoAttribute.OpcionDeEnviar):
                            return a.OpcionDeEnviar;
                    }
                    if (obligatorio)
                        throw new Exception($"Se ha solicitado el atributo {nameof(IUDtoAttribute)}.{nombreAtributo} de la clase {clase} y no está definido");
                }
            }

            return null;

        }
    }

    public class AuditoriaDto : ElementoDto, IAuditadoDto
    {
        [IUPropiedad(Visible = false
            , PorAnchoMnt = 15
            , Etiqueta = "Creado el"
            , Ordenar = true, OrdenarPor = nameof(ElementoDtm.FechaCreacion)
            , Alineada = Aliniacion.centrada)]
        public DateTime CreadoEl { get; set; }

        //----------------------------------------------
        [IUPropiedad(Visible = false
            , PorAnchoMnt = 15
            , Etiqueta = "Modificado el"
            , Ordenar = true
            , OrdenarPor = nameof(ElementoDtm.FechaModificacion)
            , Alineada = Aliniacion.centrada)]
        public DateTime? ModificadoEl { get; set; }

        //----------------------------------------------
        [IUPropiedad(Visible = false, PorAnchoMnt = 20, Etiqueta = "Creado por")]
        public string Creador { get; set; }

        //----------------------------------------------
        [IUPropiedad(Visible = false, PorAnchoMnt = 20, Etiqueta = "Modificado por")]
        public string Modificador { get; set; }
    }

    public static class ElementoDtoExtensiones
    {
        public static bool ImplementaAuditoria(this Type tipoElemento)
        {
            return tipoElemento.GetInterfaces().Contains(typeof(IAuditadoDto));
        }

        public static Aliniacion Alineada(this Type tipo)
        {
            if (tipo == typeof(string)) return Aliniacion.izquierda;
            if (tipo == typeof(int)) return Aliniacion.derecha;
            if (tipo == typeof(DateTime)) return Aliniacion.centrada;
            return Aliniacion.izquierda;
        }

        public static string ToExcel<T>(this List<T> elementos, string ruta, string fichero)
        {
            var ficheroConRuta = Path.Combine(ruta, fichero);
            var excel = new ExportarExcel<T>(ficheroConRuta, elementos.ToList());
            excel.Exportar();
            return ficheroConRuta;
        }
    }
}