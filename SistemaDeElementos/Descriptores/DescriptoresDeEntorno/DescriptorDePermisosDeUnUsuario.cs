﻿using ModeloDeDto;
using ModeloDeDto.Entorno;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDePermisosDeUnUsuario : DescriptorDeCrud<PermisosDeUnUsuarioDto>
    {
        public DescriptorDePermisosDeUnUsuario(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto: contexto
               , nameof(PermisosDeUnUsuarioController), nameof(PermisosDeUnUsuarioController.CrudPermisosDeUnUsuario), modo, "Entorno")
        {
            var fltGeneral = Mnt.Filtro.ObtenerBloquePorEtiqueta(ltrBloques.General);
            new RestrictorDeFiltro<PermisosDeUnUsuarioDto>(bloque: fltGeneral
                  , etiqueta: "Usuario"
                  , propiedad: nameof(PermisosDeUnUsuarioDto.IdUsuario)
                  , ayuda: "buscar por usuario"
                  , new Posicion { fila = 0, columna = 0 });

            BuscarControlEnFiltro(ltrFiltros.Nombre).CambiarAtributos("Permiso", nameof(PermisosDeUnUsuarioDto.Permiso), "Buscar por 'permiso'");

        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/PermisosDeUnUsuario.js¨></script>
                      <script>
                         try {{                           
                            Entorno.CrearCrudDePermisosDeUnUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
