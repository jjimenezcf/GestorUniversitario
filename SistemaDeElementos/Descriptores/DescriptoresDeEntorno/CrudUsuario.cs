using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.Seguridad;
using UtilidadesParaIu;
using Microsoft.AspNetCore.Hosting;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudUsuario : DescriptorDeCrud<UsuarioDto>
    {
        public CrudUsuario(ModoDescriptor modo)
        : base(controlador: "Usuarios", vista: "MantenimientoUsuario", elemento: "Usuario", modo: modo)
        {
            if (modo == ModoDescriptor.Mantenimiento)
                new SelectorDeFiltro<UsuarioDto, PermisoDto>(
                       padre: new BloqueDeFitro<UsuarioDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(1, 2)),
                       etiqueta: "Permiso",
                       filtrarPor: UsuariosPor.Permisos,
                       ayuda: "Seleccionar Permiso",
                       posicion: new Posicion() { fila = 0, columna = 0 },
                       paraFiltrar: nameof(PermisoDto.Id),
                       paraMostrar: nameof(PermisoDto.Nombre),
                       crudModal: new CrudPermiso(ModoDescriptor.Seleccion),
                       propiedadDondeMapear: FiltroPor.Nombre.ToString());
            
            BuscarControlEnFiltro(FiltroPor.Nombre).CambiarAtributos(UsuariosPor.NombreCompleto, "Buscar por 'apellido, nombre'");            

            DefinirColumnasDelGrid();
        }


        protected override void DefinirColumnasDelGrid()
        {
            base.DefinirColumnasDelGrid();
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<UsuarioDto> { Propiedad = nameof(UsuarioDtm.Login), Ordenar = true, PorAncho = 33 });
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<UsuarioDto> { Propiedad = nameof(UsuarioDtm.Apellido), Ordenar = true, PorAncho = 50});
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<UsuarioDto> { Propiedad = nameof(UsuarioDtm.Nombre) }); 
            Mnt.Datos.AnadirColumna(new ColumnaDelGrid<UsuarioDto>
            {
                Propiedad = nameof(UsuarioDtm.Alta),
                Tipo = typeof(DateTime),
                Alineada = Aliniacion.centrada,
                Ordenar = true
            });
        }

        public override void MapearElementosAlGrid(IEnumerable<UsuarioDto> elementos, int posicion)
        {
            base.MapearElementosAlGrid(elementos, posicion);
            foreach (var usuario in elementos)
            {
                var fila = new FilaDelGrid<UsuarioDto>();
                foreach (ColumnaDelGrid<UsuarioDto> columna in Mnt.Datos.Columnas)
                {
                    CeldaDelGrid<UsuarioDto> celda = new CeldaDelGrid<UsuarioDto>(columna);
                    if (columna.Propiedad == nameof(UsuarioDtm.Id))
                        celda.Valor = usuario.Id.ToString();
                    else
                    if (columna.Propiedad == nameof(UsuarioDtm.Login))
                        celda.Valor = usuario.Login.ToString();
                    else
                    if (columna.Propiedad == nameof(UsuarioDtm.Apellido))
                        celda.Valor = usuario.Apellido;
                    else
                    if (columna.Propiedad == nameof(UsuarioDtm.Nombre))
                        celda.Valor = usuario.Nombre.ToString();
                    else
                    if (columna.Propiedad == nameof(UsuarioDtm.Alta))
                        celda.Valor = usuario.Alta.ToString();

                    fila.Celdas.Add(celda);
                }
                Mnt.Datos.Filas.Add(fila);
            }
        }

        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../ts/usuarios/Usuarios.js¨></script>
                      <script>
                         Crud.crudMnt = new Usuarios.CrudMntUsuario('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}') 
                      </script>
                    ";
            return render.Render();
        }

    }
}
