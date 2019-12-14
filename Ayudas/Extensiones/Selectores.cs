using System;
using System.Collections.Generic;
using System.Text;

namespace Extensiones
{
    public class SelectorModal
    {
        const string _htmlModalSelector =
            @"
             <div class=¨modal fade¨ id=¨idModal¨ tabindex=¨-1¨ role=¨dialog¨ aria-labelledby=¨exampleModalLabel¨ aria-hidden=¨true¨>
               <div class=¨modal-dialog¨ role=¨document¨>
                 <div class=¨modal-content¨>
                   <div class=¨modal-header¨>
                     <h5 class=¨modal-title¨ id=¨exampleModalLabel¨>titulo</h5>
                   </div>
                   <div class=¨modal-body¨>
                     listaDeElementos
                   </div>
                   <div class=¨modal-footer¨>
                     <button type = ¨button¨ class=¨btn btn-secondary¨ data-dismiss=¨modal¨>Cerrar</button>
                     <button type = ¨button¨ class=¨btn btn-primary¨ data-dismiss=¨modal¨ onclick=¨Seleccionar()¨>Seleccionar</button>
                   </div>
                 </div>
               </div>
             </div>
             ";

        const string _htmlSelector =
              @"<div class=¨input-group mb-3¨>
                   <input id=¨idSelector¨ type = ¨text¨ class=¨form-control¨ placeholder=¨titulo¨ aria-label=¨Curso seleccionado¨ aria-describedby=¨basic-addon2¨>
                   <div class=¨input-group-append¨>
                        <button class=¨btn btn-outline-secondary¨ type=¨button¨ data-toggle=¨modal¨ data-target=¨#idModal¨ >Seleccionar</button>
                   </div>
                </div>
              ";

        const string _funcionDeSeleccion =
            @"function Seleccionar() {
                 document.getElementById(¨idSelector¨).value = 'trigonometría';
              };
             ";

        private string _idModal;
        private string _titulo;
        private string _idSelector;

        Func<string> _renderElementos;


        public string jsDeSeleccion { get; set; }
        
        public SelectorModal(string elemento, Func<string> RenderElementos)
        {
            _titulo = $"Seleccionar {elemento}";
            _idModal = $"SelectorDe{elemento}";
            _idSelector = $"id{elemento}Seleccionado";
            _renderElementos = RenderElementos;
        }

        public string RenderSelector()
        {
            return _htmlSelector
                    .Replace("idModal", _idModal)
                    .Replace("titulo", _titulo)
                    .Replace("idSelector", _idSelector)
                    .Replace("¨", "\"");
        }

        public string RenderModal()
        {
            return _htmlModalSelector
                    .Replace("idModal", _idModal)
                    .Replace("titulo", _titulo)
                    .Replace("listaDeElementos", RenderizarElementos())
                    .Replace("¨", "\"");
        }
        
        public string ScriptDeSeleccion()
        {
            return _funcionDeSeleccion
                   .Replace("idSelector", _idSelector)
                   .Replace("¨", "\"");
        }

        private string RenderizarElementos()
        {
            return _renderElementos();
        }

        public static string AnadirFila(string idSelector, int numeroDeFila,List<string> valores)
        {
            var check = "<input type=¨checkbox¨ id=¨idCheck¨ aria-label=¨Checkbox for following text input¨>"
                .Replace("idCheck",$"idCheck{numeroDeFila}_{idSelector}")
                .Replace("¨","\"");
            var fila = new StringBuilder();
            foreach (var valor in valores)
            {
                fila.AppendLine($"<td>{valor}</td>");
            }
            return $@"<tr>{fila.ToString()}<td>{check}</td><tr>";
        }

        public static string AnadirTabla(List<string> valores, StringBuilder filas)
        {
            var cabecera = new StringBuilder();
            foreach (var valor in valores)
            {
                cabecera.AppendLine($"<th><a>{valor}<a/></th>");
            }
            return $@" <table class=¨table¨>
                    <thead>
                        <tr>
                          {cabecera.ToString()}
                        <th></th>
                        </tr>
                    </thead>
                    <tbody>
                      {filas.ToString()}
                    </tbody>
                </table>";
        }

    }
}
