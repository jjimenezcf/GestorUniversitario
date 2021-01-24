using System;
using System.Collections.Generic;
using GestorDeElementos;
using GestoresDeNegocio.Entorno;
using GestoresDeNegocio.Negocio;
using ModeloDeDto;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum ModoDescriptor { Mantenimiento, Consulta, Seleccion, Relacion }

    public class DescriptorDeCrud<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        internal static string NombreCrud = $"Crud_{typeof(TElemento).Name}".ToLower();

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
            private set { _negocio = value; }
        }

        public string Vista { get; private set; }

        public DescriptorDeMantenimiento<TElemento> Mnt { get; private set; }
        public DescriptorDeCreacion<TElemento> Creador { get; private set; }
        public DescriptorDeEdicion<TElemento> Editor { get; private set; }
        public DescriptorDeBorrado<TElemento> Borrado { get; private set; }
        public DescriptorDeDetalle Detalle { get; private set; }

        public string Controlador { get; private set; }
        public ModoDescriptor Modo { get; private set; }

        public bool EsModal => Modo != ModoDescriptor.Mantenimiento;

        public string RutaVista { get; set; }
        public UsuarioDtm UsuarioConectado { get; internal set; }
        public GestorDeUsuarios GestorDeUsuario { get; internal set; }
        public GestorDeNegocio GestorDeNegocio { get; internal set; }
        public bool NegocioActivo => GestorDeNegocio.NegocioActivo(Negocio);

        public DescriptorDeCrud(string controlador, string vista, ModoDescriptor modo)
        : base(
          padre: null,
          id: $"Crud_{typeof(TElemento).Name}",
          etiqueta: typeof(TElemento).Name.Replace("Dto", ""),
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            var elemento = typeof(TElemento).Name.Replace("Dto", "");
            Tipo = TipoControl.DescriptorDeCrud;
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
                    columna.Ordenar = atributos.Ordenar;
                    columna.Alineada = atributos.Alineada;
                    columna.PorAnchoMnt = 0;
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


        internal string RenderCrudModal(string idModal)
        {
            return Mnt.RenderMntModal(idModal);
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

        internal static void AnadirOpciondeRelacion(DescriptorDeMantenimiento<TElemento> Mnt, string controlador, string vista, string relacionarCon, string navegarAlCrud, string nombreOpcion, string propiedadQueRestringe, string propiedadRestrictora)
        {
            var accionDeRelacion = new AccionDeNavegarParaRelacionar(
                    urlDelCrud: $@"/{controlador.Replace("Controller", "")}/{vista}"
                  , relacionarCon: relacionarCon
                  , nombreDelMnt: navegarAlCrud
                  , propiedadQueRestringe: propiedadQueRestringe
                  , propiedadRestrictora: propiedadRestrictora);

            var opcion = new OpcionDeMenu<TElemento>(menu: Mnt.ZonaMenu.Menu, accion: accionDeRelacion, tipoAccion: TipoDeLlamada.Post, titulo: $"{nombreOpcion}", enumModoDeAccesoDeDatos.Gestor, enumCssOpcionMenu.DeElemento);
            Mnt.ZonaMenu.Menu.Add(opcion);
        }
    }

}



