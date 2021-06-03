using UtilidadesParaIu;
using MVCSistemaDeElementos.Controllers;
using ModeloDeDto.TrabajosSometidos;
using ServicioDeDatos;
using ModeloDeDto.Entorno;
using ModeloDeDto;
using ServicioDeDatos.Elemento;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeCorreos : DescriptorDeCrud<CorreoDto>
    {
        public DescriptorDeCorreos(ContextoSe contexto, ModoDescriptor modo)
        : base( contexto: contexto
               , controlador: nameof(CorreosController)
               , vista: $"{nameof(CorreosController.CrudDeCorreos)}"
               , modo: modo
               , rutaBase: "TrabajosSometido")
        {
            RenombrarEtiqueta(nameof(INombre.Nombre), "Texto", "Buscar en el asunto o cuerpo del mensaje");
            var UsuarioReceptor = new DescriptorDeUsuario(Contexto, ModoDescriptor.SeleccionarParaFiltrar);
            new SelectorDeFiltro<CorreoDto, UsuarioDto>(padre: Mnt.BloqueGeneral,
                                              etiqueta: "Receptor",
                                              filtrarPor: UsuariosPor.eMail,
                                              ayuda: "Seleccionar usuario receptor",
                                              posicion: new Posicion() { fila = 0, columna = 1 },
                                              paraFiltrar: nameof(UsuarioDto.Id),
                                              paraMostrar: nameof(UsuarioDto.NombreCompleto),
                                              crudModal: UsuarioReceptor,
                                              propiedadDondeMapear: UsuariosPor.NombreCompleto.ToString());

            var UsuarioCreador = new DescriptorDeUsuario(Contexto, ModoDescriptor.SeleccionarParaFiltrar);
            new SelectorDeFiltro<CorreoDto, UsuarioDto>(padre: Mnt.BloqueComun,
                                              etiqueta: "Usuario",
                                              filtrarPor: nameof(CorreoDto.IdUsuario),
                                              ayuda: "Usuario creador del correo",
                                              posicion: new Posicion() { fila = 0, columna = 0 },
                                              paraFiltrar: nameof(UsuarioDto.Id),
                                              paraMostrar: nameof(UsuarioDto.NombreCompleto),
                                              crudModal: UsuarioCreador,
                                              propiedadDondeMapear: UsuariosPor.NombreCompleto.ToString());
            new CheckFiltro<CorreoDto>(Mnt.BloqueComun,
                etiqueta: "Mostrar los enviados",
                filtrarPor: nameof(ltrFltCorreosDto.seHaEnviado),
                ayuda: "Sólo los enviados",
                valorInicial: false,
                filtrarPorFalse: false,
                posicion: new Posicion(0, 1));
         
            new CheckFiltro<CorreoDto>(Mnt.BloqueComun,
                etiqueta: "Mostrar los no enviados",
                filtrarPor: nameof(ltrFltCorreosDto.NoSeHaEnviado),
                ayuda: "Sólo los no enviados",
                valorInicial: false,
                filtrarPorFalse: false,
                posicion: new Posicion(1, 1));

            new FiltroEntreFechas<CorreoDto>(bloque: Mnt.BloqueComun,
                    etiqueta: "creado entre",
                    propiedad: nameof(CorreoDto.Creado),
                    ayuda: "correos creados entre las fechas indicadas",
                    posicion: new Posicion() { fila = 1, columna = 0 });

            new FiltroEntreFechas<CorreoDto>(bloque: Mnt.BloqueComun,
                                etiqueta: "Enviado entre",
                                propiedad: nameof(CorreoDto.Enviado) ,
                                ayuda: "correos enviados entre las fechas indicadas",
                                posicion: new Posicion() { fila = 2, columna = 0 });

            Editor.MenuDeEdicion.QuitarOpcionDeMenu(TipoDeAccionDeEdicion.ModificarElemento);
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/{RutaBase}/Correos.js¨></script>
                      <script>
                         try {{                           
                            TrabajosSometido.CrearCrudDeCorreos('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
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
