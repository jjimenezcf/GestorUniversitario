using System;
using System.Collections.Generic;
using Enumerados;
using GestorDeElementos;
using GestoresDeNegocio.Entorno;
using GestoresDeNegocio.Negocio;
using ModeloDeDto;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Negocio;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum ModoDescriptor { Mantenimiento, Consulta, Seleccion, Relacion }

    public class DescriptorDeCrud<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        internal static string NombreCrud = $"Crud_{typeof(TElemento).Name}";

        public string Vista { get; private set; }

        public DescriptorDeMantenimiento<TElemento> Mnt { get; private set; }
        public DescriptorDeCreacion<TElemento> Creador { get; private set; }
        public DescriptorDeEdicion<TElemento> Editor { get; private set; }
        public DescriptorDeBorrado<TElemento> Borrado { get; private set; }
        public DescriptorDeDetalle Detalle { get; private set; }

        public string Controlador { get; private set; }
        public ModoDescriptor Modo { get; private set; }

        public bool EsModal => Modo != ModoDescriptor.Mantenimiento;

        public string RutaBase { get; set; }
        public UsuarioDtm UsuarioConectado { get; internal set; }
        public GestorDeUsuarios GestorDeUsuario { get; internal set; }
        public GestorDeNegocios GestorDeNegocio { get; internal set; }
        public bool NegocioActivo => GestorDeNegocio.NegocioActivo(_negocio);

        private enumNegocio _negocio = enumNegocio.No_Definido;
        public enumNegocio Negocio
        {
            get
            {
                if (_negocio == enumNegocio.No_Definido)
                {
                    _negocio = NegociosDeSe.ParsearDto(typeof(TElemento).Name);
                }
                return _negocio;

            }
            set { _negocio = value; }
        }

        public string RenderNegocio => negocioDtm == null ? NegociosDeSe.ToString(Negocio): negocioDtm.Nombre;
        public int RenderIdDeNegocio => negocioDtm == null ? 0 :negocioDtm.Id;

        public NegocioDtm negocioDtm = null;

        public DescriptorDeCrud(string controlador, string vista, ModoDescriptor modo, string rutaBase, string id = null)
        : base(
          padre: null,
          id: id==null? $"{NombreCrud}": id,
          etiqueta: typeof(TElemento).Name.Replace("Dto", ""),
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            var elemento = typeof(TElemento).Name.Replace("Dto", "");
            RutaBase = rutaBase;
            Tipo = enumTipoControl.DescriptorDeCrud;
            Mnt = new DescriptorDeMantenimiento<TElemento>(crud: this, etiqueta: elemento);
            Controlador = controlador.Replace("Controller", "");
            Vista = $@"{vista}";
            Modo = modo;

            DefinirColumnasDelGrid();

            Creador = new DescriptorDeCreacion<TElemento>(crud: this, etiqueta: elemento);
            Editor = new DescriptorDeEdicion<TElemento>(crud: this, etiqueta: elemento);
            Borrado = new DescriptorDeBorrado<TElemento>(crud: this, etiqueta: elemento);
            Mnt.ZonaMenu.AnadirOpcionDeIrACrear();
            Mnt.ZonaMenu.AnadirOpcionDeIrAEditarFilasSeleccionadas();
            Mnt.ZonaMenu.AnadirOpcionDeBorrarElemento();

            if (ElementoDtoExtensiones.ImplementaAuditoria(typeof(TElemento)))
            {
                var expanDeAuditoria = new DescriptorDeExpansor(Editor, $"{Editor.Id}-audt", "Auditoría", "Información de auditoría");

                var fechaCreacion = new EditorDeFecha(expanDeAuditoria, "Creado el", nameof(IAuditadoDto.CreadoEl), "fecha de cuando se creó el elemento");
                var fechaModificacion = new EditorDeFecha(expanDeAuditoria, "Modificado el", nameof(IAuditadoDto.ModificadoEl), "fecha de cuando se modificó por última vez");
                fechaCreacion.Editable = false;
                fechaModificacion.Editable = false;

                var creador = new EditorDeTexto(expanDeAuditoria, "Creado por", nameof(IAuditadoDto.Creador), "Quién lo creó");
                var modificador = new EditorDeTexto(expanDeAuditoria, "Modificado por", nameof(IAuditadoDto.Modificador), "Quién lo modificó");
                var mostrarHistorico = new NavegarDesdeEdicion(expanDeAuditoria, "Ver auditoría", "Histórico de modificaciones del registro", $"/Auditoria/CrudDeAuditoria/?negocio={RenderNegocio}");
                creador.Editable = false;
                modificador.Editable = false;

                Editor.Expanes.Add(expanDeAuditoria);
                expanDeAuditoria.Controles.Add(fechaCreacion);
                expanDeAuditoria.Controles.Add(fechaModificacion);
                expanDeAuditoria.Controles.Add(creador);
                expanDeAuditoria.Controles.Add(new DivEnBlanco(expanDeAuditoria));
                expanDeAuditoria.Controles.Add(modificador);
                expanDeAuditoria.Controles.Add(new DivEnBlanco(expanDeAuditoria));
                expanDeAuditoria.Controles.Add(mostrarHistorico);
            }

        }


        public ControlFiltroHtml BuscarControlEnFiltro(string propiedad)
        {
            return Mnt.Filtro.BuscarControl(propiedad);
        }

        public void CambiarModo(ModoDescriptor modo)
        {
            Modo = modo;
        }

        protected virtual void DefinirColumnasDelGrid()
        {
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<TElemento> { Propiedad = "chksel", Titulo = " ", PorAnchoMnt = 4, PorAnchoSel = 10, Tipo = typeof(bool) });
            var propiedades = typeof(TElemento).GetProperties();
            foreach (var p in propiedades)
            {
                var columna = new ColumnaDelGrid<TElemento> { Propiedad = p.Name, Tipo = p.PropertyType };
                IUPropiedadAttribute atributos = ElementoDto.ObtenerAtributos(p);

                if (atributos != null)
                {
                    columna.Visible = atributos.EsVisible(ModoDeTrabajo.Mantenimiento);
                    columna.Titulo = atributos.EtiquetaGrid;
                    columna.ConOrdenacion = atributos.Ordenar;

                    if (p.Name.ToLower() == CamposDeFiltrado.Nombre && atributos.Ordenar)
                        columna.cssOrdenacion = enumCssOrdenacion.Ascendente;

                    columna.OrdenarPor = atributos.OrdenarPor;
                    columna.Alineada = atributos.Alineada == Aliniacion.no_definida ? columna.Tipo.Alineada(): atributos.Alineada;
                    columna.PorAnchoMnt = atributos.PorAnchoMnt;
                    columna.PorAnchoSel = atributos.PorAnchoSel == 0 ? atributos.PorAnchoMnt : atributos.PorAnchoSel;
                    Mnt.Datos.InsertarColumna(columna, atributos.PosicionEnGrid);
                }
            }
        }

        public virtual void MapearElementosAlGrid<T>(IEnumerable<T> elementos, int cantidadPorLeer, int posicionInicial)
            where T : ElementoDto
        {
            Mnt.Datos.PosicionInicial = posicionInicial;
            Mnt.Datos.CantidadPorLeer = cantidadPorLeer;

            foreach (var elemento in elementos)
            {
                var fila = new FilaDelGrid<TElemento>(Mnt.Datos, elemento);
                foreach (ColumnaDelGrid<TElemento> columna in Mnt.Datos.Columnas)
                {
                    CeldaDelGrid<TElemento> celda = new CeldaDelGrid<TElemento>(columna);
                    var propiedades = typeof(TElemento).GetProperties();
                    foreach (var p in propiedades)
                    {
                        if (columna.Propiedad == p.Name)
                        {
                            celda.Valor = p.GetValue(elemento);
                            break;
                        }
                    }
                    fila.AnadirCelda(celda);
                }
                Mnt.Datos.AnadirFila(fila);
            }
        }

        public void TotalEnBd(int totalEnBd)
        {
            Mnt.Datos.TotalEnBd = totalEnBd;
        }

        public override string RenderControl()
        {
            try
            {
                var renderMnt = Mnt.RenderControl();
                if (ModoDescriptor.Mantenimiento == Modo)
                    return $@"
                  {renderMnt}
                  <!--  ******************* div de creacion ******************* -->
                  {Creador.RenderControl()}
                  <!--  *******************  div de edición ******************* -->
                  {Editor.RenderControl()}
                  <!--  *******************  div de borrado ******************* -->
                  {Borrado.RenderControl()}";

                if (ModoDescriptor.Consulta == Modo)
                    return $@"
                 {renderMnt}
                 <!--  *******************  div de edición -->
                 {Editor.RenderControl()}";

                return renderMnt;
            }
            catch (Exception e)
            {
                return $@"
                   <input id=error>{e.Message}</input>
                ";
            }
        }


        internal string RenderCrudModal(string idModal, enumTipoDeModal tipoDeModal)
        {
            return Mnt.RenderMntModal(idModal, tipoDeModal);
        }

        public static ModoDescriptor ParsearModo(string modo)
        {
            switch (modo)
            {
                case nameof(ModoDescriptor.Seleccion):
                    return ModoDescriptor.Seleccion;
                case nameof(ModoDescriptor.Mantenimiento):
                    return ModoDescriptor.Mantenimiento;
                case nameof(ModoDescriptor.Consulta):
                    return ModoDescriptor.Consulta;
                case nameof(ModoDescriptor.Relacion):
                    return ModoDescriptor.Relacion;
            }
            throw new Exception($"El modo {modo} no está definido");
        }

        internal static void AnadirOpciondeRelacion(DescriptorDeMantenimiento<TElemento> Mnt, string controlador, string vista, string relacionarCon, string navegarAlCrud, string nombreOpcion, string propiedadQueRestringe, string propiedadRestrictora, string ayuda)
        {
            var accionDeRelacion = new AccionDeRelacionarElemenetos(
                    urlDelCrud: $@"/{controlador.Replace("Controller", "")}/{vista}"
                  , relacionarCon: relacionarCon
                  , nombreDelMnt: navegarAlCrud
                  , propiedadQueRestringe: propiedadQueRestringe
                  , propiedadRestrictora: propiedadRestrictora
                  , ayuda);

            var opcion = new OpcionDeMenu<TElemento>(menu: Mnt.ZonaMenu.Menu, accion: accionDeRelacion, tipoAccion: TipoDeLlamada.Post, titulo: $"{nombreOpcion}", enumModoDeAccesoDeDatos.Gestor, enumCssOpcionMenu.DeElemento);
            Mnt.ZonaMenu.Menu.Add(opcion);
        }

        internal static void AnadirOpcionDeDependencias(DescriptorDeMantenimiento<TElemento> Mnt, string controlador, string vista, string datosDependientes, string navegarAlCrud, string nombreOpcion, string propiedadQueRestringe, string propiedadRestrictora, string ayuda)
        {
            var accionDeDependencias = new AccionDeGetionarDatosDependientes(
                    urlDelCrud: $@"/{controlador.Replace("Controller", "")}/{vista}"
                  , datosDependientes: datosDependientes
                  , nombreDelMnt: navegarAlCrud
                  , propiedadQueRestringe: propiedadQueRestringe
                  , propiedadRestrictora: propiedadRestrictora
                  , ayuda);

            var opcion = new OpcionDeMenu<TElemento>(menu: Mnt.ZonaMenu.Menu, accion: accionDeDependencias, tipoAccion: TipoDeLlamada.Post, titulo: $"{nombreOpcion}", enumModoDeAccesoDeDatos.Gestor, enumCssOpcionMenu.DeElemento);
            Mnt.ZonaMenu.Menu.Add(opcion);
        }
    }

}



