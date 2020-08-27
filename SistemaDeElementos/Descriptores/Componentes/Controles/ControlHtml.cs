namespace MVCSistemaDeElementos.Descriptores
{

    public class Posicion
    {
        public int fila { get; set; }
        public int columna { get; set; }
    }

    public abstract class ControlHtml
    {
        public string Id { get; private set; }
        public string IdHtml => Id.ToLower();
        public string Etiqueta { get; set; }
        public string Propiedad { get; private set; }
        public string Ayuda { get; set; }
        public Posicion Posicion { get; private set; }
        public string Tipo { get; protected set; }

        public ControlHtml Padre { get; set; }

        public ControlHtml(ControlHtml padre, string id, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        {
            Padre = padre;
            Id = id;
            Etiqueta = etiqueta;
            Propiedad = propiedad;
            Ayuda = ayuda;
            Posicion = posicion;
        }

        public string RenderLabel()
        {
            return $@"<div class=¨input-group mb-3¨>
                         {Etiqueta}
                      </div>
                  ";
        }

        public abstract string RenderControl();

        public virtual string RenderAtributos(string atributos = "")
        {
            atributos += $"tipo=¨{Tipo}¨ propiedad=¨{Propiedad.ToLower()}¨ ";
            return atributos;
        }

        public void CambiarAtributos(string propiedad, string ayuda)
        {
            Id = $"{Padre.Id}_{Tipo}_{propiedad}";
            Propiedad = propiedad;
            Ayuda = ayuda;
        }

        internal static string RenderizarModal(string idHtml, string controlador, string tituloH2, string cuerpo, string idOpcion, string opcion, string accion, string cerrar, string navegador)
        {
            var htmlModal = $@"<div id=¨{idHtml}¨ class=¨contenedor-modal¨ controlador=¨{controlador}¨>
                              		<div id=¨{idHtml}_contenido¨ class=¨contenido-modal¨>
                              		    <div id=¨{idHtml}_cabecera¨ class=¨contenido-cabecera¨>
                              		    	<h2>{tituloH2}</h2>
                                        </div>
                              		    <div id=¨{idHtml}_cuerpo¨ class=¨contenido-cuerpo¨>
                                        {cuerpo}
                                        </div>
                                        <div id=¨{idHtml}_pie¨ class=¨contenido-pie¨>
                                           <input type=¨text¨ id=¨{idOpcion}¨ class=¨boton-modal¨ value=¨{opcion}¨ readonly onclick=¨{accion}¨ />
                                           <input type=¨text¨ id=¨{idHtml}-cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨  readonly onclick=¨{cerrar}¨ />
                                           {navegador}
                                        </div>
                                      </div>
                              </div>";

            return htmlModal;
        }
    }

}