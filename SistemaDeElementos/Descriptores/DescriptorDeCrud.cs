using System;
using System.Collections.Generic;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum ModoDescriptor { Mantenimiento, Consulta, Seleccion }

    public class VistaCsHtml: ControlHtml
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

        public DescriptorMantenimiento<TElemento> Mnt { get; private set; }
        public DescriptorDeCreacion Creacion { get; private set; }
        public DescriptorDeEdicion Edicion { get; private set; }
        public DescriptorDeBorrado Borrado { get; private set; }
        public DescriptorDeDetalle Detalle { get; private set; }

        public MenuMantenimiento<TElemento> Menu { get; set; }
        public string Controlador { get; private set; }
        public ModoDescriptor Modo { get; private set; }

        public DescriptorDeCrud(string controlador, string vista, string titulo, ModoDescriptor modo)
        : base(
          padre: null,
          id: typeof(TElemento).Name.Replace("Elemento", ""),
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.DescriptorDeCrud;
            Mnt = new DescriptorMantenimiento<TElemento>(crud: this, etiqueta: titulo);
            VistaMnt = new VistaCsHtml(this, "VistaMnt", controlador, vista, titulo);
            Controlador = controlador;
            Modo = modo;

            if (Modo == ModoDescriptor.Mantenimiento)
            {
                VistaCreacion = new VistaCsHtml(this, "VistaCrt", controlador, $"Creacion{nameof(TElemento)}", $"Creacion de {nameof(TElemento)}");
                Mnt.Menu.AnadirOpcioDeCreacion();
            }
        }

        public ControlFiltroHtml BuscarControlEnFiltro(string propiedad)
        {
            return Mnt.Filtro.BuscarControl(propiedad);
        }

        protected virtual void DefinirColumnasDelGrid()
        {
        }

        public virtual void MapearElementosAlGrid(IEnumerable<TElemento> elementos)
        {

        }

        public void TotalEnBd(int totalEnBd)
        {
            Mnt.Grid.TotalEnBd = totalEnBd;
        }

        public override string RenderControl()
        {
            return Mnt.RenderControl();
        }
    }

}



