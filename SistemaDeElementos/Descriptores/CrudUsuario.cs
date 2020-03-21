using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudUsuario : DescriptorDeCrud<EleUsuario>
    {
        public CrudUsuario(ModoDescriptor modo)
        : base(controlador: "Usuarios", vista: "MantenimientoUsuario", titulo: "Mantenimiento de usuarios", modo: modo)
        {
            if (modo == ModoDescriptor.Mantenimiento)
                new Selector<PermisoDto>(padre: new Bloque(Filtro, titulo: "Específico", dimension: new Dimension(1, 2)),
                                        etiqueta: "Permiso",
                                        propiedad: UsuariosPor.CursosInscrito,
                                        ayuda: "Seleccionar Permiso",
                                        posicion: new Posicion() { fila = 0, columna = 0 },
                                        paraFiltrar: nameof(PermisoDto.Id),
                                        paraMostrar: nameof(PermisoDto.Nombre),
                                        descriptor: new CrudPermiso(ModoDescriptor.Seleccion),
                                        propiedadDondeMapear: FiltroPor.Nombre.ToString());

            DefinirVistaDeCreacion(accion: "IraCrearUsuario", textoMenu: "Crear usuario");

            BuscarControlEnFiltro(FiltroPor.Nombre).CambiarAtributos(UsuariosPor.NombreCompleto, "Buscar por 'apellido, nombre'");            

            DefinirColumnasDelGrid();
        }


        protected override void DefinirColumnasDelGrid()
        {
            base.DefinirColumnasDelGrid();

            var columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(EleUsuario.Id), Tipo = typeof(int), Visible = false };
            Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(EleUsuario.Login) };
            Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(EleUsuario.Apellido), Ordenar = true, Ruta = "Usuarios", Accion = "IraMantenimientoUsuario" };
            Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(EleUsuario.Nombre) };
            Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid
            {
                Nombre = nameof(EleUsuario.Alta),
                Tipo = typeof(DateTime),
                Alineada = Aliniacion.centrada,
                Ordenar = true,
                Ruta = "Usuarios",
                Accion = "IraMantenimientoUsuario"
            };
            Grid.Columnas.Add(columnaDelGrid);
        }

        public override void MapearElementosAlGrid(IEnumerable<EleUsuario> elementos)
        {
            base.MapearElementosAlGrid(elementos);
            foreach (var usuario in elementos)
            {
                var fila = new FilaDelGrid();
                foreach (ColumnaDelGrid columna in Grid.Columnas)
                {
                    CeldaDelGrid celda = new CeldaDelGrid(columna);
                    if (columna.Nombre == nameof(EleUsuario.Id))
                        celda.Valor = usuario.Id.ToString();
                    else
                    if (columna.Nombre == nameof(EleUsuario.Login))
                        celda.Valor = usuario.Login.ToString();
                    else
                    if (columna.Nombre == nameof(EleUsuario.Apellido))
                        celda.Valor = usuario.Apellido;
                    else
                    if (columna.Nombre == nameof(EleUsuario.Nombre))
                        celda.Valor = usuario.Nombre.ToString();
                    else
                    if (columna.Nombre == nameof(EleUsuario.Alta))
                        celda.Valor = usuario.Alta.ToString();

                    fila.Celdas.Add(celda);
                }
                Grid.Filas.Add(fila);
            }
        }

    }
}
