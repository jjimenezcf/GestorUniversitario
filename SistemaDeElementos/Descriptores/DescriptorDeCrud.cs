using System;
using System.Collections.Generic;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum ModoDescriptor { Mantenimiento, Seleccion }

    public class VistaCsHtml : ControlHtml
    {
        public string Ruta { get; private set; }
        public string Vista { get; private set; }
        public string Ir => $"Ira{Vista}";

        public VistaCsHtml(ControlHtml padre, string id, string ruta, string vista, string texto)
        : base(
          padre: padre,
          id: $"{padre.Id}_{id}",
          etiqueta: texto,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.VistaCrud;
            Ruta = ruta;
            Vista = vista;
        }

        public override string RenderControl()
        {
            throw new NotImplementedException();
        }
    }

    public class DescriptorDeCrud<TElemento> : ControlHtml
    {
        public VistaCsHtml VistaMnt { get; private set; }
        public VistaCsHtml VistaCreacion { get; private set; }

        public DescriptorMantenimiento<TElemento> DescriptorDeMantenimiento { get; private set; }
        public DescriptorDeCreacion DescriptorDeCreacion { get; private set; }
        public DescriptorDeEdicion DescriptorDeEdicion { get; private set; }
        public DescriptorDeBorrado DescriptorDeBorrado { get; private set; }
        public DescriptorDeDetalle DescriptorDeDetalle { get; private set; }

        public MenuMantenimiento<TElemento> Menu { get; set; }
        public string Controlador { get; private set; }
        public ModoDescriptor Modo { get; private set; }

        public DescriptorDeCrud(string controlador, string vista, string titulo, ModoDescriptor modo)
        : base(
          padre: null,
          id: typeof(TElemento).Name.Replace("Elemento", ""),
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.DescriptorDeCrud;
            DescriptorDeMantenimiento = new DescriptorMantenimiento<TElemento>(this);
            VistaMnt = new VistaCsHtml(this, "Mnt", controlador, vista, titulo);
            Controlador = controlador;
            Modo = modo;
        }

        public ControlFiltroHtml BuscarControlEnFiltro(string propiedad)
        {
            return DescriptorDeMantenimiento.Filtro.BuscarControl(propiedad);
        }

        protected void DefinirVistaDeCreacion(string accion, string textoMenu)
        {
            VistaCreacion = new VistaCsHtml(this, "Crear", Controlador, accion, textoMenu);
            Menu = new MenuMantenimiento<TElemento>(this, VistaCreacion);
        }

        private string RenderDescriptor()
        {
            var htmlCrud = ModoDescriptor.Mantenimiento == Modo
                   ?
                   RenderTitulo() + Environment.NewLine +
                   Menu.RenderControl() + Environment.NewLine +
                   DescriptorDeMantenimiento.Filtro.RenderControl() + Environment.NewLine +
                   DescriptorDeMantenimiento.Grid.RenderControl() + Environment.NewLine
                   :
                   DescriptorDeMantenimiento.Filtro.RenderControl() + Environment.NewLine +
                   DescriptorDeMantenimiento.Grid.RenderControl() + Environment.NewLine;

            return htmlCrud.Render();
        }

        private string RenderTitulo()
        {
            var htmlCabecera = $"<h2>{this.Etiqueta}</h2>";
            return htmlCabecera;
        }

        protected virtual void DefinirColumnasDelGrid()
        {
        }

        public virtual void MapearElementosAlGrid(IEnumerable<TElemento> elementos)
        {

        }

        public void TotalEnBd(int totalEnBd)
        {
            DescriptorDeMantenimiento.Grid.TotalEnBd = totalEnBd;
        }

        public override string RenderControl()
        {
            return RenderDescriptor();
        }
    }

}



