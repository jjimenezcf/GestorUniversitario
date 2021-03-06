﻿using System;
using System.Collections.Generic;
using Enumerados;
using ModeloDeDto;
using ModeloDeDto.Entorno;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Negocio;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public static class ltrBloques
    {
        public static string General = nameof(General);
        public static string Comun = "Común";
    }


    public class ZonaDeFiltro<TElemento> : ControlFiltroHtml where TElemento : ElementoDto
    {
        public DescriptorDeMantenimiento<TElemento> Mnt => (DescriptorDeMantenimiento<TElemento>)Padre;
        public List<BloqueDeFitro<TElemento>> Bloques { get; private set; } = new List<BloqueDeFitro<TElemento>>();

        public EditorFiltro<TElemento> FiltroDeNombre;

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
            Tipo = enumTipoControl.ZonaDeFiltro;
            var b1 = new BloqueDeFitro<TElemento>(this, ltrBloques.General , new Dimension(1, 2));
            var b2 = new BloqueDeFitro<TElemento>(this, ltrBloques.Comun, new Dimension(2, 2));
            b1.Plegado = true;
            FiltroDeNombre = new EditorFiltro<TElemento>(bloque: b1, etiqueta: nameof(INombre.Nombre), propiedad: ltrFiltros.Nombre, ayuda: "buscar por nombre", new Posicion { fila = 0, columna = 0 });

            if (ExtensionesDto.ImplementaAuditoria(typeof(TElemento)))
            {
                var modalCreador = new DescriptorDeUsuario(mnt.Crud.Contexto, ModoDescriptor.SeleccionarParaFiltrar, "modal_creador");
                new SelectorDeFiltro<TElemento, UsuarioDto>(padre: b2,
                                              etiqueta: "Creador",
                                              filtrarPor: nameof(ElementoDtm.IdUsuaCrea),
                                              ayuda: "Usuario creador",
                                              posicion: new Posicion() { fila = 1, columna = 0 },
                                              paraFiltrar: nameof(UsuarioDto.Id),
                                              paraMostrar: nameof(UsuarioDto.NombreCompleto),
                                              crudModal: modalCreador,
                                              propiedadDondeMapear: UsuariosPor.NombreCompleto.ToString());

                new FiltroEntreFechas<TElemento>(bloque: b2,
                                    etiqueta: "Creado entre",
                                    propiedad: nameof(ElementoDtm.FechaCreacion),
                                    ayuda: "filtrar por rango de fechas",
                                    posicion: new Posicion() { fila = 1, columna = 1 });

                var modalModificador = new DescriptorDeUsuario(mnt.Crud.Contexto, ModoDescriptor.SeleccionarParaFiltrar, "modal_modificador");
                new SelectorDeFiltro<TElemento, UsuarioDto>(padre: b2,
                                              etiqueta: "Modificador",
                                              filtrarPor: nameof(ElementoDtm.IdUsuaModi),
                                              ayuda: "Usuario modificador",
                                              posicion: new Posicion() { fila = 2, columna = 0 },
                                              paraFiltrar: nameof(UsuarioDto.Id),
                                              paraMostrar: nameof(UsuarioDto.NombreCompleto),
                                              crudModal: modalModificador,
                                              propiedadDondeMapear: UsuariosPor.NombreCompleto.ToString());

                new FiltroEntreFechas<TElemento>(bloque: b2,
                                    etiqueta: "Modificado entre",
                                    propiedad: nameof(ElementoDtm.FechaModificacion),
                                    ayuda: "filtrar por rango de fechas",
                                    posicion: new Posicion() { fila = 2, columna = 1 });

                var idAudtCreacion = $"{b2.Id}_{enumTipoControl.Check.Render()}_auditoria-de-creacion";
                var accionCreacion = $"onclick = javascript:Crud.{GestorDeEventos.EventosDelMantenimiento}('{TipoDeAccionDeMnt.OcultarMostrarColumnas}','{nameof(IAuditadoDto.CreadoEl)}#{nameof(IAuditadoDto.Creador)}');";
                new CheckDeAccionFlt<TElemento>(bloque: b2,
                    id: idAudtCreacion,
                    etiqueta: "Auditoría de creación",
                    ayuda: "muestra la fecha y usuario de creación",
                    valorInicial: false,
                    posicion: new Posicion(3, 0),
                    accion: accionCreacion);

                var idAudtModificacion = $"{b2.Id}_{enumTipoControl.Check.Render()}_auditoria-de-modificacion";
                var accionModificacion = $"onclick = javascript:Crud.{GestorDeEventos.EventosDelMantenimiento}('{TipoDeAccionDeMnt.OcultarMostrarColumnas}','{nameof(IAuditadoDto.ModificadoEl)}#{nameof(IAuditadoDto.Modificador)}');";
                new CheckDeAccionFlt<TElemento>(bloque: b2,
                    id: idAudtModificacion,
                    etiqueta: "Auditoría de modificación",
                    ayuda: "muestra la última fecha y usuario de modificación",
                    valorInicial: false,
                    posicion: new Posicion(3, 1),
                    accion: accionModificacion);
            }

        }


        public ControlFiltroHtml BuscarControlPorPropiedad(string propiedad)
        {
            ControlFiltroHtml c = null;
            foreach (var b in Bloques)
            {
                c = b.BuscarControlPorPropiedad(propiedad);
                if (c != null)
                    return c;
            }
            return c;
        }

        public ControlFiltroHtml BuscarControlPorEtiqueta(string etiqueta)
        {
            ControlFiltroHtml c = null;
            foreach (var b in Bloques)
            {
                c = b.BuscarControlPorEtiqueta(etiqueta);
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
                case enumTipoDeModal.ModalParaSeleccionar:
                    evento = $"javascript:Crud.{GestorDeEventos.EventosModalParaSeleccionar}('{TipoDeAccionDeMnt.TeclaPulsada}', '{Mnt.Datos.IdHtmlModal}');";
                    break;
                default:
                    throw new Exception($"Ha de definir el evento de pulsar una tecla para la modal del tipo {tipoDeModal}");
            }
            return RenderControl().Replace("eventoTeclaPulsada", evento);
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
