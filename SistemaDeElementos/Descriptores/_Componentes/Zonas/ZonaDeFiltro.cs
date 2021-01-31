using System;
using System.Collections.Generic;
using ModeloDeDto;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ZonaDeFiltro<TElemento> : ControlFiltroHtml where TElemento : ElementoDto
    {
        public DescriptorDeMantenimiento<TElemento> Mnt => (DescriptorDeMantenimiento<TElemento>)Padre;
        public List<BloqueDeFitro<TElemento>> Bloques { get; private set; } = new List<BloqueDeFitro<TElemento>>();

        public ZonaDeFiltro(DescriptorDeMantenimiento<TElemento> mnt)
        : base(
          padre: mnt,
          id: $"{mnt.Id}_Filtro",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.ZonaDeFiltro;
            var b1 = new BloqueDeFitro<TElemento>(this, "General", new Dimension(1, 2));
            new BloqueDeFitro<TElemento>(this, "Común", new Dimension(1, 2));
            new EditorFiltro<TElemento>(bloque: b1, etiqueta: "Nombre", propiedad: CamposDeFiltrado.Nombre, ayuda: "buscar por nombre", new Posicion { fila = 0, columna = 0 });
        }


        public ControlFiltroHtml BuscarControl(string propiedad)
        {
            ControlFiltroHtml c = null;
            foreach (var b in Bloques)
            {
                c = b.BuscarControl(propiedad);
                if (c != null)
                    return c;
            }
            return c;
        }

        public void AnadirBloque(BloqueDeFitro<TElemento> bloque)
        {
            if (!EstaElBloqueAnadido(bloque.Etiqueta))
                Bloques.Add(bloque);
        }

        public BloqueDeFitro<TElemento> ObtenerBloque(string identificador)
        {
            foreach (BloqueDeFitro<TElemento> b in Bloques)
            {
                if (b.Id == identificador)
                    return b;
            }

            throw new Exception($"El bloque {identificador} no está en la zona de filtrado");
        }


        public BloqueDeFitro<TElemento> ObtenerBloquePorEtiqueta(string etiqueta)
        {
            foreach (BloqueDeFitro<TElemento> b in Bloques)
            {
                if (b.Etiqueta == etiqueta)
                    return b;
            }

            throw new Exception($"El bloque con la {etiqueta} no está en la zona de filtrado");
        }

        private bool EstaElBloqueAnadido(string etiqueta)
        {
            foreach (BloqueDeFitro<TElemento> b in Bloques)
            {
                if (b.Etiqueta == etiqueta)
                    return true;
            }
            return false;
        }

        public string RenderizarLasModalesDelFiltro()
        {
            var htmlModalesEnFiltro = "";
            foreach (BloqueDeFitro<TElemento> b in Bloques)
                htmlModalesEnFiltro = $"{htmlModalesEnFiltro}{b.RenderModalesBloque()}";

            return htmlModalesEnFiltro;
        }

        public string RenderFiltroDeUnaModal(enumTipoDeModal tipoDeModal)
        {
            string evento;
            switch (tipoDeModal)
            {
                case enumTipoDeModal.ModalDeSeleccion:
                    evento = $"javascript:Crud.{GestorDeEventos.EventosModalDeSeleccion}('{TipoDeAccionDeMnt.TeclaPulsada}', '{Mnt.Datos.IdHtmlModal}');";
                    break;
                case enumTipoDeModal.ModalDeRelacion:
                    evento = $"javascript:Crud.{GestorDeEventos.EventosModalDeCrearRelaciones}('{TipoDeAccionDeMnt.TeclaPulsada}', '{Mnt.Datos.IdHtmlModal}');";
                    break;
                case enumTipoDeModal.ModalDeConsulta:
                    evento = $"javascript:Crud.{GestorDeEventos.EventosModalDeConsultaDeRelaciones}('{TipoDeAccionDeMnt.TeclaPulsada}', '{Mnt.Datos.IdHtmlModal}');";
                    break;
                default:
                    throw new Exception($"Ha de definir el evento de pulsar una tecla para la modal del tipo {tipoDeModal}");
            }
            return RenderControl().Replace("eventoTeclaPulsada",evento);
        }

        public string RenderZonaDeFiltroNoModal()
        {
            var evento = $"javascript:Crud.{GestorDeEventos.EventosDelMantenimiento}('{TipoDeAccionDeMnt.TeclaPulsada}', '');";
            return RenderControl().Replace("eventoTeclaPulsada", evento);
        }


        public override string RenderControl()
        {           
            var numeroBloques = 0;
            var areas = "";
            foreach (BloqueDeFitro<TElemento> b in Bloques)
                if (b.Tabla.Controles.Count > 0)
                    numeroBloques = numeroBloques + 1;
            var tamano = 1.00 / numeroBloques;
            var tamanos = "";

            foreach (BloqueDeFitro<TElemento> b in Bloques)
            {
                if (b.Tabla.Controles.Count > 0)
                {
                    numeroBloques = numeroBloques + 1;
                    if (areas.IsNullOrEmpty())
                    {
                        areas = $"'cuerpo-datos-filtro-bloque'";
                        tamanos = $"{tamano}fr";
                    }
                    else
                    {
                        areas = $"{areas} 'cuerpo-datos-filtro-bloque'";
                        tamanos = $"{tamanos} {tamano}fr";
                    }
                }
            }

            var estilo =
            $@"
                 style = ¨
                     grid-template-rows: {tamanos};
                     grid-template-areas: {areas};
                     ¨
                ";

            return $@"<!-- ******************* Filtro ******************* -->
                      <div id = ¨{IdHtml}¨
                           class=¨{Css.Render(enumCssCuerpo.CuerpoDatosFiltro)}¨ 
                           onkeypress=¨eventoTeclaPulsada¨
                           {estilo}>
                           {RenderDeBloquesDeFiltro()} 
                      </div> ";
        }

        private string RenderDeBloquesDeFiltro()
        {
            var htmlBloques = "";

            for (var i = 0; i < Bloques.Count; i++)
            {
                var bloque = Bloques[i];
                if (bloque.Tabla.Controles.Count > 0)
                    htmlBloques = $"{htmlBloques}{Environment.NewLine}{bloque.RenderControl()}";
            }

            return htmlBloques;
        }
    }

}
