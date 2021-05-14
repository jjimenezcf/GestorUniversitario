using System;

namespace Enumerados
{

    public enum enumInputType { text, file, check };

    public static class InputTypeExtension
    {
        public static string Render(this enumInputType tipo)
        {
            switch (tipo)
            {
                case enumInputType.file: return "file";
                case enumInputType.check: return "check";
                case enumInputType.text: return "text";
            }

            throw new Exception($"No se ha definido como renderizar el tipo de input {tipo}");
        }
    }

    public enum enumTipoControl
    {     Selector
        , ListaDeElemento
        , ListaDinamica
        , Editor
        , RestrictorDeFiltro
        , RestrictorDeEdicion
        , Archivo
        , Check
        , UrlDeArchivo
        , VisorDeArchivo
        , ImagenDelCanvas
        , DesplegableDeFiltro
        , GridModal
        , TablaBloque
        , Bloque
        , ZonaDeMenu
        , ZonaDeDatos
        , ZonaDeFiltro
        , Menu
        , VistaCrud
        , DescriptorDeCrud
        , Opcion
        , Label
        , Referencia
        , Lista
        , SelectorDeFecha
        , SelectorDeFechaHora
        , AreaDeTexto
        , Plantilla
        , Mantenimiento
        , pnlCreador
        , pnlEditor
        , pnlBorrado
        , ModalDeRelacion
        , ModalDeConsulta
        , FiltroEntreFechas
        , pnlExportacion
        , pnlEnviarCorreo
    }

    public static class TipoControlExtension
    {
        public static string Render(this enumTipoControl tipo) {

            switch(tipo)
            {
                case enumTipoControl.Selector: return "selector";
                case enumTipoControl.ListaDeElemento: return "lista-de-elemento";
                case enumTipoControl.ListaDinamica: return "lista-dinamica";
                case enumTipoControl.Editor: return "editor";
                case enumTipoControl.RestrictorDeFiltro: return "restrictor-filtro";
                case enumTipoControl.RestrictorDeEdicion: return "restrictor-edicion";
                case enumTipoControl.Archivo: return "archivo";
                case enumTipoControl.Check: return "check";
                case enumTipoControl.UrlDeArchivo: return "url-archivo";
                case enumTipoControl.VisorDeArchivo: return "visor-archivo";
                case enumTipoControl.ImagenDelCanvas: return "imagen-de-canva";
                case enumTipoControl.DesplegableDeFiltro: return "desplegable-de-filtro";
                case enumTipoControl.GridModal: return "grid-modal";
                case enumTipoControl.TablaBloque: return "tabla-bloque";
                case enumTipoControl.Bloque: return "bloque";
                case enumTipoControl.ZonaDeMenu: return "zona-menu";
                case enumTipoControl.ZonaDeDatos: return "zona-de-datos";
                case enumTipoControl.ZonaDeFiltro: return "zona-de-filtro";
                case enumTipoControl.Menu: return "menu";
                case enumTipoControl.VistaCrud: return "vista-crud";
                case enumTipoControl.DescriptorDeCrud: return "descriptor-crud";
                case enumTipoControl.Opcion: return "opcion";
                case enumTipoControl.Label: return "label";
                case enumTipoControl.Referencia: return "referencia";
                case enumTipoControl.Lista: return "lista";
                case enumTipoControl.SelectorDeFecha: return "selector-de-fecha";
                case enumTipoControl.SelectorDeFechaHora: return "selector-de-fecha-hora";
                case enumTipoControl.AreaDeTexto: return "area-de-texto";
                case enumTipoControl.Plantilla: return "plantilla";
                case enumTipoControl.Mantenimiento: return "mantenimiento";
                case enumTipoControl.pnlCreador: return "panel-creador";
                case enumTipoControl.pnlEditor: return "panel-editor";
                case enumTipoControl.pnlExportacion: return "panel-exportacion";
                case enumTipoControl.pnlEnviarCorreo: return "panel-enviar-correo";
                case enumTipoControl.pnlBorrado: return "panel-borrado";
                case enumTipoControl.ModalDeRelacion: return "modal-de-relacion";
                case enumTipoControl.ModalDeConsulta: return "modal-de-consulta";
                case enumTipoControl.FiltroEntreFechas: return "filtro-entre-fechas";
            }
            throw new Exception($"El tipo de control {tipo} no está definido como renderizarlo");
    }
}

}
