using System;
using System.Collections.Generic;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum ModoDescriptor { Mantenimiento, Consulta, Seleccion }

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
        public string NombreElemento => Etiqueta.ToLower();

        public VistaCsHtml VistaMnt { get; private set; }
        public VistaCsHtml VistaCreacion { get; private set; }

        public DescriptorMantenimiento<TElemento> Mnt { get; private set; }
        public DescriptorDeCreacion<TElemento> Creador { get; private set; }
        public DescriptorDeEdicion<TElemento> Editor { get; private set; }
        public DescriptorDeBorrado<TElemento> Borrado { get; private set; }
        public DescriptorDeDetalle Detalle { get; private set; }

        public string Controlador { get; private set; }
        public ModoDescriptor Modo { get; private set; }

        public DescriptorDeCrud(string controlador, string vista, string elemento, ModoDescriptor modo)
        : base(
          padre: null,
          id: $"Crud_{elemento}",
          etiqueta: elemento,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.DescriptorDeCrud;
            Mnt = new DescriptorMantenimiento<TElemento>(crud: this, etiqueta: elemento);
            VistaMnt = new VistaCsHtml(this, "VistaMnt", controlador, vista, elemento);
            Controlador = controlador;
            Modo = modo;

            if (Modo == ModoDescriptor.Mantenimiento)
            {
                Creador = new DescriptorDeCreacion<TElemento>(crud: this, etiqueta: elemento);
                Editor = new DescriptorDeEdicion<TElemento>(crud: this, etiqueta: elemento);
                Borrado = new DescriptorDeBorrado<TElemento>(crud: this, etiqueta: elemento);

                Mnt.MenuDeMnt.AnadirOpcionDeCreacion();
                Mnt.MenuDeMnt.AnadirOpcionDeEditarElemento();
                Mnt.MenuDeMnt.AnadirOpcionDeBorrarElemento();
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
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<TElemento> { Propiedad = "chksel", Titulo = " ", PorAncho = 6, Tipo = typeof(bool) });
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<TElemento> { Propiedad = nameof(Id), Tipo = typeof(int), Visible = false });
        }

        public virtual void MapearElementosAlGrid(IEnumerable<TElemento> elementos, int pos = 0)
        {
            Mnt.Datos.PosicionInicial = pos;
        }

        public void TotalEnBd(int totalEnBd)
        {
            Mnt.Datos.TotalEnBd = totalEnBd;
        }

        public override string RenderControl()
        {
            return Mnt.RenderControl() +
                   (
                    ModoDescriptor.Mantenimiento == Modo
                    ? $@"{Environment.NewLine}{Creador.RenderControl()}
                         {Environment.NewLine}{Editor.RenderControl()}
                         {Environment.NewLine}{Borrado.RenderControl()}"
                    : ""
                   );
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
            }
            throw new Exception($"El modo {modo} no está definido");
        }
    }

}



