﻿using ModeloDeDto.Entorno;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeVistaMvc : DescriptorDeCrud<VistaMvcDto>
    {
        public DescriptorDeVistaMvc(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto: contexto
               , nameof(VistaMvcController), nameof(VistaMvcController.CrudVistaMvc), modo, "Entorno")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta(ltrBloques.General);
            
            new EditorFiltro<VistaMvcDto>(bloque: fltGeneral
                , etiqueta: "Controlador"
                , propiedad: nameof(VistaMvcDto.Controlador)
                , ayuda: "buscar por controlador"
                , new Posicion { fila = 1, columna = 0 });

            new CheckFiltro<VistaMvcDto>(bloque: fltGeneral,
                etiqueta: "Mostrar solo las modales",
                filtrarPor: nameof(VistaMvcDto.MostrarEnModal),
                ayuda: "Sólo las las modales",
                valorInicial: false,
                filtrarPorFalse: false,
                posicion: new Posicion(1, 1));
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/VistaMvc.js¨></script>
                      <script>
                         try {{                           
                            Entorno.CrearCrudVistaMvc('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}');
                         }}
                         catch(error) {{                           
                            MensajesSe.Error('Creando el crud', error.message);
                         }}
                      </script>
                    ";
            return render.Render();
        }
    }
}
