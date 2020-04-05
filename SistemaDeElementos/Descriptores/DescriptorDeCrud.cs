﻿using System;
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
        public string NombreElemento => Etiqueta.ToLower();

        public VistaCsHtml VistaMnt { get; private set; }
        public VistaCsHtml VistaCreacion { get; private set; }

        public DescriptorMantenimiento<TElemento> Mnt { get; private set; }
        public DescriptorDeCreacion<TElemento> Creacion { get; private set; }
        public DescriptorDeEdicion Edicion { get; private set; }
        public DescriptorDeBorrado Borrado { get; private set; }
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
                Creacion =  new DescriptorDeCreacion<TElemento>(crud: this, etiqueta: elemento);
                Mnt.ZonaMenu.AnadirOpcionDeCreacion();
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
            return Mnt.RenderControl() +
                   (
                    ModoDescriptor.Mantenimiento == Modo 
                    ? $"{Environment.NewLine}{Creacion.RenderControl()}" 
                    : ""
                   );
        }
    }

}



