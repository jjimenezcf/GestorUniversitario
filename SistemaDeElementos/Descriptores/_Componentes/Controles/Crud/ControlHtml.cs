using System.Collections.Generic;
using Enumerados;
using ModeloDeDto;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{

    public class Posicion
    {
        public int fila { get; set; }
        public int columna { get; set; }
        public Posicion()
        {
        }
        public Posicion(int f, int c)
        : this()
        {
            fila = f;
            columna = c;
        }
    }

    public class AtributosHtml
    {
        public string IdHtmlContenedor { get; set; }
        public string IdHtml { get; set; }
        public string Propiedad { get; set; }
        public enumTipoControl TipoDeControl { get; set; }
        public bool Visible { get; set; }
        public bool Editable { get; set; }
        public bool Obligatorio { get; set; }
        public string AnchoMaximo { get; set; }
        public int NumeroDeFilas { get; set; }
        public object Ayuda { get; internal set; }
        public object ValorPorDefecto { get; internal set; }
        public int LongitudMaxima { get; internal set; } = 0;
        public string Etiqueta { get; set; }
        public string Url { get; set; }

        public AtributosHtml()
        {

        }

        public AtributosHtml(string idHtmlContenedor, string idHtml, string propiedad, enumTipoControl tipoDeControl, bool visible, bool editable, bool obligatorio, string anchoMaximo, int numeroDeFilas, string ayuda, object valorPorDefecto)
        {
            IdHtmlContenedor = idHtmlContenedor;
            IdHtml = idHtml;
            Propiedad = propiedad;
            TipoDeControl = tipoDeControl;
            Visible = visible;
            Editable = editable;
            Obligatorio = obligatorio;
            AnchoMaximo = anchoMaximo;
            NumeroDeFilas = numeroDeFilas;
            Ayuda = ayuda;
            ValorPorDefecto = valorPorDefecto;
        }

        public static AtributosHtml AtributosComunes(string idHtmlContenedor, string idHtml, string propiedad, enumTipoControl tipoDeControl)
        {
            var a = new AtributosHtml();
            a.IdHtmlContenedor = idHtmlContenedor;
            a.IdHtml = idHtml;
            a.Propiedad = propiedad;
            a.TipoDeControl = tipoDeControl;

            a.Editable = true;
            a.Visible = true;
            a.Obligatorio = false;

            return a;
        }

    }

    public static class AtributosHtmlExtension
    {
        public static Dictionary<string, object> MapearComunes(this AtributosHtml atributos)
        {
            var valores = new Dictionary<string, object>();

            valores["IdHtmlContenedor"] = atributos.IdHtmlContenedor;
            valores["IdHtml"] = atributos.IdHtml;
            valores["Propiedad"] = atributos.Propiedad;
            valores["Tipo"] = atributos.TipoDeControl.Render();
            valores["Obligatorio"] = atributos.Visible && atributos.Obligatorio ? "S" : "N";
            valores["Readonly"] = !atributos.Editable ? "editable=¨N¨ readonly" : "editable=¨S¨";
            valores["Estilos"] = atributos.AnchoMaximo.IsNullOrEmpty() ? "" : $"max-width: {atributos.AnchoMaximo};";
            valores["Etiqueta"] = atributos.Etiqueta;
            return valores;
        }

    }


    public abstract class ControlHtml
    {
        public string Id { get; private set; }
        public string IdHtml => Id.ToLower();
        public string Etiqueta { get; set; }
        public string Propiedad { get; private set; }
        public string PropiedadHtml => Propiedad.ToLower();
        public string Ayuda { get; set; }
        public Posicion Posicion { get; private set; }
        public enumTipoControl Tipo { get; protected set; }

        public bool Visible { get; set; } = true;
        public bool Editable { get; set; } = true;
        public bool Obligatorio { get; set; } = false;
        public string AnchoMaximo { get; set; }

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

        public string RenderEtiqueta()
        {
            return RenderEtiqueta(IdHtml, Etiqueta, Css.Render(enumCssControlesDto.ContenedorEtiqueta), Css.Render(enumCssControlesDto.Etiqueta));
        }

        public abstract string RenderControl();

        public virtual string RenderAtributos(string atributos = "")
        {
            atributos += $@"tipo=¨{Tipo.Render()}¨
                            propiedad=¨{Propiedad.ToLower()}¨ ";
            return atributos;
        }

        public void CambiarAtributos(string etiqueta, string propiedad, string ayuda)
        {
            Etiqueta = etiqueta;
            Id = $"{Padre.Id}_{Tipo.Render()}_{propiedad}";
            Propiedad = propiedad;
            Ayuda = ayuda;
        }

        public static string RenderEtiqueta(string idControl, string etiqueta, string cssContenedor, string cssEtiqueta)
        {
            Dictionary<string, object> valores = new Dictionary<string, object>();
            valores["CssContenedor"] = cssContenedor;
            valores["CssEtiqueta"] = cssEtiqueta;
            valores["IdDeControl"] = idControl;
            valores["Etiqueta"] = etiqueta;

            return PlantillasHtml.Render(PlantillasHtml.Etiqueta, valores);

        }

        internal static string RenderizarModal(string idHtml, string controlador, string tituloH2, string cuerpo, string idOpcion, string opcion, string accion, string cerrar, string navegador, enumCssOpcionMenu claseBoton, enumModoDeAccesoDeDatos permisosNecesarios)
        {
            var htmlOpcion = "";
            if (!opcion.IsNullOrEmpty() && !accion.IsNullOrEmpty())
            {
                htmlOpcion = $@"<input id=¨{idOpcion}¨ 
                                       type=¨button¨ 
                                       tipo=¨{enumTipoControl.Opcion.Render()}¨
                                       clase=¨{Css.Render(claseBoton)}¨ 
                                       permisos-necesarios=¨{permisosNecesarios.Render()}¨ 
                                       value=¨{opcion}¨ 
                                       onclick=¨{accion}¨ />";
            }

            var htmlCerrar = $@"<input id=¨{idHtml}-cerrar¨ 
                                       type=¨button¨ 
                                       tipo=¨{enumTipoControl.Opcion.Render()}¨
                                       clase=¨{Css.Render(enumCssOpcionMenu.Basico)}¨ 
                                       permisos-necesarios=¨{enumModoDeAccesoDeDatos.SinPermiso.Render()}¨ 
                                       value=¨Cerrar¨
                                       onclick=¨{cerrar}¨ />";

            var htmlModal = $@"<div id=¨{idHtml}¨ class=¨contenedor-modal¨ controlador=¨{controlador}¨>
                              		<div id=¨{idHtml}_contenido¨ class=¨contenido-modal¨>
                              		    <div id=¨{idHtml}_cabecera¨ class=¨contenido-cabecera¨>
                              		    	<h2>{tituloH2}</h2>
                                        </div>
                              		    <div id=¨{idHtml}_cuerpo¨ class=¨contenido-cuerpo¨>
                                        {cuerpo}
                                        </div>
                                        <div id=¨{idHtml}_pie¨ class=¨contenido-pie¨>
                                          <div id=¨{idHtml}_menu¨ class=¨contenido-modal-pie-menu¨>
                                           {htmlOpcion}
                                           {htmlCerrar}
                                          </div>
                                           {navegador}
                                        </div>
                                      </div>
                              </div>";

            return htmlModal;
        }

    }

}