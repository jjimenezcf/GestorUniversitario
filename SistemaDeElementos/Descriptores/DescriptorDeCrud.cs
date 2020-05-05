using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
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

    public class DescriptorDeCrud<TElemento> : ControlHtml where TElemento : Elemento
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
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<TElemento> { Propiedad = "chksel", Titulo = " ", PorAnchoMnt = 4, PorAnchoSel = 10, Tipo = typeof(bool) });
            var propiedades = typeof(TElemento).GetProperties();
            foreach (var p in propiedades)
            {
                var columna = new ColumnaDelGrid<TElemento> { Propiedad = p.Name, Tipo = p.PropertyType };
                IUPropiedadAttribute atributos = Elemento.ObtenerAtributos(p);

                if (atributos != null)
                {
                    columna.Visible = atributos.Visible;
                    columna.Titulo = atributos.Etiqueta;
                    columna.Ordenar = atributos.Ordenar;
                    columna.Alineada = atributos.Alineada;
                    columna.PorAnchoMnt = 0;
                    columna.PorAnchoSel = atributos.PorAnchoSel == 0 ? atributos.PorAnchoMnt : atributos.PorAnchoSel;
                    Mnt.Datos.InsertarColumna(columna, atributos.PosicionEnGrid);
                }
            }
        }

        public virtual void MapearElementosAlGrid(IEnumerable<TElemento> elementos, int cantidadPorLeer, int posicionInicial) 
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
            return Mnt.RenderControl() +
                   (
                    ModoDescriptor.Mantenimiento == Modo
                    ? $@"{Environment.NewLine}{Creador.RenderControl()}
                         {Environment.NewLine}{Editor.RenderControl()}
                         {Environment.NewLine}{Borrado.RenderControl()}"
                    : ""
                   );
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
            }
            throw new Exception($"El modo {modo} no está definido");
        }
    }

}



