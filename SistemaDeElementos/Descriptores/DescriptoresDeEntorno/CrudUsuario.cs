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
                new SelectorDeFiltro<UsuarioDto, PermisoDto>(padre: new BloqueDeFitro<UsuarioDto>(filtro: Mnt.Filtro, titulo: "Específico", dimension: new Dimension(1, 2)),
                                        etiqueta: "Permiso",
                                        propiedad: UsuariosPor.Permisos,
                                        ayuda: "Seleccionar Permiso",
                                        posicion: new Posicion() { fila = 0, columna = 0 },
                                        paraFiltrar: nameof(PermisoDto.Id),
                                        paraMostrar: nameof(PermisoDto.Nombre),
                                        descriptor: new CrudPermiso(ModoDescriptor.Seleccion),
                                        propiedadDondeMapear: FiltroPor.Nombre.ToString());
            
            BuscarControlEnFiltro(FiltroPor.Nombre).CambiarAtributos(UsuariosPor.NombreCompleto, "Buscar por 'apellido, nombre'");            

            DefinirColumnasDelGrid();
        }


        protected override void DefinirColumnasDelGrid()
        {
            base.DefinirColumnasDelGrid();

            var columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(UsuarioDtm.Id), Tipo = typeof(int), Visible = false };
            Mnt.Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(UsuarioDtm.Login) };
            Mnt.Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(UsuarioDtm.Apellido), Ordenar = true, Ruta = "Usuarios", Accion = "IraMantenimientoUsuario" };
            Mnt.Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(UsuarioDtm.Nombre) };
            Mnt.Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid
            {
                Nombre = nameof(UsuarioDtm.Alta),
                Tipo = typeof(DateTime),
                Alineada = Aliniacion.centrada,
                Ordenar = true,
                Ruta = "Usuarios",
                Accion = "IraMantenimientoUsuario"
            };
            Mnt.Grid.Columnas.Add(columnaDelGrid);
        }

        public override void MapearElementosAlGrid(IEnumerable<UsuarioDto> elementos)
        {
            base.MapearElementosAlGrid(elementos);
            foreach (var usuario in elementos)
            {
                var fila = new FilaDelGrid();
                foreach (ColumnaDelGrid columna in Mnt.Grid.Columnas)
                {
                    CeldaDelGrid celda = new CeldaDelGrid(columna);
                    if (columna.Nombre == nameof(UsuarioDtm.Id))
                        celda.Valor = usuario.Id.ToString();
                    else
                    if (columna.Nombre == nameof(UsuarioDtm.Login))
                        celda.Valor = usuario.Login.ToString();
                    else
                    if (columna.Nombre == nameof(UsuarioDtm.Apellido))
                        celda.Valor = usuario.Apellido;
                    else
                    if (columna.Nombre == nameof(UsuarioDtm.Nombre))
                        celda.Valor = usuario.Nombre.ToString();
                    else
                    if (columna.Nombre == nameof(UsuarioDtm.Alta))
                        celda.Valor = usuario.Alta.ToString();

                    fila.Celdas.Add(celda);
                }
                Mnt.Grid.Filas.Add(fila);
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
