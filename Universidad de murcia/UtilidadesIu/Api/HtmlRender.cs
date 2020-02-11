
using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using UniversidadDeMurcia.Descriptores;
using Utilidades;

namespace UtilidadesParaIu
{
    public enum Aliniacion { no_definida, izquierda, centrada, derecha, justificada };

    public enum ModeloGrid { Tabulator, Propio };

    public static class HtmlRender
    {
        public static string Render(this string cadena)
        {
            while (cadena.IndexOf("< ") >= 0)
                cadena = cadena.Replace("< ", "<");

            return cadena.Replace("¨", "\"");
        }

        public static string AlineacionCss(Aliniacion alineacion)
        {
            switch (alineacion)
            {
                case Aliniacion.izquierda:
                    return "text-left";
                case Aliniacion.derecha:
                    return "text-right";
                case Aliniacion.centrada:
                    return "text-center";
                case Aliniacion.justificada:
                    return "text-justify";
                default:
                    return "text-left";
            }
        }

        public static string AlineacionTabulator(Aliniacion alineada)
        {
            switch (alineada)
            {
                case Aliniacion.izquierda:
                    return "left";
                case Aliniacion.derecha:
                    return "right";
                case Aliniacion.centrada:
                    return "center";
                case Aliniacion.justificada:
                    return "justify";
                default:
                    return "left";
            }
        }



        public static string RenderModalesFiltro(ZonaDeFiltro filtro)
        {
            var htmlModalesEnFiltro = "";
            foreach (Bloque b in filtro.Bloques)
                htmlModalesEnFiltro = $"{htmlModalesEnFiltro}{(htmlModalesEnFiltro.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderModalesBloque(b)}";

            return htmlModalesEnFiltro;
        }

        private static object RenderModalesBloque(Bloque b)
        {
            var htmlModalesEnBloque = "";
            foreach (Control c in b.Controles)
            {
                if (c.Tipo == TipoControl.Selector)
                    htmlModalesEnBloque = $"{htmlModalesEnBloque}{(htmlModalesEnBloque.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderModal((Selector)c)}";

            }
            return htmlModalesEnBloque;
        }

        private static object RenderModal(Selector s)
        {

            const string _htmlModalSelector =
            @"
             <div class=¨modal fade¨ id=¨idModal¨ tabindex=¨-1¨ role=¨dialog¨ aria-labelledby=¨exampleModalLabel¨ aria-hidden=¨true¨>
               <div class=¨modal-dialog¨ role=¨document¨>
                 <div class=¨modal-content¨>
                   <div class=¨modal-header¨>
                     <h5 class=¨modal-title¨ id=¨exampleModalLabel¨>titulo</h5>
                   </div>
                   <div id=¨{idContenedor}¨ class=¨modal-body¨>
                     {gridDeElementos}
                   </div>
                   <div class=¨modal-footer¨>
                     <button type = ¨button¨ class=¨btn btn-secondary¨ data-dismiss=¨modal¨>Cerrar</button>
                     <button type = ¨button¨ class=¨btn btn-primary¨ data-dismiss=¨modal¨ onclick=¨AlSeleccionar('{idSelector}', '{idGrid}', '{referenciaChecks}')¨>Seleccionar</button>
                   </div>
                 </div>
               </div>
             </div>
             <script>
               AlAbrirLaModal
               AlCerrarLaModal
             </script>
             ";


            var nombreCheckDeSeleccion = $"chksel.{s.Id}";

            return _htmlModalSelector
                    .Replace("idModal", s.Modal.Id)
                    .Replace("titulo", s.Modal.Ayuda)
                    .Replace("{idSelector}", s.Id)
                    //.Replace("{idGrid}", IdGrid)
                    .Replace("{referenciaChecks}", $"{nombreCheckDeSeleccion}")
                    .Replace("{columnaId}", s.propiedadParaFiltrar)
                    .Replace("{columnaMostrar}", s.propiedadParaMostrar)
                    .Replace("{idContenedor}", $"contenedor.{s.Modal.Id}")
                    .Replace("{gridDeElementos}", "")
                    .Replace("AlAbrirLaModal", "")
                    .Replace("AlCerrarLaModal","")
                    .Render();
        }
    }
}
